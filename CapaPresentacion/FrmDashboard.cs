using CapaNegocio;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace PedidosApp
{
    public partial class FrmDashboard : Form
    {
        public FrmDashboard()
        {
            InitializeComponent();
            CargarEstadisticas();
            ConfigurarEstilos();
        }
        private void ConfigurarEstilos()
        {
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.ForeColor = Color.White;
            panel1.BackColor = Color.FromArgb(60, 60, 60);
            panelStats.BackColor = Color.FromArgb(40, 40, 40);
            foreach (Control panel in panelStats.Controls)
            {
                if (panel is Panel)
                {
                    panel.BackColor = Color.FromArgb(60, 60, 60);
                    panel.ForeColor = Color.White;
                    foreach (Control control in panel.Controls)
                    {
                        control.ForeColor = Color.White;
                    }
                }
            }
            panelInfoEmpresa.BackColor = Color.FromArgb(60, 60, 60);
            foreach (Control control in panelInfoEmpresa.Controls)
            {
                control.ForeColor = Color.White;
            }
        }
        private void CargarEstadisticas()
        {
            try
            {
                using (var con = new SqlConnection(NConexion.Cadena))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.suppliers WHERE is_active=1", con))
                        lblTotalClientes.Text = cmd.ExecuteScalar().ToString();
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.products", con))
                        lblTotalProductos.Text = cmd.ExecuteScalar().ToString();
                    using (var cmd = new SqlCommand("SELECT ISNULL(SUM(price*stock_actual),0) FROM dbo.vw_inventory_summary", con))
                        lblVentasMes.Text = CapaPresentacion.Moneda.Formatear(Convert.ToDecimal(cmd.ExecuteScalar()));
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.vw_inventory_summary WHERE stock_actual<=min_stock", con))
                        lblPedidosPendientes.Text = cmd.ExecuteScalar().ToString();
                    using (var cmd = new SqlCommand("SELECT TOP 1 full_name,email FROM dbo.users WHERE role='admin' ORDER BY user_id", con))
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblEmpresa.Text = "BIKE STORE · INVENTARIO";
                            lblPropietario.Text = dr["full_name"].ToString();
                            lblEmail.Text = "Email: " + dr["email"];
                        }
                    }
                }
                lblDireccion.Text = "Control de proveedores y productos";
                lblTelefono.Text = "Entradas y salidas de stock";
                lblHorario.Text = "Precios presentados en C$";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar estadísticas: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarEstadisticas();
        }
    }
}
