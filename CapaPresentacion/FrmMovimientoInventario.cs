using CapaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PedidosApp
{
    public partial class FrmMovimientoInventario : Form
    {
        private int productId;
        private string movementType;
        private int userId;
        private int? orderId;

        public int Cantidad { get; private set; }
        public string Notas { get; private set; }

        public FrmMovimientoInventario(int productId, string productName, string movementType, int userId, int? orderId = null)
        {
            InitializeComponent();
            this.productId = productId;
            this.movementType = movementType;
            this.userId = userId;
            this.orderId = orderId;

            lblProducto.Text = productName;
            Text = movementType == "ENTRADA" ? "Registrar Entrada" : "Registrar Salida";
            ConfigurarEstilos();
        }

        private void ConfigurarEstilos()
        {
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.ForeColor = Color.White;

            lblTitulo.ForeColor = Color.White;
            lblProductoLabel.ForeColor = Color.White;
            lblProducto.ForeColor = Color.White;
            lblCantidad.ForeColor = Color.White;
            lblNotas.ForeColor = Color.White;

            txtCantidad.BackColor = Color.FromArgb(60, 60, 60);
            txtCantidad.ForeColor = Color.White;
            txtNotas.BackColor = Color.FromArgb(60, 60, 60);
            txtNotas.ForeColor = Color.White;

            btnAceptar.BackColor = Color.FromArgb(70, 70, 70);
            btnAceptar.FlatAppearance.BorderColor = Color.Gray;
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.ForeColor = Color.White;

            btnCancelar.BackColor = Color.FromArgb(70, 70, 70);
            btnCancelar.FlatAppearance.BorderColor = Color.Gray;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.White;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtCantidad.Text, out int cantidad) && cantidad > 0)
            {
                Cantidad = cantidad;
                Notas = txtNotas.Text;

                try
                {
                    string resultado;
                    if (movementType == "ENTRADA")
                    {
                        resultado = NInventario.RegistrarEntrada(productId, cantidad, userId, Notas);
                    }
                    else
                    {
                        // Verificar stock disponible para salidas
                        int stockDisponible = NInventario.ObtenerStockDisponible(productId);
                        if (cantidad > stockDisponible)
                        {
                            MessageBox.Show($"Stock insuficiente. Disponible: {stockDisponible}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        resultado = NInventario.RegistrarSalida(productId, cantidad, userId, orderId, Notas);
                    }

                    if (resultado == "OK")
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar movimiento: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Ingrese una cantidad válida mayor a cero", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}