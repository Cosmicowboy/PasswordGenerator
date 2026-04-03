using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PasswordGenerator.Services;

public class PasswordService : IPasswordService
{
    private const string _lowerPool = "abcdefghijklmnopqrstuvwxyz";
    private const string _upperPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string _numberPool = "0123456789";
    private const string _specialPool = "!@#$%^&*()-_=+[]{}|;:,.<>?";
    private readonly List<string> _stringPools = [_lowerPool, _upperPool, _numberPool, _specialPool];
    private int _lastPoolSelection;
    private IPasswordBank _passwordBank;
    private static readonly string _storePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PasswordGenerator", "passwords.bin");
    private static readonly byte[] _entropy =
        Encoding.UTF8.GetBytes("7623C1309BFB21DF8F0B41ACC272657D4FD5D2271EBAED4BABDA127CE77F2A5D"); //randomly generated please generate your own before use

    public PasswordService(IPasswordBank passwordBank)
    {
        _passwordBank = passwordBank;
    }
    /// <summary>
    /// Generate Random Password with minimum 8 char in length
    /// </summary>
    /// <param name="minLength"></param>
    /// <returns></returns>
    public string GeneratePassword(byte minLength)
    {

        var sBuilder = new StringBuilder();

        var poolNum = RandomNumberGenerator.GetInt32(_stringPools.Count);

        for (byte i = 0; i < minLength; i++)
        {
            //Increase randomness by generating from a different pool
            while (poolNum == _lastPoolSelection)
            {
                poolNum = RandomNumberGenerator.GetInt32(_stringPools.Count);
            }
            _lastPoolSelection = poolNum;

            var drawingPool = _stringPools[poolNum];

            var currentChar = drawingPool[RandomNumberGenerator.GetInt32(drawingPool.Length)];

            sBuilder.Append(currentChar);

        }

        return sBuilder.ToString();
    }


    public string GetStoredPassword(string siteIdentifier)
    {

        return _passwordBank.GetPassword(siteIdentifier);
    }
    public void StorePassword(string site, string password)
    {
        _passwordBank.SavePassword(site, password);
    }
    public void DeleteStoredPassword(string siteIdentifier)
    {
        _passwordBank.DeletePassword(siteIdentifier);
    }


    public void SavePasswords()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_storePath)!);

        var jsonPasswordsString = _passwordBank.Serialize();

        byte[] data = Encoding.UTF8.GetBytes(jsonPasswordsString);

        byte[] encrypted = ProtectedData.Protect(
            data,
            _entropy,
            DataProtectionScope.CurrentUser);

        File.WriteAllBytes(_storePath, encrypted);
    }
    /// <summary>
    /// Decrytps data from local file storing passwords and fills password bank
    /// </summary>
    /// <param name="entropy"></param>
    /// <param name="scope"></param>
    /// <param name="s"></param>
    /// <param name="length"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="IOException"></exception>
    public void DecryptData(byte[] entropy, DataProtectionScope scope, Stream s, int length)
    {
        ArgumentNullException.ThrowIfNull(s);
        ArgumentNullException.ThrowIfNull(entropy);
        if (length <= 0)
            throw new ArgumentException("The given length was 0.", nameof(length));
        if (entropy.Length <= 0)
            throw new ArgumentException("The entropy length was 0.", nameof(entropy));

        byte[] inBuffer = new byte[length];
        byte[] outBuffer;

        if (s.CanRead)
        {
            s.Read(inBuffer, 0, length);

            outBuffer = ProtectedData.Unprotect(inBuffer, entropy, scope);
        }
        else
        {
            throw new IOException("Could not read the stream.");
        }

        var jsonString = Encoding.UTF8.GetString(outBuffer);

        _passwordBank.DeSerialize(jsonString);
    }
}
