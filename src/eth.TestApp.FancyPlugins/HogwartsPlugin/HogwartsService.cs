using eth.Eve.PluginSystem.Storage;
using eth.Telegram.BotApi.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eth.TestApp.FancyPlugins.HogwartsPlugin
{
    public class HogwartsService
    {
        private class MessageSender
        {
            public bool IsMember
            {
                get
                {
                    return Member != null;
                }
            }

            public bool IsAdmin
            {
                get
                {
                    return IsMember && Member.IsAdmin;
                }
            }

            public HogwartsMember Member { get; private set; }
            public HogwartsMember RepliedToMember { get; private set; }
            public User User { get; private set; }
            public User RepliedTo { get; private set; }

            public MessageSender(User user, Func<int, HogwartsMember> memberFinder, User repliedTo = null)
            {
                User = user;               
                Member = memberFinder(user.Id);

                RepliedTo = repliedTo;
                if (RepliedTo != null)
                {
                    RepliedToMember = memberFinder(repliedTo.Id);
                }
            }
        }


        private Dictionary<HogwartsCommand.CommandType, Action<HogwartsCommand, MessageSender>> CommandActions
        {
            get
            {
                return new Dictionary<HogwartsCommand.CommandType, Action<HogwartsCommand, MessageSender>>()
                {
                    { HogwartsCommand.CommandType.AddPoints, DoAddPointsCommand },
                    { HogwartsCommand.CommandType.AssignHouse, DoAssignHouseCommand },
                    { HogwartsCommand.CommandType.AddAdmin, DoAddAdminCommand },
                    { HogwartsCommand.CommandType.Bludger, DoBludgerCommand },
                    { HogwartsCommand.CommandType.ChangeHouse, DoChangeHouseCommand },
                    { HogwartsCommand.CommandType.Dodge, DoDodgeCommand },
                    { HogwartsCommand.CommandType.MembersList, DoMembersListCommand },
                    { HogwartsCommand.CommandType.NewPartronus, DoNewPatronusCommand },
                    { HogwartsCommand.CommandType.Patronus, DoPatronusCommand },
                    { HogwartsCommand.CommandType.Prihod, DoPrihodCommand },
                    { HogwartsCommand.CommandType.ResetScore, DoResetScoreCommand },
                    { HogwartsCommand.CommandType.Score, DoScoreCommand },
                    { HogwartsCommand.CommandType.Snitch, DoSnitchCommand }
                };
            }
        }


        private const string DBScoreKey = "TotalScore.";
        private const string DBMembersKey = "Members.";
        private const string DBConfigs = "Configs.";

        private string DBKeyPostfix { get; set; }
        private IPluginLocalStorage Storage { get; set; }

        private const string ExceedingLimitForAdditionFormat = "Нельзя давать больше {0} очков за раз. Добавлено {0}.";
        private const string AssignHouseFormat = "Добро пожаловать в {0}!";
        private const string AlreadyHasHouseFormat = "Вас уже определили в {0}";
        private const string AssignPatronusFormat = "Ваш патронус {0}!";
        private const string CastPatronusFormat = "{0} спешит вам на помощь!";
        private const string NotAMemberWarning = "Сначала наденьте шляпу, чтобы стать волшебником!";
        private const string NotAnAdminWarning = "Пожалуйста, пососите писос";
        private const string SnitchCatchingResultFailed = "Вы усердно всматриваетесь в небо, но снитч нигде не виден";
        private const string SnitchCatchingResultSuccessFormat = "{0}. Сегодня вы оказались самым ловким и везучим, снитч ваш, а с ним и победа для вашей комманды! {1} получает {2} очков!";
        private const string SnitchCatchingResultOnCooldownFormat = "{0}, можете попробовать поймать снитч не чаще раза в {1} минут";

        private Dictionary<HogwartsHouse, List<HogwartsMember>> Members { get; set; }
        private Dictionary<HogwartsHouse, ulong> Score { get; set; }
        private HogwartsQuidditchGame CurrentGame { get; set; }

        private Dictionary<string, object> Configs { get; set; }
        private Random Rnd;

        private const string EnableAddingPointsKey = "EnableAddingPoints";
        private const string EnableNewMembersKey = "EnableNewMembers";
        private const string EnablePatronusKey = "EnablePatronus";
        private const string EnablePatronusRerollKey = "EnablePatronusReroll";
        private const string EnableSnitchKey = "EnableSnitch";
        private const string EnablePrihodKey = "EnablePrihod";
        private const string EnableBludgerKey = "EnableBludger";
        private const string AddPointsCooldownKey = "AddPointsCooldown";
        private const string AddPointsLimitKey = "AddPointsLimit";
        private const string SnitchCooldownKey = "SnitchCooldown";
        private const string BludgerCooldownKey = "BludgerCooldown";
        private const string SnitchChanceKey = "SnitchChance";
        private const string BludgerChanceKey = "BludgerChance";
        private const string DodgeChanceKey = "DodgeChance";
        private const string SnitchCatchPointsKey = "SnitchCatchPoints";

        private Dictionary<HogwartsHouse, ulong> InitialScore
        {
            get
            {
                return new Dictionary<HogwartsHouse, ulong>()
                {
                    { HogwartsHouse.Gryffindor, 1000 },
                    { HogwartsHouse.Hufflepuff, 1000 },
                    { HogwartsHouse.Ravenclaw, 1000 },
                    { HogwartsHouse.Slytherin, 1000 }
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
        
        private readonly Dictionary<string, object> InitialConfigs = new Dictionary<string, object>()
        {
            { EnableAddingPointsKey, false },
            { EnableNewMembersKey, true },
            { EnablePatronusKey, true },
            { EnablePatronusRerollKey, true },
            { EnableSnitchKey, true },
            { EnablePrihodKey, false },
            { EnableBludgerKey, true },
            { AddPointsCooldownKey, 20 },
            { AddPointsLimitKey, 5 },
            { SnitchCooldownKey, 10 },
            { BludgerCooldownKey, 60 },
            { SnitchChanceKey, 17 },
            { BludgerChanceKey, 25 },
            { DodgeChanceKey, 5 },
            { SnitchCatchPointsKey, 150 },
        };
        
        public HogwartsService(IPluginLocalStorage storage, string keyPostfix)
        {
            Storage = storage;
            DBKeyPostfix = keyPostfix;

            Score = LoadScore();
            Members = LoadMembers();
            Configs = LoadConfig();
            
            PendingResponse = new List<string>();
            CurrentGame = new HogwartsQuidditchGame();
            Rnd = new Random();
        }

        public List<string> PendingResponse
        {
            get; private set;
        }

        public bool HandleMessage(Message msg)
        {
            if (HogwartsCommandsHelper.TryParseCommand(msg.Text, out HogwartsCommand command))
            {
                var sender = new MessageSender(msg.From, FindMemberById, msg.ReplyToMessage?.From);
                ValidateCommandUsage(command, sender);
                CommandActions.FirstOrDefault(c => c.Key == command.Type).Value?.Invoke(command, sender);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateCommandUsage(HogwartsCommand command, MessageSender sender)
        {
            if (command.RequireMembership() && !sender.IsMember)
            {
                PendingResponse.Add(NotAMemberWarning);
                return false;
            }

            if (command.RequireAdminRights() && !sender.IsAdmin)
            {
                PendingResponse.Add(NotAnAdminWarning);
                return false;
            }
            return true;
        }

        #region Commands
        private void DoAddPointsCommand(HogwartsCommand command, MessageSender sender)
        {
            var memberHouse = GetMembersHouse(sender.Member);
            if (command.Params.House != memberHouse)
            {
                AddPoints(command.Params.House, command.Params.Points);
            }
        }

        private void DoAssignHouseCommand(HogwartsCommand command, MessageSender sender)
        {
            if (sender.IsMember)
            {
                PendingResponse.Add(string.Format(AlreadyHasHouseFormat, GetMembersHouse(sender.Member)));
            }
            else
            {
                var newMember = CreateNewMember(sender.User);
                var max = Enum.GetNames(typeof(HogwartsHouse)).Length;
                var number = Rnd.Next(max);
                var house = (HogwartsHouse)number;
                AssignHouseToMember(newMember, house);
                PendingResponse.Add(string.Format(AssignHouseFormat, house));
            }
        }
        
        private void DoMembersListCommand(HogwartsCommand command, MessageSender sender)
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
            PendingResponse.Add(sb.ToString());
        }

        private void DoNewPatronusCommand(HogwartsCommand command, MessageSender sender)
        {
            AssignPatronusToMember(sender.Member);
            PendingResponse.Add(string.Format(AssignPatronusFormat, sender.Member.Patronus));
        }

        private void DoPatronusCommand(HogwartsCommand command, MessageSender sender)
        {
            var patronus = sender.Member.Patronus;
            if (patronus != HogwartsPatronus.None)
            {
                PendingResponse.Add(string.Format(CastPatronusFormat, patronus));
            }
            else
            {
                AssignPatronusToMember(sender.Member);
                PendingResponse.Add(string.Format(AssignPatronusFormat, patronus));
            }
        }

        private void DoScoreCommand(HogwartsCommand command, MessageSender sender)
        {
            var sb = new StringBuilder();
            foreach (var kvp in Score)
            {
                sb.AppendFormat("{0}: {1}\n", kvp.Key, kvp.Value);
            }
            PendingResponse.Add(sb.ToString());
        }

        private void DoSnitchCommand(HogwartsCommand command, MessageSender sender)
        {
            if (!CurrentGame.IsEnded)
            {
                var member = sender.Member;
                var result = CurrentGame.TryCatchSnitch(member);
                switch (result)
                {
                    case HogwartsQuidditchGame.SnitchCatchingResult.Success:
                        var house = GetMembersHouse(member);
                        if (house.HasValue)
                        {
                            int points = 100;
                            AddPoints(house.Value, points);
                            PendingResponse.Add(string.Format(SnitchCatchingResultSuccessFormat, member.Name, house, points));
                        }
                        break;
                    case HogwartsQuidditchGame.SnitchCatchingResult.Failed:
                        PendingResponse.Add(SnitchCatchingResultFailed);
                        break;
                    case HogwartsQuidditchGame.SnitchCatchingResult.OnCooldown:
                        PendingResponse.Add(string.Format(SnitchCatchingResultOnCooldownFormat, member.Name, CurrentGame.CatchCooldownInMinutes));
                        break;
                }
            }
        }

        private void DoPrihodCommand(HogwartsCommand command, MessageSender sender)
        {
            if (!CurrentGame.IsEnded)
            {
                var member = sender.Member;
                var result = CurrentGame.TryCatchSnitch(member);
                switch (result)
                {
                    case HogwartsQuidditchGame.SnitchCatchingResult.Success:
                        var house = GetMembersHouse(member);
                        if (house.HasValue)
                        {
                            int points = 5;
                            AddPoints(house.Value, points);
                            PendingResponse.Add(string.Format("{0}. Сегодня вы оказались самым ловким и везучим наркопотребителем, приход ваш, а с ним и победа для вашей комманды! {1} получает утешительные {2} очков!", member.Name, house, points));
                        }
                        break;
                    case HogwartsQuidditchGame.SnitchCatchingResult.Failed:
                        PendingResponse.Add("Вы усердно всматриваетесь в локтевой сгиб, но вены нигде не видно");
                        break;
                    case HogwartsQuidditchGame.SnitchCatchingResult.OnCooldown:
                        PendingResponse.Add(string.Format("{0}, можете попробовать поймать приход не чаще раза в {1} минут. Контролируйте свое потребление.", member.Name, CurrentGame.CatchCooldownInMinutes));
                        break;
                }
            }
        }

        private void DoAddAdminCommand(HogwartsCommand command, MessageSender sender)
        {
            sender.Member.IsAdmin = true;
            PendingResponse.Add("success");
            SaveMembers();
        }

        private void DoResetScoreCommand(HogwartsCommand command, MessageSender sender)
        {
            Score = InitialScore;
            SaveScore();
        }

        private void DoChangeHouseCommand(HogwartsCommand command, MessageSender sender)
        {
            var memberToMove = sender.RepliedToMember;
            if (memberToMove == null || command.Params?.House == null)
            {
                //tak ne rabotaet koroche
                return;
            }

            var currentHouse = GetMembersHouse(memberToMove);
            if (currentHouse.HasValue)
            {
                Members[currentHouse.Value].Remove(memberToMove);
            }

            Members[command.Params.House].Add(memberToMove);
            SaveMembers();
        }

        private void DoBludgerCommand(HogwartsCommand command, MessageSender sender)
        {
            throw new NotImplementedException();
        }

        private void DoDodgeCommand(HogwartsCommand command, MessageSender sender)
        {
            throw new NotImplementedException();
        }
#endregion

        private HogwartsMember CreateNewMember(User user)
        {
            return new HogwartsMember()
            {
                Id = user.Id,
                Name = string.Format("{0} {1}", user.FirstName, user.LastName),
                Patronus = HogwartsPatronus.None
            };
        }

        private void AssignHouseToMember(HogwartsMember member, HogwartsHouse house)
        {
            Members[house].Add(member);
            SaveMembers();
        }

        private HogwartsPatronus AssignPatronusToMember(HogwartsMember member)
        {
            var max = Enum.GetNames(typeof(HogwartsPatronus)).Length;
            var number = Rnd.Next(max);
            var patronus = (HogwartsPatronus)number;
            member.Patronus = patronus;
            SaveMembers();
            return patronus;
        }

        private HogwartsHouse? GetMembersHouse(HogwartsMember member)
        {
            foreach (var kvp in Members)
            {
                if (kvp.Value.Any(m => m.Id == member.Id))
                {
                    return kvp.Key;
                };
            }
            return null;
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

        private void AddPoints(HogwartsHouse HogwartsHouse, int points)
        {
            if (points < 0)
            {
                Score[HogwartsHouse] -= (uint)points;
            }
            else
            {
                Score[HogwartsHouse] += (uint)points;
            }
            SaveScore();
        }

        private void SaveScore()
        {
            var data = JsonConvert.SerializeObject(Score);
            Storage.SetString(DBScoreKey + DBKeyPostfix, data);
        }

        private Dictionary<HogwartsHouse, ulong> LoadScore()
        {
            Dictionary<HogwartsHouse, ulong> score = null;
            if (Storage.TryGetString(DBScoreKey + DBKeyPostfix, out PluginStoreString data))
            {
                score = JsonConvert.DeserializeObject<Dictionary<HogwartsHouse, ulong>>(data.Value);
            }
            return score ?? InitialScore;
        }

        private void SaveMembers()
        {
            var data = JsonConvert.SerializeObject(Members);
            Storage.SetString(DBMembersKey + DBKeyPostfix, data);
        }

        private Dictionary<HogwartsHouse, List<HogwartsMember>> LoadMembers()
        {
            Dictionary<HogwartsHouse, List<HogwartsMember>> members = null;
            if (Storage.TryGetString(DBMembersKey + DBKeyPostfix, out PluginStoreString data))
            {
                members = JsonConvert.DeserializeObject<Dictionary<HogwartsHouse, List<HogwartsMember>>>(data.Value);
            }
            return members ?? EmptyMembersList;
        }

        private void SaveConfig()
        {
            var data = JsonConvert.SerializeObject(Configs);
            Storage.SetString(DBConfigs + DBKeyPostfix, data);
        }

        private Dictionary<string, object> LoadConfig()
        {
            Dictionary<string, object> config = null;
            if (Storage.TryGetString(DBMembersKey + DBKeyPostfix, out PluginStoreString data))
            {
                config = JsonConvert.DeserializeObject<Dictionary<string, object>>(data.Value);
            }
            return config ?? InitialConfigs;
        }
    }
}
