using Microsoft.EntityFrameworkCore;

namespace Models
{

    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
            
        }

        public DbSet<Todo> Todos => Set<Todo>();
    }
}