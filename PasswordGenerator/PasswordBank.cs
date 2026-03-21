using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordGenerator
{
    public class PasswordBank : IPasswordBank
    {
        /// <summary>
        /// Password bank Key -> site, Value -> password
        /// </summary>
        private readonly Dictionary<string, string> _passwordCache = new();
        /// <summary>
        /// Returns a string of the password associated with the site or throws arugmetn exception
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GetPassword(string siteName)
        {
            if(!_passwordCache.TryGetValue(siteName, out var passsWord))
            {
                throw new ArgumentException("Site does not exist in bank");
            }

            return passsWord;
        }

        /// <summary>
        /// saves password with correlated site
        /// </summary>
        /// <param name="site"></param>
        /// <param name="password"></param>
        public void SavePassword(string site, string password)
        {
            var success = _passwordCache.TryAdd(site, password);

            if (!success)
            {
                throw new ArgumentException("Site already contains a password, please try to update");
            }
        }
    }
}
