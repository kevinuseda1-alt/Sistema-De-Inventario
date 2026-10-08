using CapaDatos;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
namespace CapaNegocio
{
    public class NUsers
    {
        private readonly DUsers dUsers = new DUsers();
        public DataTable ListarUsuarios()
        {
            return dUsers.ListarUsuarios();
        }
        public string CrearUsuario(string username, string password, string full_name, string email, string role, out int user_id)
        {
            return dUsers.InsertarUsuario(username, password, full_name, email, role, out user_id);
        }
        public string CambiarContraseña(int user_id, string newPassword)
        {
            return new DUsers().CambiarContraseña(user_id, newPassword);
        }
        public DataTable VerifyUser(int userId, string email, string username)
        {
            return dUsers.VerifyUser(userId, email, username);
        }
        public string ChangePassword(int userId, string newPassword)
        {
            return dUsers.ChangePassword(userId, newPassword);
        }
        public DataTable SearchUsers(string searchText)
        {
            return dUsers.SearchUsers(searchText);
        }
        public string CreateUser(string username, string password, string fullName,
                       string email, string role, out int userId)
        {
            return dUsers.CreateUser(username, password, fullName, email, role, out userId);
        }
        public string DeleteUser(int userId)
        {
            return dUsers.DeleteUser(userId);
        }
        public int GetNextUserId()
        {
            DataTable dt = ListarUsuarios();
            if (dt.Rows.Count == 0) return 1;

            int maxId = dt.AsEnumerable().Max(row => Convert.ToInt32(row["user_id"]));
            return maxId + 1;
        }
        public int ObtenerSiguienteId()
        {
            try
            {
                DataTable dt = ListarUsuarios();
                if (dt.Rows.Count == 0) return 1;

                int maxId = dt.AsEnumerable()
                             .Max(row => Convert.ToInt32(row["user_id"]));
                return maxId + 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener siguiente ID: " + ex.Message);
            }
        }
        public string UpdateUser(int userId, string username, string fullName, string email, string role, bool isActive)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.Cn))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_update_user", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@full_name", fullName);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@role", role);
                        command.Parameters.AddWithValue("@is_active", isActive);
                        command.ExecuteNonQuery();
                        return "Usuario actualizado correctamente";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al actualizar usuario: " + ex.Message;
            }
        }
    }
}