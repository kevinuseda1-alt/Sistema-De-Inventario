using CapaDatos;
using System.Data;

namespace CapaNegocio
{
    public class NLogin
    {
        private readonly DLogin dLogin = new DLogin();
        public DataTable Login(string username, string password)
        {
            return dLogin.Login(username, password);
        }
    }
}