using CapaDatos;
using System;
using System.Data;
using System.Globalization;
namespace CapaNegocio
{
    public class Norders
    {
        public static DataTable Mostrar()
        {
            try
            {
                Dorders dOrders = new Dorders();
                return dOrders.Mostrar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
        public static DataTable BuscarFecha(string fecha1, string fecha2)
        {
            try
            {
                Dorders dOrders = new Dorders();
                DateTime dt1 = DateTime.ParseExact(fecha1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dt2 = DateTime.ParseExact(fecha2, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dt2 = dt2.AddDays(1).AddSeconds(-1);

                return dOrders.BuscarFecha(
                    dt1.ToString("yyyy-MM-dd HH:mm:ss"),
                    dt2.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
        public static DataTable MostrarDetalle(string order_id)
        {
            try
            {
                Dorders dOrders = new Dorders();
                return dOrders.MostrarDetalle(order_id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
        public static string Insertar(int customer_id, int user_id, DataTable dtDetalle)
        {
            try
            {
                Dorders order = new Dorders();
                order.Customer_id = customer_id;
                order.User_id = user_id;
                Dorder_items[] items = new Dorder_items[dtDetalle.Rows.Count];
                for (int i = 0; i < dtDetalle.Rows.Count; i++)
                {
                    items[i] = new Dorder_items();
                    items[i].Product_id = Convert.ToInt32(dtDetalle.Rows[i]["product_id"]);
                    items[i].Quantity = Convert.ToInt32(dtDetalle.Rows[i]["quantity"]);
                    items[i].Price = Convert.ToDecimal(dtDetalle.Rows[i]["price"]);
                    items[i].Discount = Convert.ToDecimal(dtDetalle.Rows[i]["discount"]);
                }
                return order.Insertar(order, items);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
        public static string Eliminar(int order_id)
        {
            try
            {
                Dorders dOrders = new Dorders();
                return dOrders.Eliminar(order_id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
        public static DataTable ObtenerCabeceraFactura(int orderId)
        {
            try
            {
                Dorders dOrders = new Dorders();
                return dOrders.ObtenerCabeceraFactura(orderId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }

        public static DataTable ObtenerDetalleFactura(int orderId)
        {
            try
            {
                Dorders dOrders = new Dorders();
                return dOrders.ObtenerDetalleFactura(orderId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
    }
}