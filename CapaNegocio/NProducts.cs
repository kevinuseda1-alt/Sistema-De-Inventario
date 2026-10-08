using CapaDatos;
using System;
using System.Data;
using System.Data.SqlClient;
namespace CapaNegocio
{
    public class NProducts
    {
        private readonly string connectionString;
        public NProducts()
        {
            connectionString = Conexion.Cn;
        }
        public static DataTable FiltrarPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            DProducts productos = new DProducts();
            return productos.FiltrarPorFecha(fechaInicio, fechaFin);
        }
        public static DataTable Mostrar()
        {
            try
            {
                DProducts product = new DProducts();
                return product.Mostrar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
        public static string Insertar(string product_name, short model_year, decimal price, byte[] imagen, int category_id, DateTime create_date, ref int product_id)
        {
            string rpta = "";
            SqlConnection con = new SqlConnection(Conexion.Cn);
            try
            {
                SqlCommand cmd = new SqlCommand("spinsertar_products", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@product_name", product_name);
                cmd.Parameters.AddWithValue("@model_year", model_year);
                cmd.Parameters.AddWithValue("@price", price);
                SqlParameter parImagen = cmd.Parameters.Add("@imagen", SqlDbType.Image);
                parImagen.Value = (object)imagen ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@category_id", category_id);
                cmd.Parameters.AddWithValue("@create_date", create_date);

                SqlParameter parProductId = new SqlParameter("@product_id", SqlDbType.Int);
                parProductId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parProductId);

                con.Open();
                rpta = cmd.ExecuteNonQuery() > 0 ? "OK" : "No se insertó el registro";
                product_id = Convert.ToInt32(parProductId.Value);
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return rpta;
        }
        public static string Editar(int product_id, string product_name, short model_year, decimal price, byte[] imagen, int category_id, DateTime create_date)
        {
            string rpta = "";
            SqlConnection con = new SqlConnection(Conexion.Cn);
            try
            {
                SqlCommand cmd = new SqlCommand("speditar_products", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@product_name", product_name);
                cmd.Parameters.AddWithValue("@model_year", model_year);
                cmd.Parameters.AddWithValue("@price", price);
                SqlParameter parImagen = cmd.Parameters.Add("@imagen", SqlDbType.Image);
                parImagen.Value = (object)imagen ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@category_id", category_id);
                cmd.Parameters.AddWithValue("@create_date", create_date);

                con.Open();
                rpta = cmd.ExecuteNonQuery() > 0 ? "OK" : "No se actualizó el registro";
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return rpta;
        }
        public static string Eliminar(int product_id)
        {
            string rpta = "";
            SqlConnection con = new SqlConnection(Conexion.Cn);
            try
            {
                SqlCommand cmd = new SqlCommand("speliminar_products", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@product_id", product_id);

                con.Open();
                rpta = cmd.ExecuteNonQuery() > 0 ? "OK" : "No se eliminó el registro";
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return rpta;
        }
        public static DataTable Buscar(string textoBuscar)
        {
            try
            {
                DProducts product = new DProducts();
                return product.Buscar(textoBuscar);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
    }
}