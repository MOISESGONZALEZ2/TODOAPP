using System;
using System.Collections.Generic;

namespace DoItList.Data.Entities
{
    public class User
    {
        public long Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public List<TaskItem> Tasks { get; set; } = [];

        // Fecha de creación automática
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Fecha de última modificación
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
