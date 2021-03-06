using System;
using System.Security.Cryptography;
using System.Text;

namespace TestCedro.Domain.Helpers
{
    public class Token
    {
        private const string _alg = "HmacSHA256";
        private const string _salt = "1D0LObpuz2C6nPEl8PjQ";
        public static string _TestCedro_PRIVATE_KEY = "vgE3KrpkQ5i7mQMOcaIS";
        private const string _TestCedro_USERNAME = "bxuztRpqmOpl8Lq57ALg";
        private const string _TestCedro_PASSWORD = "2YYBo2Y744HiqICmonTF";
        public static int _expirationMinutes = 60;

        /// <summary>
        /// Generates a token to be used in API calls.
        /// The token is generated by hashing a message with a key, using HMAC SHA256.
        /// The message is: username:ip:userAgent:timeStamp
        /// The key is: password:ip:salt
        /// The resulting token is then concatenated with username:timeStamp and the result base64 encoded.
        /// 
        /// API calls may then be validated by:
        /// 1. Base64 decode the string, obtaining the token, username, and timeStamp.
        /// 2. Ensure the timestamp is not expired.
        /// 2. Lookup the user's password from the db (cached).
        /// 3. Hash the username:ip:userAgent:timeStamp with the key of password:salt to compute a token.
        /// 4. Compare the computed token with the one supplied and ensure they match.
        /// </summary>
        public static string GenerateToken(string email, long ticks)
        {
            string hash = string.Join(":", new string[] { email, _TestCedro_USERNAME, ticks.ToString() });
            string hashLeft = "";
            string hashRight = "";

            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] textBytes = Encoding.UTF8.GetBytes(hash);
            Byte[] keyBytes = Encoding.UTF8.GetBytes(GetHashedPassword($"{_TestCedro_PASSWORD}{email}"));

            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                hmac.ComputeHash(textBytes);
                hashLeft = Convert.ToBase64String(hmac.Hash);
                hashRight = string.Join(":", new string[] { email, ticks.ToString() });
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
        }

        /// <summary>
        /// Returns a hashed password + salt, to be used in generating a token.
        /// </summary>
        /// <param name="password">string - user's password</param>
        /// <returns>string - hashed password</returns>
        private static string GetHashedPassword(string password)
        {
            string key = string.Join(":", new string[] { password, _salt });


            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_salt)))
            {
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                return Convert.ToBase64String(hmac.Hash);
            }
        }

        /// <summary>
        /// Checks if a token is valid.
        /// </summary>
        /// <param name="token">string - generated either by GenerateToken() or via client with cryptojs etc.</param>
        /// <param name="ip">string - IP address of client, passed in by RESTAuthenticate attribute on controller.</param>
        /// <param name="userAgent">string - user-agent of client, passed in by RESTAuthenticate attribute on controller.</param>
        /// <returns>bool</returns>
        public static bool IsTokenValid(string token)
        {
            bool result = false;

            try
            {
                // Base64 decode the string, obtaining the token:username:timeStamp.
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));

                // Split the parts.
                string[] parts = key.Split(new char[] { ':' });
                if (parts.Length == 3)
                {
                    // Get the hash message, username, and timestamp.
                    string hash = parts[0];
                    string email = parts[1];
                    long ticks = long.Parse(parts[2]);
                    DateTime timeStamp = new DateTime(ticks);

                    // Ensure the timestamp is valid.
                    bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > _expirationMinutes;
                    if (!expired)
                    {
                        // Hash the message with the key to generate a token.
                        string computedToken = GenerateToken(email, ticks);

                        // Compare the computed token with the one supplied and ensure they match.
                        result = token == computedToken;
                    }
                }
            }
            catch
            {
            }

            return result;
        }
    }
}