using CapaDatos;
using System;
using System.Data;

namespace CapaNegocio
{
    public class NInventario
    {
        public static void GuardarStockMinimo(int productId,int minimo)
        {
            if(minimo<0)throw new ArgumentException("El stock mínimo no puede ser negativo.");
            new DInventario().GuardarStockMinimo(productId,minimo);
        }
        public static DataTable ObtenerStockActual()
        {
            try
            {
                DInventario inventario = new DInventario();
                return inventario.ObtenerStockActual();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener stock: " + ex.Message);
            }
        }

        public static string RegistrarEntrada(int product_id, int cantidad, int user_id, string notas = null)
        {
            try
            {
                DInventario inventario = new DInventario();
                return inventario.RegistrarMovimiento(product_id, cantidad, "ENTRADA", user_id, null, notas);
            }
            catch (Exception ex)
            {
                return "Error al registrar entrada: " + ex.Message;
            }
        }

        public static string RegistrarSalida(int product_id, int cantidad, int user_id, int? order_id = null, string notas = null)
        {
            try
            {
                DInventario inventario = new DInventario();
                return inventario.RegistrarMovimiento(product_id, cantidad, "SALIDA", user_id, order_id, notas);
            }
            catch (Exception ex)
            {
                return "Error al registrar salida: " + ex.Message;
            }
        }

        public static DataTable ObtenerMovimientosProducto(int product_id)
        {
            try
            {
                DInventario inventario = new DInventario();
                return inventario.ObtenerMovimientosProducto(product_id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener movimientos: " + ex.Message);
            }
        }

        public static int ObtenerStockDisponible(int product_id)
        {
            try
            {
                DInventario inventario = new DInventario();
                return inventario.ObtenerStockDisponible(product_id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener stock disponible: " + ex.Message);
            }
        }

    }
}
