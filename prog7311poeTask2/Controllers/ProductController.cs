using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prog7311poeTask2.Models;
using prog7311poeTask2.Repository;

namespace prog7311poeTask2.Controllers
{
    public class ProductController : Controller
    {
        private SqlConnection conn;
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            conn = new SqlConnection(constr);
        }

        //get product/getallproducts


        public ActionResult GetAllProducts(string prodSearch)
        {

                Connection();
                List<ProductModel> ProductList = new List<ProductModel>();//creating a list to populate from the databast

                string query = "select * from [dbo].[Product] where ProductName like '%" + prodSearch + "%'";
                SqlCommand GetCommand = new SqlCommand(query, conn);
                //GetCommand.CommandType = CommandType.StoredProcedure;//using the stored procedure from the database
                SqlDataAdapter da = new SqlDataAdapter(GetCommand);
                DataTable dt = new DataTable();

                conn.Open();
                da.Fill(dt);
                
                //list is binded using a data row
                foreach (DataRow dr in dt.Rows)
                {
                    ProductList.Add(
                        new ProductModel
                        {//conversions
                        PId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductDescription = Convert.ToString(dr["ProductDescription"]),
                            ProductCategory = Convert.ToString(dr["ProductCategory"]),
                            ProductPrice = Convert.ToInt32(dr["ProductPrice"]),
                            ProductPic = Convert.ToString(dr["ProductPic"]),
                        }
                        );
                    
                }
            conn.Close();
            ModelState.Clear();
            return View(ProductList);
        }

        //Get all Hardware products
        public ActionResult GetAllHardwareProducts()
        {
            ProductRepository ProductRepo = new ProductRepository();
            ModelState.Clear();
            return View(ProductRepo.GetAllHardwareProducts());
        }

        //Get all Software products
        public ActionResult GetAllSoftwareProducts()
        {
            ProductRepository ProductRepo = new ProductRepository();
            ModelState.Clear();
            return View(ProductRepo.GetAllSoftwareProducts());
        }

        //Get all Other products
        public ActionResult GetAllOtherProducts()
        {
            ProductRepository ProductRepo = new ProductRepository();
            ModelState.Clear();
            return View(ProductRepo.GetAllOtherProducts());
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
