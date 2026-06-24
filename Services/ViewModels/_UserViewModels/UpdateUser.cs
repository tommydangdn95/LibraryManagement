namespace Services.ViewModels._UserViewModels
{
    public class UpdateUser
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public string FullName { get; set; }
    }
}
