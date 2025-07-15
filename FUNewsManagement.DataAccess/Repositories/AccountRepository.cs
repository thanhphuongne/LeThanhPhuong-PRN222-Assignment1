using FUNewsManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class AccountRepository : Repository<SystemAccount>, IAccountRepository
    {
        public AccountRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<SystemAccount?> AuthenticateAsync(string email, string password)
        {
            Console.WriteLine($"Repository - Email: '{email}' (Length: {email.Length})");
            Console.WriteLine($"Repository - Password: '{password}' (Length: {password.Length})");

            // First, let's find the user by email only
            var userByEmail = await _dbSet.FirstOrDefaultAsync(a => a.AccountEmail == email);
            if (userByEmail != null)
            {
                Console.WriteLine($"Found user by email - Stored password: '{userByEmail.AccountPassword}' (Length: {userByEmail.AccountPassword.Length})");
                Console.WriteLine($"Password match: {userByEmail.AccountPassword == password}");
            }
            else
            {
                Console.WriteLine("No user found with that email");
            }

            return await _dbSet.FirstOrDefaultAsync(a => a.AccountEmail == email && a.AccountPassword == password);
        }

        public async Task<IEnumerable<SystemAccount>> GetByRoleAsync(int role)
        {
            return await _dbSet.Where(a => a.AccountRole == role).ToListAsync();
        }

        public async Task<bool> IsEmailExistsAsync(string email, short? excludeAccountId = null)
        {
            var query = _dbSet.Where(a => a.AccountEmail == email);
            if (excludeAccountId.HasValue)
            {
                query = query.Where(a => a.AccountID != excludeAccountId.Value);
            }
            return await query.AnyAsync();
        }
    }
}
