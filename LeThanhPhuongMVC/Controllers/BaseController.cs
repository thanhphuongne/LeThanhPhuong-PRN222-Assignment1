using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LeThanhPhuongMVC.Controllers
{
    public class BaseController : Controller
    {
        protected int? CurrentUserId => HttpContext.Session.GetInt32("UserId");
        protected string? CurrentUserEmail => HttpContext.Session.GetString("UserEmail");
        protected string? CurrentUserName => HttpContext.Session.GetString("UserName");
        protected int? CurrentUserRole => HttpContext.Session.GetInt32("UserRole");

        protected bool IsLoggedIn => CurrentUserId.HasValue;
        protected bool IsAdmin => CurrentUserRole == 1;
        protected bool IsStaff => CurrentUserRole == 2;
        protected bool IsLecturer => CurrentUserRole == 3;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsLoggedIn)
            {
                context.Result = RedirectToAction("Login", "Account");
                return;
            }

            ViewBag.CurrentUserName = CurrentUserName;
            ViewBag.CurrentUserRole = CurrentUserRole;
            ViewBag.IsAdmin = IsAdmin;
            ViewBag.IsStaff = IsStaff;
            ViewBag.IsLecturer = IsLecturer;

            base.OnActionExecuting(context);
        }

        protected IActionResult RedirectToUnauthorized()
        {
            TempData["ErrorMessage"] = "You are not authorized to access this page.";
            return RedirectToAction("Index", "News");
        }
    }

    public class AdminOnlyController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.Result != null) return; // Already redirected

            if (!IsAdmin)
            {
                context.Result = RedirectToUnauthorized();
            }
        }
    }

    public class StaffOnlyController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.Result != null) return; // Already redirected

            if (!IsStaff)
            {
                context.Result = RedirectToUnauthorized();
            }
        }
    }

    public class AdminOrStaffController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.Result != null) return; // Already redirected

            if (!IsAdmin && !IsStaff)
            {
                context.Result = RedirectToUnauthorized();
            }
        }
    }
}
