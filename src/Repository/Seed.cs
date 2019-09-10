using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class Seed : ISeed
    {
        private readonly RepositoryContext _context;

        public Seed(RepositoryContext context)
        {
            _context = context;
        }

        public async Task SeedValues()
        {
            if (!await _context.Values.AnyAsync())
            {
                var valuesData = System.IO.File.ReadAllText("SeedData/ValueSeedData.json");
                var values = JsonConvert.DeserializeObject<List<ValueEntity>>(valuesData);
                foreach (var value in values)
                {
                    await _context.Values.AddAsync(value);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedUsers()
        {
            if (!await _context.Users.AnyAsync())
            {
                var usersData = System.IO.File.ReadAllText("SeedData/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<UserEntity>>(usersData);
                foreach (var user in users)
                {
                    PasswordHelper.CreatePasswordHash("password", out var passwordHash, out var passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();

                    await _context.Users.AddAsync(user);
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
