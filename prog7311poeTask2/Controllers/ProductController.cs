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
        //code attribution
        //the method used for generation of views was taken from the following website
        //https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/introduction/adding-a-view
        //this method is used for creating all the views for this application 
        //end of code attribution


        private SqlConnection conn; //this is just to create the sql connection
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            conn = new SqlConnection(constr);
        }

        // this gets all the products in the database
        public ActionResult GetAllProducts(string prodSearch)
        {

                Connection();//connection to db
                List<ProductModel> ProductList = new List<ProductModel>();//creating a list to populate from the databast

                //code attribution
                //the following code for the search function was taken from a YouTube Video
                //link to video: https://www.youtube.com/watch?v=UAZABAJlO8g
                //

                string query = "select * from [dbo].[Product] where ProductName like '%" + prodSearch + "%'";

                //End of attributed code


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
            return View(ProductList);//returns the list of products
        }

        //Get all Hardware products
        //same as get all products but filtered to hardware
        public ActionResult GetAllHardwareProducts()
        {
            ProductRepository ProductRepo = new ProductRepository();//uses the repository to avoid repetitive code
            ModelState.Clear();
            return View(ProductRepo.GetAllHardwareProducts());
        }

        //Get all Software products
        //same as get all products but filtered to software
        public ActionResult GetAllSoftwareProducts()
        {
            ProductRepository ProductRepo = new ProductRepository();//using the repository again
            ModelState.Clear();
            return View(ProductRepo.GetAllSoftwareProducts());
        }

        //Get all Other products
        //same as get all products but filtered to other and accessories
        public ActionResult GetAllOtherProducts()
        {
            ProductRepository ProductRepo = new ProductRepository();//uses repository again
            ModelState.Clear();
            return View(ProductRepo.GetAllOtherProducts());
        }


        //for admins to add products
        [Authorize (Roles ="Admin")]
        public ActionResult AddProduct()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddProduct(ProductModel obj)//uses the model to get correct format
        {
            try//put it in a try so program does not crash if database is not connected 
            {
                if (ModelState.IsValid)
                {
                    ProductRepository ProductRepo = new ProductRepository();//code for this is in the repository
                    if (ProductRepo.AddProduct(obj))
                    {
                        ViewBag.Message = "Product added successfully";//just a success message but it serves as confirmation 

                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }



        //cart....when a person clicks add to cart , this handles it
        [Authorize]
        public ActionResult Cart(int id)//uses the id to find the product in the all products list
        {
            ProductRepository ProductRepo = new ProductRepository();// same repository from get all products, it holds the data needed
            return View(ProductRepo.GetAllProducts().Find(Pat => Pat.PId == id));//gets the product with the mathcing id

        }
        
        //this is just so the cart can redirect to the payment page
        [Authorize]
        [HttpPost]
        public ActionResult Cart(int id, ProductModel obj)
        {
            try
            {
                ProductRepository ProductRepo = new ProductRepository();
                ProductRepo.UpdateProduct(obj);

                return RedirectToAction("Payment");

            }
            catch
            {
                return View();

            }
        }


        //for editing products in the database
        public ActionResult EditProduct(int id)//uses the id of selected product
        {
            ProductRepository ProductRepo = new ProductRepository();//finds it using the repository
            return View(ProductRepo.GetAllProducts().Find(Pat => Pat.PId == id));//compares the id of selected product to locate it in the repo

        }
        //updates the database
        [HttpPost]
        public ActionResult EditProduct(int id, ProductModel obj)//uses the model again for formatting
        {
            try
            {
                ProductRepository ProductRepo = new ProductRepository();//referencing the repo cause thats where the code is
                ProductRepo.UpdateProduct(obj);//updates the db using the new inputs

                return RedirectToAction("GetAllProducts");//redirects back to the all products page when done

            }
            catch
            {
                return View();

            }
        }

        //code to delete a product
        //nothing special here, just deletes the product that matches id of selected product in database.
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

        //this is just to create the success page that is shown after payment is made
        public ActionResult Success()
        {
            ViewBag.Message = "the success page";

            return View();//returns the success view
        }

        //this is just to create the payment page where a user enters their payment info
        public ActionResult Payment()
        {
            ViewBag.Message = "the payment page";

            return View();//just returns the view.....POE document said assume payment was successfull so no code done here!
        }
    }
}
