using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace CapaDatos
{
    public static class SeguridadClave
    {
        private static byte[] Calcular(string clave,byte[] sal)
        {
            using(var derivador=new Rfc2898DeriveBytes(clave,sal,100000,HashAlgorithmName.SHA256))
                return derivador.GetBytes(64);
        }
        public static bool Verificar(string clave,byte[] hash,byte[] sal)
        {
            byte[] recibido=Calcular(clave,sal);
            int diferencia=hash.Length ^ recibido.Length;
            for(int i=0;i<Math.Min(hash.Length,recibido.Length);i++)diferencia|=hash[i]^recibido[i];
            return diferencia==0;
        }
        public static void Actualizar(SqlConnection con,SqlTransaction tran,int usuarioId,string clave)
        {
            if(string.IsNullOrEmpty(clave))throw new ArgumentException("La contraseña está vacía.");
            byte[] sal=new byte[32];
            using(var rng=RandomNumberGenerator.Create())rng.GetBytes(sal);
            byte[] hash=Calcular(clave,sal);
            using(var cmd=new SqlCommand(@"UPDATE dbo.users SET password_hash=@hash,password_salt=@sal,
                   password=@marcador WHERE user_id=@id",con,tran))
            {
                cmd.Parameters.Add("@hash",SqlDbType.VarBinary,64).Value=hash;
                cmd.Parameters.Add("@sal",SqlDbType.VarBinary,32).Value=sal;
                cmd.Parameters.Add("@marcador",SqlDbType.VarChar,255).Value=Guid.NewGuid().ToString("N");
                cmd.Parameters.Add("@id",SqlDbType.Int).Value=usuarioId;
                if(cmd.ExecuteNonQuery()!=1)throw new InvalidOperationException("Usuario inexistente.");
            }
        }
    }
}
