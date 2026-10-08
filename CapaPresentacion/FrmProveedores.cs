using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class FrmProveedores : Form
    {
        private readonly NProveedores servicio=new NProveedores();
        private int? seleccionado;
        public FrmProveedores()
        {
            InitializeComponent();
            btnNuevo.Click+=(s,e)=>Limpiar();
            btnGuardar.Click+=(s,e)=>Guardar();
            btnDesactivar.Click+=(s,e)=>Desactivar();
            btnVincular.Click+=(s,e)=>Vincular();
            txtBuscar.TextChanged+=(s,e)=>Cargar();
            dgvProveedores.SelectionChanged+=(s,e)=>Seleccionar();
            if(LicenseManager.UsageMode!=LicenseUsageMode.Designtime)
            {
                try { cboProducto.DataSource=servicio.Productos();cboProducto.DisplayMember="product_name";
                      cboProducto.ValueMember="product_id";Cargar(); }
                catch(Exception ex) { MessageBox.Show(this,ex.Message,"SQL Server"); }
            }
        }
        private void Cargar()
        {
            try { dgvProveedores.DataSource=servicio.Listar(txtBuscar.Text); }
            catch(Exception ex) { lblEstado.Text=ex.Message; }
        }
        private void Seleccionar()
        {
            if(dgvProveedores.CurrentRow==null || dgvProveedores.CurrentRow.Cells["supplier_id"].Value==null)return;
            DataGridViewRow row=dgvProveedores.CurrentRow;
            seleccionado=Convert.ToInt32(row.Cells["supplier_id"].Value);
            txtNombre.Text=Convert.ToString(row.Cells["supplier_name"].Value);
            txtContacto.Text=Convert.ToString(row.Cells["contact_name"].Value);
            txtTelefono.Text=Convert.ToString(row.Cells["phone"].Value);
            txtCorreo.Text=Convert.ToString(row.Cells["email"].Value);
            txtDireccion.Text=Convert.ToString(row.Cells["address"].Value);
            try { DataTable dt=servicio.ProductosDe(seleccionado.Value);
                  lblEstado.Text="Productos vinculados: "+(dt.Rows.Count==0?"ninguno":string.Join(", ",
                     System.Array.ConvertAll(dt.Select(),r=>Convert.ToString(r["product_name"])))); }
            catch(Exception ex) { lblEstado.Text=ex.Message; }
        }
        private void Limpiar()
        {
            seleccionado=null;txtNombre.Clear();txtContacto.Clear();txtTelefono.Clear();
            txtCorreo.Clear();txtDireccion.Clear();dgvProveedores.ClearSelection();txtNombre.Focus();
            lblEstado.Text="Nuevo proveedor";
        }
        private void Guardar()
        {
            try { int id=servicio.Guardar(seleccionado,txtNombre.Text,txtContacto.Text,txtTelefono.Text,txtCorreo.Text,txtDireccion.Text);
                  Cargar();lblEstado.Text="Proveedor guardado. ID: "+id; }
            catch(Exception ex) { MessageBox.Show(this,ex.Message,"Proveedor"); }
        }
        private void Desactivar()
        {
            if(!seleccionado.HasValue)return;
            if(MessageBox.Show(this,"¿Desactivar este proveedor?","Confirmar",MessageBoxButtons.YesNo)!=DialogResult.Yes)return;
            try { servicio.Desactivar(seleccionado.Value);Cargar();lblEstado.Text="Proveedor desactivado."; }
            catch(Exception ex) { MessageBox.Show(this,ex.Message,"Proveedor"); }
        }
        private void Vincular()
        {
            if(!seleccionado.HasValue || !(cboProducto.SelectedValue is int))
            { MessageBox.Show(this,"Seleccioná un proveedor y un producto.");return; }
            try { servicio.Vincular(seleccionado.Value,(int)cboProducto.SelectedValue);Seleccionar(); }
            catch(Exception ex) { MessageBox.Show(this,ex.Message,"Producto / proveedor"); }
        }
    }
}
