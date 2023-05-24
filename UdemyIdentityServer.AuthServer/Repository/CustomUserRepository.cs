using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentityServer.AuthServer.Models;

namespace UdemyIdentityServer.AuthServer.Repository
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private readonly CustomDbContext _dbContext;

        public CustomUserRepository(CustomDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomUser> FindByEmail(string email)
        {
            return await _dbContext.CustomUsers.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<CustomUser> FindById(int id)
        {
            return await _dbContext.CustomUsers.FindAsync(id);
        }

        public async Task<bool> Validate(string email, string password)
        {
            return await _dbContext.CustomUsers.AnyAsync(x => x.Email == email && x.Password == password);
        }
    }
}
