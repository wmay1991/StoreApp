using System;
using NUnit.Framework;
using StoreApp.Controllers;
using System.Web.Mvc;
using StoreApp.Data;
using StoreApp.Core;
using Moq;
using System.Data.Entity;
using StoreApp.Models;

namespace StoreApp.Tests.Controllers
{
    [TestFixture]
    public class CustomersTests
    {
        [Test]
        public void ReturnCustomersViewRenders()
        {
            var db = new StoreDbContext();
            var controller = new CustomersController(db);
            var result = controller.Index();

            Assert.IsAssignableFrom(typeof(ViewResult), result);
        }


        [Test]
        public void AddCustomersSuccessViewRenders()
        {
            var db = new StoreDbContext();
            var controller = new CustomersController(db);
            var result = controller.Add();

            Assert.IsAssignableFrom(typeof(ViewResult), result);
        }


        [Test]
        public void AddCustomersSuccessPosttoMockDb()
        {
            var cust_id = Guid.NewGuid();
            var cust = new Customer
            {
                customer_id = cust_id,
                name = "Test1",
                billing_address = " 123 Cherry Oak Street",
                billing_city = "Hilliard",
                billing_state = "OH",
                billing_zip = "43221",
                isActive = true
            };

            var vm = new CustomerViewModel(cust);

            var mockCust = new Mock<DbSet<Customer>>();
            mockCust.Setup(m => m.Find(cust_id)).Returns(cust);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Customers).Returns(mockCust.Object);

            var controller = new CustomersController(mockContext.Object);
            var result = controller.Add(vm);

            mockContext.Verify(m => m.SaveChanges());
            mockCust.Verify(m =>m.Add(It.IsAny<Customer>()), Times.Once());
            Assert.IsAssignableFrom(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void EditCustomersSuccessViewRenders()
        {
            var cust_id = Guid.NewGuid();
            var existing_cust = new Customer
            {
                customer_id = cust_id,
                name = "Test2",
                billing_address = " 123 Cherry Oak Street",
                billing_city = "Hilliard",
                billing_state = "OH",
                billing_zip = "43221",
                isActive = true
            };

            var mockCust = new Mock<DbSet<Customer>>();
            mockCust.Setup(m => m.Find(cust_id)).Returns(existing_cust);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Customers).Returns(mockCust.Object);

            var controller = new CustomersController(mockContext.Object);
            var result = controller.Edit(cust_id);

            Assert.IsAssignableFrom(typeof(ViewResult), result);
        }

        [Test]
        public void EditCustomersInvalidThrowNotFoundException()
        {
        }

        [Test]
        public void EditCustomersSuccessPosttoMockDb()
        {
            var cust_id = Guid.NewGuid();
            var existing_cust = new Customer
            {
                customer_id = cust_id,
                name = "Test2",
                billing_address = " 123 Cherry Oak Street",
                billing_city = "Hilliard",
                billing_state = "OH",
                billing_zip = "43221",
                isActive = true
            };

            var updated_cust = new Customer
            {
                customer_id = cust_id,
                name = "Test2",
                billing_address = " 223 Pine Street",
                billing_city = "Dublin",
                billing_state = "OH",
                billing_zip = "43220",
                isActive = true
            };

            var vm = new CustomerViewModel(updated_cust);

            var mockCust = new Mock<DbSet<Customer>>();
            mockCust.Setup(m => m.Find(cust_id)).Returns(existing_cust);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Customers).Returns(mockCust.Object);

            var controller = new CustomersController(mockContext.Object);
            var result = controller.Edit(vm);

            mockContext.Verify(m => m.SaveChanges());
            Assert.IsAssignableFrom(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void DeleteCustomersSuccessViewRenders()
        {
            var cust_id = Guid.NewGuid();
            var cust = new Customer
            {
                customer_id = cust_id,
                name = "Test2",
                billing_address = " 123 Cherry Oak Street",
                billing_city = "Hilliard",
                billing_state = "OH",
                billing_zip = "43221",
                isActive = true
            };

            var mockCust = new Mock<DbSet<Customer>>();
            mockCust.Setup(m => m.Find(cust_id)).Returns(cust);

            var mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(m => m.Customers).Returns(mockCust.Object);

            var controller = new CustomersController(mockContext.Object);
            var result = controller.Delete(cust_id);

            Assert.IsAssignableFrom(typeof(ViewResult), result);
        }


        [Test]
        public void DeleteCustomersInvalidThrowNotFoundExceptions()
        {
        }

        [Test]
        public void DeleteCustomersSuccessMockDbUpdated()
        {
        }
    }
}
