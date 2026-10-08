using System;
using System.Data;
using System.Data.SqlClient;
namespace CapaDatos
{
    public class DProducts
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public short ModelYear { get; set; }
        public decimal Price { get; set; }
        public byte[] Imagen { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Insertar(DProducts prod, ref int idGenerado)
        {
            string rpta = "";
            using (SqlConnection SqlCon = new SqlConnection(Conexion.Cn))
            {
                try
                {
                    SqlCon.Open();
                    SqlCommand cmd = new SqlCommand("spinsertar_products", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    SqlParameter parId = new SqlParameter("@product_id", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(parId);
                    cmd.Parameters.AddWithValue("@product_name", prod.ProductName);
                    cmd.Parameters.AddWithValue("@model_year", prod.ModelYear);
                    cmd.Parameters.AddWithValue("@price", prod.Price);
                    SqlParameter parImagen = cmd.Parameters.Add("@imagen", SqlDbType.Image);
                    parImagen.Value = (object)prod.Imagen ?? DBNull.Value;
                    cmd.Parameters.AddWithValue("@category_id", prod.CategoryId);
                    cmd.Parameters.AddWithValue("@create_date", prod.CreateDate);
                    rpta = cmd.ExecuteNonQuery() == 1 ? "OK" : "No se insertó el registro";
                    idGenerado = Convert.ToInt32(cmd.Parameters["@product_id"].Value);
                }
                catch (Exception ex) { rpta = ex.Message; }
            }
            return rpta;
        }
        public DataTable FiltrarPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            DataTable tabla = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(Conexion.Cn))
            {
                try
                {
                    SqlCommand comando = new SqlCommand("sp_filtrar_productos_por_fecha", sqlCon);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add("@fecha_inicio", SqlDbType.DateTime).Value = fechaInicio;
                    comando.Parameters.Add("@fecha_fin", SqlDbType.DateTime).Value = fechaFin;

                    SqlDataAdapter da = new SqlDataAdapter(comando);
                    da.Fill(tabla);
                    return tabla;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al filtrar productos por fecha: " + ex.Message);
                }
            }
        }
        public string Editar(DProducts prod)
        {
            string rpta = "";
            using (SqlConnection SqlCon = new SqlConnection(Conexion.Cn))
            {
                try
                {
                    SqlCon.Open();
                    SqlCommand cmd = new SqlCommand("speditar_products", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@product_id", prod.ProductId);
                    cmd.Parameters.AddWithValue("@product_name", prod.ProductName);
                    cmd.Parameters.AddWithValue("@model_year", prod.ModelYear);
                    cmd.Parameters.AddWithValue("@price", prod.Price);
                    SqlParameter parImagen = cmd.Parameters.Add("@imagen", SqlDbType.Image);
                    parImagen.Value = (object)prod.Imagen ?? DBNull.Value;
                    cmd.Parameters.AddWithValue("@category_id", prod.CategoryId);
                    cmd.Parameters.AddWithValue("@create_date", prod.CreateDate);

                    rpta = cmd.ExecuteNonQuery() == 1 ? "OK" : "No se actualizó";
                }
                catch (Exception ex) { rpta = ex.Message; }
            }
            return rpta;
        }
        public string Eliminar(int id)
        {
            string rpta = "";
            using (SqlConnection SqlCon = new SqlConnection(Conexion.Cn))
            {
                try
                {
                    SqlCon.Open();
                    SqlCommand cmd = new SqlCommand("speliminar_products", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@product_id", id);

                    rpta = cmd.ExecuteNonQuery() == 1 ? "OK" : "No se eliminó";
                }
                catch (Exception ex) { rpta = ex.Message; }
            }
            return rpta;
        }
        public DataTable Mostrar()
        {
            DataTable dtResult = new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon.ConnectionString = Conexion.Cn;
                SqlCommand sqlCmd = new SqlCommand("spmostrar_products", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDat = new SqlDataAdapter(sqlCmd);
                sqlDat.Fill(dtResult);
            }
            catch (Exception ex)
            {
                dtResult = null;
                throw new Exception("Error al mostrar productos: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return dtResult;
        }
        public DataTable BuscarNombre(string textBuscar)
        {
            DataTable DtResultado = new DataTable("products");
            using (SqlConnection SqlCon = new SqlConnection(Conexion.Cn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spbuscar_products", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@textobuscar", textBuscar);
                    SqlDataAdapter SqlDat = new SqlDataAdapter(cmd);
                    SqlDat.Fill(DtResultado);
                }
                catch { DtResultado = null; }
            }
            return DtResultado;
        }
        public DataTable Buscar(string textoBuscar)
        {
            DataTable dtResult = new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon.ConnectionString = Conexion.Cn;
                SqlCommand sqlCmd = new SqlCommand("spbuscar_products", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@textobuscar", textoBuscar);
                SqlDataAdapter sqlDat = new SqlDataAdapter(sqlCmd);
                sqlDat.Fill(dtResult);
            }
            catch (Exception ex)
            {
                dtResult = null;
                throw new Exception("Error al buscar productos: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return dtResult;
        }
    }
}