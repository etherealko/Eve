using eth.Telegram.BotApi.Objects;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace eth.TestApp.FancyPlugins.HogwartsPlugin
{
    public static class HogwartsCommandsHelper
    {
        public static HogwartsCommand GetCommand(Message msg)
        {
            HogwartsCommand command;

            var text = msg.Text;
            command = IsUpdatePointsCommand(text);
            if (command != null)
            {
                return command;
            }

            command = IsAssignHouseCommand(text);
            if (command != null)
            {
                return command;
            }

            command = IsNewPartronusCommand(text);
            if (command != null)
            {
                return command;
            }

            command = IsPatronusCommand(text);
            if (command != null)
            {
                return command;
            }

            command = IsScoreCommand(text);
            if (command != null)
            {
                return command;
            }

            command = IsMembersListCommand(text);
            if (command != null)
            {
                return command;
            }

            command = IsSnitchCommand(text);
            if (command != null)
            {
                return command;
            }

            command = IsPrihodCommand(text);
            if (command != null)
            {
                return command;
            }

            command = IsSetAdminCommand(text, msg.From, msg.ReplyToMessage?.From);
            if (command != null)
            {
                return command;
            }

            command = IsAddAdminCommand(text, msg.From, msg.ReplyToMessage?.From);
            if (command != null)
            {
                return command;
            }

            command = IsChangeHouseCommand(text, msg.ReplyToMessage?.From);
            if (command != null)
            {
                return command;
            }

            command = IsBludgerCommand(text, msg.ReplyToMessage?.From);
            if (command != null)
            {
                return command;
            }

            command = IsDodgeCommand(text);
            if (command != null)
            {
                return command;
            }
            return null;
        }

        private static HogwartsCommand IsUpdatePointsCommand(string text)
        {
            var match = Regex.Match(text, @"\d+ очк");
            if (!match.Success)
            {
                return null;
            }

            var isValidHouse = TryParseHouse(text, out HogwartsHouse house);
            var isValidAmountOfPoints = int.TryParse(match.Value.Replace("очк", string.Empty), out int points);

            if (!isValidHouse || !isValidAmountOfPoints)
            {
                return null;
            }

            return new AddPointsCommand(true, false, HogwartsCommandType.AddPoints, points, house);
        }

        public static HogwartsCommand IsAssignHouseCommand(string text)
        {
            if (text.StartsWith("надеть шляпу", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(false, false, HogwartsCommandType.AssignHouse);
            }
            return null;
        }

        public static HogwartsCommand IsNewPartronusCommand(string text)
        {
            if (text.StartsWith("новый патронус", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(true, false, HogwartsCommandType.NewPartronus);
            }
            return null;
        }

        public static HogwartsCommand IsPatronusCommand(string text)
        {
            if (text.StartsWith("экспекто патронум", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(true, false, HogwartsCommandType.Patronus);
            }
            return null;
        }

        private static HogwartsCommand IsScoreCommand(string text)
        {
            if (text.StartsWith("очки", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(false, false, HogwartsCommandType.Score);
            }
            return null;
        }

        private static HogwartsCommand IsMembersListCommand(string text)
        {
            if (text.StartsWith("список", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(false, false, HogwartsCommandType.MembersList);
            }
            return null;
        }

        private static HogwartsCommand IsSnitchCommand(string text)
        {
            if (text.StartsWith("поймать снитч", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(true, false, HogwartsCommandType.Snitch);
            }
            return null;
        }

        private static HogwartsCommand IsPrihodCommand(string text)
        {
            if (text.StartsWith("поймать приход", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(true, false, HogwartsCommandType.Prihod);
            }
            return null;
        }

        private static AdminCommand IsSetAdminCommand(string text, User from, User replied)
        {
            if (text.StartsWith("set admin", StringComparison.InvariantCultureIgnoreCase))
            {
                if (text.Contains("-me"))
                {
                    return new AdminCommand(true, false, HogwartsCommandType.SetAdmin, from.Id);
                }     
            }
            return null;
        }

        private static HogwartsCommand IsAddAdminCommand(string text, User from, User replied)
        {
            if (text.StartsWith("add admin", StringComparison.InvariantCultureIgnoreCase))
            {
                if (text.Contains("-this") && replied != null)
                {
                    return new AdminCommand(true, true, HogwartsCommandType.SetAdmin, replied.Id);
                }
            }
            return null;
        }

        private static HogwartsCommand IsResetScoreCommand(string text)
        {
            if (text.StartsWith("reset score", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(true, true, HogwartsCommandType.ResetScore);
            }
            return null;
        }

        private static HogwartsCommand IsChangeHouseCommand(string text, User replied)
        {
            if (text.StartsWith("change house", StringComparison.InvariantCultureIgnoreCase))
            {
                if (text.Contains("-this") && replied != null)
                {
                    var match = Regex.Match(text, @"-to \d+");
                    if (!match.Success)
                    {
                        return null;
                    }

                    if (int.TryParse(match.Value.Replace("-to ", string.Empty), out int houseId) 
                        && houseId >= 0 && houseId < Enum.GetNames(typeof(HogwartsHouse)).Length)
                    {
                        return new ChangeHouseCommand(true, true, HogwartsCommandType.ChangeHouse, replied.Id, (HogwartsHouse)houseId);
                    }
                }
            }
            return null;
        }

        private static HogwartsCommand IsChangeConfigCommand(string text)
        {
            if (text.StartsWith("change config", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            return null;
        }

        private static HogwartsCommand IsBludgerCommand(string text, User replied)
        {
            if (text.StartsWith("запустить бладжер", StringComparison.InvariantCultureIgnoreCase))
            {
                return new BludgerCommand(true, false, HogwartsCommandType.Bludger, replied?.Id);
            }
            return null;
        }

        private static HogwartsCommand IsDodgeCommand(string text)
        {
            if (text.StartsWith("увернуться", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(true, false, HogwartsCommandType.Dodge);
            }
            return null;
        }

        private static bool TryParseHouse(string input, out HogwartsHouse HogwartsHouse)
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
    }
}
