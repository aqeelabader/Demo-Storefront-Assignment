using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prog7311poeTask2.Models;
using prog7311poeTask2.Repository;

namespace prog7311poeTask2.Controllers
{
    public class ProductController : Controller
    {
     //get product/getallproducts
     public ActionResult GetAllProducts()
        {
            ProductRepository ProductRepo = new ProductRepository();
            ModelState.Clear();
            return View(ProductRepo.GetAllProducts());
        }

        //get product/returnproductview
        public ActionResult AddProduct()
        {
            return View();
        }
        //post product/addproduct
        [HttpPost]
        public ActionResult AddProduct(ProductModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductRepository ProductRepo = new ProductRepository();
                    if (ProductRepo.AddProduct(obj))
                    {
                        ViewBag.Message = "Product added successfully";

                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
        //get product/returneditproductview
        public ActionResult EditProduct(int id)
        {
            ProductRepository ProductRepo = new ProductRepository();
            return View(ProductRepo.GetAllProducts().Find(Pat => Pat.PId == id));

        }
        //post product/editexistingproduct
        [HttpPost]
        public ActionResult EditProduct(int id, ProductModel obj)
        {
            try
            {
                ProductRepository ProductRepo = new ProductRepository();
                ProductRepo.UpdateProduct(obj);

                return RedirectToAction("GetAllProducts");

            }
            catch
            {
                return View();

            }
        }
        //get product/deleteproduct
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                ProductRepository ProductRepo = new ProductRepository();
                if (ProductRepo.DeleteProduct(id))
                {
                    ViewBag.AlertMsg = "product deleted successfully";

                }
                return RedirectToAction("GetAllProducts");
            }
            catch
            {
                return View();
            }
        }
    }
}
