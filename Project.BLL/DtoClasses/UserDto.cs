using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class UserDto:BaseDto
    {
        [Required, StringLength(50)]
        public string UserName { get; set; } = null!;// Kullanıcının kullanıcı adı

        [Required, StringLength(100)]
        public string Password { get; set; } = null!; // Kullanıcı şifresi

        [Required, EmailAddress]
        public string Email { get; set; } = null!; // Kullanıcının e-posta adresi

        public Guid ActivationCode { get; set; } // Hesap aktivasyon kodu

        public int? RoleId { get; set; } // AppRole FK'si
        public string? RoleName { get; set; } // UI için okunabilir ad (opsiyonel)

        public bool IsActivated { get; set; }
        public UserProfileDto UserProfile { get; set; }
    }
}
