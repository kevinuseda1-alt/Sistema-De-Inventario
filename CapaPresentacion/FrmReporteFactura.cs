using CapaNegocio;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using FontAwesome.Sharp;
namespace PedidosApp
{
    public partial class FrmReporteFactura : Form
    {
        public int OrderId { get; set; }
        private DataTable cabeceraFactura;
        private DataTable detalleFactura;
        private PrintDocument printDocument;

        public FrmReporteFactura()
        {
            InitializeComponent();
            ConfigureStyles();
            InitializePrintDocument();
        }

        private void ConfigureStyles()
        {
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.ForeColor = Color.White;
            gbDatosFactura.BackColor = Color.FromArgb(40, 40, 40);
            gbDatosFactura.ForeColor = Color.White;
            gbDetalleFactura.BackColor = Color.FromArgb(40, 40, 40);
            gbDetalleFactura.ForeColor = Color.White;
            lblNumeroFactura.ForeColor = Color.White;
            lblFechaFactura.ForeColor = Color.White;
            lblCliente.ForeColor = Color.White;
            lblEmail.ForeColor = Color.White;
            lblTelefono.ForeColor = Color.White;
            lblDireccion.ForeColor = Color.White;
            lblUsuario.ForeColor = Color.White;
            lblTotal.ForeColor = Color.White;
            dgvDetalle.BackgroundColor = Color.FromArgb(60, 60, 60);
            dgvDetalle.GridColor = Color.Gray;
            dgvDetalle.BorderStyle = BorderStyle.None;
            dgvDetalle.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvDetalle.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvDetalle.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvDetalle.EnableHeadersVisualStyles = false;
            dgvDetalle.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvDetalle.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetalle.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvDetalle.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetalle.RowsDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvDetalle.RowsDefaultCellStyle.ForeColor = Color.White;
            dgvDetalle.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 80, 80);
            dgvDetalle.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            ConfigureFontAwesomeButton(btnImprimir, "Imprimir", IconChar.Print);
            ConfigureFontAwesomeButton(btnVistaPrevia, "Vista Previa", IconChar.Eye);
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

        private void InitializePrintDocument()
        {
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void FrmReporteFactura_Load(object sender, EventArgs e)
        {
            try
            {
                CargarDatosFactura();
                MostrarDatosEnFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la factura: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatosFactura()
        {
            try
            {
                SqlConnection con = new SqlConnection(NConexion.Cadena);
                {
                    con.Open();
                    SqlCommand cmdCabecera = new SqlCommand("sp_rpt_obtener_cabecera_pedido", con);
                    cmdCabecera.CommandType = CommandType.StoredProcedure;
                    cmdCabecera.Parameters.AddWithValue("@pedido_id", OrderId);

                    SqlDataAdapter daCabecera = new SqlDataAdapter(cmdCabecera);
                    cabeceraFactura = new DataTable();
                    daCabecera.Fill(cabeceraFactura);

                    SqlCommand cmdDetalle = new SqlCommand("sp_rpt_obtener_detalle_pedido", con);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@pedido_id", OrderId);

                    SqlDataAdapter daDetalle = new SqlDataAdapter(cmdDetalle);
                    detalleFactura = new DataTable();
                    daDetalle.Fill(detalleFactura);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener datos de la factura: " + ex.Message);
            }
        }

        private void MostrarDatosEnFormulario()
        {
            if (cabeceraFactura.Rows.Count > 0)
            {
                DataRow row = cabeceraFactura.Rows[0];

                lblNumeroFactura.Text = $"Factura N°: {row["order_id"]}";
                lblFechaFactura.Text = $"Fecha: {Convert.ToDateTime(row["fecha_pedido"]):dd/MM/yyyy HH:mm}";
                lblCliente.Text = $"Cliente: {row["nombre_cliente"]}";
                lblEmail.Text = $"Email: {row["email"]}";
                lblTelefono.Text = $"Teléfono: {row["phone"]}";
                lblDireccion.Text = $"Dirección: {row["direccion_completa"]}";
                lblUsuario.Text = $"Atendido por: {row["usuario"]}";
                lblTotal.Text = $"TOTAL: {CapaPresentacion.Moneda.Formatear(Convert.ToDecimal(row["total"]))}";
            }
            dgvDetalle.DataSource = detalleFactura;
            ConfigurarColumnasDetalle();
        }

        private void ConfigurarColumnasDetalle()
        {
            if (dgvDetalle.Columns.Contains("product_id"))
            {
                dgvDetalle.Columns["product_id"].HeaderText = "ID";
                dgvDetalle.Columns["product_id"].Width = 50;
            }

            if (dgvDetalle.Columns.Contains("producto"))
            {
                dgvDetalle.Columns["producto"].HeaderText = "Producto";
                dgvDetalle.Columns["producto"].Width = 200;
            }

            if (dgvDetalle.Columns.Contains("cantidad"))
            {
                dgvDetalle.Columns["cantidad"].HeaderText = "Cant.";
                dgvDetalle.Columns["cantidad"].Width = 60;
                dgvDetalle.Columns["cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvDetalle.Columns.Contains("precio"))
            {
                dgvDetalle.Columns["precio"].HeaderText = "Precio Unit.";
                dgvDetalle.Columns["precio"].Width = 100;
                dgvDetalle.Columns["precio"].DefaultCellStyle.Format = "C2";
                dgvDetalle.Columns["precio"].DefaultCellStyle.FormatProvider = CapaPresentacion.Moneda.Cultura;
                dgvDetalle.Columns["precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvDetalle.Columns.Contains("descuento"))
            {
                dgvDetalle.Columns["descuento"].HeaderText = "Descuento";
                dgvDetalle.Columns["descuento"].Width = 80;
                dgvDetalle.Columns["descuento"].DefaultCellStyle.Format = "C2";
                dgvDetalle.Columns["descuento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvDetalle.Columns.Contains("subtotal"))
            {
                dgvDetalle.Columns["subtotal"].HeaderText = "Subtotal";
                dgvDetalle.Columns["subtotal"].Width = 100;
                dgvDetalle.Columns["subtotal"].DefaultCellStyle.Format = "C2";
                dgvDetalle.Columns["subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void btnVistaPrevia_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPreviewDialog previewDialog = new PrintPreviewDialog();
                previewDialog.Document = printDocument;
                previewDialog.BackColor = Color.FromArgb(40, 40, 40);
                previewDialog.ForeColor = Color.White;
                previewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en vista previa: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Font titleFont = new Font("Arial", 18, FontStyle.Bold);
                Font headerFont = new Font("Arial", 12, FontStyle.Bold);
                Font contentFont = new Font("Arial", 10);
                Font smallFont = new Font("Arial", 9);

                Brush blackBrush = Brushes.Black;
                Brush grayBrush = Brushes.Gray;

                int yPos = 50;
                int leftMargin = 80;
                int rightMargin = 720;
                g.DrawString("BIKE STORE", titleFont, blackBrush, leftMargin, yPos);
                yPos += 25;
                g.DrawString("Sistema de Gestión de Pedidos", contentFont, grayBrush, leftMargin, yPos);
                yPos += 40;
                g.DrawLine(Pens.Black, leftMargin, yPos, rightMargin, yPos);
                yPos += 20;

                if (cabeceraFactura.Rows.Count > 0)
                {
                    DataRow cabecera = cabeceraFactura.Rows[0];
                    g.DrawString($"FACTURA N°: {cabecera["order_id"]}", headerFont, blackBrush, leftMargin, yPos);
                    g.DrawString($"FECHA: {Convert.ToDateTime(cabecera["fecha_pedido"]):dd/MM/yyyy HH:mm}", headerFont, blackBrush, rightMargin - 200, yPos);
                    yPos += 40;
                    g.DrawString("DATOS DEL CLIENTE:", headerFont, blackBrush, leftMargin, yPos);
                    yPos += 25;

                    g.DrawString($"Cliente: {cabecera["nombre_cliente"]}", contentFont, blackBrush, leftMargin, yPos);
                    yPos += 20;
                    g.DrawString($"Email: {cabecera["email"]}", contentFont, blackBrush, leftMargin, yPos);
                    yPos += 20;
                    g.DrawString($"Teléfono: {cabecera["phone"]}", contentFont, blackBrush, leftMargin, yPos);
                    yPos += 20;
                    g.DrawString($"Dirección: {cabecera["direccion_completa"]}", contentFont, blackBrush, leftMargin, yPos);
                    yPos += 20;
                    g.DrawString($"Atendido por: {cabecera["usuario"]}", contentFont, blackBrush, leftMargin, yPos);
                    yPos += 40;
                    g.DrawString("DETALLE DE PRODUCTOS:", headerFont, blackBrush, leftMargin, yPos);
                    yPos += 30;
                    int col1 = leftMargin;
                    int col2 = col1 + 50;
                    int col3 = col2 + 280;
                    int col4 = col3 + 60;
                    int col5 = col4 + 80;
                    int col6 = col5 + 80;

                    g.DrawString("ID", headerFont, blackBrush, col1, yPos);
                    g.DrawString("PRODUCTO", headerFont, blackBrush, col2, yPos);
                    g.DrawString("CANT.", headerFont, blackBrush, col3, yPos);
                    g.DrawString("PRECIO", headerFont, blackBrush, col4, yPos);
                    g.DrawString("DESC.", headerFont, blackBrush, col5, yPos);
                    g.DrawString("SUBTOTAL", headerFont, blackBrush, col6, yPos);
                    yPos += 25;
                    g.DrawLine(Pens.Black, leftMargin, yPos, rightMargin, yPos);
                    yPos += 10;
                    decimal totalGeneral = 0;
                    foreach (DataRow item in detalleFactura.Rows)
                    {
                        if (yPos > 950)
                        {
                            e.HasMorePages = true;
                            return;
                        }

                        string id = item["product_id"].ToString();
                        string producto = item["producto"].ToString();
                        if (producto.Length > 35) producto = producto.Substring(0, 35) + "...";

                        string cantidad = item["cantidad"].ToString();
                        decimal precio = Convert.ToDecimal(item["precio"]);
                        decimal descuento = Convert.ToDecimal(item["descuento"]);
                        decimal subtotal = Convert.ToDecimal(item["subtotal"]);
                        totalGeneral += subtotal;

                        g.DrawString(id, contentFont, blackBrush, col1, yPos);
                        g.DrawString(producto, contentFont, blackBrush, col2, yPos);
                        g.DrawString(cantidad, contentFont, blackBrush, col3, yPos);
                        g.DrawString(CapaPresentacion.Moneda.Formatear(precio), contentFont, blackBrush, col4, yPos);
                        g.DrawString(CapaPresentacion.Moneda.Formatear(descuento), contentFont, blackBrush, col5, yPos);
                        g.DrawString(CapaPresentacion.Moneda.Formatear(subtotal), contentFont, blackBrush, col6, yPos);

                        yPos += 20;
                    }
                    yPos += 10;
                    g.DrawLine(Pens.Black, col4, yPos, rightMargin, yPos);
                    yPos += 20;
                    g.DrawString("TOTAL GENERAL:", headerFont, blackBrush, col5 - 20, yPos);
                    g.DrawString(CapaPresentacion.Moneda.Formatear(totalGeneral), headerFont, blackBrush, col6, yPos);
                    yPos += 40;
                    g.DrawString("¡Gracias por su compra!", contentFont, blackBrush, leftMargin, yPos);
                    yPos += 20;
                    g.DrawString($"Documento generado el {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont, grayBrush, leftMargin, yPos);
                }

                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar la impresión: " + ex.Message);
            }
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
