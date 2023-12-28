using BikeOnlineStore.Data;
using BikeOnlineStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeOnlineStore.Controllers
{
    public class BikeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BikeController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }


        public ActionResult Index(string searchString)
        {
            if (searchString != null)
            {
                IEnumerable<Bike> bikeList = _db.Bikes.Where(b => b.Title.Contains(searchString));
                return View(bikeList);
            }
            else
            {
                IEnumerable<Bike> bikeList = _db.Bikes;
                return View(bikeList);
            }
        }


        public ActionResult BikeInfo(int id)
        {
            Bike bike = _db.Bikes.FirstOrDefault(b => b.Id == id);
            return View(bike);
        }


        public ActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Bike bike)
        {
            if (ModelState.IsValid)
            {
                if (bike.CoverPhoto != null)
                {
                    string folder = "bikes/cover/";
                    folder += Guid.NewGuid().ToString() + "_" + bike.CoverPhoto.FileName;

                    bike.CoverImagePath = "/" + folder;

                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                    bike.CoverPhoto.CopyTo(new FileStream(serverFolder, FileMode.Create));
                }

                var b = _db.Bikes.FirstOrDefault(b => b.Title == bike.Title);
                if (b == null)
                {
                    _db.Bikes.Add(bike);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(bike);
        }


        public ActionResult Edit(int Id)
        {
            var bike = _db.Bikes.FirstOrDefault(b => b.Id == Id);
            return View(bike);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Bike bike)
        {
            if (ModelState.IsValid)
            {
                if (bike.CoverPhoto != null)
                {
                    string folder = "bikes/cover/";
                    folder += Guid.NewGuid().ToString() + "_" + bike.CoverPhoto.FileName;

                    bike.CoverImagePath = "/" + folder;

                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                    bike.CoverPhoto.CopyTo(new FileStream(serverFolder, FileMode.Create));
                }

                var b = _db.Bikes.AsNoTracking().FirstOrDefault(b => b.Id == bike.Id);
                if (b != null)
                {
                    _db.Bikes.Update(bike);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(bike);
        }


        public ActionResult Delete(int Id)
        {
            var bike = _db.Bikes.FirstOrDefault(b => b.Id == Id);
            if (bike != null)
            {
                _db.Bikes.Remove(bike);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}