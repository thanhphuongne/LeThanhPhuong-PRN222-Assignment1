using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Services;
using FUNewsManagement.BusinessObjects;
using LeThanhPhuongMVC.Models;

namespace LeThanhPhuongMVC.Controllers
{
    public class AdminController : AdminOnlyController
    {
        private readonly IAccountService _accountService;
        private readonly INewsArticleService _newsService;

        public AdminController(IAccountService accountService, INewsArticleService newsService)
        {
            _accountService = accountService;
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return View(accounts);
        }

        [HttpGet]
        public async Task<IActionResult> ManageAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            ViewBag.UserId = CurrentUserId;
            return View(accounts);
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var account = new SystemAccount
            {
                AccountName = model.AccountName,
                AccountEmail = model.AccountEmail,
                AccountPassword = model.AccountPassword!,
                AccountRole = model.AccountRole
            };

            var result = await _accountService.CreateAccountAsync(account);
            if (result)
            {
                TempData["SuccessMessage"] = "Account created successfully.";
                return RedirectToAction("ManageAccounts");
            }

            ModelState.AddModelError("", "Failed to create account. Email may already exist.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAccount(short id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            var model = new AccountViewModel
            {
                AccountId = account.AccountID,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var account = await _accountService.GetAccountByIdAsync(model.AccountId);
            if (account == null)
            {
                return NotFound();
            }

            account.AccountName = model.AccountName;
            account.AccountEmail = model.AccountEmail;
            account.AccountRole = model.AccountRole;

            if (!string.IsNullOrEmpty(model.AccountPassword))
            {
                account.AccountPassword = model.AccountPassword;
            }

            var result = await _accountService.UpdateAccountAsync(account);
            if (result)
            {
                TempData["SuccessMessage"] = "Account updated successfully.";
                return RedirectToAction("ManageAccounts");
            }

            ModelState.AddModelError("", "Failed to update account. Email may already exist.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(short id)
        {
            var result = await _accountService.DeleteAccountAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Account deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete account.";
            }

            return RedirectToAction("ManageAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> Reports(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            if (startDate.HasValue && endDate.HasValue)
            {
                var news = await _newsService.GetNewsByDateRangeAsync(startDate.Value, endDate.Value);
                return View(news.OrderByDescending(n => n.CreatedDate));
            }

            return View(new List<NewsArticle>());
        }
    }
}
