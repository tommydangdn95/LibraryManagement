using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models._Users
{
    public class AppUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Tên đầy đủ của người dùng
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Đường dẫn ảnh đại diện
        /// </summary>
        public string? ProfileImage { get; set; }

        /// <summary>
        /// Tiểu sử/mô tả ngắn
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// Phòng ban/bộ phận
        /// </summary>
        public string? Department { get; set; }

        /// <summary>
        /// Ngày tạo tài khoản
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Ngày cập nhật cuối cùng
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; } = true;

    }
}
