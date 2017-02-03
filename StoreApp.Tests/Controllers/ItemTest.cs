using System;
using NUnit.Framework;
using StoreApp.Core;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Data.Entity;
using StoreApp.Data;
using StoreApp.Controllers;
using StoreApp.Models;
using System.Web.Mvc;

namespace StoreApp.Tests.Controllers
{
    [TestFixture]
    public class ItemTest
    {
        [Test]
        public void ViewItemListSuccess()
        {
            var items = new List<Item> { new Item { item_id = Guid.NewGuid(), name = "item1", price = 40.00M, quantity = 15},
                new Item { item_id = Guid.NewGuid(), name = "item2", price = 50.00M , quantity =20 } }.AsQueryable();

            var mockItems = new Mock<DbSet<Item>>();
            mockItems.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(items.Provider);
            mockItems.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(items.Expression);
            mockItems.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(items.ElementType);
            mockItems.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(items.GetEnumerator());

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(i => i.Items).Returns(mockItems.Object);

            var controller = new ItemsController(mockContext.Object);
            controller.Index();

            Assert.AreEqual(mockContext.Object.Items.Count(), 2);
        }



        [Test]
        public void AddItemSuccessViewRenders()
        {

            var _db = new StoreDbContext();
            var controller = new ItemsController(_db);
            var result = controller.AddItem();

            Assert.IsAssignableFrom(typeof(ViewResult), result);

        }

        [Test]
        public void AddItemSuccessPosttoMockDb()
        {
            var item = new Item { item_id = Guid.NewGuid(), name = "item4", price = 40.00M , quantity = 15 };

            var mockItem = new Mock<DbSet<Item>>();
            mockItem.Setup(m => m.Find(item.item_id)).Returns(item);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(i => i.Items).Returns(mockItem.Object);

            var controller = new ItemsController(mockContext.Object);
            var vm = new ItemViewModel(item);
            controller.AddItem(vm);

            mockItem.Verify(m => m.Add(It.IsAny<Item>()), Times.Once());


        }


        [Test]
        public void EdittemSuccessViewRenders()
        {
            var id = Guid.NewGuid();
            var existing_item = new Item { item_id = id, name = "item4", price = 40.00M , quantity = 30};
            //var updated_item = new Item { item_id = id , name = "item4", price = 50.00M };

            var mockItem = new Mock<DbSet<Item>>();
            mockItem.Setup(m => m.Find(id)).Returns(existing_item);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Items).Returns(mockItem.Object);

            var controller = new ItemsController(mockContext.Object);

            var result = controller.Edit(id);

            Assert.IsAssignableFrom(typeof(ViewResult), result);

        }


        [Test]
        public void EdittemInvalidIdThrowException()
        {
            var id = Guid.NewGuid();
            var existing_item = new Item { item_id = id, name = "item4", price = 40.00M , quantity = 50 };
            //var updated_item = new Item { item_id = id , name = "item4", price = 50.00M };

            var mockItem = new Mock<DbSet<Item>>();
            mockItem.Setup(m => m.Find(id)).Returns(existing_item);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Items).Returns(mockItem.Object);

            var controller = new ItemsController(mockContext.Object);

            var result = controller.Edit(Guid.NewGuid());

            Assert.IsAssignableFrom(typeof(HttpNotFoundResult), result);

        }


        [Test]
        public void EdittemSuccessUpdateMockDb()
        {
            var id = Guid.NewGuid();
            var existing_item = new Item { item_id = id, name = "item4", price = 40.00M , quantity = 20 };
            var updated_item = new Item { item_id = id , name = "item4", price = 50.00M , quantity = 20 };


            var vm = new ItemViewModel(updated_item);
            var mockItem = new Mock<DbSet<Item>>();
            mockItem.Setup(m => m.Find(id)).Returns(existing_item);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Items).Returns(mockItem.Object);

            var controller = new ItemsController(mockContext.Object);

            var result = controller.Edit(vm);

            Assert.AreEqual(mockContext.Object.Items.Find(id).price, 50.00M);

        }

        [Test]
        public void DeleteItemSuccessRemovedFromDb()
        {
            var id = new Guid();
            var del_item = new Item { item_id = id, name = "Test Delete", price = 20.00M , quantity = 10};

            var mockItem = new Mock<DbSet<Item>>();
            mockItem.Setup(m => m.Find(id)).Returns(del_item);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Items).Returns(mockItem.Object);

            var controller = new ItemsController(mockContext.Object);
            var result =controller.DeleteItem(id);


            mockContext.Verify(x => x.SaveChanges());
            Assert.IsAssignableFrom(typeof(RedirectToRouteResult), result);

        }
    }
}
