using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class TodoContext : DbContext
    {
        /// <summary>
        /// the constructor for the database
        /// </summary>
        /// <param name="options"></param>
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<TodoList> TodoList { get; set; }
    }
}