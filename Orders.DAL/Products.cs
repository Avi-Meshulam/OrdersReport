using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public partial class Products
    {
        private readonly string _connectionStr;

        public Products(string connectionString)
        {
            _connectionStr = connectionString;
        }

        public DataTable GetProducts()
        {
            DataTable productsData = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(PRODUCTS_SQL, conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(productsData);
            }

            return productsData;
        }

        public DataTable GetProductById(int productID)
        {
            DataTable productData = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_PRODUCT_SQL, conn);
                cmd.Parameters.Add(new SqlParameter("@ProductID", productID));

                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(productData);
            }

            return productData;
        }

        /// <summary>
        /// Use SqlDataAdapter for sync offline DataTable with the database
        /// </summary>
        /// <param name="productData"></param>
        public int UpdateProduct(DataRow productData)
        {
            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(UPDATE_PRODUCT_SQL, conn);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

                // Use SqlCommandBuilder to generate Insert/Update/Delete commands 
                // for sync DataTable with the data in the database
                SqlCommandBuilder cmdBld = new SqlCommandBuilder(dataAdapter);
                dataAdapter.DeleteCommand = cmdBld.GetDeleteCommand();
                dataAdapter.InsertCommand = cmdBld.GetInsertCommand();
                dataAdapter.UpdateCommand = cmdBld.GetUpdateCommand();

                // Sync the database
                return dataAdapter.Update(new DataRow[] { productData });
            }
        }

        public DataTable GetSuppliers()
        {
            DataTable suppliersData = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SUPPLIERS_SQL, conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(suppliersData);
            }

            return suppliersData;
        }

        public DataTable GetCategories()
        {
            DataTable categoriesData = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(CATEGORIES_SQL, conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(categoriesData);
            }

            return categoriesData;
        }
    }
}
