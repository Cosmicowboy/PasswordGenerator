using PasswordGenerator;

namespace PassWordGeneratorTest
{
    public class PasswordBankTests
    {
        private readonly PasswordBank _passwordBank = new();


        [Fact]
        public void CorrectlyGetsPasswordFromBank()
        {
            
            _passwordBank.SavePassword("test", "test123");

            Assert.Equal("test123", _passwordBank.GetPassword("test"));
        }

        [Fact]
        public void ThrowsOnNullInput()
        {
            Assert.Throws<ArgumentNullException>(() => 
                _passwordBank.SavePassword(string.Empty, "test123"));
        }
    }
}
