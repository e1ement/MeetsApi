using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("user")]
    public class UserEntity : IEntity
    {
        [Key]
        [Column("UserId")]
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public DateTime? Deleted { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}
