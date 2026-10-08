using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class DCategories
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public DCategories() { }

        public DCategories(int categoryId, string categoryName)
        {
            this.CategoryId = categoryId;
            this.CategoryName = categoryName;
        }

        public string Insertar(DCategories cat, ref int idGenerado)
        {
            string rpta = "";
            using (SqlConnection SqlCon = new SqlConnection(Conexion.Cn))
            {
                try
                {
                    SqlCon.Open();
                    SqlCommand cmd = new SqlCommand("spinsertar_categories", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlParameter parId = new SqlParameter("@category_id", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(parId);
                    cmd.Parameters.AddWithValue("@category_name", cat.CategoryName);

                    rpta = cmd.ExecuteNonQuery() == 1 ? "OK" : "No se insertó el registro";
                    idGenerado = Convert.ToInt32(cmd.Parameters["@category_id"].Value);
                }
                catch (Exception ex) { rpta = ex.Message; }
            }
            return rpta;
        }

        public string Editar(DCategories cat)
        {
            string rpta = "";
            using (SqlConnection SqlCon = new SqlConnection(Conexion.Cn))
            {
                try
                {
                    SqlCon.Open();
                    SqlCommand cmd = new SqlCommand("speditar_categories", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@category_id", cat.CategoryId);
                    cmd.Parameters.AddWithValue("@category_name", cat.CategoryName);

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
                    SqlCommand cmd = new SqlCommand("speliminar_categories", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@category_id", id);

                    rpta = cmd.ExecuteNonQuery() == 1 ? "OK" : "No se eliminó";
                }
                catch (Exception ex) { rpta = ex.Message; }
            }
            return rpta;
        }

        public DataTable Mostrar()
        {
            DataTable DtResultado = new DataTable("categories");
            using (SqlConnection SqlCon = new SqlConnection(Conexion.Cn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spmostrar_categories", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    SqlDataAdapter SqlDat = new SqlDataAdapter(cmd);
                    SqlDat.Fill(DtResultado);
                }
                catch { DtResultado = null; }
            }
            return DtResultado;
        }

        public DataTable BuscarNombre(string textBuscar)
        {
            DataTable DtResultado = new DataTable("categories");
            using (SqlConnection SqlCon = new SqlConnection(Conexion.Cn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spbuscar_categories", SqlCon)
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
    }
}
