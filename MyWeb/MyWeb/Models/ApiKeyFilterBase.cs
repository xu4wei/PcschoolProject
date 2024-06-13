using Microsoft.AspNetCore.Mvc.Filters;

namespace MyWeb.Models
{
    public abstract class ApiKeyFilterBase
    {
        public abstract Task OnAuthorizationAsync(AuthorizationFilterContext context);
    }
}