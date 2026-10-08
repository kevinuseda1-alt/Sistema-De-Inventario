using System.Data;

namespace CapaPresentacion.Forms
{
    internal class ReportDataSource
    {
        private string v;
        private DataTable dataTable;

        public ReportDataSource(string v, DataTable dataTable)
        {
            this.v = v;
            this.dataTable = dataTable;
        }
    }
}