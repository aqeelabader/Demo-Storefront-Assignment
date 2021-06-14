using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using prog7311poeTask2.Models;
using System.Text;

namespace prog7311poeTask2.Repository
{
    public class ProductRepository
    {
        private SqlConnection conn;//makes the sql connection variable

        private void Connection()//class for the sql connection so it can be used where needed
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();//the database used is the default db generated for login
            conn = new SqlConnection(constr);
        }

        //get all products in list
        public List<ProductModel> GetAllProducts()
        {
            Connection();
            List<ProductModel> ProductList = new List<ProductModel>();//creating a list to populate from the databast

            SqlCommand GetCommand = new SqlCommand("GetAllProducts", conn);
            GetCommand.CommandType = CommandType.StoredProcedure;//using the stored procedure from the database
            SqlDataAdapter da = new SqlDataAdapter(GetCommand);
            DataTable dt = new DataTable();

            conn.Open();
            da.Fill(dt);
            conn.Close();
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
            return ProductList;
        }

        //same as get all products but for hardware
        public List<ProductModel> GetAllHardwareProducts()
        {
            Connection();
            List<ProductModel> ProductList = new List<ProductModel>();//creating a list to populate from the databast

            SqlCommand GetCommand = new SqlCommand("GetAllHardwareProducts", conn);//pulls hardware products from the database
            GetCommand.CommandType = CommandType.StoredProcedure;//using the stored procedure from the database
            SqlDataAdapter da = new SqlDataAdapter(GetCommand);
            DataTable dt = new DataTable();//the data will be added to this table

            conn.Open();//opening connection and executing , then closing connection
            da.Fill(dt);
            conn.Close();
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
            return ProductList;//returns the list that matches hardware parameter
        }
        //same as above but for software products only 
        public List<ProductModel> GetAllSoftwareProducts()
        {
            Connection();
            List<ProductModel> ProductList = new List<ProductModel>();//creating a list to populate from the databast

            SqlCommand GetCommand = new SqlCommand("GetAllSoftwareProducts", conn);
            GetCommand.CommandType = CommandType.StoredProcedure;//using the stored procedure from the database
            SqlDataAdapter da = new SqlDataAdapter(GetCommand);
            DataTable dt = new DataTable();

            conn.Open();
            da.Fill(dt);
            conn.Close();
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
            return ProductList;//returns all software products
        }

        //same as get all hardware but for the other products
        public List<ProductModel> GetAllOtherProducts()
        {
            Connection();
            List<ProductModel> ProductList = new List<ProductModel>();//creating a list to populate from the databast

            SqlCommand GetCommand = new SqlCommand("GetAllOtherProducts", conn);
            GetCommand.CommandType = CommandType.StoredProcedure;//using the stored procedure from the database
            SqlDataAdapter da = new SqlDataAdapter(GetCommand);
            DataTable dt = new DataTable();

            conn.Open();
            da.Fill(dt);
            conn.Close();
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
            return ProductList;//returns all other products
        }

        //adding new products
        //this is the repository code for adding a product to the database
        public bool AddProduct(ProductModel obj)//uses product model 
        {
            //code attribution
            //the process below of getting the file path was taken from Stack Overflow
            ///original question: https://stackoverflow.com/questions/1268738/asp-net-mvc-find-absolute-path-to-the-app-data-folder-from-controller
            //answer user details:
            //Alex from Jitbit
            //https://stackoverflow.com/users/56621/alex-from-jitbit
            //

            string FileName = Path.GetFileNameWithoutExtension(obj.ImageFile.FileName) ;//this is just getting the path for the product picture
            string FileExtension = Path.GetExtension(obj.ImageFile.FileName);

            string relativePath = ConfigurationManager.AppSettings["ImagePath"].ToString();//converts the path to a string

            FileName = FileName.Trim() + FileExtension;//gets extension

            obj.ProductPic = relativePath + FileName;//gets the path of the project and the image in the project....this is what is actually saved in the db

            //end of attributed code

            Connection();
            SqlCommand AddCommand = new SqlCommand("AddNewProduct", conn);//sql connection
            AddCommand.CommandType = CommandType.StoredProcedure;//specifying command type

            //adding values
            AddCommand.Parameters.AddWithValue("@ProductName", obj.ProductName);
            AddCommand.Parameters.AddWithValue("@ProductDescription", obj.ProductDescription);
            AddCommand.Parameters.AddWithValue("@ProductCategory", obj.ProductCategory);
            AddCommand.Parameters.AddWithValue("@ProductPrice", obj.ProductPrice);
            AddCommand.Parameters.AddWithValue("@ProductPic", obj.ProductPic);

            conn.Open();//open connection to db
            int i = AddCommand.ExecuteNonQuery();//executing command 
            conn.Close();//close connection to db
            if(i >= 1)//checking if it was successfully added
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        //updating an existing product in the database
        public bool UpdateProduct(ProductModel obj)//this is the edit product code
        {
            Connection();
            SqlCommand UpdateCommand = new SqlCommand("UpdateProduct", conn);//db connection and command

            //these values update the product that was searched for using id in the controller
            UpdateCommand.CommandType = CommandType.StoredProcedure;
            UpdateCommand.Parameters.AddWithValue("@PId", obj.PId);
            UpdateCommand.Parameters.AddWithValue("@ProductName", obj.ProductName);
            UpdateCommand.Parameters.AddWithValue("@ProductDescription", obj.ProductDescription);
            UpdateCommand.Parameters.AddWithValue("@ProductCategory", obj.ProductCategory);
            UpdateCommand.Parameters.AddWithValue("@ProductPrice", obj.ProductPrice);
            UpdateCommand.Parameters.AddWithValue("@ProductPic", obj.ProductPic);

            conn.Open();//open db connection
            int i = UpdateCommand.ExecuteNonQuery();//execute
            conn.Close();//close db connection
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // deleting a product
        public bool DeleteProduct(int ProductId)
        {
            Connection();
            SqlCommand DeleteCommand = new SqlCommand("DeleteProductById", conn);//gets the product by id
            DeleteCommand.CommandType = CommandType.StoredProcedure;//specifies stored procedure
            DeleteCommand.Parameters.AddWithValue("@PId", ProductId);

            //db open , execute and close
            conn.Open();
            int i = DeleteCommand.ExecuteNonQuery();
            conn.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}