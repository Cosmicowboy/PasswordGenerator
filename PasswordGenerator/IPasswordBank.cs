namespace PasswordGenerator
{
    public interface IPasswordBank
    {
        string GetPassword(string site);
        void SavePassword(string site, string password);
    }
}