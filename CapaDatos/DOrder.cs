using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class Dorders
    {
        public int Order_id { get; set; }
        public int Customer_id { get; set; }
        public int User_id { get; set; }
        public DateTime Order_date { get; set; }

        public Dorders() { }

        public DataTable Mostrar()
        {
            DataTable dtResult = new DataTable();
            SqlConnection sqlCon = new SqlConnection();

            try
            {
                sqlCon.ConnectionString = Conexion.Cn;
                SqlCommand sqlCmd = new SqlCommand("spmostrar_order", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDat = new SqlDataAdapter(sqlCmd);
                sqlDat.Fill(dtResult);
            }
            catch (Exception ex)
            {
                dtResult = null;
                throw new Exception("Error al mostrar órdenes: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return dtResult;
        }

        public DataTable BuscarFecha(string fecha1, string fecha2)
        {
            DataTable dtResult = new DataTable();
            SqlConnection sqlCon = new SqlConnection();

            try
            {
                sqlCon.ConnectionString = Conexion.Cn;
                SqlCommand sqlCmd = new SqlCommand("spbuscar_order_fecha", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@textobuscar1", fecha1);
                sqlCmd.Parameters.AddWithValue("@textobuscar2", fecha2);
                SqlDataAdapter sqlDat = new SqlDataAdapter(sqlCmd);
                sqlDat.Fill(dtResult);
            }
            catch (Exception ex)
            {
                dtResult = null;
                throw new Exception("Error al buscar por fecha: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return dtResult;
        }

        public DataTable MostrarDetalle(string order_id)
        {
            DataTable dtResult = new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon.ConnectionString = Conexion.Cn;
                SqlCommand sqlCmd = new SqlCommand("spmostrar_order_items", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@textobuscar", order_id);
                SqlDataAdapter sqlDat = new SqlDataAdapter(sqlCmd);
                sqlDat.Fill(dtResult);
            }
            catch (Exception ex)
            {
                dtResult = null;
                throw new Exception("Error al mostrar detalle: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return dtResult;
        }

        public string Insertar(Dorders order, Dorder_items[] items)
        {
            string rpta = "";
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon.ConnectionString = Conexion.Cn;
                sqlCon.Open();
                SqlTransaction sqlTran = sqlCon.BeginTransaction();
                try
                {
                    SqlCommand sqlCmd = new SqlCommand("spinsertar_orders", sqlCon, sqlTran);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@customer_id", order.Customer_id);
                    sqlCmd.Parameters.AddWithValue("@user_id", order.User_id);

                    SqlParameter parOrder_id = new SqlParameter();
                    parOrder_id.ParameterName = "@order_id";
                    parOrder_id.SqlDbType = SqlDbType.Int;
                    parOrder_id.Direction = ParameterDirection.Output;
                    sqlCmd.Parameters.Add(parOrder_id);

                    sqlCmd.ExecuteNonQuery();
                    int order_id = Convert.ToInt32(sqlCmd.Parameters["@order_id"].Value);

                    foreach (Dorder_items item in items)
                    {
                        SqlCommand sqlCmdDet = new SqlCommand("spinsertar_order_items", sqlCon, sqlTran);
                        sqlCmdDet.CommandType = CommandType.StoredProcedure;
                        sqlCmdDet.Parameters.AddWithValue("@order_id", order_id);
                        sqlCmdDet.Parameters.AddWithValue("@product_id", item.Product_id);
                        sqlCmdDet.Parameters.AddWithValue("@quantity", item.Quantity);
                        sqlCmdDet.Parameters.AddWithValue("@price", item.Price);
                        sqlCmdDet.Parameters.AddWithValue("@discount", item.Discount);

                        SqlParameter parOrderItemId = new SqlParameter();
                        parOrderItemId.ParameterName = "@order_item_id";
                        parOrderItemId.SqlDbType = SqlDbType.Int;
                        parOrderItemId.Direction = ParameterDirection.Output;
                        sqlCmdDet.Parameters.Add(parOrderItemId);

                        sqlCmdDet.ExecuteNonQuery();

                        SqlCommand sqlCmdStock = new SqlCommand("sp_registrar_movimiento_inventario", sqlCon, sqlTran);
                        sqlCmdStock.CommandType = CommandType.StoredProcedure;
                        sqlCmdStock.Parameters.AddWithValue("@product_id", item.Product_id);
                        sqlCmdStock.Parameters.AddWithValue("@quantity", item.Quantity);
                        sqlCmdStock.Parameters.AddWithValue("@movement_type", "SALIDA");
                        sqlCmdStock.Parameters.AddWithValue("@user_id", order.User_id);
                        sqlCmdStock.Parameters.AddWithValue("@order_id", order_id);
                        sqlCmdStock.Parameters.AddWithValue("@notes", "Venta asociada a orden #" + order_id);
                        sqlCmdStock.ExecuteNonQuery();
                    }
                    sqlTran.Commit();
                    rpta = "OK";
                }
                catch (Exception ex)
                {
                    sqlTran.Rollback();
                    rpta = ex.Message;
                }
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

        public string Eliminar(int order_id)
        {
            string rpta = "";
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon.ConnectionString = Conexion.Cn;
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("speliminar_order", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@order_id", order_id);

                int filasAfectadas = sqlCmd.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    rpta = "OK";
                else
                    rpta = "No se eliminó el registro";
            }
            catch (SqlException sqlEx)
            {
                rpta = "Error SQL al eliminar el pedido: " + sqlEx.Message;
            }
            catch (Exception ex)
            {
                rpta = "Error general al eliminar el pedido: " + ex.Message;
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();
            }
            return rpta;
        }

        public DataTable ObtenerCabeceraFactura(int orderId)
        {
            DataTable dtResult = new DataTable();
            SqlConnection sqlCon = new SqlConnection();

            try
            {
                sqlCon.ConnectionString = Conexion.Cn;
                SqlCommand sqlCmd = new SqlCommand("sp_rpt_obtener_cabecera_pedido", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@pedido_id", orderId);
                SqlDataAdapter sqlDat = new SqlDataAdapter(sqlCmd);
                sqlDat.Fill(dtResult);
            }
            catch (Exception ex)
            {
                dtResult = null;
                throw new Exception("Error al obtener cabecera de factura: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return dtResult;
        }

        public DataTable ObtenerDetalleFactura(int orderId)
        {
            DataTable dtResult = new DataTable();
            SqlConnection sqlCon = new SqlConnection();

            try
            {
                sqlCon.ConnectionString = Conexion.Cn;
                SqlCommand sqlCmd = new SqlCommand("sp_rpt_obtener_detalle_pedido", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@pedido_id", orderId);
                SqlDataAdapter sqlDat = new SqlDataAdapter(sqlCmd);
                sqlDat.Fill(dtResult);
            }
            catch (Exception ex)
            {
                dtResult = null;
                throw new Exception("Error al obtener detalle de factura: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return dtResult;
        }
    }
}