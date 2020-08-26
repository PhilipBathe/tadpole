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
                ["Input_Email"] = "happy@path.com",
                ["Input_Password"] = "Aa1!passwordshhhhh"
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
                ["Input_Password"] = "Aa1!passwordshhhhh"
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
                ["Input_Email"] = "happy@path.com",
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

        private static async Task inputFailedRequiredValidation(HttpResponseMessage responseMessage, string id, string label)
        {
            var resultDoc = await HtmlHelpers.GetDocumentAsync(responseMessage);
            var invalidInputElement = resultDoc.QuerySelector($"#{id}");
            invalidInputElement.ClassList.ShouldContain("input-validation-error");

            var invalidValidationMessageElement = resultDoc.QuerySelector($"#{id} + span");
            invalidValidationMessageElement.Text().ShouldBe($"The {label} field is required.");
        }
    }
}
