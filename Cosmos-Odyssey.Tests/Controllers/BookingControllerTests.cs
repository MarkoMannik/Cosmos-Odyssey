using System.Collections.Generic;
using System.Threading.Tasks;
using Cosmos_Odyssey.Controllers;
using Cosmos_Odyssey.Models;
using Cosmos_Odyssey.Services;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Cosmos_Odyssey.Entities;
using System;

namespace Cosmos_Odyssey.Tests.Controllers
{
    public class BookingControllerTests
    {
        private readonly Mock<IDataService> _dataServiceMock = new Mock<IDataService>();
        private List<string> _planetsFrom;

        [SetUp]
        public void Setup()
        {
            _planetsFrom = new List<string> { "Mercury", "Venus", "Earth", "Mars", "Saturn", "Jupiter", "Uranus", "Neptune" };
        }

        [Test]
        public async Task FromPlanet_ReturnsPlanets()
        {
            _dataServiceMock.Setup(m => m.GetPlanetsFromAsync()).ReturnsAsync(_planetsFrom);
            var controller = new BookingController(_dataServiceMock.Object);
            var result = await controller.FromPlanet() as ViewResult;

            _dataServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BookingViewModel>(result.Model);
            Assert.NotNull((result.Model as BookingViewModel)?.FromPlanets);
            CollectionAssert.AreEqual(_planetsFrom, (result.Model as BookingViewModel)?.FromPlanets.Items);
        }

        [Test]
        public async Task ToPlanets_ReturnsPlanets()
        {
            var planetFrom = "Earth";
            var toPlanets = new List<string> { "Jupiter", "Uranus" };
            _dataServiceMock.Setup(m => m.GetPlanetsToAsync(planetFrom)).ReturnsAsync(toPlanets);
            var controller = new BookingController(_dataServiceMock.Object);
            var result = await controller.ToPlanet(planetFrom) as ViewResult;

            _dataServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BookingViewModel>(result.Model);
            Assert.NotNull((result.Model as BookingViewModel)?.ToPlanets);
            Assert.AreEqual(planetFrom, (result.Model as BookingViewModel)?.FromPlanetName);
            CollectionAssert.AreEqual(toPlanets, (result.Model as BookingViewModel)?.ToPlanets.Items);
        }

        [Test]
        public async Task Route_ReturnsRouteInfoId()
        {
            var from = "Earth";
            var to = "Uranus";
            var routeInfoId = "routeInfoId_1";
            _dataServiceMock.Setup(m => m.GetRouteInfoId(from, to)).ReturnsAsync(routeInfoId);
            var controller = new BookingController(_dataServiceMock.Object);
            var result = await controller.Route(from, to) as ViewResult;

            _dataServiceMock.VerifyAll();

            Assert.IsNull(result);
        }

        [Test]
        public async Task AvailableDeals_ReturnsAvailableDeals()
        {
            var routeInfoId = "routeInfoId_1";
            var providers = new List<Provider>
            {
                new Provider
                {
                    Id = "providerId_1",
                    FlightEnd = DateTime.Now.AddDays(2),
                    FlightStart = DateTime.Now.AddDays(1),
                    CompanyId = "companyId_1",
                    LegId = "legId_1",
                    Price = 10000,
                    Company = new Company
                    {
                        Id = "companyId_1",
                        Name = "Nova Space"
                    },
                    Leg = new Leg
                    {
                        Id = "legId_1",
                        RouteInfo = new RouteInfo
                        {
                            Id = routeInfoId,
                            ToId = "toId_1",
                            To = new To
                            {
                                Id = "toId_1",
                                Name = "Mars"
                            },
                            FromId = "fromId_1",
                            From = new From
                            {
                                Id = "fromId_1",
                                Name = "Jupiter"
                            }
                        }
                    }
                },
                new Provider
                {
                    Id = "providerId_2",
                    FlightEnd = DateTime.Now.AddDays(4),
                    FlightStart = DateTime.Now.AddDays(3),
                    CompanyId = "companyId_1",
                    LegId = "legId_1",
                    Price = 20000
                },
                new Provider
                {
                    Id = "providerId_2",
                    FlightEnd = DateTime.Now.AddDays(2),
                    FlightStart = DateTime.Now.AddDays(1),
                    CompanyId = "companyId_1",
                    LegId = "legId_1",
                    Price = 30000
                }
            };


            _dataServiceMock.Setup(m => m.GetProvidersAsync(routeInfoId, string.Empty)).ReturnsAsync(providers);
            var controller = new BookingController(_dataServiceMock.Object);
            var result = await controller.AvailableDeals(routeInfoId, string.Empty) as ViewResult;

            _dataServiceMock.VerifyAll();

            Assert.IsNotNull((result));
            Assert.IsInstanceOf<BookingViewModel>(result.Model);
            Assert.AreEqual((result.Model as BookingViewModel)?.RouteInfoId, routeInfoId);
            Assert.AreEqual((result.Model as BookingViewModel)?.FromPlanetName, "Jupiter");
            Assert.AreEqual((result.Model as BookingViewModel)?.ToPlanetName, "Mars");
            CollectionAssert.AreEquivalent((result.Model as BookingViewModel)?.Providers, providers);

        }
    }
}
