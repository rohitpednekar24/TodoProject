using Microsoft.EntityFrameworkCore;

namespace TodoProject.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Status> Statuses { get; set; } = null!;

        public DbSet<Priority> Priorities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId="work",CategoryName="Work" },
                new Category { CategoryId = "personal", CategoryName = "Personal" },
                new Category { CategoryId = "shop", CategoryName = "Shopping" }
                );

            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId="open",StatusName="Open"},
                new Status { StatusId = "closed", StatusName = "Completed" }
                );

            modelBuilder.Entity<Priority>().HasData(
               new Priority { PriorityId = "high", PriorityOrder = "High" },
               new Priority { PriorityId = "medium", PriorityOrder = "Medium" },
               new Priority { PriorityId = "low", PriorityOrder = "Low" }

               );

        }

    }

   
}
