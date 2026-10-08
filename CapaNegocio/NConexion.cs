using CapaDatos;

namespace CapaNegocio
{
    public static class NConexion
    {
        public static string Cadena { get { return Conexion.Cn; } }
    }
}
