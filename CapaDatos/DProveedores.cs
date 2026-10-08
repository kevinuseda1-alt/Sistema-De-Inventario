using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class DProveedores
    {
        public DataTable Listar(string texto)
        {
            using (var con = new SqlConnection(Conexion.Cn))
            using (var cmd = new SqlCommand(@"SELECT supplier_id,supplier_name,contact_name,phone,email,address,is_active
                FROM dbo.suppliers WHERE supplier_name LIKE @texto ORDER BY supplier_name", con))
            {
                cmd.Parameters.Add("@texto",SqlDbType.NVarChar,120).Value="%"+texto.Trim()+"%";
                using(var da=new SqlDataAdapter(cmd)) { var dt=new DataTable();da.Fill(dt);return dt; }
            }
        }
        public DataTable Productos()
        {
            using(var con=new SqlConnection(Conexion.Cn))
            using(var da=new SqlDataAdapter("SELECT product_id,product_name FROM dbo.products ORDER BY product_name",con))
            { var dt=new DataTable();da.Fill(dt);return dt; }
        }
        public DataTable ProductosDe(int proveedor)
        {
            using(var con=new SqlConnection(Conexion.Cn))
            using(var cmd=new SqlCommand(@"SELECT p.product_name FROM dbo.product_suppliers ps
                JOIN dbo.products p ON p.product_id=ps.product_id WHERE ps.supplier_id=@id ORDER BY p.product_name",con))
            {
                cmd.Parameters.Add("@id",SqlDbType.Int).Value=proveedor;
                using(var da=new SqlDataAdapter(cmd)) { var dt=new DataTable();da.Fill(dt);return dt; }
            }
        }
        public int Guardar(int? id,string nombre,string contacto,string telefono,string correo,string direccion)
        {
            string sql=id.HasValue
                ? @"UPDATE dbo.suppliers SET supplier_name=@nombre,contact_name=@contacto,phone=@telefono,
                    email=@correo,address=@direccion WHERE supplier_id=@id;SELECT @id;"
                : @"INSERT dbo.suppliers(supplier_name,contact_name,phone,email,address)
                    VALUES(@nombre,@contacto,@telefono,@correo,@direccion);SELECT CONVERT(INT,SCOPE_IDENTITY());";
            using(var con=new SqlConnection(Conexion.Cn))
            using(var cmd=new SqlCommand(sql,con))
            {
                cmd.Parameters.Add("@nombre",SqlDbType.NVarChar,120).Value=nombre;
                cmd.Parameters.Add("@contacto",SqlDbType.NVarChar,120).Value=(object)contacto??DBNull.Value;
                cmd.Parameters.Add("@telefono",SqlDbType.NVarChar,30).Value=(object)telefono??DBNull.Value;
                cmd.Parameters.Add("@correo",SqlDbType.NVarChar,150).Value=(object)correo??DBNull.Value;
                cmd.Parameters.Add("@direccion",SqlDbType.NVarChar,250).Value=(object)direccion??DBNull.Value;
                if(id.HasValue)cmd.Parameters.Add("@id",SqlDbType.Int).Value=id.Value;
                con.Open();return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public void Desactivar(int id)
        {
            using(var con=new SqlConnection(Conexion.Cn))
            using(var cmd=new SqlCommand("UPDATE dbo.suppliers SET is_active=0 WHERE supplier_id=@id",con))
            { cmd.Parameters.Add("@id",SqlDbType.Int).Value=id;con.Open();cmd.ExecuteNonQuery(); }
        }
        public void Vincular(int proveedor,int producto)
        {
            using(var con=new SqlConnection(Conexion.Cn))
            using(var cmd=new SqlCommand(@"IF NOT EXISTS(SELECT 1 FROM dbo.product_suppliers WHERE supplier_id=@s AND product_id=@p)
                INSERT dbo.product_suppliers(supplier_id,product_id) VALUES(@s,@p)",con))
            { cmd.Parameters.Add("@s",SqlDbType.Int).Value=proveedor;cmd.Parameters.Add("@p",SqlDbType.Int).Value=producto;
              con.Open();cmd.ExecuteNonQuery(); }
        }
    }
}
