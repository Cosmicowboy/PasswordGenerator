using PasswordGenerator;

namespace PassWordGeneratorTest
{
    public class PasswordBankTests
    {
        private readonly PasswordBank _passwordBank = new();
        //Correectly adds passwords

        [Fact]
        public void PreventsDuplicatePasswords()
        {
            _passwordBank.SavePassword("test", "test123");
            _passwordBank.SavePassword("test", "test123");
            
        }

        [Fact]
        public void CorrectlyGetsPasswordFromBank()
        {
            _passwordBank.SavePassword("test", "test123");

            Assert.Equal("test123", _passwordBank.GetPassword("test"));
        }

        [Fact]
        public void ThrowsOnNullInput()
        {
            var argNull = new Type();

            Assert.Throws(argNull,() => _passwordBank.SavePassword(string.Empty, "test123"));
        }
    }
}
