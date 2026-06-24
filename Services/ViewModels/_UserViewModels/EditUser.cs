using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.ViewModels._UserViewModels
{
    public class EditUser
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public Guid RoleId { get; set; }
        public bool IsActive { get; set; }
        public List<SelectListItem> RoleList { get; set; }
        public EditUser()
        {
            this.RoleList = new List<SelectListItem>();
        }
    }
}
