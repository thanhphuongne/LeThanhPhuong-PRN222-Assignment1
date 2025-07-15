using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Services;
using LeThanhPhuongMVC.Models;
using FUNewsManagement.BusinessObjects;

namespace LeThanhPhuongMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If user is already logged in, redirect to appropriate dashboard
            if (HttpContext.Session.GetString("UserEmail") != null)
            {
                var role = HttpContext.Session.GetInt32("UserRole");
                return RedirectToDashboard(role ?? 0);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _accountService.AuthenticateAsync(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            // Set session variables
            HttpContext.Session.SetString("UserEmail", user.AccountEmail);
            HttpContext.Session.SetString("UserName", user.AccountName);
            HttpContext.Session.SetInt32("UserRole", user.AccountRole);
            HttpContext.Session.SetInt32("UserId", user.AccountID);

            return RedirectToDashboard(user.AccountRole);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private IActionResult RedirectToDashboard(int role)
        {
            return role switch
            {
                1 => RedirectToAction("Index", "Admin"), // Admin
                2 => RedirectToAction("Index", "Staff"), // Staff
                3 => RedirectToAction("Index", "News"), // Lecturer - can view news
                _ => RedirectToAction("Index", "News") // Default to news view
            };
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _accountService.GetAccountByIdAsync((short)userId.Value);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new AccountViewModel
            {
                AccountId = user.AccountID,
                AccountName = user.AccountName,
                AccountEmail = user.AccountEmail,
                AccountRole = user.AccountRole
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(AccountViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _accountService.GetAccountByIdAsync((short)userId.Value);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Update user information
            user.AccountName = model.AccountName;
            user.AccountEmail = model.AccountEmail;

            // Update password if provided
            if (!string.IsNullOrEmpty(model.AccountPassword))
            {
                user.AccountPassword = model.AccountPassword;
            }

            var result = await _accountService.UpdateAccountAsync(user);
            if (result)
            {
                // Update session
                HttpContext.Session.SetString("UserName", user.AccountName);
                HttpContext.Session.SetString("UserEmail", user.AccountEmail);
                
                TempData["SuccessMessage"] = "Profile updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update profile. Email may already exist.";
            }

            return View(model);
        }
    }
}
