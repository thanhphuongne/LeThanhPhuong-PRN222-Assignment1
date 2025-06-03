using FUNewsManagement.BusinessObjects;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface IAccountRepository : IRepository<SystemAccount>
    {
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<SystemAccount?> AuthenticateAsync(string email, string password);
        Task<IEnumerable<SystemAccount>> GetByRoleAsync(int role);
        Task<bool> IsEmailExistsAsync(string email, short? excludeAccountId = null);
    }
}
