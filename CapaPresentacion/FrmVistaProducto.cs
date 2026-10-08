using CapaNegocio;
using System;
using System.Windows.Forms;
namespace PedidosApp
{
    public partial class FrmVistaProducto : Form
    {
        public string IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }

        public FrmVistaProducto()
        {
            InitializeComponent();
        }
        private void FrmVistaProducto_Load(object sender, EventArgs e)
        {
            MostrarProductos();
            OcultarColumnas();
        }
        private void MostrarProductos()
        {
            try
            {
                dataListado.DataSource = NProducts.Mostrar();
                lblTotal.Text = "Total productos: " + dataListado.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OcultarColumnas()
        {
            dataListado.Columns["product_id"].Visible = false;
            dataListado.Columns["imagen"].Visible = false;
            dataListado.Columns["category_id"].Visible = false;
            dataListado.Columns["create_date"].Visible = false;
        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dataListado.DataSource = NProducts.Buscar(txtBuscar.Text);
                lblTotal.Text = "Total productos: " + dataListado.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataListado_DoubleClick(object sender, EventArgs e)
        {
            if (dataListado.Rows.Count == 0)
            {
                return;
            }

            IdProducto = dataListado.CurrentRow.Cells["product_id"].Value.ToString();
            NombreProducto = dataListado.CurrentRow.Cells["product_name"].Value.ToString();
            Precio = Convert.ToDecimal(dataListado.CurrentRow.Cells["price"].Value);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void dataListado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}