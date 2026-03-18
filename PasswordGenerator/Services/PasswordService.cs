using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordGenerator.Services;

public class PasswordService
{
    private const string _lowerPool = "abcdefghijklmnopqrstuvwxyz";
    private const string _upperPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string _numberPool = "0123456789";
    private const string _specialPool = "!@#$%^&*()-_=+[]{}|;:,.<>?";
    public string GeneratePassword(ushort minLength)
    {
        //each pool gets a num 
        //each iteration draw from a pool and record number
        // each draw is also a random num
        var sBuilder = new StringBuilder();
        return password;
    }
    //serialize passwords
    //deserialize passwords
}
