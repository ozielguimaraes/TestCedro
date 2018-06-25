
namespace TestCedro.Infra.CrossCutting.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }

            public static class Emails
            {
                public const string Sender = "naorespondi@gmail.com";
            }

            public static class System
            {
                public const string Name = "Test Cedro";
            }
        }
    }
}