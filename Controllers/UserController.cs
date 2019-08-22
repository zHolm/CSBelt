using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Belt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers
{
    public class UserController : Controller
    {
        private MyContext dbContext;
        public UserController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    user.Password = Hasher.HashPassword(user, user.Password);
                    dbContext.Add(user);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("UserID", user.UserId);
                    ViewBag.UserId = user.UserId;
                    return RedirectToAction("Auctions", "Home");
                    // return RedirectToAction("Success");
                }
            }
            else
            {
                return View("Index");
            }
        }

        // [HttpGet("/processlogin")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.EmailLogin);
                if(userInDb == null)
                {
                    ModelState.AddModelError("EmailLogin", "Invalid Email/Password");
                    return View("Index");
                }

                var hasher = new PasswordHasher<LoginUser>();

                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.PasswordLogin);
                if(result == 0)
                {
                    ModelState.AddModelError("EmailLogin", "Invalid Email/Password");
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserID", userInDb.UserId);
                    int? Id = HttpContext.Session.GetInt32("UserID");
                    ViewBag.UserId = userInDb.UserId;
                    // return RedirectToAction("Account", new {id=userInDb.UserId});
                    return RedirectToAction("Auctions", "Home");
                    // return RedirectToAction("Success");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("/login")]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpGet("/success")]
        public IActionResult Success()
        {
            int? Id = HttpContext.Session.GetInt32("UserID");
            ViewBag.UserId = Id;
            if(Id != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            System.Console.WriteLine("Session cleared");
            return RedirectToAction("Index");
        }
    }
}
