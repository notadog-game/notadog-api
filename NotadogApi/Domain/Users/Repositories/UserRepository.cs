using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Repositories;
using NotadogApi.Domain.Contexts;

namespace NotadogApi.Domain.Users.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User> CreateAsync(string name, string email, string password)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                Password = password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<List<User>> GetAllAsync() => await _context.Users.ToListAsync();
        public async Task<User> GetOneAsync(int id) => await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        public async Task<User> GetOneByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());
        public async Task UpdateOneAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
