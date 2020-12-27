using System.Collections.Generic;
using System.Threading.Tasks;
using Cosmos_Odyssey.Controllers;
using Cosmos_Odyssey.Services;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Cosmos_Odyssey.Entities;
using System;

namespace Cosmos_Odyssey.Tests.Controllers
{
    public class PricelistControllerTests
    {
        private readonly Mock<IDataService> _dataServiceMock = new Mock<IDataService>();

        [Test]
        public async Task Index()
        {
            var pricelists = new List<Pricelist>
            {
                new Pricelist
                {
                   Id = "priceListId_1",
                   ValidUntil = DateTime.Now.AddDays(1),
                   Ready = true
                },
                new Pricelist
                {
                   Id = "priceListId_2",
                   ValidUntil = DateTime.Now.AddDays(-2),
                   Ready = true
                }
            };

            _dataServiceMock.Setup(m => m.GetAllPricelistsAsync()).ReturnsAsync(pricelists);
            var controller = new PricelistController(_dataServiceMock.Object);
            var result = await controller.Index() as ViewResult;

            _dataServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Pricelist>>(result.Model);
            CollectionAssert.AreEqual(pricelists, result.Model as List<Pricelist>);
        }
    }
}
