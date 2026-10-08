using CapaNegocio;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
namespace PedidosApp
{
    public partial class FrmOrders : Form
    {
        private bool EsNuevo = false;
        public string Usuario_id = string.Empty;
        public string Nombre = string.Empty;
        private DataTable dtDetalle = null;
        private decimal totalPagado = 0;
        private static FrmOrders _instancia;
        public static FrmOrders GetInstancia()
        {
            if (_instancia == null) _instancia = new FrmOrders();
            return _instancia;
        }
        public void setCliente(string idcliente, string nombre)
        {
            txtCliente_id.Text = idcliente;
            txtCliente.Text = nombre;
        }
        public void setProducto(string idproducto, string nombre)
        {
            txtProducto_id.Text = idproducto;
            txtProducto.Text = nombre;
            txtCantidad.Focus();
        }
        public FrmOrders()
        {
            InitializeComponent();
            txtCliente_id.Visible = false;
            txtProducto_id.Visible = false;
            txtCliente.ReadOnly = true;
            txtProducto.ReadOnly = true;
            txtDescuento.Text = "0";
            txtCantidad.Text = "1";
            txtCantidad.KeyPress += txtCantidad_KeyPress;
            txtPrecio.KeyPress += txtPrecio_KeyPress;
            txtDescuento.KeyPress += txtDescuento_KeyPress;
        }
        private void MensajeOK(string mensaje)
        {
            MessageBox.Show(mensaje, "Pedidos App", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void MensajeError(string mensaje)
        {
            MessageBox.Show(mensaje, "Pedidos App", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void Limpiar()
        {
            txtIdPedido.Text = string.Empty;
            txtCliente_id.Text = string.Empty;
            txtCliente.Text = string.Empty;
            lblFecha.Text = "Fecha Pedido:";
            lblTotal_orden.Text = "Total orden:";
            totalPagado = 0;
        }
        private void LimpiarDetalle()
        {
            txtProducto_id.Text = string.Empty;
            txtProducto.Text = string.Empty;
            txtCantidad.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtDescuento.Text = "0";
        }
        private void Habilitar(bool valor)
        {
            txtCantidad.ReadOnly = !valor;
            txtPrecio.ReadOnly = !valor;
            txtDescuento.ReadOnly = !valor;
            btnBuscarCliente.Enabled = valor;
            btnBuscarProducto.Enabled = valor;
            btnAgregar.Enabled = valor;
            btnQuitar.Enabled = valor;
        }
        private void Botones()
        {
            if (EsNuevo)
            {
                Habilitar(true);
                btnNuevo.Enabled = false;
                btnGuardar.Enabled = true;
                btnCancelar.Enabled = true;
                btnImprimir.Enabled = false;
            }
            else
            {
                Habilitar(false);
                btnNuevo.Enabled = true;
                btnGuardar.Enabled = false;
                btnCancelar.Enabled = false;
                btnImprimir.Enabled = true;
            }
        }
        private void OcultarColumnas()
        {
            dataListado.Columns[0].Visible = false;
            dataListado.Columns[1].Visible = true;
        }
        private void Mostrar()
        {
            try
            {
                dataListado.DataSource = Norders.Mostrar();
                OcultarColumnas();
                lblTotal.Text = "Registros encontrados: " + dataListado.Rows.Count;
            }
            catch (Exception ex)
            {
                MensajeError(ex.Message);
            }
        }

        private void BuscarFechas()
        {
            try
            {
                dataListado.DataSource = Norders.BuscarFecha(
                    dtFecha1.Value.ToString("dd/MM/yyyy"),
                    dtFecha2.Value.ToString("dd/MM/yyyy"));
                OcultarColumnas();
                lblTotal.Text = "Registros encontrados: " + dataListado.Rows.Count;
            }
            catch (Exception ex)
            {
                MensajeError(ex.Message);
            }
        }
        private void MostrarDetalle()
        {
            try
            {
                dataListadoDetalle.DataSource = Norders.MostrarDetalle(txtIdPedido.Text);
            }
            catch (Exception ex)
            {
                MensajeError(ex.Message);
            }
        }
        private void crearTabla()
        {
            dtDetalle = new DataTable("Detalle");
            dtDetalle.Columns.Add("product_id", typeof(int));
            dtDetalle.Columns.Add("producto", typeof(string));
            dtDetalle.Columns.Add("quantity", typeof(int));
            dtDetalle.Columns.Add("price", typeof(decimal));
            dtDetalle.Columns.Add("discount", typeof(decimal));
            dtDetalle.Columns.Add("subtotal", typeof(decimal));
            dataListadoDetalle.DataSource = dtDetalle;
        }
        private void FrmOrders_Load(object sender, EventArgs e)
        {
            Mostrar();
            Habilitar(false);
            Botones();
            crearTabla();
            lblUsuario.Text = "Usuario: " + Nombre;
            lblFecha.Text = "Fecha: " + DateTime.Now.ToShortDateString();
        }
        private void FrmOrders_FormClosing(object sender, FormClosingEventArgs e)
        {
            _instancia = null;
        }
        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                FrmVistaCliente vista = new FrmVistaCliente();
                vista.ShowDialog();

                if (vista.DialogResult == DialogResult.OK)
                {
                    txtCliente_id.Text = vista.IdCliente;
                    txtCliente.Text = vista.NombreCliente;
                    btnBuscarProducto.Focus();
                }
            }
            catch (Exception ex)
            {
                MensajeError("Error al buscar cliente: " + ex.Message);
            }
        }
        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCliente_id.Text))
                {
                    MensajeError("Primero debe seleccionar un cliente");
                    return;
                }
                FrmVistaProducto vista = new FrmVistaProducto();
                vista.ShowDialog();

                if (vista.DialogResult == DialogResult.OK)
                {
                    txtProducto_id.Text = vista.IdProducto;
                    txtProducto.Text = vista.NombreProducto;
                    txtPrecio.Text = vista.Precio.ToString("0.00");
                    txtCantidad.Text = "1";
                    txtDescuento.Text = "0";
                    txtCantidad.Focus();
                }
            }
            catch (Exception ex)
            {
                MensajeError("Error al buscar producto: " + ex.Message);
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarFechas();
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                bool haySeleccionados = false;
                foreach (DataGridViewRow row in dataListado.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        haySeleccionados = true;
                        break;
                    }
                }

                if (!haySeleccionados)
                {
                    MensajeError("No hay registros seleccionados para eliminar");
                    return;
                }

                DialogResult opcion = MessageBox.Show(
                    "¿Realmente desea eliminar los registros seleccionados?",
                    "Pedidos App",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (opcion != DialogResult.OK)
                {
                    return;
                }
                bool algunError = false;
                string mensajeError = "";
                int eliminados = 0;

                foreach (DataGridViewRow row in dataListado.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        try
                        {
                            int orderId = Convert.ToInt32(row.Cells[1].Value);
                            string rpta = Norders.Eliminar(orderId);

                            if (rpta.Equals("OK"))
                            {
                                eliminados++;
                            }
                            else
                            {
                                algunError = true;
                                mensajeError += $"Error al eliminar pedido {orderId}: {rpta}\n";
                            }
                        }
                        catch (Exception ex)
                        {
                            algunError = true;
                            mensajeError += $"Error al procesar fila: {ex.Message}\n";
                        }
                    }
                }

                if (eliminados > 0)
                {
                    MensajeOK($"Se eliminaron {eliminados} pedidos correctamente");
                    Mostrar();
                }

                if (algunError)
                {
                    MensajeError($"Hubo errores al eliminar:\n{mensajeError}");
                }
            }
            catch (Exception ex)
            {
                MensajeError($"Error inesperado: {ex.Message}");
            }
        }
        private void dataListado_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                if (dataListado.CurrentRow == null || dataListado.CurrentRow.IsNewRow)
                {
                    MensajeError("Por favor seleccione un registro válido");
                    return;
                }

                txtIdPedido.Text = dataListado.CurrentRow.Cells[1].Value?.ToString() ?? "";
                txtCliente_id.Text = dataListado.CurrentRow.Cells[2].Value?.ToString() ?? ""; 
                var clienteCell = dataListado.CurrentRow.Cells.Cast<DataGridViewCell>()
                                      .FirstOrDefault(c => c.OwningColumn.HeaderText == "Cliente");
                txtCliente.Text = clienteCell?.Value?.ToString() ?? "N/A";

                var usuarioCell = dataListado.CurrentRow.Cells.Cast<DataGridViewCell>()
                                      .FirstOrDefault(c => c.OwningColumn.HeaderText == "usuario");
                lblUsuario.Text = "Usuario: " + (usuarioCell?.Value?.ToString() ?? "N/A");

                var fechaCell = dataListado.CurrentRow.Cells.Cast<DataGridViewCell>()
                                      .FirstOrDefault(c => c.OwningColumn.HeaderText == "order_date");
                if (fechaCell?.Value != null)
                {
                    lblFecha.Text = "Fecha: " + Convert.ToDateTime(fechaCell.Value).ToShortDateString();
                }

                var totalCell = dataListado.CurrentRow.Cells.Cast<DataGridViewCell>()
                                      .FirstOrDefault(c => c.OwningColumn.HeaderText == "Total");
                lblTotal_orden.Text = "Total: " + (totalCell?.Value?.ToString() ?? "0.00");

                MostrarDetalle();
                tabControl1.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MensajeError("Error al seleccionar el registro: " + ex.Message);
            }
        }
        private void dataListado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataListado.Columns["Eliminar"].Index)
            {
                DataGridViewCheckBoxCell chkEliminar = (DataGridViewCheckBoxCell)dataListado.Rows[e.RowIndex].Cells["Eliminar"];
                chkEliminar.Value = !Convert.ToBoolean(chkEliminar.Value);
            }
        }
        private void chkEliminar_CheckedChanged(object sender, EventArgs e)
        {
            dataListado.Columns[0].Visible = chkEliminar.Checked;
        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            EsNuevo = true;
            Botones();
            Limpiar();
            LimpiarDetalle();
            crearTabla();
            Habilitar(true);
            txtCliente.Focus();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine($"Cliente ID: {txtCliente_id.Text}");
                Console.WriteLine($"Usuario ID: {Usuario_id}");
                Console.WriteLine($"Items en detalle: {dtDetalle?.Rows.Count ?? 0}");

                if (string.IsNullOrEmpty(txtCliente_id.Text) || !int.TryParse(txtCliente_id.Text, out int customerId))
                {
                    MensajeError("Debe seleccionar un cliente válido");
                    errorIcono.SetError(txtCliente, "Seleccione un cliente");
                    return;
                }

                if (dtDetalle == null || dtDetalle.Rows.Count == 0)
                {
                    MensajeError("Debe agregar al menos un producto al detalle");
                    return;
                }

                if (string.IsNullOrEmpty(Usuario_id) || !int.TryParse(Usuario_id, out int userId))
                {
                    MensajeError("Usuario no válido");
                    return;
                }
                foreach (DataRow row in dtDetalle.Rows)
                {
                    Console.WriteLine($"Producto: {row["product_id"]}, Cantidad: {row["quantity"]}, Precio: {row["price"]}");
                }

                string rpta = Norders.Insertar(customerId, userId, dtDetalle);

                if (rpta.Equals("OK"))
                {
                    MensajeOK("Pedido registrado correctamente");
                    EsNuevo = false;
                    Botones();
                    Limpiar();
                    Mostrar();
                }
                else
                {
                    MensajeError(rpta);
                }
            }
            catch (Exception ex)
            {
                MensajeError($"Error completo: {ex.ToString()}");
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            EsNuevo = false;
            Botones();
            Limpiar();
            LimpiarDetalle();
            Habilitar(false);
            tabControl1.SelectedIndex = 0;
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtProducto_id.Text) || !int.TryParse(txtProducto_id.Text, out int productId))
                {
                    MensajeError("Debe seleccionar un producto válido");
                    errorIcono.SetError(txtProducto, "Seleccione un producto");
                    return;
                }
                if (string.IsNullOrEmpty(txtCantidad.Text) || !int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
                {
                    MensajeError("La cantidad debe ser un número mayor a cero");
                    errorIcono.SetError(txtCantidad, "Ingrese una cantidad válida");
                    return;
                }
                if (string.IsNullOrEmpty(txtPrecio.Text) || !decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
                {
                    MensajeError("El precio debe ser un valor mayor a cero");
                    errorIcono.SetError(txtPrecio, "Ingrese un precio válido");
                    return;
                }
                if (string.IsNullOrEmpty(txtDescuento.Text) || !decimal.TryParse(txtDescuento.Text, out decimal descuento) || descuento < 0)
                {
                    MensajeError("El descuento debe ser un valor numérico positivo");
                    errorIcono.SetError(txtDescuento, "Ingrese un descuento válido");
                    return;
                }
                foreach (DataRow row in dtDetalle.Rows)
                {
                    if (Convert.ToInt32(row["product_id"]) == productId)
                    {
                        MensajeError("El producto ya está en el detalle");
                        return;
                    }
                }
                decimal subtotal = cantidad * precio - descuento;
                DataRow fila = dtDetalle.NewRow();
                fila["product_id"] = productId;
                fila["producto"] = txtProducto.Text;
                fila["quantity"] = cantidad;
                fila["price"] = precio;
                fila["discount"] = descuento;
                fila["subtotal"] = subtotal;
                dtDetalle.Rows.Add(fila);
                totalPagado += subtotal;
                lblTotal_orden.Text = "Total: " + CapaPresentacion.Moneda.Formatear(totalPagado);
                LimpiarDetalle();
            }
            catch (Exception ex)
            {
                MensajeError("Error al agregar producto: " + ex.Message);
            }
        }
        private void btnQuitar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataListadoDetalle.Rows.Count == 0)
                {
                    MensajeError("No hay items para quitar");
                    return;
                }
                int indice = dataListadoDetalle.CurrentRow.Index;
                decimal subtotal = Convert.ToDecimal(dtDetalle.Rows[indice]["subtotal"]);
                dtDetalle.Rows.RemoveAt(indice);
                totalPagado -= subtotal;
                lblTotal_orden.Text = "Total: " + CapaPresentacion.Moneda.Formatear(totalPagado);
            }
            catch (Exception ex)
            {
                MensajeError(ex.Message);
            }
        }
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtIdPedido.Text))
                {
                    if (dataListado.CurrentRow != null && !dataListado.CurrentRow.IsNewRow)
                    {
                        txtIdPedido.Text = dataListado.CurrentRow.Cells[1].Value?.ToString();
                    }

                    if (string.IsNullOrEmpty(txtIdPedido.Text))
                    {
                        MensajeError("Debe seleccionar un pedido para imprimir");
                        return;
                    }
                }

                FrmReporteFactura reporte = new FrmReporteFactura();
                reporte.OrderId = Convert.ToInt32(txtIdPedido.Text);
                reporte.ShowDialog();
            }
            catch (Exception ex)
            {
                MensajeError("Error al generar reporte: " + ex.Message);
            }
        }
        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void txtDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCliente_id_TextChanged(object sender, EventArgs e)
        {

        }
    }
}