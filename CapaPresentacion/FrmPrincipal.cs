using CapaNegocio;
using CapaPresentacion.Forms;
using FontAwesome.Sharp;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using PedidosApp;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace CapaPresentacion
{
    public partial class FrmPrincipal : Form
    {
        private int childFormNumber = 0;
        public string idTrabajador;
        public string nombre;
        public string apellido;
        public string acceso;
        private Form formularioActivo = null;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private string _userRole;
        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        private const int AW_SLIDE = 0x00040000;
        private const int AW_VER_POSITIVE = 0x00000004;
        private const int AW_ACTIVATE = 0x00020000;
        public FrmPrincipal(string role)
        {
            InitializeComponent();
            if (role.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
            {
                _userRole = "admin";
            }
            else if (role.Equals("Vendedor", StringComparison.OrdinalIgnoreCase))
            {
                _userRole = "caja";
            }
            else if (role.Equals("Almacenista", StringComparison.OrdinalIgnoreCase))
            {
                _userRole = "gerente";
            }
            else
            {
                _userRole = role.ToLower();
            }
            panelTitleBar.MouseDown += PanelTitleBar_MouseDown;
            panelTitleBar.MouseMove += PanelTitleBar_MouseMove;
            panelTitleBar.MouseUp += PanelTitleBar_MouseUp;
            pictureBox1.Click += PictureBox1_Click;
            // Inventario: conservar tablas históricas de ventas, pero no ofrecer su flujo en esta entrega.
            btnVentas.Visible = false;
            tsVentas.Visible = false;
            mnuVentas.Visible = false;
            clientesToolStripMenuItem.Visible = false;
            mnuSisVentas.Text = "Bike Store · Inventario";
            mnuCompras.Text = "Proveedores";
            btnClientes.Text = "PROVEEDORES";
            btnClientes.IconChar = IconChar.Truck;
            btnClientes.Click -= btnClientes_Click;
            btnClientes.Click += btnProveedores_Click;
            proveedoresToolStripMenuItem.Click += btnProveedores_Click;
            ResetToHome();
        }
        private void PanelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }
        private void AbrirFormularioPedidos()
        {
            FrmOrders frm = new FrmOrders()
            {
                Usuario_id = this.idTrabajador,
                Nombre = $"{this.nombre} {this.apellido}"
            };
            AbrirFormularioEncimaDelPanel(frm);
        }
        private void PanelTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }
        private void PanelTitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        private void ResetToHome()
        {
            iconCurrentChildForm.IconChar = IconChar.House;
            iconCurrentChildForm.IconColor = Color.Gray;
            lblTitleChildForm.Text = "Inicio";
        }
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            ResetToHome();
            FrmInicio frm = new FrmInicio();
            AbrirFormularioEncimaDelPanel(frm);

        }
        private void ActivateButton(object sender, IconChar icon, string title)
        {
            iconCurrentChildForm.IconChar = icon;
            iconCurrentChildForm.IconColor = Color.Gray;
            lblTitleChildForm.Text = title;
        }
        private void GestionAcceso()
        {
            if (acceso.Equals("admin", StringComparison.OrdinalIgnoreCase) || acceso.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
            {
                mnuAlmacen.Enabled = true;
                mnuCompras.Enabled = true;
                mnuVentas.Enabled = true;
                mnuMantenimiento.Enabled = true;
                mnuConsultas.Enabled = true;
                mnuHerramientas.Enabled = true;
                tsIngreso.Enabled = true;
                tsVentas.Enabled = true;
            }
            else if (acceso.Equals("caja", StringComparison.OrdinalIgnoreCase) || acceso.Equals("Vendedor", StringComparison.OrdinalIgnoreCase))
            {
                mnuAlmacen.Enabled = false;
                mnuCompras.Enabled = false;
                mnuVentas.Enabled = true;
                mnuMantenimiento.Enabled = false;
                mnuConsultas.Enabled = true;
                mnuHerramientas.Enabled = true;
                tsIngreso.Enabled = false;
                tsVentas.Enabled = true;
            }
            else if (acceso.Equals("gerente", StringComparison.OrdinalIgnoreCase) || acceso.Equals("Almacenista", StringComparison.OrdinalIgnoreCase))
            {
                mnuAlmacen.Enabled = true;
                mnuCompras.Enabled = true;
                mnuVentas.Enabled = false;
                mnuMantenimiento.Enabled = false;
                mnuConsultas.Enabled = true;
                mnuHerramientas.Enabled = true;
                tsIngreso.Enabled = true;
                tsVentas.Enabled = false;
            }
            else
            {
                mnuAlmacen.Enabled = false;
                mnuCompras.Enabled = false;
                mnuVentas.Enabled = false;
                mnuMantenimiento.Enabled = false;
                mnuConsultas.Enabled = false;
                mnuHerramientas.Enabled = false;
                tsIngreso.Enabled = false;
                tsVentas.Enabled = false;
            }
        }
        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }
        private void AbrirFormularioEncimaDelPanel(Form frmHijo)
        {
            try
            {
                if (formularioActivo != null)
                {
                    formularioActivo.Close();
                }
                formularioActivo = frmHijo;
                frmHijo.TopLevel = false;
                frmHijo.FormBorderStyle = FormBorderStyle.None;
                frmHijo.Dock = DockStyle.None;

                this.Controls.Add(frmHijo);
                frmHijo.BringToFront();
                Point posicionAbsoluta = flowLayoutPanel1.PointToScreen(Point.Empty);
                Point posicionRelativa = this.PointToClient(posicionAbsoluta);
                frmHijo.Location = new Point(posicionRelativa.X + 5, posicionRelativa.Y + 5);
                frmHijo.Size = new Size(flowLayoutPanel1.Width - 10, flowLayoutPanel1.Height - 10);
                frmHijo.Show();
                AnimateWindow(frmHijo.Handle, 200, AW_SLIDE | AW_VER_POSITIVE | AW_ACTIVATE);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Acceso denegado: No tiene permisos suficientes para abrir este formulario.", "Permisos insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Acceso denegado: No tiene permisos suficientes para abrir este formulario.","Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }
        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }
        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }
        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }
        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }
        private void btnArticulos_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, IconChar.Tags, "ARTÍCULOS");
            FrmProducts frm = new FrmProducts();
            AbrirFormularioEncimaDelPanel(frm);
        }
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, IconChar.ChartLine, "DASHBOARD");
            FrmDashboard frm = new FrmDashboard();
            AbrirFormularioEncimaDelPanel(frm);
        }
        private void btnCategorias_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, IconChar.CubesStacked, "CATEGORÍAS");
            FrmCategories frm = new FrmCategories();
            AbrirFormularioEncimaDelPanel(frm);
        }
        private void btnClientes_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, IconChar.ContactBook, "CLIENTES");
            FrmCustomers frm = new FrmCustomers();
            AbrirFormularioEncimaDelPanel(frm);
        }
        private void btnIngresos_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, IconChar.Shopware, "INGRESOS");
            FrmInventario frm = new FrmInventario(int.Parse(idTrabajador));
            AbrirFormularioEncimaDelPanel(frm);
        }
        private void btnPresentacion_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, IconChar.Weight, "PRESENTACIÓN");
        }
        private void btnProveedores_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, IconChar.Truck, "PROVEEDORES");
            AbrirFormularioEncimaDelPanel(new FrmProveedores());
        }
        private void btnTrabajadores_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, IconChar.PersonChalkboard, "TRABAJADORES");
            FrmUsers frm = new FrmUsers(_userRole);
            AbrirFormularioEncimaDelPanel(frm);
        }
        private void btnVentas_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, IconChar.CartShopping, "VENTAS");
            AbrirFormularioPedidos();
        }
        private void iconCurrentChildForm_Click(object sender, EventArgs e)
        {
            ResetToHome();
        }
        private void lblTitleChildForm_Click(object sender, EventArgs e)
        {
            ResetToHome();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else WindowState = FormWindowState.Normal;
        }
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            GestionAcceso();
        }
        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void stockDeToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void categoríasToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void presentaciónToolStripMenuItem_Click_1(object sender, EventArgs e) { }
        private void artículosToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void clientesToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void trabajadoresToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void ingresosToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void ventasToolStripMenuItem1_Click(object sender, EventArgs e) { }
        private void backupToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void tsIngreso_Click(object sender, EventArgs e) { }
        private void tsVentas_Click(object sender, EventArgs e) { }
        private void ARTICULOSToolStripMenuItem_Click(object sender, EventArgs e) { }
    }
}
