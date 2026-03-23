using System.Text;

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

    public PasswordService(IPasswordBank passwordBank)
    {
        _passwordBank = passwordBank;
    }

    public string GeneratePassword(byte minLength)
    {

        var sBuilder = new StringBuilder();
        var rnd = new Random();

        var poolNum = rnd.Next(_stringPools.Count);

        for (byte i = 0; i < minLength; i++)
        {
            //random number to get the pool

            while (poolNum == _lastPoolSelection)
            {
                poolNum = rnd.Next(_stringPools.Count);
            }
            _lastPoolSelection = poolNum;

            var drawingPool = _stringPools[poolNum];

            var currentChar = drawingPool[rnd.Next(drawingPool.Length)];

            sBuilder.Append(currentChar);

        }
        // each draw is also a random num

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

    //serialize passwords
    //deserialize passwords
}
