using PasswordGenerator.Services;
using Xunit;

namespace PassWordGenerationTests
{
    public class PassWordTestClass
    {
        [Fact]
        public void PasswordMinLengthGood()
        {
            var rnd = new Random();
            var pWordService = new PasswordService();
            var minLength = rnd.Next(100);
            var pWord = pWordService.GeneratePassword(minLength);

            Assert.Equal(minLength, pWord.Length);
        }
    }
}
