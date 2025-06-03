using FUNewsManagement.BusinessObjects;
using FUNewsManagement.DataAccess.Repositories;

namespace FUNewsManagement.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public async Task<SystemAccount?> GetAccountByIdAsync(short accountId)
        {
            return await _accountRepository.GetByIdAsync(accountId);
        }

        public async Task<SystemAccount?> GetAccountByEmailAsync(string email)
        {
            return await _accountRepository.GetByEmailAsync(email);
        }

        public async Task<SystemAccount?> AuthenticateAsync(string email, string password)
        {
            return await _accountRepository.AuthenticateAsync(email, password);
        }

        public async Task<IEnumerable<SystemAccount>> GetAccountsByRoleAsync(int role)
        {
            return await _accountRepository.GetByRoleAsync(role);
        }

        public async Task<bool> CreateAccountAsync(SystemAccount account)
        {
            try
            {
                if (await _accountRepository.IsEmailExistsAsync(account.AccountEmail))
                {
                    return false; // Email already exists
                }

                await _accountRepository.AddAsync(account);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAccountAsync(SystemAccount account)
        {
            try
            {
                if (await _accountRepository.IsEmailExistsAsync(account.AccountEmail, account.AccountID))
                {
                    return false; // Email already exists for another account
                }

                await _accountRepository.UpdateAsync(account);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAccountAsync(short accountId)
        {
            try
            {
                await _accountRepository.DeleteByIdAsync(accountId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsEmailExistsAsync(string email, short? excludeAccountId = null)
        {
            return await _accountRepository.IsEmailExistsAsync(email, excludeAccountId);
        }

        public async Task<bool> ChangePasswordAsync(short accountId, string newPassword)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(accountId);
                if (account == null) return false;

                account.AccountPassword = newPassword;
                await _accountRepository.UpdateAsync(account);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
