using Entities.Models;

namespace Entities.Extensions
{
    public static class UserExtensions
    {
        public static void Map(this UserEntity dbUser, UserEntity user)
        {
            dbUser.Name = user.Name;
        }
    }
}
