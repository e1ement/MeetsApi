using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Meets.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var userId = Guid.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repository = resultContext.HttpContext.RequestServices.GetService<IRepositoryWrapper>();
            var user = await repository.User.GetUserByIdAsync(userId);

            await repository.User.UpdateUserLastActiveDateAsync(user);
        }
    }
}
