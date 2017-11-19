using Microsoft.EntityFrameworkCore;
using TodoApi.Core.Domain;

namespace TodoApi.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Todo> TodoItems { get; set; }

    }
}