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
            if(string.IsNullOrWhiteSpace(site))
            {
                throw new ArgumentNullException(nameof(site));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            _passwordCache.Add(site, password);
        }

        /// <summary>
        /// deletes password for correlated site
        /// </summary>
        /// <param name="site"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DeletePassword(string site)
        {
            if (string.IsNullOrWhiteSpace(site))
            {
                throw new ArgumentNullException(nameof(site));
            }

            if(!_passwordCache.TryGetValue(site, out var _))
            {
                throw new ArgumentException($"Site: {site} does not exist in bank");
            }

            _passwordCache.Remove(site);
        }
    }
}
