using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class DLogin
    {
        public DataTable Login(string username,string password)
        {
            var tabla=new DataTable();
            using(var conexion=new SqlConnection(Conexion.Cn))
            using(var cmd=new SqlCommand(@"SELECT user_id,username,full_name,email,role,
                password AS clave_legacy,password_hash,password_salt
                FROM dbo.users WHERE username=@usuario AND is_active=1",conexion))
            {
                cmd.Parameters.Add("@usuario",SqlDbType.VarChar,50).Value=(username??"").Trim();
                conexion.Open();
                using(var da=new SqlDataAdapter(cmd))da.Fill(tabla);
                if(tabla.Rows.Count==0)return tabla;
                DataRow fila=tabla.Rows[0];
                bool migrar=fila.IsNull("password_hash") || fila.IsNull("password_salt");
                bool correcto=migrar
                    ? string.Equals(Convert.ToString(fila["clave_legacy"]),password,StringComparison.Ordinal)
                    : SeguridadClave.Verificar(password,(byte[])fila["password_hash"],(byte[])fila["password_salt"]);
                if(!correcto) { tabla.Clear();return tabla; }
                if(migrar)
                {
                    using(var tran=conexion.BeginTransaction())
                    {
                        try { SeguridadClave.Actualizar(conexion,tran,Convert.ToInt32(fila["user_id"]),password);tran.Commit(); }
                        catch { tran.Rollback();throw; }
                    }
                }
                tabla.Columns.Remove("clave_legacy");
                tabla.Columns.Remove("password_hash");
                tabla.Columns.Remove("password_salt");
                return tabla;
            }
        }
    }
}
