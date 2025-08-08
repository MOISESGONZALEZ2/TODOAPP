using System;

namespace DoItList.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
