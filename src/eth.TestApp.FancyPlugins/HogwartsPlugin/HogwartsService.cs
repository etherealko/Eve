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
            var command = HogwartsCommandsHelper.GetCommand(msg);
            if (command == null)
            {
                return false;
            }

            var member = FindMemberById(msg.From.Id);
            var isMember = member != null;
            if (command.RequireMembership && !isMember)
            {
                PendingResponse.Add(NotAMemberWarning);
                return true;
            }

            if (command.RequireAdminRights && !member.IsAdmin)
            {
                PendingResponse.Add(NotAnAdminWarning);
                return true;
            }

            DoCommand(member, command, msg.From);
            return true;
        }

#region Commands
        private void DoCommand(HogwartsMember member, HogwartsCommand command, User user)
        {
            switch (command.Type)
            {
                case HogwartsCommandType.AddPoints:
                    if (Configs[EnableAddingPointsKey].Equals(true))
                    {
                        DoAddPointsCommand(command as AddPointsCommand);
                    }
                    break;

                case HogwartsCommandType.AssignHouse:
                    if (Configs[EnableNewMembersKey].Equals(true))
                    {
                        if (member != null)
                        {
                            DoAssignHouseCommand(member);
                        }
                        else
                        {
                            DoAssignHouseCommand(user);
                        }
                    }
                    break;

                case HogwartsCommandType.MembersList:
                    DoMembersListCommand();
                    break;

                case HogwartsCommandType.NewPartronus:
                    if (Configs[EnablePatronusRerollKey].Equals(true))
                    {
                        DoNewPatronusCommand(member);
                    }
                    break;

                case HogwartsCommandType.Patronus:
                    if (Configs[EnablePatronusKey].Equals(true))
                    {
                        DoPatronusCommand(member);
                    }
                    break;

                case HogwartsCommandType.Score:
                    DoScoreCommand();
                    break;

                case HogwartsCommandType.Snitch:
                    if (Configs[EnableSnitchKey].Equals(true))
                    {
                        DoSnitchCommand(member);
                    }
                    break;

                case HogwartsCommandType.Prihod:
                    if (Configs[EnablePrihodKey].Equals(true))
                    {
                        DoPrihodCommand(member);
                    }
                    break;

                case HogwartsCommandType.AddAdmin:
                    DoAddAdminCommand(member);
                    break;

                case HogwartsCommandType.SetAdmin:
                    DoAddAdminCommand(member);
                    break;

                case HogwartsCommandType.ResetScore:
                    DoResetScoreCommand();
                    break;

                case HogwartsCommandType.ChangeHouse:
                    DoChangeHouseCommand(command as ChangeHouseCommand);
                    break;

                case HogwartsCommandType.ChangeConfig:
                    break;

                case HogwartsCommandType.Bludger:
                    DoBludgerCommand(command as BludgerCommand);
                    break;

                case HogwartsCommandType.Dodge:
                    DoDodgeCommand();
                    break;
            }
        }

        private void DoAddPointsCommand(AddPointsCommand command)
        {
            if (command != null)
            {
                AddPoints(command.House, command.Points);
            }
        }

        private void DoAssignHouseCommand(HogwartsMember member)
        {
            PendingResponse.Add(string.Format(AlreadyHasHouseFormat, GetMembersHouse(member)));
        }

        private void DoAssignHouseCommand(User user)
        {
            var newMember = CreateNewMember(user);
            var max = Enum.GetNames(typeof(HogwartsHouse)).Length;
            var number = Rnd.Next(max);
            var house = (HogwartsHouse)number;
            AssignHouseToMember(newMember, house);
            PendingResponse.Add(string.Format(AssignHouseFormat, house));
        }

        private void DoMembersListCommand()
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

        private void DoNewPatronusCommand(HogwartsMember member)
        {
            AssignPatronusToMember(member);
            PendingResponse.Add(string.Format(AssignPatronusFormat, member.Patronus));
        }

        private void DoPatronusCommand(HogwartsMember member)
        {
            if (member.Patronus != HogwartsPatronus.None)
            {
                PendingResponse.Add(string.Format(CastPatronusFormat, member.Patronus));
            }
            else
            {
                AssignPatronusToMember(member);
                PendingResponse.Add(string.Format(AssignPatronusFormat, member.Patronus));
            }
        }

        private void DoScoreCommand()
        {
            var sb = new StringBuilder();
            foreach (var kvp in Score)
            {
                sb.AppendFormat("{0}: {1}\n", kvp.Key, kvp.Value);
            }
            PendingResponse.Add(sb.ToString());
        }

        private void DoSnitchCommand(HogwartsMember member)
        {
            if (!CurrentGame.IsEnded)
            {
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

        private void DoPrihodCommand(HogwartsMember member)
        {
            if (!CurrentGame.IsEnded)
            {
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

        private void DoAddAdminCommand(HogwartsMember member)
        {
            member.IsAdmin = true;
            PendingResponse.Add("success");
            SaveMembers();
        }

        private void DoResetScoreCommand()
        {
            Score = InitialScore;
            SaveScore();
        }

        private void DoChangeHouseCommand(ChangeHouseCommand command)
        {
            if (command == null)
            {
                return;
            }

            HogwartsMember memberToMove = null;
            foreach (var kvp in Members)
            {
                if (kvp.Value.Any(member => member.Id == command.Id))
                {
                    if (kvp.Key != command.House)
                    {
                        memberToMove = kvp.Value.Find(member => member.Id == command.Id);
                        kvp.Value.Remove(memberToMove);
                        break;
                    }
                };
            }
            Members[command.House].Add(memberToMove);
            SaveMembers();
        }

        private void DoBludgerCommand(BludgerCommand command)
        {
            if (command == null)
            {
                return;
            }
            CurrentGame.

        }

        private void DoDodgeCommand()
        {

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
