namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IValueRepository Value { get; }
        IAuthRepository Auth { get; }
        IUserRepository User { get; }
    }
}
