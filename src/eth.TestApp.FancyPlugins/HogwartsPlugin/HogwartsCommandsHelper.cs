using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace eth.TestApp.FancyPlugins.HogwartsPlugin
{
    public static class HogwartsCommandsHelper
    {
        public static bool TryParseCommand(string text, out HogwartsCommand command)
        {
            foreach (var parser in CommandParsers)
            {
                command = parser(text);
                if (command != null)
                {
                    return true;
                }
            }
            command = null;
            return false;
        }

        #region Parse helpers

        private static List<Func<string, HogwartsCommand>> CommandParsers = new List<Func<string, HogwartsCommand>>
        {
            IsUpdatePointsCommand,
            IsAssignHouseCommand,
            IsNewPartronusCommand,
            IsPatronusCommand,
            IsScoreCommand,
            IsMembersListCommand,
            IsSnitchCommand,
            IsPrihodCommand,
            IsAddAdminCommand,
            IsChangeHouseCommand,
            IsBludgerCommand,
            IsDodgeCommand
        };

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

            var cParams = new HogwartsCommand.CommandParams()
            {
                Points = points,
                House = house,
            };

            return new HogwartsCommand(HogwartsCommand.CommandType.AddPoints, cParams);
        }

        public static HogwartsCommand IsAssignHouseCommand(string text)
        {
            if (text.StartsWith("надеть шляпу", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(HogwartsCommand.CommandType.AssignHouse);
            }
            return null;
        }

        public static HogwartsCommand IsNewPartronusCommand(string text)
        {
            if (text.StartsWith("новый патронус", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(HogwartsCommand.CommandType.NewPartronus);
            }
            return null;
        }

        public static HogwartsCommand IsPatronusCommand(string text)
        {
            if (text.StartsWith("экспекто патронум", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(HogwartsCommand.CommandType.Patronus);
            }
            return null;
        }

        private static HogwartsCommand IsScoreCommand(string text)
        {
            if (text.StartsWith("очки", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(HogwartsCommand.CommandType.Score);
            }
            return null;
        }

        private static HogwartsCommand IsMembersListCommand(string text)
        {
            if (text.StartsWith("список", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(HogwartsCommand.CommandType.MembersList);
            }
            return null;
        }

        private static HogwartsCommand IsSnitchCommand(string text)
        {
            if (text.StartsWith("поймать снитч", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(HogwartsCommand.CommandType.Snitch);
            }
            return null;
        }

        private static HogwartsCommand IsPrihodCommand(string text)
        {
            if (text.StartsWith("поймать приход", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(HogwartsCommand.CommandType.Prihod);
            }
            return null;
        }

        private static HogwartsCommand IsAddAdminCommand(string text)
        {
            if (text.StartsWith("add admin", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(HogwartsCommand.CommandType.AddAdmin);
            }
            return null;
        }

        private static HogwartsCommand IsResetScoreCommand(string text)
        {
            if (text.StartsWith("reset score", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(HogwartsCommand.CommandType.ResetScore);
            }
            return null;
        }

        private static HogwartsCommand IsChangeHouseCommand(string text)
        {
            if (text.StartsWith("change house", StringComparison.InvariantCultureIgnoreCase))
            {
                var match = Regex.Match(text, @"-to \d+");
                if (!match.Success)
                {
                    return null;
                }

                if (int.TryParse(match.Value.Replace("-to ", string.Empty), out int houseId)
                    && houseId >= 0 && houseId < Enum.GetNames(typeof(HogwartsHouse)).Length)
                {
                    var cParams = new HogwartsCommand.CommandParams()
                    {
                        House = (HogwartsHouse)houseId
                    };
                    return new HogwartsCommand(HogwartsCommand.CommandType.ChangeHouse, cParams);
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

        private static HogwartsCommand IsBludgerCommand(string text)
        {
            if (text.StartsWith("бладжер", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(HogwartsCommand.CommandType.Bludger);
            }
            return null;
        }

        private static HogwartsCommand IsDodgeCommand(string text)
        {
            if (text.StartsWith("увернуться", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HogwartsCommand(HogwartsCommand.CommandType.Dodge);
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

    #endregion
}