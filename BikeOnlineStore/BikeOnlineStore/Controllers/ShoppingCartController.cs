using Microsoft.AspNetCore.Mvc;
using BikeOnlineStore.Data;
using BikeOnlineStore.Models;
using System.Collections.Generic;

namespace BikeOnlineStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly AppDbContext _db;

        public ShoppingCartController(AppDbContext db)
        {
            _db = db;
        }



        public ActionResult Index()
        {
            string str = "";
            var x = User.Claims.ToList();
            foreach (var claim in x)
            {
                str += claim.ToString();
                str = str.Substring(str.Length - 1);
                break;
            };
            
            // Создание объектов: корзина, список велосипедов

            ShoppingCart shoppingCart = _db.ShoppingCarts.FirstOrDefault(s => s.Username == str);
            List<BikeForShoppingCart> bikes = new List<BikeForShoppingCart>();


            if (shoppingCart.BikeList != "")
            {
                List<string> bikeIdListShoppingCart = shoppingCart.BikeList.Split(",").ToList();

                var bikeDictionary = new Dictionary<string, int>();
                foreach (string bikeId in bikeIdListShoppingCart)
                {
                    if (bikeDictionary.ContainsKey(bikeId)) bikeDictionary[bikeId] += 1;
                    else bikeDictionary.Add(bikeId, 1);
                }

                foreach (var bike in bikeDictionary)
                {
                    var b = _db.Bikes.FirstOrDefault(b => b.Id == int.Parse(bike.Key));
                    bikes.Add(new BikeForShoppingCart()
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Price = b.Price,
                        Quantity = bikeDictionary[bike.Key],
                        PriceMultiplyQuantity = b.Price * bikeDictionary[bike.Key],
                        CoverImagePath = b.CoverImagePath
                    });
                }

                TempData["isBikeListEmpty"] = false;
            }
            else
            {
                
                TempData["isBikeListEmpty"] = true;
            }

            bikes.Sort((left, right) => left.Title.CompareTo(right.Title));

            // Создание объекта типа ShoppingCartPlusBikeForShoppingCart

            ShoppingCartPlusBikeForShoppingCart obj = new()
            {
                shoppingCart = shoppingCart,
                bikeForShoppingCart = bikes
            };

            return View(obj);
        }


        public ActionResult AddToShoppingCart(int id)
        {
            string str = "";
            var x = User.Claims.ToList();
            foreach (var claim in x)
            {
                str += claim.ToString();
                str = str.Substring(str.Length - 1);
                break;
            };

            ShoppingCart shoppingCart = _db.ShoppingCarts.FirstOrDefault(s => s.Username == str);
            List<BikeForShoppingCart> bikes = new List<BikeForShoppingCart>();

            var c = _db.ShoppingCarts.FirstOrDefault(u => u.Username == str);
            if (shoppingCart.BikeList == "") c.BikeList += id.ToString();
            else c.BikeList += "," + id.ToString();
            _db.ShoppingCarts.Update(c);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult DeleteWhileInShoppingCart(int id)
        {
            string str = "";
            var x = User.Claims.ToList();
            foreach (var claim in x)
            {
                str += claim.ToString();
                str = str.Substring(str.Length - 1);
                break;
            };

            var c = _db.ShoppingCarts.FirstOrDefault(u => u.Username == str);
            string s = c.BikeList;
            string s0 = id.ToString();
            string sFinal = "";
            int bikeCount = (s.Length - s.Replace(s0, "").Length) / s0.Length;
            Console.WriteLine(bikeCount);

            if (c.BikeList.Contains(s0 + ","))
            {
                sFinal = c.BikeList.Replace(s0 + ",", "");
            }
            else if (c.BikeList != s0)
            {
                sFinal = c.BikeList.Replace("," + s0, "");
            }
            else
            {
                sFinal = "";
            }

            sFinal = sFinal.Trim(',');

            for (int i = 0; i < bikeCount - 2; i++) // -2, чтобы кол-во велосипедов в корзине уменьшалось на 1
            {
                sFinal += "," + s0;
            }

            c.BikeList = sFinal;

            _db.ShoppingCarts.Update(c);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
