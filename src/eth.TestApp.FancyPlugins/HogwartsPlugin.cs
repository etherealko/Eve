using eth.Eve.PluginSystem;
using eth.Eve.PluginSystem.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace eth.TestApp.FancyPlugins
{
    public class HogwartsPlugin : IPlugin
    {
        private enum House
        {
            Gryffindor,
            Hufflepuff,
            Ravenclaw,
            Slytherin
        }

        private Dictionary<House, ulong> Score { get; set; }
        private const int MaxPointsForAddition = 20;

        private Dictionary<House, ulong> InitialScore
        {
            get
            {
                return new Dictionary<House, ulong>()
                {
                    { House.Gryffindor, 0 },
                    { House.Hufflepuff, 0 },
                    { House.Ravenclaw, 0 },
                    { House.Slytherin, 0 }
                };
            }
        }

        private const string DBScoreKey = "TotalScore";
        private const string TotalScoreFormat = "Гриффиндор {0}\nПуффендуй {1}\nКогтевран {2}\nСлизерин {3}";
        private const string ExceedingLimitForAdditionFormat = "Нельзя давать больше {0} очков за раз. Добавлено {0}.";

        private IPluginContext _ctx;

        public PluginInfo Info { get; } = new PluginInfo(new Guid("4d9b36d0-ec43-4270-89f2-262cf05f1814"),
                                                         "HogwartsPlugin",
                                                         "Hogwarts Plugin",
                                                         "0.0.0.1");

        public HandleResult Handle(IUpdateContext c)
        {
            if (c.IsInitiallyPolled)
            {
                return HandleResult.Ignored;
            }

            var msg = c.Update.Message;
            if (msg == null || msg.Text == null)
            {
                return HandleResult.Ignored;
            }

            var match = Regex.Match(msg.Text, @"\d+ очков");
            if (!match.Success)
            {
                return HandleResult.Ignored;
            }

            var isValidHouse = TryParseHouse(msg.Text, out House house);
            var isValidAmountOfPoints = uint.TryParse(match.Value.Replace("очков", string.Empty), out uint points);

            if (!isValidHouse || !isValidAmountOfPoints)
            {
                return HandleResult.Ignored;
            }

            AddPoints(house, points);
            SaveScore();

            var chatId = msg.Chat.Id;
            if (points > MaxPointsForAddition)
            {
                _ctx.BotApi.SendMessageAsync(chatId, string.Format(ExceedingLimitForAdditionFormat, MaxPointsForAddition));
            }

            PrintScore(chatId);
            return HandleResult.HandledCompletely;
        }

        public void Initialize(IPluginContext ctx)
        {
            _ctx = ctx;
        }

        public void Initialized()
        {
            var storage = _ctx.GetStorage();
            if (storage.TryGetBinary(DBScoreKey, out PluginStoreBinary binary))
            {
                var score = DeserializeScore(binary.Value);
                Score = score ?? InitialScore;
            }
            else
            {
                Score = InitialScore;
            }
        }

        public void Teardown()
        {
        }

        private bool TryParseHouse(string input, out House house)
        {
            if (input.Contains("Гриффиндору"))
            {
                house = House.Gryffindor;
                return true;
            }
            if (input.Contains("Пуффендую"))
            {
                house = House.Hufflepuff;
                return true;
            }
            if (input.Contains("Когтеврану"))
            {
                house = House.Ravenclaw;
                return true;
            }
            if (input.Contains("Слизерину"))
            {
                house = House.Slytherin;
                return true;
            }

            house = default(House);
            return false;
        }

        private void AddPoints(House house, uint points)
        {
            points = Math.Min(points, MaxPointsForAddition);
            Score[house] += points;
        }

        private void PrintScore(long chatId)
        {
            var text = string.Format(TotalScoreFormat,
                Score[House.Gryffindor],
                Score[House.Hufflepuff],
                Score[House.Ravenclaw],
                Score[House.Slytherin]);

            _ctx.BotApi.SendMessageAsync(chatId, text);
        }

        private void SaveScore()
        {
            var bytes = SerializeScore();
            var storage = _ctx.GetStorage();
            storage.SetBinary(DBScoreKey, bytes);
        }

#region Serialization Helper
        private byte[] SerializeScore()
        {
            var singleScoreBytesSize = sizeof(ulong);
            var result = new byte[singleScoreBytesSize * 4];

            var griffindorPoints = GetBytesScoreForHouse(House.Gryffindor);
            griffindorPoints.CopyTo(result, 0);

            var hufflepuffPoints = GetBytesScoreForHouse(House.Hufflepuff);
            hufflepuffPoints.CopyTo(result, singleScoreBytesSize);

            var ravenclawPoints = GetBytesScoreForHouse(House.Ravenclaw);
            ravenclawPoints.CopyTo(result, singleScoreBytesSize * 2);

            var slytherinPoints = GetBytesScoreForHouse(House.Slytherin);
            slytherinPoints.CopyTo(result, singleScoreBytesSize * 3);

            return result;
        }

        private byte[] GetBytesScoreForHouse(House house)
        {
            var bytes = BitConverter.GetBytes(Score[house]);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        private Dictionary<House, ulong> DeserializeScore(byte[] data)
        {
            var result = new Dictionary<House, ulong>();

            var singleScoreBytesSize = sizeof(ulong);
            byte[] pointsBytes = new byte[singleScoreBytesSize];

            Array.Copy(data, pointsBytes, singleScoreBytesSize);
            result.Add(House.Gryffindor, ParseBytesScore(pointsBytes));

            Array.Copy(data, singleScoreBytesSize, pointsBytes, 0, singleScoreBytesSize);
            result.Add(House.Hufflepuff, ParseBytesScore(pointsBytes));

            Array.Copy(data, singleScoreBytesSize * 2, pointsBytes, 0, singleScoreBytesSize);
            result.Add(House.Ravenclaw, ParseBytesScore(pointsBytes));

            Array.Copy(data, singleScoreBytesSize * 3, pointsBytes, 0, singleScoreBytesSize);
            result.Add(House.Slytherin, ParseBytesScore(pointsBytes));

            return result;
        }

        private uint ParseBytesScore(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt32(bytes, 0);
        }
#endregion
    }
}
