using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using banhang.Models;

namespace banhang.Controllers
{
    public class UsersController : Controller
    {
        private LTWebEntities db = new LTWebEntities();

        // GET: Users
        public ActionResult Index()
        {
            if (Session["Level"] == null)
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (Session["Level"].ToString() == "3")
            {
                return RedirectToAction("TrangChu", "Home");
            }
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idUser,Username,Password,Email,Lever")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idUser,Username,Password,Email,Lever")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult TaiKhoan(string id)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("LogIn", "Home");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult GetOrders(string stat)
        {
            if (stat == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string uname = (string)Session["Username"];
            var model = db.Orders.ToList(); ;
            if (Session["Level"].ToString() == "3")
            {
                model = model.Where(x => x.user.Username == uname).ToList();
            }
            if (stat != "TatCa")
                model = model.Where(x=>x.TrangThai == stat).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult XacNhan(int? OrderID)
        {
            if (OrderID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Order = db.Orders.Find(OrderID);
            switch (Order.TrangThai)
            {
                case "ChoXacNhan":
                    Order.TrangThai = "ChoLayHang";
                    break;

                case "ChoLayHang":
                    Order.TrangThai = "DangGiao";
                    break;

                case "DangGiao":
                    Order.TrangThai = "DaGiao";
                    break;

                default: 
                    break;
            }
            db.SaveChanges();
            return null;
        }
        public ActionResult Chart()
        {
            if (Session["Level"].ToString() != "3")
                return View();
            else
                return RedirectToAction("TrangChu", "Home");
        }
        public ActionResult Chartdata(string tg)
        {
            if(Session["Level"]==null)
                return null;
            int lvl = int.Parse(Session["Level"].ToString());
            if (lvl > 1)
                return null;
            if (tg == "nam")
            {
                int n = DateTime.Now.AddYears(-1).Year;
                List<Order> all = db.Orders.Where(x => x.NgayDat.Year == n).ToList();
                double[] data = new double[12];
                foreach (Order o in all)
                {
                    int m = o.NgayDat.Month - 1;
                    data[m] += o.Tong;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if(tg == "thang")
            {
                int n = DateTime.Now.AddMonths(-1).Month;
                List<Order> all = db.Orders.Where(x => x.NgayDat.Month == n).ToList();
                int d = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);
                double[] data = new double[d];
                foreach (Order o in all)
                {
                    int m = o.NgayDat.Day - 1;
                    data[m] += o.Tong;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public ActionResult ChiTietDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order od = db.Orders.Include("items").Where(x=>x.OrderID == id).FirstOrDefault();
            if(od.user.Username != Session["Username"].ToString() && Session["Level"].ToString() == "3")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(od);
        }
    }
}
