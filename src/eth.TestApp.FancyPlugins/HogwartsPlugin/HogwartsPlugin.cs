using eth.Eve.PluginSystem;
using eth.Eve.PluginSystem.Storage;
using eth.Telegram.BotApi.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace eth.TestApp.FancyPlugins.HogwartsPlugin
{
    public class HogwartsPlugin : IPlugin
    {
        private const string DBScoreKey = "TotalScore.";
        private const string DBMembersKey = "Members.";

        private Random Rnd;

        private bool AllowPrivateChat = false;
        private bool RestrictOtherConf = true;
        private long ConfId = -1001013065325;

        private Dictionary<HogwartsHouse, List<HogwartsMember>> Members { get; set; }
        private Dictionary<HogwartsHouse, ulong> Score { get; set; }
        private HogwartsQuidditchGame CurrentGame { get; set; }

        private const int MaxPointsForAddition = 20;

        private const string ExceedingLimitForAdditionFormat = "Нельзя давать больше {0} очков за раз. Добавлено {0}.";
        private const string AssignHouseFormat = "Добро пожаловать в {0}!";
        private const string AlreadyHasHouseFormat = "Вас уже определили в {0}";
        private const string AssignPatronusFormat = "Ваш патронус {0}!";
        private const string CastPatronusFormat = "{0} спешит вам на помощь!";
        private const string NotAMemberWarning = "Сначала наденьте шляпу, чтобы стать волшебником!";
        private const string SnitchCatchingResultFailed = "Вы усердно всматриваетесь в небо, но снитч нигде не виден";
        private const string SnitchCatchingResultSuccessFormat = "{0}. Сегодня вы оказались самым ловким и везучим, снитч ваш, а с ним и победа для вашей комманды! {1} получает {2} очков!";
        private const string SnitchCatchingResultOnCooldownFormat = "{0}, можете попробовать поймать снитч не чаще раза в {1} минут";

        private bool _pinQuidditchSuccess;

        private Dictionary<HogwartsHouse, ulong> EmptyScore
        {
            get
            {
                return new Dictionary<HogwartsHouse, ulong>()
                {
                    { HogwartsHouse.Gryffindor, 0 },
                    { HogwartsHouse.Hufflepuff, 0 },
                    { HogwartsHouse.Ravenclaw, 0 },
                    { HogwartsHouse.Slytherin, 0 }
                };
            }
        }

        private Dictionary<HogwartsHouse, List<HogwartsMember>> EmptyMembersList
        {
            get
            {
                return new Dictionary<HogwartsHouse, List<HogwartsMember>>
                {
                    { HogwartsHouse.Gryffindor, new List<HogwartsMember>() },
                    { HogwartsHouse.Hufflepuff, new List<HogwartsMember>() },
                    { HogwartsHouse.Ravenclaw, new List<HogwartsMember>() },
                    { HogwartsHouse.Slytherin, new List<HogwartsMember>() }
                };
            }
        }

        #region IPlugin API
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

            if (!AllowPrivateChat && msg.Chat.Type == Telegram.BotApi.Objects.Enums.ChatType.Private)
            {
                return HandleResult.Ignored;
            }

            if (RestrictOtherConf && msg.Chat.Id != ConfId)
            {
                return HandleResult.Ignored;
            }

            if (IsUpdatePointsCommand(msg)
                || IsAssignHouseCommand(msg)
                || IsMembersListCommand(msg)
                || IsScoreCommand(msg)
                //|| IsNewPartronusCommand(msg)
                || IsPatronusCommand(msg)
                || IsSnitchCommand(msg)
                || IsPrihodCommand(msg))
            {
                return HandleResult.HandledCompletely;
            }
            else
            {
                return HandleResult.Ignored;
            }
        }

        public void Initialize(IPluginContext ctx)
        {
            _ctx = ctx;

            Score = LoadScore();
            Members = LoadMembers();
            CurrentGame = new HogwartsQuidditchGame();

            Rnd = new Random(); 

            using (var s = _ctx.GetStorage())
            {
                if (s.TryGetString("pinQuidditchSuccess", out var pinQuidditchSuccess))
                    _pinQuidditchSuccess = string.Equals(pinQuidditchSuccess?.Value, "true", StringComparison.OrdinalIgnoreCase);
            }
        }

        public void Initialized()
        {
        }

        public void Teardown()
        {
        }
        #endregion

        private readonly Dictionary<int, DateTime> _pointFloodControl = new Dictionary<int, DateTime>();

        private bool IsUpdatePointsCommand(Message msg)
        {
            var match = Regex.Match(msg.Text, @"\d+ очк");
            if (!match.Success)
            {
                return false;
            }

            var isValidHouse = TryParseHouse(msg.Text, out HogwartsHouse house);
            var isValidAmountOfPoints = uint.TryParse(match.Value.Replace("очк", string.Empty), out uint points);

            if (!isValidHouse || !isValidAmountOfPoints)
            {
                return false;
            }

            if (!_pointFloodControl.TryGetValue(msg.From.Id, out var lastHandled) || DateTime.Now - lastHandled > TimeSpan.FromSeconds(30))
                _pointFloodControl[msg.From.Id] = DateTime.Now;
            else
                return true;

            AddPoints(house, points);
            SaveScore();

            var chatId = msg.Chat.Id;
            if (points > MaxPointsForAddition)
            {
                _ctx.BotApi.SendMessageAsync(chatId, string.Format(ExceedingLimitForAdditionFormat, MaxPointsForAddition));
            }

            PrintScore(chatId);
            return true;
        }

        private bool IsAssignHouseCommand(Message msg)
        {
            if (!msg.Text.StartsWith("надеть шляпу", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            
            var chatId = msg.Chat.Id;
            foreach (var kvp in Members)
            {
                if (kvp.Value.Any(member => member.Id == msg.From.Id))
                {
                    _ctx.BotApi.SendMessageAsync(chatId, string.Format(AlreadyHasHouseFormat, kvp.Key));
                    return true;
                };
            }

            var newMember = CreateNewMember(msg.From);
            var house = AssignHouseToMember(newMember);
            _ctx.BotApi.SendMessageAsync(chatId, string.Format(AssignHouseFormat, house));

            SaveMembers();
            return true;
        }

        private bool IsNewPartronusCommand(Message msg)
        {
            if (!msg.Text.StartsWith("новый патронус", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var chatId = msg.Chat.Id;
            var member = FindMemberById(msg.From.Id);
            if (member != null)
            {
                AssignPatronusToMember(member);
                _ctx.BotApi.SendMessageAsync(chatId, string.Format(AssignPatronusFormat, member.Patronus));
            }
            else
            {
                _ctx.BotApi.SendMessageAsync(chatId, NotAMemberWarning);
            }
            return true;
        }

        private bool IsPatronusCommand(Message msg)
        {
            if (!msg.Text.StartsWith("экспекто патронум", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var chatId = msg.Chat.Id;
            var member = FindMemberById(msg.From.Id);
            if (member != null)
            {
                if (member.Patronus == HogwartsPatronus.None)
                {
                    AssignPatronusToMember(member);
                    _ctx.BotApi.SendMessageAsync(chatId, string.Format(AssignPatronusFormat, member.Patronus));
                }
                else
                {
                    _ctx.BotApi.SendMessageAsync(chatId, string.Format(CastPatronusFormat, member.Patronus));
                }
            }
            else
            {
                _ctx.BotApi.SendMessageAsync(chatId, NotAMemberWarning);
            }

            return true;
        }

        private bool IsScoreCommand(Message msg)
        {
            if (!msg.Text.StartsWith("очки", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            PrintScore(msg.Chat.Id);
            return true;
        }

        private bool IsMembersListCommand(Message msg)
        {
            if (!msg.Text.StartsWith("список", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            PrintMembers(msg.Chat.Id);
            return true;
        }

        private bool IsSnitchCommand(Message msg)
        {
            if (!msg.Text.StartsWith("поймать снитч", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var chatId = msg.Chat.Id;
            var member = FindMemberById(msg.From.Id);
            if (member == null)
            {
                _ctx.BotApi.SendMessageAsync(chatId, NotAMemberWarning);
                return true;
            }

            if (!CurrentGame.IsEnded)
            {
                var result = CurrentGame.TryCatchSnitch(member);
                if (result == HogwartsQuidditchGame.SnitchCatchingResult.Success)
                {
                    var house = HogwartsHouse.Gryffindor;
                    foreach (var kvp in Members)
                    {
                        if (kvp.Value.Any(m => m.Id == member.Id))
                        {
                            house = kvp.Key;
                        };
                    }

                    uint points = 200;
                    Score[house] += points;
                    var task = _ctx.BotApi.SendMessageAsync(chatId, string.Format(SnitchCatchingResultSuccessFormat, member.Name, house, points));
                        
                    if (_pinQuidditchSuccess)
                        task.ContinueWith(m => 
                        {
                            var message = m.Result;

                            _ctx.BotApi.PinChatMessage(message.Chat.Id, message.MessageId);
                        });
                }
                else if (result == HogwartsQuidditchGame.SnitchCatchingResult.Failed)
                {
                    _ctx.BotApi.SendMessageAsync(chatId, SnitchCatchingResultFailed);
                }
                else if (result == HogwartsQuidditchGame.SnitchCatchingResult.OnCooldown)
                {
                    _ctx.BotApi.SendMessageAsync(chatId, string.Format(SnitchCatchingResultOnCooldownFormat, member.Name, CurrentGame.CatchCooldownInMinutes));
                }
            }
            return true;
        }

        private bool IsPrihodCommand(Message msg)
        {
            if (!msg.Text.StartsWith("поймать приход", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var chatId = msg.Chat.Id;
            var member = FindMemberById(msg.From.Id);
            if (member == null)
            {
                _ctx.BotApi.SendMessageAsync(chatId, NotAMemberWarning);
                return true;
            }

            if (!CurrentGame.IsEnded)
            {
                var result = CurrentGame.TryCatchSnitch(member);
                if (result == HogwartsQuidditchGame.SnitchCatchingResult.Success)
                {
                    var house = HogwartsHouse.Gryffindor;
                    foreach (var kvp in Members)
                    {
                        if (kvp.Value.Any(m => m.Id == member.Id))
                        {
                            house = kvp.Key;
                        };
                    }

                    uint points = 5;
                    AddPoints(house, points);
                    _ctx.BotApi.SendMessageAsync(chatId, string.Format("{0}. Сегодня вы оказались самым ловким и везучим наркопотребителем, приход ваш, а с ним и победа для вашей комманды! {1} получает утешительные {2} очков!", member.Name, house, points));
                }
                else if (result == HogwartsQuidditchGame.SnitchCatchingResult.Failed)
                {
                    _ctx.BotApi.SendMessageAsync(chatId, "Вы усердно всматриваетесь в локтевой сгиб, но вены нигде не видно");
                }
                else if (result == HogwartsQuidditchGame.SnitchCatchingResult.OnCooldown)
                {
                    _ctx.BotApi.SendMessageAsync(chatId, string.Format("{0}, можете попробовать поймать приход не чаще раза в {1} минут. Контролируйте свое потребление.", member.Name, CurrentGame.CatchCooldownInMinutes));
                }
            }
            return true;
        }

        private HogwartsMember CreateNewMember(User user)
        {
            return new HogwartsMember()
            {
                Id = user.Id,
                Name = string.Format("{0} {1}", user.FirstName, user.LastName),
                Patronus = HogwartsPatronus.None
            };
        }

        private HogwartsHouse AssignHouseToMember(HogwartsMember member)
        {
            var max = Enum.GetNames(typeof(HogwartsHouse)).Length;
            var number = Rnd.Next(max);
            var house = (HogwartsHouse)number;
            Members[house].Add(member);
            return house;
        }

        private HogwartsPatronus AssignPatronusToMember(HogwartsMember member)
        {
            var max = Enum.GetNames(typeof(HogwartsPatronus)).Length;
            var number = Rnd.Next(max);
            var patronus = (HogwartsPatronus)number;
            member.Patronus = patronus;
            return patronus;
        }

        private HogwartsMember FindMemberById(int id)
        {
            foreach (var kvp in Members)
            {
                if (kvp.Value.Any(member => member.Id == id))
                {
                    return kvp.Value.Find(member => member.Id == id);
                };
            }
            return null;
        }

        private bool TryParseHouse(string input, out HogwartsHouse HogwartsHouse)
        {
            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            if (compareInfo.IndexOf(input, "гриффиндору", CompareOptions.IgnoreCase) >= 0)
            {
                HogwartsHouse = HogwartsHouse.Gryffindor;
                return true;
            }
            if (compareInfo.IndexOf(input, "пуффендую", CompareOptions.IgnoreCase) >= 0)
            {
                HogwartsHouse = HogwartsHouse.Hufflepuff;
                return true;
            }
            if (compareInfo.IndexOf(input, "когтеврану", CompareOptions.IgnoreCase) >= 0)
            {
                HogwartsHouse = HogwartsHouse.Ravenclaw;
                return true;
            }
            if (compareInfo.IndexOf(input, "слизерину", CompareOptions.IgnoreCase) >= 0)
            {
                HogwartsHouse = HogwartsHouse.Slytherin;
                return true;
            }

            HogwartsHouse = default(HogwartsHouse);
            return false;
        }

        private void AddPoints(HogwartsHouse HogwartsHouse, uint points)
        {
            points = Math.Min(points, MaxPointsForAddition);
            Score[HogwartsHouse] += points;
        }

        private void PrintMembers(long chatId)
        {
            var sb = new StringBuilder();
            foreach (var kvp in Members)
            {
                sb.AppendFormat("{0}: ", kvp.Key);                
                foreach (var user in kvp.Value)
                {
                    sb.AppendFormat("{0}, ", user.Name);
                }
                sb.Append("\n");
            }

            _ctx.BotApi.SendMessageAsync(chatId, sb.ToString());
        }

        private void PrintScore(long chatId)
        {
            var sb = new StringBuilder();
            foreach (var kvp in Score)
            {
                sb.AppendFormat("{0}: {1}\n", kvp.Key, kvp.Value);
            }

            _ctx.BotApi.SendMessageAsync(chatId, sb.ToString());
        }

        #region Serialization Helper
        private void SaveScore()
        {
            var data = JsonConvert.SerializeObject(Score);
            var storage = _ctx.GetStorage();
            storage.SetString(DBScoreKey + ConfId, data);
        }

        private Dictionary<HogwartsHouse, ulong> LoadScore()
        {
            Dictionary<HogwartsHouse, ulong> score = null;
            var storage = _ctx.GetStorage();
            if (storage.TryGetString(DBScoreKey + ConfId, out PluginStoreString data))
            {
                score = JsonConvert.DeserializeObject<Dictionary<HogwartsHouse, ulong>>(data.Value);
            }
            return score ?? EmptyScore;
        }

        private void SaveMembers()
        {
            var data = JsonConvert.SerializeObject(Members);
            var storage = _ctx.GetStorage();
            storage.SetString(DBMembersKey + ConfId, data);
        }

        private Dictionary<HogwartsHouse, List<HogwartsMember>> LoadMembers()
        {
            Dictionary<HogwartsHouse, List<HogwartsMember>> members = null;
            var storage = _ctx.GetStorage();
            if (storage.TryGetString(DBMembersKey + ConfId, out PluginStoreString data))
            {
                members = JsonConvert.DeserializeObject<Dictionary<HogwartsHouse, List<HogwartsMember>>>(data.Value);
            }

            return members ?? EmptyMembersList;
        }
        #endregion
    }
}
