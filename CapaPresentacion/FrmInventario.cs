using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaNegocio;

namespace PedidosApp
{
    public partial class FrmInventario : Form
    {
        private int userId;

        public FrmInventario(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            CargarEstilos();
            dgvInventario.SelectionChanged += (s,e) =>
            {
                if(dgvInventario.CurrentRow!=null && dgvInventario.Columns.Contains("min_stock"))
                {
                    object valor=dgvInventario.CurrentRow.Cells["min_stock"].Value;
                    if(valor!=null && valor!=DBNull.Value)
                        numMinimo.Value=Math.Max(numMinimo.Minimum,Math.Min(numMinimo.Maximum,Convert.ToDecimal(valor)));
                }
            };
            MostrarInventario();
        }

        private void CargarEstilos()
        {
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.ForeColor = Color.White;

            dgvInventario.BackgroundColor = Color.FromArgb(60, 60, 60);
            dgvInventario.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvInventario.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvInventario.RowsDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvInventario.RowsDefaultCellStyle.ForeColor = Color.White;

            btnActualizar.BackColor = Color.FromArgb(70, 70, 70);
            btnEntrada.BackColor = Color.FromArgb(70, 70, 70);
            btnSalida.BackColor = Color.FromArgb(70, 70, 70);
            btnMovimientos.BackColor = Color.FromArgb(70, 70, 70);
        }

        private void MostrarInventario()
        {
            try
            {
                DataTable dt = NInventario.ObtenerStockActual();
                dgvInventario.DataSource = dt;
                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar inventario: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvInventario.Columns.Contains("product_id"))
            {
                dgvInventario.Columns["product_id"].HeaderText = "ID";
                dgvInventario.Columns["product_id"].Width = 50;
            }

            if (dgvInventario.Columns.Contains("product_name"))
            {
                dgvInventario.Columns["product_name"].HeaderText = "Producto";
                dgvInventario.Columns["product_name"].Width = 200;
            }

            if (dgvInventario.Columns.Contains("stock_actual"))
            {
                dgvInventario.Columns["stock_actual"].HeaderText = "Stock Actual";
                dgvInventario.Columns["stock_actual"].Width = 100;
                dgvInventario.Columns["stock_actual"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvInventario.CellFormatting += (sender, e) =>
                {
                    if (e.ColumnIndex == dgvInventario.Columns["stock_actual"].Index && e.Value != null)
                    {
                        int minimo=Convert.ToInt32(dgvInventario.Rows[e.RowIndex].Cells["min_stock"].Value);
                        if (Convert.ToInt32(e.Value) <= minimo)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(80, 0, 0);
                            e.CellStyle.ForeColor = Color.White;
                        }
                    }
                };
            }
            if(dgvInventario.Columns.Contains("price"))
            {
                dgvInventario.Columns["price"].HeaderText="Precio (C$)";
                dgvInventario.Columns["price"].DefaultCellStyle.Format="C2";
                dgvInventario.Columns["price"].DefaultCellStyle.FormatProvider=CapaPresentacion.Moneda.Cultura;
            }
            if(dgvInventario.Columns.Contains("min_stock"))
                dgvInventario.Columns["min_stock"].HeaderText="Stock mínimo";
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            MostrarInventario();
        }
        private void btnMinimo_Click(object sender,EventArgs e)
        {
            if(dgvInventario.CurrentRow==null)return;
            try
            {
                int id=Convert.ToInt32(dgvInventario.CurrentRow.Cells["product_id"].Value);
                NInventario.GuardarStockMinimo(id,(int)numMinimo.Value);
                MostrarInventario();
            }
            catch(Exception ex) { MessageBox.Show("No se pudo actualizar: "+ex.Message,"Stock mínimo"); }
        }

        private void btnEntrada_Click(object sender, EventArgs e)
        {
            if (dgvInventario.CurrentRow != null)
            {
                int productId = Convert.ToInt32(dgvInventario.CurrentRow.Cells["product_id"].Value);
                string productName = dgvInventario.CurrentRow.Cells["product_name"].Value.ToString();

                FrmMovimientoInventario frm = new FrmMovimientoInventario(productId, productName, "ENTRADA", userId);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    MostrarInventario();
                }
            }
        }

        private void btnSalida_Click(object sender, EventArgs e)
        {
            if (dgvInventario.CurrentRow != null)
            {
                int productId = Convert.ToInt32(dgvInventario.CurrentRow.Cells["product_id"].Value);
                string productName = dgvInventario.CurrentRow.Cells["product_name"].Value.ToString();

                FrmMovimientoInventario frm = new FrmMovimientoInventario(productId, productName, "SALIDA", userId);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    MostrarInventario();
                }
            }
        }

        private void btnMovimientos_Click(object sender, EventArgs e)
        {
            if (dgvInventario.CurrentRow != null)
            {
                int productId = Convert.ToInt32(dgvInventario.CurrentRow.Cells["product_id"].Value);
                string productName = dgvInventario.CurrentRow.Cells["product_name"].Value.ToString();

                FrmHistorialMovimientos frm = new FrmHistorialMovimientos(productId, productName);
                frm.ShowDialog();
            }
        }
    }
}
