using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoItList.Data.Entities
{
    public class TaskItem
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to User
public long UserId { get; set; }

        // Descripción (opcional)
        public string? Description { get; set; }

        // Prioridad: LOW, MEDIUM o HIGH
        public string Priority { get; set; } = "LOW";

        // Fecha límite (opcional)
        public DateTime? DueDate { get; set; }

        // Estado de finalización
        [NotMapped]
        public bool Completed { get => IsCompleted; set => IsCompleted = value; }

        // Fecha de última modificación
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
