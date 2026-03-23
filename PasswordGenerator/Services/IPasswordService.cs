using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordGenerator.Services;

public interface IPasswordService
{
    string GeneratePassword(byte minLength);
    string GetStoredPassword(string siteIdentifier);
    void StorePassword(string site, string passWord);
}
