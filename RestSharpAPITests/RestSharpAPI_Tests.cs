using RestSharp;
using System.Net;
using System.Text.Json;

namespace RestSharpAPITests
{
    public class RestSharpAPI_Tests
    {
        private RestClient client;
        private const string baseUrl = "https://shorturl.velinski.repl.co/api";

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(baseUrl);
        }

        [Test]
        public void Test_ListAlShortUrls()
        {
            // Arrange
            var request = new RestRequest("urls", Method.Get);

            // Act
            var response = this.client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var urls = JsonSerializer.Deserialize<List<urls>>(response.Content);
            Assert.That(urls, Is.Not.Empty);
        }

        [Test]
        public void Test_FindUrlsByShortCode()
        {
            // Arrange
            var request = new RestRequest("urls/nak", Method.Get);

            // Act
            var response = this.client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var urls = JsonSerializer.Deserialize<urls>(response.Content);
            Assert.That(urls.shortCode, Is.EqualTo("nak"));
            Assert.That(urls.url, Is.EqualTo("https://nakov.com"));

        }

        [Test]

        public void Test_FindInvalid_ShortCode()
        {
            var request = new RestRequest("urls/vinnie", Method.Get);

            var response = this.client.Execute(request);

            var urls = JsonSerializer.Deserialize<urls>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
           

        }

        [Test]

        public void Test_CreateShortUrl()
        {
            var request = new RestRequest("urls", Method.Post);
            var newurl = "url" + DateTime.Now.Ticks;
            var urlBody = new
            {
                url = $"https://{newurl}.com",
                shortCode = newurl
            };

            request.AddBody(urlBody);

            var response = this.client.Execute(request);

            var newUrl = JsonSerializer.Deserialize<newUrl>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(newUrl.msg, Is.EqualTo("Short code added."));

        }

        [Test]

        public void Test_DuplicateShortUrl()
        {
            var request = new RestRequest("urls", Method.Post);
            var reqBody = new
            {
                url = "https://cnn.com",
                shortCode = "cnn8",
            };

            request.AddBody(reqBody);

            var response = this.client.Execute(request);

            var newUrl = JsonSerializer.Deserialize<newUrl>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        }
    }
}