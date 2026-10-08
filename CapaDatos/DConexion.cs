using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class Conexion
    {
        // Se configura en CapaPresentacion/App.config para cada computadora.
        public static string Cn
        {
            get
            {
                var item = ConfigurationManager.ConnectionStrings["BikeStore"];
                if (item == null || string.IsNullOrWhiteSpace(item.ConnectionString))
                    throw new InvalidOperationException("Falta BikeStore en App.config. Configurá la instancia SQL Server.");
                var builder = new SqlConnectionStringBuilder(item.ConnectionString);
                if (!string.Equals(builder.InitialCatalog, "Bike_Store", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("La conexión debe usar la base Bike_Store.");
                return builder.ConnectionString;
            }
        }
    }
}
