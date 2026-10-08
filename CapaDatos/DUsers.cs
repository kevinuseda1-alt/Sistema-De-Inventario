using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class DUsers
    {
        public DataTable ListarUsuarios()
        {
            DataTable tabla = new DataTable();
            SqlConnection conexion = new SqlConnection(Conexion.Cn);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_get_all_users", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
                return tabla;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar usuarios: " + ex.Message);
            }
        }

        public string InsertarUsuario(string username,string password,string full_name,string email,string role,out int user_id)
        {
            return CreateUser(username,password,full_name,email,role,out user_id);
        }
        public string CambiarContraseña(int user_id,string newPassword)
        {
            return ChangePassword(user_id,newPassword);
        }
        public DataTable VerifyUser(int userId, string email, string username)
        {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection conexion = new SqlConnection(Conexion.Cn);

            try
            {
                SqlCommand cmd = new SqlCommand("sp_verify_user", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@username", username);
                conexion.Open();
                resultado = cmd.ExecuteReader();
                tabla.Load(resultado);
                return tabla;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar usuario: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open) conexion.Close();
            }
        }
        public string ChangePassword(int userId,string newPassword)
        {
            try
            {
                if(string.IsNullOrEmpty(newPassword) || newPassword.Length<6)
                    throw new ArgumentException("La contraseña necesita 6 o más caracteres.");
                using(var con=new SqlConnection(Conexion.Cn))
                { con.Open();SeguridadClave.Actualizar(con,null,userId,newPassword); }
                return "Contraseña cambiada exitosamente";
            }
            catch(Exception ex) { return "Error al cambiar contraseña: "+ex.Message; }
        }
        public DataTable SearchUsers(string searchText)
        {
            DataTable tabla = new DataTable();
            SqlConnection conexion = new SqlConnection(Conexion.Cn);

            try
            {
                SqlCommand cmd = new SqlCommand("sp_search_users", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@search_text", searchText);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
                return tabla;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar usuarios: " + ex.Message);
            }
        }
        public string CreateUser(string username,string password,string fullName,string email,string role,out int userId)
        {
            userId=0;
            try
            {
                if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Usuario y correo son obligatorios.");
                if(string.IsNullOrEmpty(password) || password.Length<6)
                    throw new ArgumentException("La contraseña necesita 6 o más caracteres.");
                using(var con=new SqlConnection(Conexion.Cn))
                { 
                    con.Open();
                    using(var tran=con.BeginTransaction())
                    {
                        try
                        {
                            using(var cmd=new SqlCommand(@"INSERT dbo.users(username,password,full_name,email,role)
                                VALUES(@u,@placeholder,@n,@e,@r);SELECT CONVERT(INT,SCOPE_IDENTITY());",con,tran))
                            {
                                cmd.Parameters.Add("@u",SqlDbType.VarChar,50).Value=username.Trim();
                                cmd.Parameters.Add("@placeholder",SqlDbType.VarChar,255).Value=Guid.NewGuid().ToString("N");
                                cmd.Parameters.Add("@n",SqlDbType.VarChar,100).Value=fullName;
                                cmd.Parameters.Add("@e",SqlDbType.VarChar,100).Value=email.Trim();
                                cmd.Parameters.Add("@r",SqlDbType.VarChar,20).Value=role;
                                userId=Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            SeguridadClave.Actualizar(con,tran,userId,password);
                            tran.Commit();
                        }
                        catch { tran.Rollback();throw; }
                    }
                }
                return "Usuario creado exitosamente";
            }
            catch(Exception ex) { userId=0;return "Error al crear usuario: "+ex.Message; }
        }
        public string DeleteUser(int userId)
        {
            SqlConnection conexion = new SqlConnection(Conexion.Cn);

            try
            {
                SqlCommand cmd = new SqlCommand("sp_delete_user", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_id", userId);

                conexion.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0
                    ? "Usuario eliminado correctamente"
                    : "No se encontró el usuario a eliminar";
            }
            catch (Exception ex)
            {
                return "Error al eliminar usuario: " + ex.Message;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open) conexion.Close();
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
