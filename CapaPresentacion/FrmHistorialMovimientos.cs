using CapaNegocio;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PedidosApp
{
    public partial class FrmHistorialMovimientos : Form
    {
        public FrmHistorialMovimientos(int productId, string productName)
        {
            InitializeComponent();
            Text = $"Historial de Movimientos - {productName}";
            CargarMovimientos(productId);
            ConfigurarEstilos();
        }

        private void CargarMovimientos(int productId)
        {
            try
            {
                DataTable dt = NInventario.ObtenerMovimientosProducto(productId);
                dgvMovimientos.DataSource = dt;
                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar movimientos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvMovimientos.Columns.Contains("movement_date"))
            {
                dgvMovimientos.Columns["movement_date"].HeaderText = "Fecha";
                dgvMovimientos.Columns["movement_date"].Width = 120;
            }

            if (dgvMovimientos.Columns.Contains("movement_type"))
            {
                dgvMovimientos.Columns["movement_type"].HeaderText = "Tipo";
                dgvMovimientos.Columns["movement_type"].Width = 80;
            }

            if (dgvMovimientos.Columns.Contains("quantity"))
            {
                dgvMovimientos.Columns["quantity"].HeaderText = "Cantidad";
                dgvMovimientos.Columns["quantity"].Width = 80;
                dgvMovimientos.Columns["quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvMovimientos.Columns.Contains("usuario"))
            {
                dgvMovimientos.Columns["usuario"].HeaderText = "Usuario";
                dgvMovimientos.Columns["usuario"].Width = 120;
            }

            if (dgvMovimientos.Columns.Contains("referencia"))
            {
                dgvMovimientos.Columns["referencia"].HeaderText = "Referencia";
                dgvMovimientos.Columns["referencia"].Width = 100;
            }

            if (dgvMovimientos.Columns.Contains("notes"))
            {
                dgvMovimientos.Columns["notes"].HeaderText = "Notas";
                dgvMovimientos.Columns["notes"].Width = 200;
            }
        }

        private void ConfigurarEstilos()
        {
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.ForeColor = Color.White;

            dgvMovimientos.BackgroundColor = Color.FromArgb(60, 60, 60);
            dgvMovimientos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvMovimientos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMovimientos.RowsDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvMovimientos.RowsDefaultCellStyle.ForeColor = Color.White;

            btnCerrar.BackColor = Color.FromArgb(70, 70, 70);
            btnCerrar.FlatAppearance.BorderColor = Color.Gray;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.White;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}