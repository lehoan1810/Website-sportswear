using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using banhang.Models;
using PagedList;

namespace banhang.Controllers
{
    public class ProductsController : Controller
    {
        private LTWebEntities db = new LTWebEntities();

        // GET: Products
        public ActionResult Index(int? page)
        {
            if(Session["Level"]== null)
            {
                return RedirectToAction("TrangChu","Home");
            }
            if(Session["Level"].ToString()=="3")
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (page == null) page = 1;
            var products = db.Products.ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        public ActionResult AddToCart(int? id, int quantity)
        {
            if (Session["cart"] == null)
            {
                Session["cart"] = new Order
                {
                    items = new List<Item>()
                };
            }
            Order o = (Order)Session["cart"];
            Item i = o.items.Where(x => x.ProductId == id).FirstOrDefault();
            if (i != null)
            {
                i.Quantity+= quantity;
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
            return RedirectToAction("ShoppingCart","Home");
        }
        public ActionResult RemoveItem(int id)
        {
            if (Session["Level"] == null)
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (Session["Level"].ToString() == "3")
            {
                return RedirectToAction("TrangChu", "Home");
            }
            Order o = (Order)Session["cart"];
            Item i = o.items.Where(x => x.ProductId == id).FirstOrDefault();
            if (i != null)
            {
                o.items.Remove(i);
            }
            Session["cart"] = o;
            return RedirectToAction("ShoppingCart", "Home");
        }
        // GET: Products/Create
        public ActionResult Create()
        {
            if (Session["Level"] == null)
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (Session["Level"].ToString() == "3")
            {
                return RedirectToAction("TrangChu", "Home");
            }
            return View();
        }
        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProduct,ProductName,Price,Category,SubCategory,Detail")] Product product, HttpPostedFileBase Image)
        {
            if (Session["Level"] == null)
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (Session["Level"].ToString() == "3")
            {
                return RedirectToAction("TrangChu", "Home");
            }
            ModelState.Remove("Image");
            ModelState.Remove("NgayThem");
            if (ModelState.IsValid)
            {
                product.NgayThem = DateTime.Now.Date;
                if (Image != null)
                {
                    string strname = DateTime.Now.ToString("yyyyMMddHHmmss")+ Image.FileName.ToString();
                    Image.SaveAs(Server.MapPath("~/upload/") + strname);
                    product.Image = strname;
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Level"] == null)
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (Session["Level"].ToString() == "3")
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProduct,ProductName,Price,Category,SubCategory,Detail,NgayThem")] Product product, HttpPostedFileBase Image)
        {
            if (Session["Level"] == null)
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (Session["Level"].ToString() == "3")
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (ModelState.IsValid)
            {
                Product temp = db.Products.Find(product.idProduct);
                if (Image != null)
                {
                    string strname = DateTime.Now.ToString("yyyyMMddHHmmss")+Image.FileName.ToString() ;
                    Image.SaveAs(Server.MapPath("~/upload/") + strname);
                    temp.Image = strname;
                }
                temp.NgayThem = DateTime.Now.Date;
                temp.ProductName = product.ProductName;
                temp.SubCategory = product.SubCategory;
                temp.Price = product.Price;
                temp.Category = product.Category;
                temp.Detail = product.Detail;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Level"] == null)
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (Session["Level"].ToString() == "3")
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Level"] == null)
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (Session["Level"].ToString() == "3")
            {
                return RedirectToAction("TrangChu", "Home");
            }
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
        public ActionResult Import(HttpPostedFileBase FileUpload1)
        {
            if (Session["Level"] == null)
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (Session["Level"].ToString() == "3")
            {
                return RedirectToAction("TrangChu", "Home");
            }
            if (FileUpload1 != null)
            {
                try
                {
                    string path = string.Concat(Server.MapPath("~/UploadFile/" + FileUpload1.FileName));
                    FileUpload1.SaveAs(path);
                    // Connection String to Excel Workbook  
                    string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
                    using (OleDbConnection con = new OleDbConnection(excelCS))
                    {
                        OleDbCommand cmd = new OleDbCommand("select * from [Sheet1$]", con);
                        con.Open();
                        // Create DbDataReader to Data Worksheet  
                        DbDataReader dr = cmd.ExecuteReader();
                        // SQL Server Connection String  
                        string CS = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=C:\Users\Asus\Desktop\banhang14\banhang\App_Data\Database1.mdf;Initial Catalog=Database1;Integrated Security=True";
                        // Bulk Copy to SQL Server   
                        SqlBulkCopy bulkInsert = new SqlBulkCopy(CS);
                        bulkInsert.DestinationTableName = "Product";
                        bulkInsert.WriteToServer(dr);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
