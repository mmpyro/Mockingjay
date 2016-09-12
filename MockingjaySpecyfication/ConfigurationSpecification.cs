using MockingJay.Validation;
using MockingjaySpecyfication.Helpers;
using NUnit.Framework;

namespace MockingjaySpecyfication
{
    [TestFixture]
    public class ConfigurationSpecification
    {
        [Test]
        public void ShouldVerifyThatUriIsCorrect()
        {
            //Given
            IValidator validator = new NullResponseValidator();
            var configBuilder = new ConfigBuilder();
            //When
            bool result = validator.IsValidConfiguration(configBuilder
                                                            .WithUrl("/users")
                                                            .Get()
                                                            .WithResponseCode(201)
                                                            .WithResponseContent("Accepted"));
            //Then
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldVerifyThatConfigurationWithoutResponseIsInvalid()
        {
            //Given
            IValidator validator = new NullResponseValidator();
            var configBuilder = new ConfigBuilder();
            //When
            bool result = validator.IsValidConfiguration(configBuilder
                                                            .Head()
                                                            .WithResponseStructure(null));
            //Then
            Assert.That(result, Is.False);
            Assert.That(validator.ErrorMessage, Is.EqualTo("Return object cannot be null."));
        }

        [Test]
        public void ShouldVerifyThatUriIsValid()
        {
            //Given
            IValidator validator = new UrlValidator();
            var configBuilder = new ConfigBuilder();
            string url = "'http://localhost";
            //When
            bool result = validator.IsValidConfiguration(configBuilder
                                                            .Delete()
                                                            .WithUrl(url));
            //Then
            Assert.That(result, Is.False);
            Assert.That(validator.ErrorMessage, Is.EqualTo($"This: {url} is not valid url."));
        }

        [TestCase(100, true, null)]
        [TestCase(99, false, "This: 99 is not valid. Valid value is between 100 and 511.")]
        [TestCase(511, true, null)]
        [TestCase(512, false, "This: 512 is not valid. Valid value is between 100 and 511.")]
        public void ShouldVerifyThatStatusCodeIsValidHttpStatusCode(int statusCode, bool expectedResult, string errorMessage)
        {
            //Given
            IValidator validator = new StatusValidator();
            var configBuilder = new ConfigBuilder();
            //When
            bool result = validator.IsValidConfiguration(configBuilder
                                                            .Put()
                                                            .WithResponseCode(statusCode));
            //Then
            Assert.That(result, Is.EqualTo(expectedResult));
            Assert.That(validator.ErrorMessage, Is.EqualTo(errorMessage));
        }
        
        [Test]
        public void ShouldValidateConfigurationBasedOnManyConditions()
        {
            //Given
            IValidator validator = new ComplexValidator(new NullResponseValidator(), new UrlValidator(), new StatusValidator());
            var configBuilder = new ConfigBuilder();
            //When
            bool result = validator.IsValidConfiguration(configBuilder
                                                .WithUrl("/users")
                                                .Patch()
                                                .WithResponseCode(512)
                                                .WithResponseContent("Accepted"));
            //Then
            Assert.That(result, Is.False);
            Assert.That(validator.ErrorMessage, Is.EqualTo("This: 512 is not valid. Valid value is between 100 and 511."));
        }
    }
}
