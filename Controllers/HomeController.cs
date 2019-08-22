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

namespace Belt.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Auctions()
        {
            Transfer();
            int? Id = HttpContext.Session.GetInt32("UserID");
            if(Id != null)
            {
                GetUser();
                return View(GetAllProducts());
            }
            else
            {
                return RedirectToAction("Index");
            } 
        }
        public IActionResult NewAuction()
        {
            return View();
        }
        public IActionResult CreateAuction(Product newAuction)
        {
            if(ModelState.IsValid)
            {
                newAuction.UserId = (int) HttpContext.Session.GetInt32("UserID");
                dbContext.Add(newAuction);
                dbContext.SaveChanges();
                return RedirectToAction("Auctions");
            }
            else
            {
                return View("NewAuction");
            }
        }

        public IActionResult ViewAuction(int ProductId)
        {
            GetUser();
            return View(GetOneProduct(ProductId));
        }
        public IActionResult MakeBid(Bid newBid)
        {
            Product ThisProduct = GetOneProduct(newBid.ProductId);
            Bid Highest = GetHighestBid(newBid.ProductId);
            User ThisUser = GetUser();
            if(newBid.BidPrice>ThisUser.Wallet)
            {
                ModelState.AddModelError("BidPrice", "Can not bid more than what is in your wallet");
                System.Console.WriteLine("Not enough in wallet");
            }
            if(Highest != null)
            {
                if(newBid.BidPrice < Highest.BidPrice+5)
                {
                    ModelState.AddModelError("BidPrice", "Bid must be higher than current highest bid");
                    System.Console.WriteLine("BID TOO LOW");
                }
            }
            else
            {
                if(newBid.BidPrice< ThisProduct.StartingBid)
                {
                    ModelState.AddModelError("BidPrice", "Bid must be higher than starting price");
                    System.Console.WriteLine("BID TOO LOW");
                }
            }
            
            if(ModelState.IsValid)
            {

                newBid.UserId = (int) HttpContext.Session.GetInt32("UserID");
                dbContext.Add(newBid);
                dbContext.SaveChanges();
                System.Console.WriteLine("REDIRECT TO VIEWAUCTION");
                return RedirectToAction("Auctions");
            }
            else
            {
                System.Console.WriteLine("BAD");
                return RedirectToAction("Auctions");
            }
        }

        public IActionResult RemoveAuction(int ProductId)
        {
            Product BadProduct = dbContext.products.FirstOrDefault(p=>p.ProductId==ProductId);
            dbContext.products.Remove(BadProduct);
            dbContext.SaveChanges();
            return RedirectToAction("Auctions");
        }


        //CRUD methods

        private Bid GetHighestBid(int ProductId)
        {
            return dbContext.bids
                .Include(b => b.Product)
                .FirstOrDefault(p=>p.ProductId==ProductId);
        }
        private Product GetOneProduct(int ProductId)
        {
            return dbContext.products
                .Include(p => p.User)
                .Include(p => p.HighestBid)
                .ThenInclude(b => b.User)
                .FirstOrDefault(p => p.ProductId==ProductId);
        }
        private List<Product> GetAllProducts()
        {
            return dbContext.products
                .Include(p => p.HighestBid)
                .Include(p => p.User)
                .OrderBy(p=>p.Deadline)
                .ToList();
        }

        private User GetUser()
        {
            int? Id = HttpContext.Session.GetInt32("UserID");
            User LogginUser = dbContext.Users.FirstOrDefault(u =>u.UserId == Id);
            ViewBag.FirstName = LogginUser.FirstName;
            ViewBag.Wallet = LogginUser.Wallet;
            ViewBag.UserId = Id;
            return LogginUser;
        }

        private List<Product> GetExpired()
        {
            return dbContext.products
                .Include(p=>p.HighestBid)
                .Where(p=>p.Deadline<DateTime.Now)
                .ToList();
        }

        private void Transfer()
        {
            List<Product> Expired = GetExpired();
            foreach(var p in Expired)
            {
                int WinnerId = p.HighestBid.UserId;
                int ListerId = p.UserId;
                User Winner = dbContext.Users
                    .FirstOrDefault(u=>u.UserId==WinnerId);
                User Lister = dbContext.Users
                    .FirstOrDefault(u=>u.UserId==ListerId);
                Winner.Wallet -= p.HighestBid.BidPrice;
                Lister.Wallet += p.HighestBid.BidPrice;
                dbContext.products.Remove(p);
                dbContext.SaveChanges();
            }
        }
    }

}
