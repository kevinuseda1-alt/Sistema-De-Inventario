using CapaNegocio;
using System;
using System.Windows.Forms;
namespace PedidosApp
{
    public partial class FrmVistaCliente : Form
    {
        public string IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public FrmVistaCliente()
        {
            InitializeComponent();
        }
        private void FrmVistaCliente_Load(object sender, EventArgs e)
        {
            Mostrar();
        }
        private void Mostrar()
        {
            try
            {
                dataListado.DataSource = NCustomers.Mostrar();
                OcultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void OcultarColumnas()
        {
            dataListado.Columns[0].Visible = false;
        }
        private void dataListado_DoubleClick(object sender, EventArgs e)
        {
            if (dataListado.Rows.Count == 0)
            {
                return;
            }

            try
            {
                IdCliente = dataListado.CurrentRow.Cells["customer_id"].Value.ToString();
                NombreCliente = dataListado.CurrentRow.Cells["first_name"].Value.ToString() + " " +
                               dataListado.CurrentRow.Cells["last_name"].Value.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar cliente: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dataListado.DataSource = NCustomers.Buscar(txtBuscar.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}