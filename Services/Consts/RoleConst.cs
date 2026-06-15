using Services.Enums;

namespace Services.Consts
{
    public readonly struct RoleConst
    {
        public const string User = "User";
        public const string Admin = "Admin";
        public const string Staff = "Staff";

        public static string GetRoleName(RoleType roleType)
        {
            return roleType switch
            {
                RoleType.User => User,
                RoleType.Admin => Admin,
                RoleType.Staff => Staff,
                _ => throw new ArgumentException("Invalid role type")
            };
        }
    }
}
