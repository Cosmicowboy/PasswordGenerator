using System.Text;

namespace PasswordGenerator.Services;

public class PasswordService
{
    private const string _lowerPool = "abcdefghijklmnopqrstuvwxyz";
    private const string _upperPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string _numberPool = "0123456789";
    private const string _specialPool = "!@#$%^&*()-_=+[]{}|;:,.<>?";
    private readonly List<string> _stringPools = [_lowerPool, _upperPool, _numberPool, _specialPool];
    private int _lastPoolSelection;
    
    public string GeneratePassword(int minLength)
    {

        var sBuilder = new StringBuilder();
        var rnd = new Random();

        var poolNum = rnd.Next(_stringPools.Count);

        for (int i = 0; i < minLength; i++)
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
    //serialize passwords
    //deserialize passwords
}
