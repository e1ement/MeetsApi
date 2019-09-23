using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public sealed class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreatedAsync();
        }
        
        public DbSet<ValueEntity> Values { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}
