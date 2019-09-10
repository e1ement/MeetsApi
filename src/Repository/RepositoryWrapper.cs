using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly RepositoryContext _context;
        private ValueRepository _value;
        private AuthRepository _auth;
        private UserRepository _user;

        public RepositoryWrapper(RepositoryContext context)
        {
            _context = context;
        }

        public IValueRepository Value => _value ?? (_value = new ValueRepository(_context));
        public IAuthRepository Auth => _auth ?? (_auth = new AuthRepository(_context));
        public IUserRepository User => _user ?? (_user = new UserRepository(_context));
    }
}
