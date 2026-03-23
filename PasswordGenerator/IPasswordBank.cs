namespace PasswordGenerator
{
    public interface IPasswordBank
    {
        void DeletePassword(string site);
        string GetPassword(string site);
        void SavePassword(string site, string password);
    }
}