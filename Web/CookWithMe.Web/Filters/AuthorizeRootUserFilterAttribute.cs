namespace CookWithMe.Web.Filters
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Services.Data.Users;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Configuration;

    public class AuthorizeRootUserFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;

        public AuthorizeRootUserFilterAttribute(IConfiguration configuration, IUserService userService)
        {
            this.configuration = configuration;
            this.userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await this.userService.GetByIdAsync(userId);

            if (user.UserName != this.configuration["Root:UserName"])
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
