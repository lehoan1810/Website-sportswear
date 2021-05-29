using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using banhang.Models;
using PagedList;

namespace banhang.Controllers
{
    public class CategoryController : Controller
    {
        LTWebEntities db = new LTWebEntities();
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SoccerClothes(int? page, string sub)
        {
            string subtv = "";
            if (!String.IsNullOrWhiteSpace(sub))
            {
                switch (sub)
                {
                    case "AoDoiTuyen":
                        subtv = "Áo Đội Tuyển";
                        break;

                    case "AoCLB":
                        subtv = "Áo CLB";
                        break;

                    case "DoTapGym":
                        subtv = "Đồ Tập Gym";
                        break;

                    case "GiayBongDa":
                        subtv = "Giày Bóng Đá";
                        break;

                    case "DoTapYoGa":
                        subtv = "Đồ Tập YoGa";
                        break;
                    case "AoBra":
                        subtv = "Áo Bra";
                        break;
                    default:
                        subtv = "khác";
                        break;
                }

                ViewBag.Category = subtv;
            }
            if (page == null) page = 1;
            var products = db.Products.Where(x=>x.Category == "Đồ Bóng Đá").ToList();

            


            if (!String.IsNullOrWhiteSpace(sub) && sub!="Khác")
            {
                products = products.Where(x => x.SubCategory == subtv).ToList();
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DoTapYoGa(int? page, string sub)
        {
            string subtv = "";
            if (!String.IsNullOrWhiteSpace(sub))
            {
                switch (sub)
                {                  
                    case "":
                        subtv = "";
                        break;
                    default:
                        subtv = "";
                        break;
                }

                ViewBag.Category = subtv;
            }
            if (page == null) page = 1;
            var products = db.Products.Where(x => x.Category == "Đồ Tập YoGa").ToList();




            if (!String.IsNullOrWhiteSpace(sub) && sub != "Khác")
            {
                products = products.Where(x => x.SubCategory == subtv).ToList();
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }
    }
}