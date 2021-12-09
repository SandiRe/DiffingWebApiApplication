using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DiffingWebApiApplication.Tests
{
    public class DiffingWebApiApplicationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task LeftValue_SendingNullData_ResponseStatusIs400BadRequest()
        {
            await using var application = new DiffingApplication();
            var client = application.CreateClient();

            var putResponse = await client.PutAsJsonAsync("/v1/diff/1/left", new DiffingData(null));
            Assert.AreEqual(putResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task RightValue_SendingNullData_ResponseStatusIs400BadRequest()
        {
            await using var application = new DiffingApplication();
            var client = application.CreateClient();

            var putResponse = await client.PutAsJsonAsync("/v1/diff/1/right", new DiffingData(null));
            Assert.AreEqual(putResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task LeftValue_SendingData_OperationSuccessfull()
        {
            await using var application = new DiffingApplication();
            var client = application.CreateClient();
            
            var putResponse = await client.PutAsJsonAsync("/v1/diff/1/left", new DiffingData("AAAAAA=="));
            Assert.AreEqual(putResponse.StatusCode, HttpStatusCode.Created);

            var getResponse = await client.GetFromJsonAsync<DiffingData>("/v1/diff/1/left");
            Assert.AreEqual(getResponse.Data, "AAAAAA==");
        }

        [Test]
        public async Task RightValue_SendingData_OperationSuccessfull()
        {
            await using var application = new DiffingApplication();
            var client = application.CreateClient();

            var putResponse = await client.PutAsJsonAsync("/v1/diff/1/right", new DiffingData("AQABAQ=="));
            Assert.AreEqual(putResponse.StatusCode, HttpStatusCode.Created);

            var getResponse = await client.GetFromJsonAsync<DiffingData>("/v1/diff/1/right");
            Assert.AreEqual(getResponse.Data, "AQABAQ==");
        }

        [Test]
        public async Task CalculatedDifference_DifferenceIsCalculatedNoLeftValueNoRightValue_ResponseStatusIs404NotFound()
        {
            await using var application = new DiffingApplication();
            var client = application.CreateClient();

            var response = await client.GetAsync("/v1/diff/1");
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task CalculatedDifference_DifferenceIsCalculatedNoLeftValue_ResponseStatusIs404NotFound()
        {
            await using var application = new DiffingApplication();
            var client = application.CreateClient();

            var putResponse = await client.PutAsJsonAsync("/v1/diff/1/right", new DiffingData("AQABAQ=="));
            Assert.AreEqual(putResponse.StatusCode, HttpStatusCode.Created);

            var response = await client.GetAsync("/v1/diff/1");
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task CalculatedDifference_DifferenceIsCalculatedNoRightValue_ResponseStatusIs404NotFound()
        {
            await using var application = new DiffingApplication();
            var client = application.CreateClient();

            var putResponse = await client.PutAsJsonAsync("/v1/diff/1/left", new DiffingData("AAAAAA=="));
            Assert.AreEqual(putResponse.StatusCode, HttpStatusCode.Created);

            var response = await client.GetAsync("/v1/diff/1");
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task CalculatedDifference_DifferenceIsCalculatedLeftValueIsEqualRightValue_CorrectResponseIsReturned()
        {
            await using var application = new DiffingApplication();
            var client = application.CreateClient();

            var putLeftResponse = await client.PutAsJsonAsync("/v1/diff/1/left", new DiffingData("AAAAAA=="));
            Assert.AreEqual(putLeftResponse.StatusCode, HttpStatusCode.Created);

            var putRightResponse = await client.PutAsJsonAsync("/v1/diff/1/right", new DiffingData("AAAAAA=="));
            Assert.AreEqual(putRightResponse.StatusCode, HttpStatusCode.Created);

            var response = await client.GetFromJsonAsync<DiffingResultData>("/v1/diff/1");
            Assert.AreEqual(response.DiffingResult, DiffingResultType.Equals);
            Assert.IsNull(response.Differences);
        }

        [Test]
        public async Task CalculatedDifference_DifferenceIsCalculatedLeftValueContentDoesNotMatchRightValueContent_CorrectResponseIsReturned()
        {
            await using var application = new DiffingApplication();
            var client = application.CreateClient();

            var putLeftResponse = await client.PutAsJsonAsync("/v1/diff/1/left", new DiffingData("AAAAAA=="));
            Assert.AreEqual(putLeftResponse.StatusCode, HttpStatusCode.Created);

            var putRightResponse = await client.PutAsJsonAsync("/v1/diff/1/right", new DiffingData("AQABAQ=="));
            Assert.AreEqual(putRightResponse.StatusCode, HttpStatusCode.Created);

            var response = await client.GetFromJsonAsync<DiffingResultData>("/v1/diff/1");
            Assert.AreEqual(response.DiffingResult, DiffingResultType.ContentDoNotMatch);
            Assert.AreEqual(response.Differences.Count, 2);
            Assert.AreEqual(response.Differences[0].Offset, 0);
            Assert.AreEqual(response.Differences[0].Length, 1);
            Assert.AreEqual(response.Differences[1].Offset, 2);
            Assert.AreEqual(response.Differences[1].Length, 2);
        }

        [Test]
        public async Task CalculatedDifference_DifferenceIsCalculatedLeftValueSizeDoesNotMatchRightValueSize_CorrectResponseIsReturned()
        {
            await using var application = new DiffingApplication();
            var client = application.CreateClient();

            var putLeftResponse = await client.PutAsJsonAsync("/v1/diff/1/left", new DiffingData("AAA="));
            Assert.AreEqual(putLeftResponse.StatusCode, HttpStatusCode.Created);

            var putRightResponse = await client.PutAsJsonAsync("/v1/diff/1/right", new DiffingData("AQABAQ=="));
            Assert.AreEqual(putRightResponse.StatusCode, HttpStatusCode.Created);

            var response = await client.GetFromJsonAsync<DiffingResultData>("/v1/diff/1");
            Assert.AreEqual(response.DiffingResult, DiffingResultType.SizeDoNotMatch);
            Assert.IsNull(response.Differences);
        }
    }
}