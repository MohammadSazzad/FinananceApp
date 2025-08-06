using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Controllers
{
    public class BaseController : Controller
    {
        protected string? GetCurrentUsername()
        {
            return HttpContext.Session.GetString("Username");
        }

        protected int? GetCurrentUserId()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            return null;
        }

        protected bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")) && 
                   !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));
        }

        protected string? GetCurrentUserEmail()
        {
            return HttpContext.Session.GetString("UserEmail");
        }

        protected string? GetCurrentUserRole()
        {
            return HttpContext.Session.GetString("UserRole");
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            ViewBag.IsLoggedIn = IsUserLoggedIn();
            ViewBag.CurrentUsername = GetCurrentUsername();
            ViewBag.CurrentUserId = GetCurrentUserId();
            ViewBag.CurrentUserEmail = GetCurrentUserEmail();
            ViewBag.CurrentUserRole = GetCurrentUserRole();
            base.OnActionExecuting(context);
        }
    }
}
