using Microsoft.EntityFrameworkCore;
using DoItList.Data.Entities;

namespace DoItList.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor que recibe las opciones configuradas en Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Aquí le decimos a EF Core qué tablas (DbSet) tenemos
        public DbSet<User> Users => Set<User>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Índice único en Email de User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Relación 1:N: un User puede tener muchas Tasks
            modelBuilder.Entity<TaskItem>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // borrar tareas al borrar usuario
        }
    }
}
