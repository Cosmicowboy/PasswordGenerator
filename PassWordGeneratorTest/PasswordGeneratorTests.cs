using PasswordGenerator.Services;

namespace PassWordGeneratorTest
{
    public class PasswordGeneratorTests
    {
       private readonly PasswordService _pWordService = new();
        [Fact]
        public void PasswordMinLength()
        {
            var rnd = new Random();
            
            var minLength = (byte)rnd.Next(100);
            var pWord = _pWordService.GeneratePassword(minLength);

            Assert.Equal(minLength, pWord.Length);
        }
        [Fact]
        public void ContainsAtLeastOneOfEachEnabledType()
        {
            var pwd = _pWordService.GeneratePassword(20);

            Assert.Matches("[A-Z]", pwd);          // uppercase
            Assert.Matches("[a-z]", pwd);          // lowercase
            Assert.Matches("[0-9]", pwd);          // digits
            Assert.Matches("[^A-Za-z0-9]", pwd);   // symbols
        }
        [Fact]
        public void TwoConsecutivePasswordsAreDifferent()
        {
            var p1 = _pWordService.GeneratePassword(20);
            var p2 = _pWordService.GeneratePassword(20);

            Assert.NotEqual(p1, p2);
        }

        [Fact]
        public void AdhersToMinimumLength()
        {
            Assert.Equal(8,_pWordService.GeneratePassword(0).Length);
        }
    }
}
