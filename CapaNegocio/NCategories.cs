using System.Data;
using CapaDatos;
namespace CapaNegocio
{
    public class NCategories
    {
        public static string Insertar(string categoryName, ref int idGenerado)
        {
            DCategories obj = new DCategories
            {
                CategoryName = categoryName
            };
            return obj.Insertar(obj, ref idGenerado);
        }
        public static string Editar(int id, string categoryName)
        {
            DCategories obj = new DCategories
            {
                CategoryId = id,
                CategoryName = categoryName
            };
            return obj.Editar(obj);
        }
        public static string Eliminar(int id)
        {
            return new DCategories().Eliminar(id);
        }
        public static DataTable Mostrar()
        {
            return new DCategories().Mostrar();
        }
        public static DataTable Buscar(string textBuscar)
        {
            return new DCategories().BuscarNombre(textBuscar);
        }
    }
}
