namespace PasswordGenerator.Services
{
    public interface IPasswordBank
    {
        void DeletePassword(string site);
        void Deserialize(string jsonString);
        string GetPassword(string site);
        void SavePassword(string site, string password);

        string Serialize();
    }
}