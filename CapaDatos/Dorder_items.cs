using System;
using System.Data;
using System.Data.SqlClient;
namespace CapaDatos
{
    public class Dorder_items
    {
        public int Order_item_id { get; set; }
        public int Order_id { get; set; }
        public int Product_id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public Dorder_items() { }
        public string InsertarOrderItem(int order_id, int product_id, int quantity, decimal price, decimal discount)
        {
            string rpta = "";
            SqlConnection sqlCon = new SqlConnection(Conexion.Cn);
            try
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("spinsertar_order_items", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@order_id", order_id);
                sqlCmd.Parameters.AddWithValue("@product_id", product_id);
                sqlCmd.Parameters.AddWithValue("@quantity", quantity);
                sqlCmd.Parameters.AddWithValue("@price", price);
                sqlCmd.Parameters.AddWithValue("@discount", discount);
                rpta = sqlCmd.ExecuteNonQuery() == 1 ? "OK" : "No se insertó el registro";
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return rpta;
        }
        public DataTable MostrarOrderItems(int order_id)
        {
            DataTable dtResult = new DataTable();
            SqlConnection sqlCon = new SqlConnection(Conexion.Cn);
            try
            {
                SqlCommand sqlCmd = new SqlCommand("spmostrar_order_items", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@textobuscar", order_id.ToString());
                SqlDataAdapter sqlDat = new SqlDataAdapter(sqlCmd);
                sqlDat.Fill(dtResult);
            }
            catch (Exception ex)
            {
                dtResult = null;
                throw new Exception("Error al mostrar items: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return dtResult;
        }
    }
}