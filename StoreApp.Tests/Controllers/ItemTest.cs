using System;
using NUnit.Framework;
using StoreApp.Core;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Data.Entity;
using StoreApp.Data;
using StoreApp.Controllers;

namespace StoreApp.Tests.Controllers
{
    [TestFixture]
    public class ItemTest
    {
        [Test]
        public void ViewItemListSuccess()
        {
            var items = new List<Item> { new Item { item_id = Guid.NewGuid(), name = "item1", price = 40.00M},
                new Item { item_id = Guid.NewGuid(), name = "item2", price = 50.00M } }.AsQueryable();

            var mockItems = new Mock<DbSet<Item>>();
            mockItems.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(items.Provider);
            mockItems.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(items.Expression);
            mockItems.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(items.ElementType);
            mockItems.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(items.GetEnumerator());

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(i => i.Items).Returns(mockItems.Object);

            var controller = new ItemsController(mockContext.Object);
            controller.Index();
        }
    }
}
