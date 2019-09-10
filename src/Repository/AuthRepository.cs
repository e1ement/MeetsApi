using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class AuthRepository : RepositoryBase<UserEntity>, IAuthRepository
    {
        public AuthRepository(RepositoryContext context) : base(context)
        {

        }

        public async Task<UserEntity> Register(UserEntity user, string password)
        {
            PasswordHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Created = DateTime.Now;
            user.LastActive = DateTime.Now;

            Create(user);
            await SaveAsync();

            return user;
        }

        public async Task<UserEntity> Login(string username, string password)
        {
            var user = await FindByCondition(o => o.Username.Equals(username))
                .DefaultIfEmpty(new UserEntity())
                .SingleAsync();

            //Deleted user cannot log in to the system
            if (user.IsEmptyObject() || user.Deleted != null)
            {
                return null;
            }

            return !PasswordHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? null : user;
        }
    }
}
