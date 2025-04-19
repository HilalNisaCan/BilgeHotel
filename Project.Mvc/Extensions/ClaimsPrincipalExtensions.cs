using System.Security.Claims;

namespace Project.MvcUI.Extensions
{

    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrEmpty(id) ? 0 : int.Parse(id);
        }
    }
}
