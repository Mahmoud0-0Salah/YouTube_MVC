using Microsoft.AspNetCore.Identity;
using outTube.Models;
using outTube.Data;
using Microsoft.EntityFrameworkCore;

namespace outTube.Middlewares
{
    public class BanCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public BanCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<User> userManager, ApplicationDbContext dbContext)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = userManager.GetUserId(context.User);
                if (!string.IsNullOrEmpty(userId))
                {
                    // Check if user is locked out in Identity
                    var user = await userManager.FindByIdAsync(userId);
                    if (user != null && await userManager.IsLockedOutAsync(user))
                    {
                        // Invalidate cookie and logout
                        context.Response.Cookies.Delete("X-Access-Token");
                        context.Response.Redirect("/Account/Banned");
                        return;
                    }

                    // Check if user is in BannedUsers table (redundant but safe)
                    var isBanned = await dbContext.BannedUsers.AnyAsync(bu => bu.UserId == userId);
                    if (isBanned)
                    {
                        context.Response.Cookies.Delete("X-Access-Token");
                        context.Response.Redirect("/Account/Banned");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
