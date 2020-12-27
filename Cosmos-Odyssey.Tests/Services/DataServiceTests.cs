using System.Collections.Generic;
using System.Threading.Tasks;
using Cosmos_Odyssey.Services;
using NUnit.Framework;
using Cosmos_Odyssey.Data;
using Microsoft.EntityFrameworkCore;
using Cosmos_Odyssey.Entities;
using System;
using System.Linq;

namespace Cosmos_Odyssey.Tests.Services
{
    public class DataServiceTests
    {
        private DatabaseContext _databaseContext;

        public DataServiceTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "CosmosOdyssey_tests")
            .Options;

            _databaseContext = new DatabaseContext(options);

            var priceList = new Pricelist
            {
                Id = "priceListId_1",
                Ready = true,
                ValidUntil = DateTime.Now.AddDays(3)
            };
            _databaseContext.PriceList.Add(priceList);
            _databaseContext.PriceList.Add(new Pricelist
            {
                Id = "priceListId_2",
                Ready = true,
                ValidUntil = DateTime.Now.AddDays(-1)
            });
            _databaseContext.SaveChanges();

            var planetsFrom = new List<From>
            {
                new From { Id = "fromId_1", Name = "Earth" },
                new From { Id = "fromId_2", Name = "Mercury" },
                new From { Id = "fromId_3", Name = "Venus" },
                new From { Id = "fromId_4", Name = "Mars" },
                new From { Id = "fromId_5", Name = "Saturn" },
                new From { Id = "fromId_6", Name = "Jupiter" },
                new From { Id = "fromId_7", Name = "Uranus" },
                new From { Id = "fromId_8", Name = "Neptune" }
            };
            _databaseContext.From.AddRange(planetsFrom);
            _databaseContext.SaveChanges();

            var planetsTo = new List<To>
            {
                new To {Id = "toId_1", Name = "Jupiter"},
                new To { Id = "toId_2", Name = "Uranus"},
                new To { Id = "toId_3", Name = "Venus"}
            };
            _databaseContext.To.AddRange(planetsTo);
            _databaseContext.SaveChanges();

            var routeInfos = new List<RouteInfo>
            {
                new RouteInfo
                {
                    Id = "routeInfoId_1",
                    Distance = 123000,
                    FromId = "fromId_1",
                    ToId = "toId_1"
                },
                new RouteInfo
                {
                    Id = "routeInfoId_2",
                    Distance = 113000,
                    FromId = "fromId_1",
                    ToId = "toId_2"
                },
                new RouteInfo
                {
                    Id = "routeInfoId_3",
                    Distance = 173000,
                    FromId = "fromId_2",
                    ToId = "toId_3"
                }
            };
            _databaseContext.RouteInfo.AddRange(routeInfos);
            _databaseContext.SaveChanges();

            var companies = new List<Company>
            {
                new Company
                {
                    Id = "companyId_1",
                    Name = "SpaceX"
                },
                new Company
                {
                    Id = "companyId_2",
                    Name = "Nasa"
                },
                new Company
                {
                    Id = "companyId_3",
                    Name = "Space Tallink"
                }
            };
            _databaseContext.Company.AddRange(companies);
            _databaseContext.SaveChanges();

            var legs = new List<Leg>
            {
                new Leg
                {
                    Id = "legId_1",
                    PriceListId = priceList.Id,
                    RouteInfoId = "routeInfoId_1"
                },
                new Leg
                {
                    Id = "legId_2",
                    PriceListId = priceList.Id,
                    RouteInfoId = "routeInfoId_2"
                },
                new Leg
                {
                    Id = "legId_3",
                    PriceListId = priceList.Id,
                    RouteInfoId = "routeInfoId_3"
                },
                new Leg
                {
                    Id = "legId_4",
                    PriceListId = priceList.Id,
                    RouteInfoId = "routeInfoId_2"
                },
            };
            _databaseContext.Leg.AddRange(legs);
            _databaseContext.SaveChanges();

            var providers = new List<Provider>
            {
                new Provider
                {
                    Id = "providerId_1",
                    CompanyId = "companyId_1",
                    FlightStart = DateTime.Now.AddDays(1),
                    FlightEnd = DateTime.Now.AddDays(3),
                    LegId = "legId_1",
                    Price = 100
                },
                new Provider
                {
                    Id = "providerId_2",
                    CompanyId = "companyId_2",
                    FlightStart = DateTime.Now.AddDays(3),
                    FlightEnd = DateTime.Now.AddDays(7),
                    LegId = "legId_2",
                    Price = 200,
                },
                new Provider
                {
                    Id = "providerId_3",
                    CompanyId = "companyId_3",
                    FlightStart = DateTime.Now.AddDays(1),
                    FlightEnd = DateTime.Now.AddDays(7),
                    LegId = "legId_3",
                    Price = 300
                },
                new Provider
                {
                    Id = "providerId_4",
                    CompanyId = "companyId_3",
                    FlightStart = DateTime.Now.AddDays(1),
                    FlightEnd = DateTime.Now.AddDays(7),
                    LegId = "legId_4",
                    Price = 400
                }
            };
            _databaseContext.Provider.AddRange(providers);
            _databaseContext.SaveChanges();

            var demandingCustomers = new List<DemandingCustomer>
            {
                new DemandingCustomer
                {
                    FirstName = "Tom",
                    LastName = "Cruise"
                },
                new DemandingCustomer
                {
                    FirstName = "Russel",
                    LastName = "Crowe"
                }
            };
            _databaseContext.DemandingCustomer.AddRange(demandingCustomers);
            _databaseContext.SaveChanges();
        }

        [Test]
        public async Task GetPlanetsFromAsync_ReturnsPlanets()
        {
            var service = new DataService(_databaseContext);
            var results = await service.GetPlanetsFromAsync();
            CollectionAssert.AreEquivalent(results, new List<string> { "Mercury", "Earth" });
        }

        [Test]
        public async Task GetPlanetsToAsync_ReturnsUranusAndJupiter()
        {
            var service = new DataService(_databaseContext);
            var results = await service.GetPlanetsToAsync("Earth");
            CollectionAssert.AreEquivalent(results, new List<string> { "Uranus", "Jupiter" });
        }

        [Test]
        public async Task GetPlanetsToAsync_ReturnsZeroPlanets()
        {
            var service = new DataService(_databaseContext);
            var result = await service.GetPlanetsToAsync("NoExistingPlanet");
            Assert.AreEqual(result.Count, 0);
        }

        [Test]
        public async Task GetRouteInfoId_ReturnsCorrectRouteInfo()
        {
            var service = new DataService(_databaseContext);
            var result = await service.GetRouteInfoId("Mercury", "Venus");
            Assert.AreEqual(result, "routeInfoId_3");
        }

        [Test]
        public async Task GetProvidersAsync_ReturnsCorrectProviders()
        {
            var service = new DataService(_databaseContext);
            var results = await service.GetProvidersAsync("routeInfoId_2", string.Empty);
            CollectionAssert.AreEquivalent(results.Select(x => x.Id).ToList(), new List<string> { "providerId_4", "providerId_2" });
        }

        [Test]
        public async Task GetProvidersAsync_ReturnsSpaceTallinkProvider()
        {
            var service = new DataService(_databaseContext);
            var results = await service.GetProvidersAsync("routeInfoId_2", "Tallink");
            Assert.AreEqual(results.Select(x => x.Company.Name).Single(), "Space Tallink");
        }

        [Test]
        public async Task GetProvidersAsync_ReturnsZeroProviders()
        {
            var service = new DataService(_databaseContext);
            var results = await service.GetProvidersAsync("invalidId", string.Empty);
            Assert.AreEqual(results.Count, 0);
        }

        [Test]
        public async Task GetProviderAsync_ReturnsCorrectProvider()
        {
            var providerId = "providerId_2";
            var service = new DataService(_databaseContext);
            var result = await service.GetProviderAsync(providerId);

            Assert.AreEqual(result.Id, providerId);
            Assert.AreEqual(result.CompanyId, "companyId_2");
            Assert.AreEqual(result.Company.Name, "Nasa");
        }

        [Test]
        public async Task GetProviderAsync_ReturnsNoProvider()
        {
            var service = new DataService(_databaseContext);
            var result = await service.GetProviderAsync("invalidId");

            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateNewReservationAsync_CreatesNewReservation()
        {
            var service = new DataService(_databaseContext);
            await service.CreateNewReservationAsync("Mr", "Bean", "providerId_2");

            Assert.IsTrue(_databaseContext.Reservation.Where(
                x => x.DemandingCustomer.FirstName == "Mr" &&
                x.DemandingCustomer.LastName == "Bean" && x.ProviderId == "providerId_2").Count() == 1);

            //Cleanup:
            _databaseContext.Reservation.Remove(_databaseContext.Reservation.Single(x => x.ProviderId == "providerId_2" && 
            x.DemandingCustomer.FirstName == "Mr" && 
            x.DemandingCustomer.LastName == "Bean"));

            _databaseContext.SaveChanges();
            _databaseContext.DemandingCustomer.Remove(_databaseContext.DemandingCustomer.First(x => x.FirstName == "Mr" &&
            x.LastName == "Bean"));
            _databaseContext.SaveChanges();
        }

        [Test]
        public async Task CreateNewReservationAsync_DoesNotCreateReservationWithInvalidProvider()
        {
            var service = new DataService(_databaseContext);
            await service.CreateNewReservationAsync("John", "Doe", "invalidProviderId");

            Assert.IsTrue(_databaseContext.Reservation.Count(x => x.DemandingCustomer.FirstName == "John" && x.DemandingCustomer.LastName == "Doe") == 0);
        }

        [Test]
        public async Task GetAllDemandingCustomersAsync_ReturnsAllDemandingCustomers()
        {
            var service = new DataService(_databaseContext);
            var results = await service.GetAllDemandingCustomersAsync();

            CollectionAssert.AreEquivalent(results.Select(x => x.FirstName).ToList(), new List<string> { "Tom", "Russel" });
            CollectionAssert.AreEquivalent(results.Select(x => x.LastName).ToList(), new List<string> { "Cruise", "Crowe" });
        }

        [Test]
        public async Task GetAllPricelistsAsync_ReturnsPriceLists()
        {
            var service = new DataService(_databaseContext);
            var results = await service.GetAllPricelistsAsync();
            CollectionAssert.AreEquivalent(results.Select(x => x.Id).ToList(), new List<string> { "priceListId_1", "priceListId_2" });
        }
    }
}
