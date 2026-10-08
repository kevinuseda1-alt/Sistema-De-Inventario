using CapaDatos;
using System;
using System.Data;
namespace CapaNegocio
{
    public class NCustomers
    {
        private readonly DCustomers _dataLayer;
        public NCustomers()
        {
            _dataLayer = new DCustomers();
        }
        public DataTable GetCustomers()
        {
            return _dataLayer.ShowCustomers();
        }
        public void AddCustomer(int customerId, string firstName, string lastName,
                              string phone, string email, string street,
                              string city, string state)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException("El nombre es requerido");
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("El email es requerido");
            _dataLayer.InsertCustomer(customerId, firstName, lastName, phone,
                                    email, street, city, state);
        }
        public void UpdateCustomer(int customerId, string firstName, string lastName,
                                 string phone, string email, string street,
                                 string city, string state)
        {
            if (customerId <= 0)
                throw new ArgumentException("ID de cliente inválido");

            _dataLayer.UpdateCustomer(customerId, firstName, lastName, phone,
                                    email, street, city, state);
        }
        public void DeleteCustomer(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("ID de cliente inválido");

            _dataLayer.DeleteCustomer(customerId);
        }
        public DataTable SearchCustomers(string columnName, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
                return GetCustomers();

            return _dataLayer.SearchCustomers(columnName, searchValue);
        }
        public static DataTable Mostrar()
        {
            try
            {
                DCustomers customer = new DCustomers();
                return customer.Mostrar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
        public static DataTable Buscar(string textoBuscar)
        {
            try
            {
                DCustomers customer = new DCustomers();
                return customer.Buscar(textoBuscar);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa negocio: " + ex.Message);
            }
        }
    }
}