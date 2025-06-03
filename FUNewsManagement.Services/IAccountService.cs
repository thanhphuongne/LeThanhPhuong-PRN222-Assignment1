using FUNewsManagement.BusinessObjects;

namespace FUNewsManagement.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<SystemAccount>> GetAllAccountsAsync();
        Task<SystemAccount?> GetAccountByIdAsync(short accountId);
        Task<SystemAccount?> GetAccountByEmailAsync(string email);
        Task<SystemAccount?> AuthenticateAsync(string email, string password);
        Task<IEnumerable<SystemAccount>> GetAccountsByRoleAsync(int role);
        Task<bool> CreateAccountAsync(SystemAccount account);
        Task<bool> UpdateAccountAsync(SystemAccount account);
        Task<bool> DeleteAccountAsync(short accountId);
        Task<bool> IsEmailExistsAsync(string email, short? excludeAccountId = null);
        Task<bool> ChangePasswordAsync(short accountId, string newPassword);
    }
}
