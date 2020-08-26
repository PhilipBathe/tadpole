using NUnit.Framework;
using Shouldly;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tadpole.IntegrationTests.Tests
{
    [TestFixture]
    public class BasicUriTests
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var _factory = new IntegrationTestWebApplicationFactory<Tadpole.Web.Startup>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task HomepageShouldReturnSuccessAndCorrectContentType()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            response.Content.Headers.ContentType.ToString().ShouldBe("text/html; charset=utf-8");
        }
    }
}