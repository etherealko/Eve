namespace eth.TestApp.FancyPlugins.HogwartsPlugin
{
    public enum HogwartsCommandType
    {
        AddPoints,
        AssignHouse,
        NewPartronus,
        Patronus,
        Score,
        MembersList,
        Snitch,
        Prihod,
        SetAdmin,
        AddAdmin,
        ResetScore,
        ChangeHouse,
        ChangeConfig,
        Bludger,
        Dodge
    }

    public class HogwartsCommand
    {
        public bool RequireMembership { get; private set; }
        public bool RequireAdminRights { get; private set; }
        public HogwartsCommandType Type { get; private set; }

        public HogwartsCommand(bool requireMembership, bool requireAdminRights, HogwartsCommandType type)
        {
            RequireMembership = RequireMembership;
            RequireAdminRights = requireAdminRights;
            Type = type;
        }
    }

    public class AddPointsCommand : HogwartsCommand
    {
        public int Points { get; private set; }
        public HogwartsHouse House { get; private set; }

        public AddPointsCommand(bool requireMembership, bool requireAdminRights, HogwartsCommandType type, int points, HogwartsHouse house) 
            : base(requireMembership, requireAdminRights, type)
        {
            Points = points;
            House = house;
        }
    }

    public class AdminCommand : HogwartsCommand
    {
        public int Id { get; private set; }

        public AdminCommand(bool requireMembership, bool requireAdminRights, HogwartsCommandType type, int id) 
            : base(requireMembership, requireAdminRights, type)
        {
            Id = id;
        }
    }

    public class ChangeHouseCommand : HogwartsCommand
    {
        public int Id { get; private set; }
        public HogwartsHouse House { get; private set; }

        public ChangeHouseCommand(bool requireMembership, bool requireAdminRights, HogwartsCommandType type, int id, HogwartsHouse house) 
            : base(requireMembership, requireAdminRights, type)
        {
            Id = id;
            House = House;
        }
    }

    public class ChangeConfigCommand : HogwartsCommand
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public ChangeConfigCommand(bool requireMembership, bool requireAdminRights, HogwartsCommandType type, string key, string value)
            : base(requireMembership, requireAdminRights, type)
        {
            Key = key;
            Value = value;
        }
    }

    public class BludgerCommand : HogwartsCommand
    {
        public int? Id { get; private set; }

        public BludgerCommand(bool requireMembership, bool requireAdminRights, HogwartsCommandType type, int? id) 
            : base(requireMembership, requireAdminRights, type)
        {
            Id = id;
        }
    }
}