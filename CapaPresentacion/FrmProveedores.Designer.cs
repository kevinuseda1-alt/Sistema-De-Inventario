namespace CapaPresentacion
{
    partial class FrmProveedores
    {
        private System.ComponentModel.IContainer components=null;
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.DataGridView dgvProveedores;
        private System.Windows.Forms.TextBox txtBuscar,txtNombre,txtContacto,txtTelefono,txtCorreo,txtDireccion;
        private System.Windows.Forms.ComboBox cboProducto;
        private System.Windows.Forms.Button btnNuevo,btnGuardar,btnDesactivar,btnVincular;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Label lblNombre,lblContacto,lblTelefono,lblCorreo,lblDireccion,lblProducto;
        protected override void Dispose(bool disposing)
        { if(disposing && components!=null)components.Dispose();base.Dispose(disposing); }
        private void InitializeComponent()
        {
            this.panelEditor=new System.Windows.Forms.Panel();
            this.dgvProveedores=new System.Windows.Forms.DataGridView();
            this.txtBuscar=new System.Windows.Forms.TextBox();
            this.txtNombre=new System.Windows.Forms.TextBox();
            this.txtContacto=new System.Windows.Forms.TextBox();
            this.txtTelefono=new System.Windows.Forms.TextBox();
            this.txtCorreo=new System.Windows.Forms.TextBox();
            this.txtDireccion=new System.Windows.Forms.TextBox();
            this.cboProducto=new System.Windows.Forms.ComboBox();
            this.btnNuevo=new System.Windows.Forms.Button();
            this.btnGuardar=new System.Windows.Forms.Button();
            this.btnDesactivar=new System.Windows.Forms.Button();
            this.btnVincular=new System.Windows.Forms.Button();
            this.lblEstado=new System.Windows.Forms.Label();
            this.lblNombre=new System.Windows.Forms.Label();this.lblContacto=new System.Windows.Forms.Label();
            this.lblTelefono=new System.Windows.Forms.Label();this.lblCorreo=new System.Windows.Forms.Label();
            this.lblDireccion=new System.Windows.Forms.Label();this.lblProducto=new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.Text="Proveedores · Inventario Bike Store";
            this.BackColor=System.Drawing.Color.FromArgb(245,248,252);
            this.Font=new System.Drawing.Font("Segoe UI",10F);
            this.MinimumSize=new System.Drawing.Size(760,480);
            this.Size=new System.Drawing.Size(930,570);
            this.panelEditor.Dock=System.Windows.Forms.DockStyle.Right;
            this.panelEditor.Width=330;
            this.panelEditor.AutoScroll=true;
            this.panelEditor.BackColor=System.Drawing.Color.White;
            this.txtBuscar.Dock=System.Windows.Forms.DockStyle.Top;
            this.txtBuscar.Height=34;
            this.dgvProveedores.Dock=System.Windows.Forms.DockStyle.Fill;
            this.dgvProveedores.ReadOnly=true;
            this.dgvProveedores.AllowUserToAddRows=false;
            this.dgvProveedores.MultiSelect=false;
            this.dgvProveedores.SelectionMode=System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProveedores.AutoSizeColumnsMode=System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvProveedores.BackgroundColor=System.Drawing.Color.White;
            this.lblNombre.Text="Proveedor *";this.lblNombre.SetBounds(16,3,290,20);
            this.lblContacto.Text="Contacto";this.lblContacto.SetBounds(16,55,290,20);
            this.lblTelefono.Text="Teléfono";this.lblTelefono.SetBounds(16,108,290,20);
            this.lblCorreo.Text="Correo";this.lblCorreo.SetBounds(16,161,290,20);
            this.lblDireccion.Text="Dirección";this.lblDireccion.SetBounds(16,214,290,20);
            this.lblProducto.Text="Producto que suministra";this.lblProducto.SetBounds(16,267,290,20);
            this.txtNombre.SetBounds(16,24,290,28);
            this.txtContacto.SetBounds(16,77,290,28);
            this.txtTelefono.SetBounds(16,130,290,28);
            this.txtCorreo.SetBounds(16,183,290,28);
            this.txtDireccion.SetBounds(16,236,290,28);
            this.txtNombre.Name="txtNombre";this.txtContacto.Name="txtContacto";
            this.txtTelefono.Name="txtTelefono";this.txtCorreo.Name="txtCorreo";
            this.txtDireccion.Name="txtDireccion";
            this.cboProducto.SetBounds(16,290,290,28);
            this.cboProducto.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProducto.Name="cboProducto";
            this.btnNuevo.SetBounds(16,340,90,38);this.btnNuevo.Text="Nuevo";
            this.btnGuardar.SetBounds(112,340,94,38);this.btnGuardar.Text="Guardar";
            this.btnDesactivar.SetBounds(212,340,94,38);this.btnDesactivar.Text="Desactivar";
            this.btnVincular.SetBounds(16,390,290,38);this.btnVincular.Text="Vincular producto seleccionado";
            this.btnGuardar.BackColor=System.Drawing.Color.FromArgb(30,88,130);
            this.btnGuardar.ForeColor=System.Drawing.Color.White;
            this.btnVincular.BackColor=System.Drawing.Color.FromArgb(25,105,82);
            this.btnVincular.ForeColor=System.Drawing.Color.White;
            this.lblEstado.Dock=System.Windows.Forms.DockStyle.Bottom;
            this.lblEstado.Height=45;
            this.lblEstado.AutoEllipsis=true;
            this.lblEstado.Padding=new System.Windows.Forms.Padding(16,8,0,0);
            this.lblEstado.Text="Seleccioná un proveedor o ingresá uno nuevo.";
            this.panelEditor.Controls.Add(this.txtNombre);this.panelEditor.Controls.Add(this.txtContacto);
            this.panelEditor.Controls.Add(this.lblNombre);this.panelEditor.Controls.Add(this.lblContacto);
            this.panelEditor.Controls.Add(this.lblTelefono);this.panelEditor.Controls.Add(this.lblCorreo);
            this.panelEditor.Controls.Add(this.lblDireccion);this.panelEditor.Controls.Add(this.lblProducto);
            this.panelEditor.Controls.Add(this.txtTelefono);this.panelEditor.Controls.Add(this.txtCorreo);
            this.panelEditor.Controls.Add(this.txtDireccion);this.panelEditor.Controls.Add(this.cboProducto);
            this.panelEditor.Controls.Add(this.btnNuevo);this.panelEditor.Controls.Add(this.btnGuardar);
            this.panelEditor.Controls.Add(this.btnDesactivar);this.panelEditor.Controls.Add(this.btnVincular);
            this.Controls.Add(this.dgvProveedores);this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.panelEditor);this.Controls.Add(this.lblEstado);
            this.ResumeLayout(false);
        }
    }
}
