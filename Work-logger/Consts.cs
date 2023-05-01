namespace WorkLogger
{
    public static class Consts
    {
        public const string AdminRole = "Admin";
        public const string ModeratorRole = "Moderator";
        public const string UserRole = "User";

        public static List<string> RolesList => new() { AdminRole, ModeratorRole, UserRole };
    }
}
