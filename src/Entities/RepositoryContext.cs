using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options) { }
        
        public DbSet<ValueEntity> Values { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}
