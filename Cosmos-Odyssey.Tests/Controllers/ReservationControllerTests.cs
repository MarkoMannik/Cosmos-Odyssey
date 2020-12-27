using System.Collections.Generic;
using System.Threading.Tasks;
using Cosmos_Odyssey.Controllers;
using Cosmos_Odyssey.Models;
using Cosmos_Odyssey.Services;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Cosmos_Odyssey.Entities;

namespace Cosmos_Odyssey.Tests.Controllers
{
    public class ReservationControllerTests
    {
        private readonly Mock<IDataService> _dataServiceMock = new Mock<IDataService>();

        [Test]
        public async Task CreateReservation()
        {
            var firstName = "John";
            var lastName = "Doe";
            var providerId = "providerId_1";
            _dataServiceMock.Setup(m => m.CreateNewReservationAsync(firstName, lastName, providerId));
            var controller = new ReservationController(_dataServiceMock.Object);
            var result = await controller.CreateReservation(providerId, firstName, lastName) as ViewResult;

            _dataServiceMock.VerifyAll();

            Assert.IsNull(result);
        }

        [Test]
        public async Task Index()
        {
            var demandingCustomers = new List<DemandingCustomer>
            {
                new DemandingCustomer
                {
                    FirstName = "John",
                    LastName = "Rambo"
                },
                new DemandingCustomer
                {
                    FirstName = "Tom",
                    LastName = "Cruise"
                }
            };

            _dataServiceMock.Setup(m => m.GetAllDemandingCustomersAsync()).ReturnsAsync(demandingCustomers);
            var controller = new ReservationController(_dataServiceMock.Object);
            var result = await controller.Index() as ViewResult;

            _dataServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ReservationViewModel>(result.Model);
            Assert.NotNull((result.Model as ReservationViewModel)?.DemandingCustomers);
            CollectionAssert.AreEqual(demandingCustomers, (result.Model as ReservationViewModel)?.DemandingCustomers);
        }
    }
}
