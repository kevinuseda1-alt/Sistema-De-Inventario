using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class DInventario
    {
        public void GuardarStockMinimo(int productId,int minimo)
        {
            using(var con=new SqlConnection(Conexion.Cn))
            using(var cmd=new SqlCommand("UPDATE dbo.products SET min_stock=@min WHERE product_id=@id AND is_active=1",con))
            {
                cmd.Parameters.Add("@min",SqlDbType.Int).Value=minimo;
                cmd.Parameters.Add("@id",SqlDbType.Int).Value=productId;
                con.Open();if(cmd.ExecuteNonQuery()!=1)throw new InvalidOperationException("Producto inexistente o inactivo.");
            }
        }
        public DataTable ObtenerStockActual()
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cn))
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT product_id,product_name,price,min_stock,stock_actual
                      FROM dbo.vw_inventory_summary ORDER BY product_name", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public string RegistrarMovimiento(int product_id, int quantity, string movement_type, int user_id, int? order_id = null, string notes = null)
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_registrar_movimiento_inventario", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@movement_type", movement_type);
                cmd.Parameters.AddWithValue("@user_id", user_id);

                if (order_id.HasValue)
                    cmd.Parameters.AddWithValue("@order_id", order_id);
                else
                    cmd.Parameters.AddWithValue("@order_id", DBNull.Value);

                if (!string.IsNullOrEmpty(notes))
                    cmd.Parameters.AddWithValue("@notes", notes);
                else
                    cmd.Parameters.AddWithValue("@notes", DBNull.Value);

                cmd.ExecuteNonQuery();
                return "OK";
            }
        }

        public DataTable ObtenerMovimientosProducto(int product_id)
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cn))
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT i.movement_date, i.movement_type, i.quantity, 
                      u.username AS usuario, i.notes,
                      CASE WHEN i.order_id IS NOT NULL THEN 'Pedido #' + CAST(i.order_id AS VARCHAR) 
                           ELSE '' END AS referencia
                      FROM inventory i
                      INNER JOIN users u ON i.user_id = u.user_id
                      WHERE i.product_id = @product_id
                      ORDER BY i.movement_date DESC", con);

                cmd.Parameters.AddWithValue("@product_id", product_id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public int ObtenerStockDisponible(int product_id)
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cn))
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT ISNULL(SUM(CASE WHEN movement_type = 'ENTRADA' THEN quantity 
                                        WHEN movement_type = 'SALIDA' THEN -quantity 
                                        ELSE 0 END), 0)
                      FROM inventory
                      WHERE product_id = @product_id", con);

                cmd.Parameters.AddWithValue("@product_id", product_id);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
