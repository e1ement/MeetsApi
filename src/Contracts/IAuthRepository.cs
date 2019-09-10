using Entities.Models;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAuthRepository
    {
        Task<UserEntity> Register(UserEntity user, string password);
        Task<UserEntity> Login(string username, string password);
    }
}
