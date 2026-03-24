using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordGenerator.Services;

public interface IPasswordService
{
    void DeleteStoredPassword(string siteIdentifier);
    string GeneratePassword(byte minLength = 8);
    string GetStoredPassword(string siteIdentifier);
    void StorePassword(string site, string passWord);
}
