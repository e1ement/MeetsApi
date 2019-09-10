using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
        {
            return await FindAll().OrderBy(o => o.Name).ToListAsync();
        }

        public async Task<IEnumerable<UserEntity>> GetAllActiveUsersAsync()
        {
            return await FindAll().Where(w => w.Deleted == null).OrderBy(o => o.Name).ToListAsync();
        }

        public async Task<IEnumerable<UserEntity>> GetAllInactiveUsersAsync()
        {
            return await FindAll().Where(w => w.Deleted != null).OrderBy(o => o.Name).ToListAsync();
        }

        public async Task<UserEntity> GetUserByIdAsync(Guid id)
        {
            return await FindByCondition(o => o.Id.Equals(id))
                .DefaultIfEmpty(new UserEntity())
                .SingleAsync();
        }

        public async Task CreateUserAsync(UserEntity user, string password)
        {
            PasswordHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Created = DateTime.Now;
            user.LastActive = DateTime.Now;

            Create(user);
            await SaveAsync();
        }

        public async Task UpdateUserAsync(UserEntity dbUser, UserEntity user)
        {
            dbUser.Map(user);
            Update(dbUser);
            await SaveAsync();
        }

        public async Task UpdateUserLastActiveDateAsync(UserEntity user)
        {
            user.LastActive = DateTime.Now;

            Update(user);
            await SaveAsync();
        }

        public async Task UpdateUserPasswordAsync(UserEntity dbUser, string password)
        {
            PasswordHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            dbUser.PasswordHash = passwordHash;
            dbUser.PasswordSalt = passwordSalt;

            Update(dbUser);
            await SaveAsync();
        }

        public async Task DeleteUserAsync(UserEntity user)
        {
            Delete(user);
            await SaveAsync();
        }

        public async Task DeactivateUserAsync(UserEntity dbUser)
        {
            dbUser.Deleted = DateTime.Now;

            Update(dbUser);
            await SaveAsync();
        }

        public async Task ActivateUserAsync(UserEntity dbUser)
        {
            dbUser.Deleted = null;

            Update(dbUser);
            await SaveAsync();
        }

        public async Task<bool> UserExists(string username)
        {
            return await FindByCondition(o => o.Username.Equals(username)).AnyAsync();
        }
    }
}
