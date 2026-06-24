namespace Services.ViewModels._UserViewModels
{
    public class UserItem
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid RoleId { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

        public bool IsSeedAdminUser
        {
            get
            {
                return this.Email == "admin@newspaper.com";
            }
        }
    }
}
