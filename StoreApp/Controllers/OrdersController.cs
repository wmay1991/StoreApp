using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreApp.Core;
using StoreApp.Data;
using StoreApp.Models;

namespace StoreApp.Controllers
{
    public class OrdersController : Controller
    {
        private StoreDbContext _db = new StoreDbContext();


        public OrdersController(StoreDbContext db)
        {
            _db = db;
        }

        public OrdersController()
        {
            _db = new StoreDbContext();
        }
        // GET: Orders
        public ActionResult Index()
        {
            var orders = _db.Orders.ToList();
            return View(orders);

        }
        // GET: Orders/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = _db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult AddOrder()
        {
            ViewBag.item_id = new SelectList(_db.Items, "item_id", "name");
            ViewBag.cust = new SelectList(_db.Customers, "cust_id", "name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrder(OrderViewModel order_vm)
        {
            if (ModelState.IsValid)
            {
                order_vm.order_id = Guid.NewGuid();

                var ord = new Order();
                var order = new OrderViewModel(ord, order_vm);
                var order_item = _db.Items.Find(ord.item_id);

                order_item.CalculateItemQuantity(ord, order_item, "Add");

                _db.Orders.Add(ord);
                _db.Entry(order_item).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.item_id = new SelectList(_db.Items, "item_id", "name", order_vm.item_id);
            ViewBag.cust = new SelectList(_db.Customers, "cust_id", "name", order_vm.customer_id);
            return View(order_vm);
        }

        // GET: Orders/Edit/5
        public ActionResult EditOrder(Guid? id)
        {
          if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = _db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            var vm = new OrderViewModel(order);
            ViewBag.item_id = new SelectList(_db.Items, "item_id", "name", order.item_id);
            ViewBag.cust = new SelectList(_db.Customers, "cust_id", "name", order.customer_id);
            return View(vm);
        }



        // Used separate item quantit for edit since it needs to pass the vm which needs to done in controller instead of model
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrder(OrderViewModel order_vm)
        {
            if (ModelState.IsValid)
            {
                var order = _db.Orders.Find(order_vm.order_id);
                var order_item = _db.Items.Find(order.item_id);
                CalculateItemQuantityEdit(order, order_item, order_vm);

                var vm = new OrderViewModel(order, order_vm);


                _db.Entry(order).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.item = new SelectList(_db.Items, "item_id", "name", order_vm.item_id);
            ViewBag.cust = new SelectList(_db.Customers, "cust_id", "name", order_vm.customer_id);
            return View(order_vm);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _db.Orders.Find(id);
            var vm = new OrderViewModel(order);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = _db.Orders.Find(id);
            var order_item = _db.Items.Find(order.item_id);

            order_item.CalculateItemQuantity(order, order_item, "Delete");

            _db.Orders.Remove(order);
            _db.Entry(order_item).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void CalculateItemQuantityEdit(Order ord, Item item, OrderViewModel vm)
        {
            // if neither don't update anything
            if (ord.quantity != vm.quantity)
            {
                if (ord.quantity < vm.quantity)
                {
                    item.quantity -=  (vm.quantity - ord.quantity);
                }
                else if (ord.quantity > vm.quantity)
                {
                    item.quantity += (ord.quantity - vm.quantity);
                }

                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();
            }

        }
    }
}
