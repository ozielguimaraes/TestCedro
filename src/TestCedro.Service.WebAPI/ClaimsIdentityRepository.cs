using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace TestCedro.Service.WebAPI
{
    public class ClaimsIdentityRepository
    {
        public static void Add(string type, string value)
        {
            if (!(HttpContext.Current.User.Identity is ClaimsIdentity identity)) return;

            Remove(type);

            identity.AddClaim(new Claim(type, value));
        }

        public static void Remove(string type)
        {
            if (!(HttpContext.Current.User.Identity is ClaimsIdentity identity)) return;

            var existingClaim = identity.FindFirst(type);
            if (existingClaim != null)
                identity.RemoveClaim(existingClaim);
        }

        public static string GetByType(string type)
        {
            if (!(HttpContext.Current.User.Identity is ClaimsIdentity identity)) return string.Empty;

            var byType = identity.Claims.FirstOrDefault(c => c.Type == type);
            return byType != null ? byType.Value : string.Empty;
        }

        public static bool IsInRole(string role)
        {
            return HttpContext.Current.User.IsInRole(role);
        }

        public static List<Claim> GetAll()
        {
            if (!(HttpContext.Current.User.Identity is ClaimsIdentity identity)) return null;

            return identity.Claims.ToList();
        }

        public static bool IsAuthenticated()
        {
            if (!(HttpContext.Current.User.Identity is ClaimsIdentity identity)) return false;

            return identity.IsAuthenticated;
        }
    }
}