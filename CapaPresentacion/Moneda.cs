using System.Globalization;

namespace CapaPresentacion
{
    internal static class Moneda
    {
        public static CultureInfo Cultura
        {
            get
            {
                var cultura=(CultureInfo)CultureInfo.GetCultureInfo("es-NI").Clone();
                cultura.NumberFormat.CurrencySymbol="C$";
                return cultura;
            }
        }
        public static string Formatear(decimal importe)
        {
            return "C$ " + importe.ToString("N2", Cultura);
        }
    }
}
