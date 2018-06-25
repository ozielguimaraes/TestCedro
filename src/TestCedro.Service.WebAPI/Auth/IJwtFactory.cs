using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestCedro.Service.WebAPI.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, Guid id);
    }
}