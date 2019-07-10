using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotadogApi.Domain.Models;
using NotadogApi.Domain.Repositories;
using NotadogApi.Persistence.Contexts;

namespace NotadogApi.Persistence.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();
        public async Task<User> GetOneAsync(int id) => await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        public async Task<User> GetOneByEmailAsync(string email) => await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }
}
