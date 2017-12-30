using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DAL
{
    public partial class Orders
    {
        private readonly string _connectionStr;

        public Orders(string connectionString)
        {
            _connectionStr = connectionString;
        }

        public DataTable GetAnnualReport()
        {
            DataTable annualReportData = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ANNUAL_REPORT_SQL, conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(annualReportData);
            }

            return annualReportData;
        }

        public DataTable GetOrdersByYear(int year)
        {
            DataTable ordersData = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ORDERS_SQL, conn);

                SqlParameter paramYear = new SqlParameter("@Year", year);
                cmd.Parameters.Add(paramYear);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(ordersData);
            }

            return ordersData;
        }

        public DataTable GetOrderItems(int orderID)
        {
            DataTable orderItemsData = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ORDER_ITEMS_SQL, conn);

                SqlParameter paramOrderId = new SqlParameter("@OrderID", orderID);
                cmd.Parameters.Add(paramOrderId);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(orderItemsData);
            }

            return orderItemsData;
        }
    }
}
