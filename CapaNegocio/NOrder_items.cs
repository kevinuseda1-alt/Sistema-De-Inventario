using CapaDatos;
using System;
using System.Data;
namespace CapaNegocio
{
    public class NOrder_items
    {
        private readonly Dorder_items dOrderItems = new Dorder_items();

        public DataTable MostrarOrderItems(int order_id)
        {
            try
            {
                return dOrderItems.MostrarOrderItems(order_id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
        public string Insertar(int order_id, int product_id, int quantity, decimal price, decimal discount)
        {
            try
            {
                if (quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero");
                if (price <= 0) throw new ArgumentException("El precio debe ser mayor a cero");
                if (discount < 0) throw new ArgumentException("El descuento no puede ser negativo");
                return dOrderItems.InsertarOrderItem(order_id, product_id, quantity, price, discount);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
    }
}