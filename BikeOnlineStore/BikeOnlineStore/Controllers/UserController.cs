using Microsoft.AspNetCore.Mvc;
using BikeOnlineStore.Data;
using BikeOnlineStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace BikeOnlineStore.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _db;

        public UserController(AppDbContext db)
        {
            _db = db;
        }


        public ActionResult Index()
        {
            IEnumerable<User> userList = _db.Users;
            return View(userList);
        }


        public ActionResult Edit(int Id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == Id);
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var u = _db.Users.AsNoTracking().FirstOrDefault(u => u.Id == user.Id);
                if (u != null)
                {
                    _db.Users.Update(user);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }


        public ActionResult Delete(int Id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == Id);
            if (user != null)
            {
                var shoppingCart = _db.ShoppingCarts.FirstOrDefault(s => s.Username == user.Username);
                _db.Users.Remove(user);

                if (shoppingCart != null)
                {
                    _db.ShoppingCarts.Remove(shoppingCart);
                }

                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(u => u.Username == model.Username);
                if (user != null)
                {
                    if (user.Password == model.Password)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim (ClaimsIdentity.DefaultNameClaimType, user.Username),
                            new Claim (ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                        };
                        var result = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(result));
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(model);
        }

        
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(u => u.Username == model.Username);
                if (user == null)
                {
                    user = new User()
                    {
                        Surname = model.Surname,
                        Name = model.Name,
                        Patronymic = model.Patronymic,
                        PhoneHumber = model.PhoneHumber,
                        Username = model.Username,
                        Password = model.Password,
                        Role = model.Role
                    };
                    _db.Users.Add(user);
                    _db.SaveChanges();
                    var claims = new List<Claim>
                    {
                        new Claim (ClaimsIdentity.DefaultNameClaimType, user.Username),
                        new Claim (ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                    };
                    var result = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(result));

                    // СОЗДАНИЕ ПЕРСОНАЛЬНОЙ КОРЗИНЫ
                    if (user.Role == 1)
                    {
                        ShoppingCart shoppingCart = new()
                        {
                            Username = user.Username,
                            BikeList = ""
                        };
                        _db.ShoppingCarts.Add(shoppingCart);
                        _db.SaveChanges();
                    }

                    return RedirectToAction("Index", "Home");
                }
                return View(model);
            }
            return View(model);
        }
    }
}
