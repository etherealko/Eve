namespace eth.TestApp.FancyPlugins.HogwartsPlugin
{
    public class HogwartsCommand
    {
        public enum CommandType
        {
            AddPoints,
            AssignHouse,
            NewPartronus,
            Patronus,
            Score,
            MembersList,
            Snitch,
            Prihod,
            AddAdmin,
            ResetScore,
            ChangeHouse,
            Bludger,
            Dodge
        }

        public class CommandParams
        {
            public int Points { get; set; }
            public HogwartsHouse House { get; set; }
        }
        
        public CommandType Type { get; private set; }
        public CommandParams Params { get; private set; }

        public HogwartsCommand(CommandType type, CommandParams cParams = null)
        {
            Type = type;
            Params = cParams;
        }
    }

    public static class CommandExtensions
    {
        public static bool RequireMembership(this HogwartsCommand command)
        {
            return command.Type != HogwartsCommand.CommandType.AssignHouse
                || command.Type != HogwartsCommand.CommandType.Score;
        }

        public static bool RequireAdminRights(this HogwartsCommand command)
        {
            return command.Type == HogwartsCommand.CommandType.AddAdmin
                || command.Type == HogwartsCommand.CommandType.ResetScore
                || command.Type == HogwartsCommand.CommandType.ChangeHouse;
        }
    }
}