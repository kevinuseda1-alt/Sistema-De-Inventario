using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class DCustomers
    {
        public DataTable ShowCustomers()
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cn))
            {
                SqlCommand cmd = new SqlCommand("spmostrar_customers", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public void InsertCustomer(int customerId, string firstName, string lastName,
                                string phone, string email, string street,
                                string city, string state)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cn))
            {
                SqlCommand cmd = new SqlCommand("spinsertar_customers", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@customer_id", customerId);
                cmd.Parameters.AddWithValue("@first_name", firstName);
                cmd.Parameters.AddWithValue("@last_name", lastName);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@street", street);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@state", state);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateCustomer(int customerId, string firstName, string lastName,
                                 string phone, string email, string street,
                                 string city, string state)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cn))
            {
                SqlCommand cmd = new SqlCommand("speditar_customers", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@customer_id", customerId);
                cmd.Parameters.AddWithValue("@first_name", firstName);
                cmd.Parameters.AddWithValue("@last_name", lastName);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@street", street);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@state", state);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteCustomer(int customerId)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cn))
            {
                SqlCommand cmd = new SqlCommand("speliminar_customers", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@customer_id", customerId);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public DataTable SearchCustomers(string columnName, string searchValue)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cn))
            {
                SqlCommand cmd = new SqlCommand("spbuscar_customers", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@column_name", columnName);
                cmd.Parameters.AddWithValue("@search_value", searchValue);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable Mostrar()
        {
            DataTable dtResult = new DataTable();
            SqlConnection sqlCon = new SqlConnection();

            try
            {
                sqlCon.ConnectionString = Conexion.Cn;
                SqlCommand sqlCmd = new SqlCommand("spmostrar_customers", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter sqlDat = new SqlDataAdapter(sqlCmd);
                sqlDat.Fill(dtResult);
            }
            catch (Exception ex)
            {
                dtResult = null;
                throw new Exception("Error al mostrar clientes: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return dtResult;
        }
        public DataTable Buscar(string textoBuscar)
        {
            DataTable dtResult = new DataTable();
            SqlConnection sqlCon = new SqlConnection();

            try
            {
                sqlCon.ConnectionString = Conexion.Cn;
                SqlCommand sqlCmd = new SqlCommand("spbuscar_customers", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@textobuscar", textoBuscar);

                SqlDataAdapter sqlDat = new SqlDataAdapter(sqlCmd);
                sqlDat.Fill(dtResult);
            }
            catch (Exception ex)
            {
                dtResult = null;
                throw new Exception("Error al buscar clientes: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return dtResult;
        }
    }
}