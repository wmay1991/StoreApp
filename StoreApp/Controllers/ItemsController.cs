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
    public class ItemsController : Controller
    {
        private StoreDbContext _db = new StoreDbContext();

        public ItemsController(StoreDbContext db)
        {
            _db = db;
        }

        public ItemsController()
        {
            _db = new StoreDbContext();
        }


        // GET: Items
        public ActionResult Index()
        {
            return View(_db.Items.ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult AddItem()
        {
            return View();
        }

        [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult AddItem(ItemViewModel vm)
        {
                vm.item_id = Guid.NewGuid();
                var mdl = new Item();
                var item = new ItemViewModel(vm, mdl);

                _db.Items.Add(mdl);
                _db.SaveChanges();

                return RedirectToAction("Index");
        }


        // GET: Items/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var item = _db.Items.Find(vm.item_id);
                var mdl = new ItemViewModel(vm, item);

                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }


        // POST: Items/Delete/5
        public ActionResult DeleteItem(Guid item_id)
        {
            Item item = _db.Items.Find(item_id);
            _db.Items.Remove(item);
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

        public decimal GetItemPrice(Guid item_id)
        {
            var item = _db.Items.Find(item_id);
            return item.price;
        }
    }
}
