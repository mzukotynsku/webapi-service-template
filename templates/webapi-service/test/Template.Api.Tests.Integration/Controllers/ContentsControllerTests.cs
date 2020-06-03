using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Template.Domain.Responses;
using Xunit;

namespace Template.Api.Tests.Integration
{
    public class ContentsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private WebApplicationFactory<Startup> _factory;

        public ContentsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_should_return_ok_with_content()
        {
            // Arrange
            var id = 1;
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/contents/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<Content>(responseContent);
            content.Value.Should().Be($"value{id}");
        }
    }
}
