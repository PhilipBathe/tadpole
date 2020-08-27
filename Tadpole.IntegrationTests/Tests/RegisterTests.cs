using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tadpole.IntegrationTests.Helpers;

namespace Tadpole.IntegrationTests.Tests
{
    [TestFixture]
    public class RegisterTests
    {
        private HttpClient _client;
        private IHtmlFormElement _form;

        private const string validEmail = "happy@path.com";
        private const string validPassword = "Aa1!passwordshhhhh";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var _factory = new IntegrationTestWebApplicationFactory<Tadpole.Web.Startup>();
            _client = _factory.CreateClient();
        }

        [SetUp]
        public async Task SetUp()
        {
            var registerPage = await _client.GetAsync("/");
            var doc = await HtmlHelpers.GetDocumentAsync(registerPage);
            _form = (IHtmlFormElement)doc.QuerySelector("form");
        }

        [Test]
        public async Task ValidRegistrationShouldReturnSuccessMessage()
        {
            // Arrange
            var formDictionary = new Dictionary<string, string>
            {
                ["Input_Email"] = validEmail,
                ["Input_Password"] = validPassword
            };

            // Act
            var responseMessage = await _client.SendAsync(_form, formDictionary);

            // Assert
            responseMessage.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            var resultDoc = await HtmlHelpers.GetDocumentAsync(responseMessage);
            var messageElement = resultDoc.QuerySelector("h2");

            messageElement.Text().ShouldBe("You are now registered!");
        }

        [Test]
        public async Task MissingEmailShouldReturnValidationError()
        {
            // Arrange
            var formDictionary = new Dictionary<string, string>
            {
                ["Input_Email"] = "",
                ["Input_Password"] = validPassword
            };

            // Act
            var responseMessage = await _client.SendAsync(_form, formDictionary);

            // Assert
            responseMessage.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            string id = "Input_Email";
            string label = "Email";

            await inputFailedRequiredValidation(responseMessage, id, label);
        }

        [Test]
        public async Task MissingPasswordShouldReturnValidationError()
        {
            // Arrange
            var formDictionary = new Dictionary<string, string>
            {
                ["Input_Email"] = validEmail,
                ["Input_Password"] = ""
            };

            // Act
            var responseMessage = await _client.SendAsync(_form, formDictionary);

            // Assert
            responseMessage.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            string id = "Input_Password";
            string label = "Password";

            await inputFailedRequiredValidation(responseMessage, id, label);
        }

        //We are using the built in email validation so no need to explore all of the possible bad email address variations
        [Test]
        public async Task InvalidEmailShouldReturnValidationError()
        {
            // Arrange
            var formDictionary = new Dictionary<string, string>
            {
                ["Input_Email"] = "wibble",
                ["Input_Password"] = validPassword
            };

            // Act
            var responseMessage = await _client.SendAsync(_form, formDictionary);

            // Assert
            responseMessage.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            string id = "Input_Email";
            string label = "Email";

            await inputFailedEmailValidation(responseMessage, id, label);
        }

        [Test]
        public async Task ShortPasswordShouldReturnValidationError()
        {
            // Arrange
            var formDictionary = new Dictionary<string, string>
            {
                ["Input_Email"] = validEmail,
                ["Input_Password"] = "Aa1!"
            };

            // Act
            var responseMessage = await _client.SendAsync(_form, formDictionary);

            // Assert
            responseMessage.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            string id = "Input_Password";
            string errorMessage = "Password must be between 12 and 100 characters long.";

            await inputFailedCustomValidation(responseMessage, id, errorMessage);
        }

        [Test]
        public async Task LongPasswordShouldReturnValidationError()
        {
            // Arrange
            var formDictionary = new Dictionary<string, string>
            {
                ["Input_Email"] = validEmail,
                ["Input_Password"] = "Aa1!123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890"
            };

            // Act
            var responseMessage = await _client.SendAsync(_form, formDictionary);

            // Assert
            responseMessage.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            string id = "Input_Password";
            string errorMessage = "Password must be between 12 and 100 characters long.";

            await inputFailedCustomValidation(responseMessage, id, errorMessage);
        }

        #region form validation helper methods

        //move these to a helper class if we ever need to check validation in another form

        private static async Task inputFailedCustomValidation(HttpResponseMessage responseMessage, string id, string errorMessage)
        {
            var resultDoc = await HtmlHelpers.GetDocumentAsync(responseMessage);
            var invalidInputElement = resultDoc.QuerySelector($"#{id}");
            invalidInputElement.ClassList.ShouldContain("input-validation-error");

            var invalidValidationMessageElement = resultDoc.QuerySelector($"#{id} + span");
            invalidValidationMessageElement.Text().ShouldBe(errorMessage);
        }

        private static async Task inputFailedRequiredValidation(HttpResponseMessage responseMessage, string id, string label)
        {
            string errorMessage = $"The {label} field is required.";

            await inputFailedCustomValidation(responseMessage, id, errorMessage);
        }

        private static async Task inputFailedEmailValidation(HttpResponseMessage responseMessage, string id, string label)
        {
            string errorMessage = $"The {label} field is not a valid e-mail address.";

            await inputFailedCustomValidation(responseMessage, id, errorMessage);
        }

        #endregion
    }
}
