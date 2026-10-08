using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaNegocio;
using FontAwesome.Sharp;

namespace CapaPresentacion.Forms
{
    public partial class FrmReporteProducto : Form
    {
        private DataTable productosReporte;

        public FrmReporteProducto()
        {
            InitializeComponent();
            ConfigureStyles();
        }

        private void FrmReporteProducto_Load(object sender, EventArgs e)
        {
            CargarProductos();
            ConfigureDatePickers();
        }

        private void ConfigureStyles()
        {
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.ForeColor = Color.White;

            gbFiltros.BackColor = Color.FromArgb(40, 40, 40);
            gbFiltros.ForeColor = Color.White;

            gbResultados.BackColor = Color.FromArgb(40, 40, 40);
            gbResultados.ForeColor = Color.White;

            lblFechaInicio.ForeColor = Color.White;
            lblFechaFin.ForeColor = Color.White;
            lblBuscar.ForeColor = Color.White;
            lblTotal.ForeColor = Color.White;
            txtBuscar.BackColor = Color.FromArgb(60, 60, 60);
            txtBuscar.ForeColor = Color.White;
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            dtpFechaInicio.BackColor = Color.FromArgb(60, 60, 60);
            dtpFechaInicio.ForeColor = Color.White;
            dtpFechaFin.BackColor = Color.FromArgb(60, 60, 60);
            dtpFechaFin.ForeColor = Color.White;
            dgvReporte.BackgroundColor = Color.FromArgb(60, 60, 60);
            dgvReporte.GridColor = Color.Gray;
            dgvReporte.BorderStyle = BorderStyle.None;
            dgvReporte.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvReporte.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvReporte.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvReporte.EnableHeadersVisualStyles = false;
            dgvReporte.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvReporte.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReporte.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvReporte.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReporte.RowsDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvReporte.RowsDefaultCellStyle.ForeColor = Color.White;
            dgvReporte.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 80, 80);
            dgvReporte.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            ConfigureFontAwesomeButton(btnBuscar, "Buscar", IconChar.Search);
            ConfigureFontAwesomeButton(btnFiltrarFecha, "Filtrar por Fecha", IconChar.Filter);
            ConfigureFontAwesomeButton(btnTodos, "Mostrar Todos", IconChar.List);
            ConfigureFontAwesomeButton(btnImprimir, "Imprimir", IconChar.Print);
            ConfigureFontAwesomeButton(btnCerrar, "Cerrar", IconChar.Times);
        }

        private void ConfigureFontAwesomeButton(Button btn, string text, IconChar iconChar)
        {
            var icon = new IconButton
            {
                IconChar = iconChar,
                IconColor = Color.White,
                IconSize = 16,
                Text = text,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleRight
            };

            btn.Text = icon.Text;
            btn.Image = icon.Image;
            btn.TextImageRelation = icon.TextImageRelation;
            btn.ImageAlign = icon.ImageAlign;
            btn.TextAlign = icon.TextAlign;

            StyleButton(btn);
        }

        private void StyleButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(70, 70, 70);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.Gray;
            btn.FlatAppearance.BorderSize = 1;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btn.Padding = new Padding(5, 2, 5, 2);
            btn.Cursor = Cursors.Hand;
        }

        private void ConfigureDatePickers()
        {
            dtpFechaInicio.Value = DateTime.Now.AddMonths(-1);
            dtpFechaFin.Value = DateTime.Now;
        }

        private void CargarProductos()
        {
            try
            {
                productosReporte = NProducts.Mostrar();
                dgvReporte.DataSource = productosReporte;
                ConfigurarColumnasGrid();
                ActualizarContador();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnasGrid()
        {
            if (dgvReporte.Columns.Contains("product_id"))
            {
                dgvReporte.Columns["product_id"].HeaderText = "ID";
                dgvReporte.Columns["product_id"].Width = 60;
            }

            if (dgvReporte.Columns.Contains("product_name"))
            {
                dgvReporte.Columns["product_name"].HeaderText = "Nombre del Producto";
                dgvReporte.Columns["product_name"].Width = 200;
            }

            if (dgvReporte.Columns.Contains("model_year"))
            {
                dgvReporte.Columns["model_year"].HeaderText = "Año Modelo";
                dgvReporte.Columns["model_year"].Width = 80;
            }

            if (dgvReporte.Columns.Contains("price"))
            {
                dgvReporte.Columns["price"].HeaderText = "Precio";
                dgvReporte.Columns["price"].Width = 100;
                dgvReporte.Columns["price"].DefaultCellStyle.Format = "C2";
                dgvReporte.Columns["price"].DefaultCellStyle.FormatProvider = CapaPresentacion.Moneda.Cultura;
                dgvReporte.Columns["price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvReporte.Columns.Contains("Category"))
            {
                dgvReporte.Columns["Category"].HeaderText = "Categoría";
                dgvReporte.Columns["Category"].Width = 120;
            }

            if (dgvReporte.Columns.Contains("create_date"))
            {
                dgvReporte.Columns["create_date"].HeaderText = "Fecha Creación";
                dgvReporte.Columns["create_date"].Width = 100;
                dgvReporte.Columns["create_date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            if (dgvReporte.Columns.Contains("imagen"))
                dgvReporte.Columns["imagen"].Visible = false;

            if (dgvReporte.Columns.Contains("category_id"))
                dgvReporte.Columns["category_id"].Visible = false;
        }

        private void ActualizarContador()
        {
            int totalRegistros = dgvReporte.Rows.Count;
            lblTotal.Text = $"Total de productos: {totalRegistros}";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string textoBuscar = txtBuscar.Text.Trim();

                if (string.IsNullOrEmpty(textoBuscar))
                {
                    MessageBox.Show("Ingrese un texto para buscar", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                productosReporte = NProducts.Buscar(textoBuscar);
                dgvReporte.DataSource = productosReporte;
                ConfigurarColumnasGrid();
                ActualizarContador();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar productos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFiltrarFecha_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = dtpFechaInicio.Value.Date;
                DateTime fechaFin = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1);

                if (fechaInicio > fechaFin)
                {
                    MessageBox.Show("La fecha de inicio no puede ser mayor a la fecha fin", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                productosReporte = NProducts.FiltrarPorFecha(fechaInicio, fechaFin);
                dgvReporte.DataSource = productosReporte;
                ConfigurarColumnasGrid();
                ActualizarContador();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar por fecha: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            CargarProductos();
            txtBuscar.Text = "";
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (productosReporte == null || productosReporte.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos para imprimir", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                GenerarReporteImpresion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerarReporteImpresion()
        {
            try
            {
                System.Drawing.Printing.PrintDocument printDoc = new System.Drawing.Printing.PrintDocument();
                printDoc.PrintPage += PrintDoc_PrintPage;
                PrintPreviewDialog previewDialog = new PrintPreviewDialog();
                previewDialog.Document = printDoc;
                previewDialog.BackColor = Color.FromArgb(40, 40, 40);
                previewDialog.ForeColor = Color.White;
                previewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar reporte: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Font titleFont = new Font("Arial", 16, FontStyle.Bold);
                Font headerFont = new Font("Arial", 10, FontStyle.Bold);
                Font contentFont = new Font("Arial", 9);

                Brush blackBrush = Brushes.Black;
                int yPos = 50;
                int leftMargin = 50;
                string titulo = "REPORTE DE PRODUCTOS - BIKE STORE";
                g.DrawString(titulo, titleFont, blackBrush, leftMargin, yPos);
                yPos += 40;
                string fecha = $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}";
                g.DrawString(fecha, contentFont, blackBrush, leftMargin, yPos);
                yPos += 30;
                string total = $"Total de productos: {productosReporte.Rows.Count}";
                g.DrawString(total, contentFont, blackBrush, leftMargin, yPos);
                yPos += 40;
                int col1 = leftMargin;
                int col2 = col1 + 60;
                int col3 = col2 + 250;
                int col4 = col3 + 80;
                int col5 = col4 + 100;

                g.DrawString("ID", headerFont, blackBrush, col1, yPos);
                g.DrawString("Nombre", headerFont, blackBrush, col2, yPos);
                g.DrawString("Año", headerFont, blackBrush, col3, yPos);
                g.DrawString("Precio", headerFont, blackBrush, col4, yPos);
                g.DrawString("Categoría", headerFont, blackBrush, col5, yPos);
                yPos += 25;

                g.DrawLine(Pens.Black, leftMargin, yPos, 750, yPos);
                yPos += 10;
                foreach (DataRow row in productosReporte.Rows)
                {
                    if (yPos > 1000)
                    {
                        e.HasMorePages = true;
                        return;
                    }

                    string id = row["product_id"].ToString();
                    string nombre = row["product_name"].ToString();
                    if (nombre.Length > 30) nombre = nombre.Substring(0, 30) + "...";

                    string año = row["model_year"].ToString();
                    string precio = CapaPresentacion.Moneda.Formatear(Convert.ToDecimal(row["price"]));
                    string categoria = row["Category"]?.ToString() ?? "";

                    g.DrawString(id, contentFont, blackBrush, col1, yPos);
                    g.DrawString(nombre, contentFont, blackBrush, col2, yPos);
                    g.DrawString(año, contentFont, blackBrush, col3, yPos);
                    g.DrawString(precio, contentFont, blackBrush, col4, yPos);
                    g.DrawString(categoria, contentFont, blackBrush, col5, yPos);

                    yPos += 20;
                }

                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en impresión: " + ex.Message);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnBuscar_Click(sender, e);
            }
        }
    }
}