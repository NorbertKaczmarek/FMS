using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FMS.UnitTests;

public class FakeUserFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimsPrincipal = new ClaimsPrincipal();

        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "389c9463-05d4-4bb9-95c9-ccf0239c21bc"),
                new Claim(ClaimTypes.Email, "test1@test.com"),
                new Claim("FullName", "test1"),
            }));

        context.HttpContext.User = claimsPrincipal;

        await next();
    }
}
