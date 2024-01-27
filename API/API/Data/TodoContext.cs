using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        { }

        public DbSet<Todo> Todos { get; set; }
    }

    public class Todo
    {
        [Key]
        public Guid Id { get; set; } 
        public required string Task { get; set; }
        public bool Done { get; set; }

    }

}
