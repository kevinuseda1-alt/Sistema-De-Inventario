using System;
using System.Data;
using CapaDatos;

namespace CapaNegocio
{
    public class NProveedores
    {
        private readonly DProveedores datos=new DProveedores();
        public DataTable Listar(string texto) { return datos.Listar(texto??""); }
        public DataTable Productos() { return datos.Productos(); }
        public DataTable ProductosDe(int id) { return datos.ProductosDe(id); }
        public int Guardar(int? id,string nombre,string contacto,string telefono,string correo,string direccion)
        {
            if(string.IsNullOrWhiteSpace(nombre))throw new ArgumentException("Ingresá el nombre del proveedor.");
            return datos.Guardar(id,nombre.Trim(),Normalizar(contacto),Normalizar(telefono),Normalizar(correo),Normalizar(direccion));
        }
        private static string Normalizar(string s) { return string.IsNullOrWhiteSpace(s)?null:s.Trim(); }
        public void Desactivar(int id) { datos.Desactivar(id); }
        public void Vincular(int proveedor,int producto) { datos.Vincular(proveedor,producto); }
    }
}
