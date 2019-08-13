namespace CookWithMe.Web.Filters
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services.Data.Users;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class AuthorizeRootUserFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IUserService userService;

        public AuthorizeRootUserFilterAttribute(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await this.userService.GetByIdAsync(userId);

            if (user.UserName != GlobalConstants.RootUsername)
            {
                context.Result = new ForbidResult();
            }
            else
            {
                await next();
            }
        }
    }
}
