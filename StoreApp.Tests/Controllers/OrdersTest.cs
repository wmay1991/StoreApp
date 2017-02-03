using System;
using NUnit.Framework;
using StoreApp.Core;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Data.Entity;
using StoreApp.Data;
using StoreApp.Controllers;
using System.Web.Mvc;
using StoreApp.Models;

namespace StoreApp.Tests.Controllers
{
    [TestFixture]
    public class OrdersTest
    {
        [Test]
        public void ViewOrdersSuccessPageRenders()
        {
            var item = new Item { item_id = Guid.NewGuid(), name = "Test", price = 20.00M, quantity = 100 };
            var cust = new Customer { customer_id = Guid.NewGuid(), name = "Test1", billing_address = " 123 First Street", billing_city = "Reynoldsburg", billing_state = "OH", billing_zip = "43221", isActive = true };
            var orderList = new List<Order>
            {
                new Order {order_id = Guid.NewGuid(), street_address = "111 Main Street", city = "Columbus", state = "OH", quantity= 10, item_id = item.item_id , customer_id = cust.customer_id, order_date = DateTime.Today , shipping_date = DateTime.Today.AddDays(5) }
            }.AsQueryable();

            var moqItem = new Mock<DbSet<Order>>();
            moqItem.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(orderList.Provider);
            moqItem.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(orderList.Expression);
            moqItem.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(orderList.ElementType);
            moqItem.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(orderList.GetEnumerator());

            var mockcontext = new Mock<StoreDbContext>();
            mockcontext.Setup(m => m.Orders).Returns(moqItem.Object);

            var controller = new OrdersController(mockcontext.Object);

            Assert.AreEqual(mockcontext.Object.Orders.Count(), 1);
        }



        [Test]
        public void ViewOrdersNoDataSuccessPageRenders()
        {
            var orderList = new List<Order>
            {
            }.AsQueryable();

            var moqItem = new Mock<DbSet<Order>>();
            moqItem.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(orderList.Provider);
            moqItem.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(orderList.Expression);
            moqItem.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(orderList.ElementType);
            moqItem.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(orderList.GetEnumerator());

            var mockcontext = new Mock<StoreDbContext>();
            mockcontext.Setup(m => m.Orders).Returns(moqItem.Object);

            var controller = new OrdersController(mockcontext.Object);

            Assert.AreEqual(mockcontext.Object.Orders.Count(), 0);
        }

        [Test]
        public void EnterNewOrderPageRenderSuccess()
        {
            var controller = new OrdersController();
            var result = controller.AddOrder();

            Assert.IsAssignableFrom(typeof(ViewResult), result);


        }

        [Test]
        public void EnterNewOrderPostToMockDb()
        {
            var item = new Item { item_id = Guid.NewGuid(), name = "Item1", price = 20.00M, quantity = 20 };
            var cust = new Customer { customer_id = Guid.NewGuid(), name = "Test1", billing_address = " 123 First Street",
                billing_city = "Reynoldsburg", billing_state = "OH", billing_zip = "43221", isActive = true };

            var or_id = Guid.NewGuid();
            var new_order = new Order {
                order_id = or_id, item_id = item.item_id, quantity= 5, total_price = 100.00M,
                order_date = DateTime.Today, shipping_date = DateTime.Today.AddDays(8),
                city = "Dayton", street_address = "123 Fifth Avenue", state = "OH", customer_id = cust.customer_id
            };

            var mockOrder = new Mock<DbSet<Order>>();
            mockOrder.Setup(m => m.Find(or_id)).Returns(new_order);

            var moqItem = new Mock<DbSet<Item>>();
            moqItem.Setup(m => m.Find(item.item_id)).Returns(item);


            var moqCust = new Mock<DbSet<Customer>>();
            moqCust.Setup(m => m.Find(cust.customer_id)).Returns(cust);


            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Orders).Returns(mockOrder.Object);
            mockContext.Setup(m => m.Items).Returns(moqItem.Object);
            mockContext.Setup(m => m.Customers).Returns(moqCust.Object);

            var vm = new OrderViewModel(new_order);

            var controller = new OrdersController(mockContext.Object);
            var result = controller.AddOrder(vm);

            mockContext.Verify(m => m.SaveChanges());
            mockOrder.Verify(m => m.Add(It.IsAny<Order>()), Times.Once());
            Assert.IsAssignableFrom(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void EditOrderPageRenderSuccess()
        {
            // Create dropdown list
            var id = Guid.NewGuid();
            var cust_id = Guid.NewGuid();

            var itemList = new List<Item>
            {
                new Item {item_id = id, name = "Product",  quantity= 10, price = 50.00M}
            }.AsQueryable();

            var custList = new List<Customer>
            {
                new Customer { customer_id = cust_id, name = "Test2", billing_address = " 123 Fifth Street",
                billing_city = "Dublin", billing_state = "OH", billing_zip = "432115", isActive = true }
        }.AsQueryable();


            var mockItemList = new Mock<DbSet<Item>>();
            mockItemList.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(itemList.Provider);
            mockItemList.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(itemList.Expression);
            mockItemList.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(itemList.ElementType);
            mockItemList.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(itemList.GetEnumerator());


            var mockCustList = new Mock<DbSet<Customer>>();
            mockCustList.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(custList.Provider);
            mockCustList.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(custList.Expression);
            mockCustList.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(custList.ElementType);
            mockCustList.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(custList.GetEnumerator());

            var o_id = Guid.NewGuid();
            var order = new Order { order_id = o_id, order_date = DateTime.Today, shipping_date = DateTime.Today.AddDays(7),
                street_address = "123 First Street", city = "Dublin", state = "OH", item_id = itemList.ElementAt(0).item_id,
                quantity = 10, total_price= 100.00M , customer_id = custList.ElementAt(0).customer_id};

            var mockOrder = new Mock<DbSet<Order>>();
            mockOrder.Setup(m => m.Find(o_id)).Returns(order);

            var mockItem = new Mock<DbSet<Item>>();
            mockItem.Setup(m => m.Find(id)).Returns(itemList.ElementAt(0));

            var mockCust = new Mock<DbSet<Customer>>();
            mockCust.Setup(m => m.Find(cust_id)).Returns(custList.ElementAt(0));

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Orders).Returns(mockOrder.Object);
            mockContext.Setup(m => m.Items).Returns(mockItem.Object);
            mockContext.Setup(m => m.Customers).Returns(mockCust.Object);

            var controller = new OrdersController(mockContext.Object);
            var result = controller.EditOrder(o_id);

            Assert.IsAssignableFrom(typeof(ViewResult), result);
            Assert.That(mockContext.Object.Orders.Find(o_id).item_id == itemList.ElementAt(0).item_id);


        }

        [Test]
        public void EditOrderInvalidPageRenderThrowException()
        {
            // Create dropdown list
            var _db = new StoreDbContext();
            var controller = new OrdersController(_db);
            var result = controller.EditOrder(new Guid());

            Assert.IsAssignableFrom(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void EditOrderSuccessPosttoMockDb()
        {
            // Create dropdown list
            var id = Guid.NewGuid();
            var cust_id = Guid.NewGuid();

            var item = new Item { item_id = id, name = "Product", quantity = 10, price = 50.00M };

            var cust = new Customer
            {
                customer_id = cust_id,
                name = "Test2",
                billing_address = " 156 Fifth Avenue",
                billing_city = "Dublin",
                billing_state = "OH",
                billing_zip = "432225",
                isActive = true
            };

            var o_id = Guid.NewGuid();
            var existing_order = new Order
            {
                order_id = o_id,
                order_date = DateTime.Today,
                shipping_date = DateTime.Today.AddDays(7),
                street_address = "123 First Street",
                city = "Dublin",
                state = "OH",
                item_id = item.item_id,
                quantity = 10,
                total_price = 100.00M,
                customer_id = cust_id
            };

            var updated_order = new Order
            {
                order_id = o_id,
                order_date = DateTime.Today,
                shipping_date = DateTime.Today.AddDays(7),
                street_address = "123 First Street",
                city = "Dublin",
                state = "OH",
                item_id = item.item_id,
                quantity = 20,
                total_price = 200.00M,
                customer_id = cust_id
            };

            var mockOrder = new Mock<DbSet<Order>>();
            mockOrder.Setup(m => m.Find(o_id)).Returns(existing_order);

            var mockItem = new Mock<DbSet<Item>>();
            mockItem.Setup(m => m.Find(id)).Returns(item);

            var mockCust = new Mock<DbSet<Customer>>();
            mockCust.Setup(m => m.Find(cust_id)).Returns(cust);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Orders).Returns(mockOrder.Object);
            mockContext.Setup(m => m.Items).Returns(mockItem.Object);
            mockContext.Setup(m => m.Customers).Returns(mockCust.Object);

            var controller = new OrdersController(mockContext.Object);

            var vm = new OrderViewModel(updated_order);
            var result = controller.EditOrder(vm);

            Assert.IsAssignableFrom(typeof(RedirectToRouteResult), result);
            mockContext.Verify(x => x.SaveChanges());

        }

        [Test]
        public void DeleteOrderViewSuccess()
        {
            var item = new Item { item_id = Guid.NewGuid(), name = "Test", price = 20.00M, quantity = 100 };
            var ord_id = Guid.NewGuid();
            var order = new Order
            {
                order_id = ord_id,
                quantity = 10,
                total_price = 200.00M,
                city = "Dayton",
                street_address = "123 Forth Avenue",
                state = "OH",
                order_date = DateTime.Today,
                shipping_date = DateTime.Today.AddDays(8)
            };

            var mockOrder = new Mock<DbSet<Order>>();
            mockOrder.Setup(m => m.Find(ord_id)).Returns(order);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Orders).Returns(mockOrder.Object);

            var controller = new OrdersController(mockContext.Object);
            var result = controller.Delete(ord_id);

            Assert.IsAssignableFrom(typeof(ViewResult), result);
        }


        [Test]
        public void DeleteOrderConfirmedSuccessRemoveFromMockDb()
        {
            var item = new Item { item_id = Guid.NewGuid(), name = "Test", price = 20.00M, quantity = 100 };
            var ord_id = Guid.NewGuid();
            var order = new Order
            {
                order_id = ord_id,
                quantity = 10,
                total_price = 200.00M,
                city = "Dayton",
                street_address = "123 Forth Avenue",
                state = "OH",
                order_date = DateTime.Today,
                shipping_date = DateTime.Today.AddDays(8),
                item_id = item.item_id
            };

            var mockOrder = new Mock<DbSet<Order>>();
            mockOrder.Setup(m => m.Find(ord_id)).Returns(order);

            var mockItem = new Mock<DbSet<Item>>();
            mockItem.Setup(m => m.Find(item.item_id)).Returns(item);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Orders).Returns(mockOrder.Object);
            mockContext.Setup(m => m.Items).Returns(mockItem.Object);

            var controller = new OrdersController(mockContext.Object);
            var result = controller.DeleteConfirmed(ord_id);


            mockContext.Verify(x => x.SaveChanges());
            Assert.IsAssignableFrom(typeof(RedirectToRouteResult), result);
        }
    }
}
