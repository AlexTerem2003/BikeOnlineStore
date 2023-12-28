using Microsoft.AspNetCore.Mvc;
using BikeOnlineStore.Data;
using BikeOnlineStore.Models;

namespace BikeOnlineStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly AppDbContext _db;

        public CatalogController(AppDbContext db)
        {
            _db = db;
        }

        public ActionResult Index(string searchString)
        {
            if (searchString != null)
            {
                IEnumerable<Bike> bikeList = _db.Bikes.Where(b => b.Title.Contains(searchString)).ToList();
                return View(bikeList);
            }
            else
            {
                IEnumerable<Bike> bikeList = _db.Bikes.ToList();
                return View(bikeList);
            }
        }
    }
}
