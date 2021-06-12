using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using prog7311poeTask2.Models;


namespace prog7311poeTask2.Repository
{
    public class ProductRepository
    {
        private SqlConnection conn;

        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
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


        //adding new products

        public bool AddProduct(ProductModel obj)
        {
            Connection();
            SqlCommand AddCommand = new SqlCommand("AddNewProduct", conn);
            AddCommand.CommandType = CommandType.StoredProcedure;
            AddCommand.Parameters.AddWithValue("@ProductName", obj.ProductName);
            AddCommand.Parameters.AddWithValue("@ProductDescription", obj.ProductDescription);
            AddCommand.Parameters.AddWithValue("@ProductCategory", obj.ProductCategory);
            AddCommand.Parameters.AddWithValue("@ProductPrice", obj.ProductPrice);
            AddCommand.Parameters.AddWithValue("@ProductPic", obj.ProductPic);

            conn.Open();
            int i = AddCommand.ExecuteNonQuery();
            conn.Close();
            if(i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //updating an existing product in the database

        public bool UpdateProduct(ProductModel obj)
        {
            Connection();
            SqlCommand UpdateCommand = new SqlCommand("UpdateProduct", conn);

            UpdateCommand.CommandType = CommandType.StoredProcedure;
            UpdateCommand.Parameters.AddWithValue("@PId", obj.PId);
            UpdateCommand.Parameters.AddWithValue("@ProductName", obj.ProductName);
            UpdateCommand.Parameters.AddWithValue("@ProductDescription", obj.ProductDescription);
            UpdateCommand.Parameters.AddWithValue("@ProductCategory", obj.ProductCategory);
            UpdateCommand.Parameters.AddWithValue("@ProductPrice", obj.ProductPrice);
            UpdateCommand.Parameters.AddWithValue("@ProductPic", obj.ProductPic);

            conn.Open();
            int i = UpdateCommand.ExecuteNonQuery();
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




        // deleting a product

        public bool DeleteProduct(int ProductId)
        {
            Connection();
            SqlCommand DeleteCommand = new SqlCommand("DeleteProductById", conn);
            DeleteCommand.CommandType = CommandType.StoredProcedure;
            DeleteCommand.Parameters.AddWithValue("@PId", ProductId);

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