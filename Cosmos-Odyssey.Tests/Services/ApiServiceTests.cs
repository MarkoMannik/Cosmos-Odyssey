using Cosmos_Odyssey.Helpers;
using Cosmos_Odyssey.Services;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cosmos_Odyssey.Tests.Services
{
    public class ApiServiceTests
    {
        private readonly IOptions<AppSettings> _options;

        public ApiServiceTests()
        {
            _options = Options.Create(new AppSettings
            {
                ApiUrl = "http://apiTestUrl",
                DatabaseUpdateIntervalSeconds = 10,
                MaxPriceListCount = 15
            });
        }

        [Test]
        public async Task GetPriceListAsync()
        {
            var json = "{\"id\":\"292f02b6-2cf8-4a5c-91b9-fd84995af119\",\"validUntil\":\"2020-12-30T10:59:50.7572304Z\",\"legs\":[{\"id\":\"124730d6-a22f-400d-ad15-3040c8e9e2ba\",\"routeInfo\":{\"id\":\"ac599e87-72fe-4807-ba6c-f33f78d29255\",\"from\":{\"id\":\"36ba7004-4e86-4c20-b415-ea2d0134146b\",\"name\":\"Earth\"},\"to\":{\"id\":\"7fdd9282-a9c5-472f-ac0b-de70b699be16\",\"name\":\"Jupiter\"},\"distance\":628730000},\"providers\":[{\"id\":\"b03077f7-7d73-467c-9989-9d1e5fd3305f\",\"company\":{\"id\":\"020c10e5-fc25-4ef6-bea3-c594ee12bb69\",\"name\":\"SpaceX\"},\"price\":441439.58,\"flightStart\":\"2021-01-05T12:28:50.7572484Z\",\"flightEnd\":\"2021-01-09T00:24:50.7572484Z\"},{\"id\":\"2547147e-93ac-4945-bf32-ac9e553bd46c\",\"company\":{\"id\":\"89de5fbb-87d9-4f62-abbe-6ccbb06541c0\",\"name\":\"Explore Dynamite\"},\"price\":266585.47,\"flightStart\":\"2021-01-08T10:53:50.7572765Z\",\"flightEnd\":\"2021-01-13T20:46:50.7572765Z\"},{\"id\":\"45807d6e-1720-4917-b068-10e0da1ffe3b\",\"company\":{\"id\":\"bd8e4a38-ec88-45f7-ad98-f73a30a5ef4d\",\"name\":\"Space Voyager\"},\"price\":223775.94,\"flightStart\":\"2021-01-10T06:30:50.757303Z\",\"flightEnd\":\"2021-01-15T11:30:50.757303Z\"},{\"id\":\"f3d95024-6793-411b-b31d-0b546197b5bf\",\"company\":{\"id\":\"b5d98c05-faf9-42de-90a0-2993badc9da5\",\"name\":\"Spacegenix\"},\"price\":61168.50,\"flightStart\":\"2021-01-01T18:33:50.7573323Z\",\"flightEnd\":\"2021-01-06T04:32:50.7573323Z\"},{\"id\":\"5e745e2d-b3a7-4434-a323-bef233c8acd2\",\"company\":{\"id\":\"f9d75821-054d-4cbc-9987-cdbc1545606a\",\"name\":\"Travel Nova\"},\"price\":207093.60,\"flightStart\":\"2020-12-30T18:25:50.7573581Z\",\"flightEnd\":\"2021-01-02T04:05:50.7573581Z\"},{\"id\":\"a3628f92-3fa3-466d-a7bc-44ece62510c5\",\"company\":{\"id\":\"7812687d-6795-4442-a7ce-889f763c17b3\",\"name\":\"Explore Origin\"},\"price\":412932.20,\"flightStart\":\"2020-12-31T04:53:50.757384Z\",\"flightEnd\":\"2021-01-01T20:42:50.757384Z\"},{\"id\":\"dc0cf374-ddb6-4bfc-8d5a-455330c5333a\",\"company\":{\"id\":\"89de5fbb-87d9-4f62-abbe-6ccbb06541c0\",\"name\":\"Explore Dynamite\"},\"price\":107423.91,\"flightStart\":\"2021-01-05T16:40:50.7574112Z\",\"flightEnd\":\"2021-01-10T04:29:50.7574112Z\"},{\"id\":\"da63ff84-97e9-49d9-b05a-2c298ba17645\",\"company\":{\"id\":\"f8e6f90e-27c5-4afa-adf1-a0086149d794\",\"name\":\"Galaxy Express\"},\"price\":99258.08,\"flightStart\":\"2021-01-13T18:48:50.7574369Z\",\"flightEnd\":\"2021-01-19T14:53:50.7574369Z\"},{\"id\":\"f05aa58e-dda2-439d-bb28-5ab589812aa2\",\"company\":{\"id\":\"f9d75821-054d-4cbc-9987-cdbc1545606a\",\"name\":\"Travel Nova\"},\"price\":248367.57,\"flightStart\":\"2020-12-31T00:16:50.757463Z\",\"flightEnd\":\"2021-01-04T22:19:50.757463Z\"},{\"id\":\"28d82b2d-31d1-44de-9242-acdfe1e33ac0\",\"company\":{\"id\":\"bd8e4a38-ec88-45f7-ad98-f73a30a5ef4d\",\"name\":\"Space Voyager\"},\"price\":455948.80,\"flightStart\":\"2021-01-14T07:27:50.7574888Z\",\"flightEnd\":\"2021-01-16T21:52:50.7574888Z\"},{\"id\":\"d327732d-079e-4056-8bda-252100f9dfb5\",\"company\":{\"id\":\"f8e6f90e-27c5-4afa-adf1-a0086149d794\",\"name\":\"Galaxy Express\"},\"price\":51201.62,\"flightStart\":\"2020-12-31T20:39:50.7575175Z\",\"flightEnd\":\"2021-01-06T15:20:50.7575175Z\"},{\"id\":\"a6eef2f2-b0bd-4b80-91af-62bc4a349889\",\"company\":{\"id\":\"7812687d-6795-4442-a7ce-889f763c17b3\",\"name\":\"Explore Origin\"},\"price\":570972.96,\"flightStart\":\"2021-01-11T16:51:50.7575428Z\",\"flightEnd\":\"2021-01-16T00:14:50.7575428Z\"},{\"id\":\"b748df73-b80e-4fc9-b473-6a2e4f0f0000\",\"company\":{\"id\":\"7812687d-6795-4442-a7ce-889f763c17b3\",\"name\":\"Explore Origin\"},\"price\":404176.65,\"flightStart\":\"2021-01-09T22:34:50.7575679Z\",\"flightEnd\":\"2021-01-14T23:26:50.7575679Z\"},{\"id\":\"30a7bb56-73a8-4517-a097-b9a95ea4a151\",\"company\":{\"id\":\"89de5fbb-87d9-4f62-abbe-6ccbb06541c0\",\"name\":\"Explore Dynamite\"},\"price\":60825.33,\"flightStart\":\"2021-01-04T09:15:50.7575952Z\",\"flightEnd\":\"2021-01-07T04:57:50.7575952Z\"},{\"id\":\"6a4a2f68-3b4b-4352-add7-82da47a10a20\",\"company\":{\"id\":\"18098c29-abb7-4773-8c15-b1566566cb87\",\"name\":\"Space Odyssey\"},\"price\":151307.28,\"flightStart\":\"2021-01-13T04:15:50.7596114Z\",\"flightEnd\":\"2021-01-15T01:51:50.7596114Z\"}]}]}";

            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json),
            };

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new ApiService(_options, httpClient);

            var result = await service.GetPriceListAsync();

            Assert.NotNull(result);
            Assert.AreEqual(result.Id, "292f02b6-2cf8-4a5c-91b9-fd84995af119");
            Assert.AreEqual(result.ValidUntil.ToLocalTime(), Convert.ToDateTime("2020-12-30T10:59:50.7572304Z"));
            Assert.AreEqual(result.Legs.Count, 1);
            Assert.AreEqual(result.Legs.First().Id, "124730d6-a22f-400d-ad15-3040c8e9e2ba");
            Assert.AreEqual(result.Legs.First().RouteInfo.Id, "ac599e87-72fe-4807-ba6c-f33f78d29255");
            Assert.AreEqual(result.Legs.First().Providers.Count, 15);
            Assert.AreEqual(result.Legs.First().Providers.First().Id, "b03077f7-7d73-467c-9989-9d1e5fd3305f");

            //TODO: test more fields or use something universal...
        }

        [Test]
        public async Task GetPriceListAsync_ReturnsNull()
        {
            var json = "The service is unavailable.";

            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json),
            };

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new ApiService(_options, httpClient);

            var result = await service.GetPriceListAsync();

            Assert.Null(result);
        }
    }
}
