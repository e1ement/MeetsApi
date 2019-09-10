using System;

namespace Entities.Dto.User
{
    public class UserForDetailedDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public DateTime? Deleted { get; set; }
    }
}
