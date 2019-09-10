using System.Threading.Tasks;

namespace Contracts
{
    public interface ISeed
    {
        Task SeedValues();
        Task SeedUsers();
    }
}
