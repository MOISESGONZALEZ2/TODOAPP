using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoItList.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        // Action: "login" or "register"
        public string Action { get; set; } = string.Empty;
    }
}
