using BikeOnlineStore.Data;
using BikeOnlineStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeOnlineStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db)
        {
            _db = db;
        }


        public ActionResult Index()
        {
            IEnumerable<Order> orderList = _db.Orders;
            return View(orderList);
        }


        public ActionResult Edit(int Id)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == Id);
            return View(order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                var o = _db.Orders.AsNoTracking().FirstOrDefault(o => o.Id == order.Id);
                if (o != null)
                {
                    _db.Orders.Update(order);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(order);
        }


        public ActionResult Delete(int Id)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == Id);
            if (order != null)
            {
                _db.Orders.Remove(order);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public List<string> GetBikesId(string bikeString)
        {
            List<string> bikeList = bikeString.Split(',').ToList();
            return bikeList;
        }
    }
}