using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetAllUsersAsync();
        Task<IEnumerable<UserEntity>> GetAllActiveUsersAsync();
        Task<IEnumerable<UserEntity>> GetAllInactiveUsersAsync();
        Task<UserEntity> GetUserByIdAsync(Guid id);
        Task CreateUserAsync(UserEntity user, string password);
        Task UpdateUserAsync(UserEntity dbUser, UserEntity user);
        Task UpdateUserLastActiveDateAsync(UserEntity user);
        Task UpdateUserPasswordAsync(UserEntity dbUser, string password);
        Task DeleteUserAsync(UserEntity user);
        Task DeactivateUserAsync(UserEntity user);
        Task ActivateUserAsync(UserEntity user);
        Task<bool> UserExists(string username);
    }
}
