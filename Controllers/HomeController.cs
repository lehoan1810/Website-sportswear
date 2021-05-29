using banhang.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace banhang.Controllers
{
    public class HomeController : Controller
    {
        public LTWebEntities db = new LTWebEntities();
        public ActionResult About()
        {
            var data = (from s in db.Users select s).ToList();
            ViewBag.users = data;
            return View();
        }

        public ActionResult TrangChu()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return View("TrangChu");
        }
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            User a = db.Users.Where(x => x.Username == username).FirstOrDefault();
            if (a == null)
                return View();
            if(a.Password != password)
                return View();
            Session["Username"] = a.Username;
            Session["Level"] = a.Lever.ToString();
            return View("TrangChu");
        }
        [HttpGet]
        public ActionResult LogUp()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult LogUp(FormCollection form)
        //{
        //    if (form["password"] != form["cpassword"])
        //        return View("LogUp");
        //    User a = new User();
        //    a.Lever = 3;
        //    a.Username = form["name"];
        //    a.Password = form["password"];
        //    a.Email = form["email"];
        //    _db.Users.Add(a); 
        //    _db.SaveChanges();
        //    return View("TrangChu");
        //}
        [HttpPost]
        public ActionResult Validate(User user, string cpassword)
        {
            if(ModelState.IsValid && cpassword == user.Password)
            {
                user.Lever = 3;
                db.Users.Add(user);
                db.SaveChanges();
                return View("TrangChu");
            }
            return View("LogUp");
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult SoccerClothes()
        {
            var data = (from s in db.Products select s).Where(s=>s.Category == "SoccerClothes").ToList();
            ViewBag.Products = data;
            return View();
        }
        public void AddToCart(int? id)
        {
            if(Session["cart"]==null)
            {
                Session["cart"] = new Order
                {
                    items = new List<Item>()
                };
            }
            Order o = (Order)Session["cart"];
            Item i = o.items.Where(x => x.ProductId == id).FirstOrDefault();
            if(i != null)
            {
                i.Quantity++;
            }
            else
            {
                i = new Item();
                i.Product = db.Products.Where(x => x.idProduct == id).FirstOrDefault();
                i.ProductId = i.Product.idProduct;
                i.Quantity = 1;
                o.items.Add(i);
            }
            Session["cart"] = o;
        }
        public ActionResult ShoppingCart()
        {
            if(Session["cart"]==null)
            {
                Session["cart"] = new Order
                {
                    items = new List<Item>()
                };
            }
            Order o = (Order)Session["cart"];
            return View(o);
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["UserName"] == null)
                return RedirectToAction("LogIn");
            if (Session["cart"] == null)
            {
                return RedirectToAction("TrangChu");
            }
            Order o = (Order)Session["cart"];
            if(o.items.Count == 0)
                return RedirectToAction("TrangChu");
            return View();
        }
        [HttpPost]
        public ActionResult DatHang(string DiaChi, string GhiChu)
        {
            if (Session["cart"] == null)
            { 
                return RedirectToAction("TrangChu");
            }
            if (Session["UserName"] == null)
                return RedirectToAction("LogIn");

            Order b = (Order)Session["cart"];
            if (b.items.Count == 0)
            {
                return RedirectToAction("TrangChu");
            }
            Order o = new Order();
            string uname = (string)Session["Username"];
            o.Username = uname;
            o.user = db.Users.Where(x => x.Username == uname).FirstOrDefault();
            if (o.user is null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            o.NgayDat = DateTime.Now;
            o.TrangThai = "ChoXacNhan";
            o.DiaChi = DiaChi;
            o.GhiChu = GhiChu;
            db.Orders.Add(o);
            o.items = new List<Item>();
            int id = 1;
            double total = 0;
            foreach (Item i in b.items)
            {
                i.OrderId = o.OrderID;
                i.ItemId = id;
                id++;
                total += i.Product.Price;
                i.Product = db.Products.Find(i.ProductId);
                o.items.Add(i);
            }
            o.Tong = total;
            db.SaveChanges();
            Session.Remove("cart");
            return View("TrangChu");
        }
    }
}