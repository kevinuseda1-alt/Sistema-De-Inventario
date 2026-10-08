using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CapaNegocio;
namespace CapaPresentacion.Forms
{
    public partial class FrmCustomers : Form
    {
        private readonly NCustomers _customersBusiness;
        private bool _isEditing = false;
        public FrmCustomers()
        {
            InitializeComponent();
            _customersBusiness = new NCustomers();
            dgvCustomers.CellDoubleClick += DgvCustomers_CellDoubleClick;
        }
        private void FrmCustomers_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            ConfigureGrid();
            ResetForm();
            btnNew.Click += BtnNew_Click;
            btnSave.Click += BtnSave_Click;
            btnEdit.Click += BtnEdit_Click;
            btnCancel.Click += BtnCancel_Click;
            btnSearch.Click += BtnSearch_Click;
            btnDeleteSelected.Click += BtnDeleteSelected_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            dgvCustomers.SelectionChanged += DgvCustomers_SelectionChanged;
        }
        private void LoadCustomers()
        {
            try
            {
                DataTable customers = _customersBusiness.GetCustomers();
                dgvCustomers.DataSource = customers;
                lblTotalRecords.Text = $"Total: {customers.Rows.Count} registros";
                if (dgvCustomers.Columns.Count > 0)
                {
                    dgvCustomers.Columns["customer_id"].HeaderText = "ID";
                    dgvCustomers.Columns["first_name"].HeaderText = "Nombres";
                    dgvCustomers.Columns["last_name"].HeaderText = "Apellidos";
                    dgvCustomers.Columns["phone"].HeaderText = "Teléfono";
                    dgvCustomers.Columns["email"].HeaderText = "Email";
                    dgvCustomers.Columns["street"].HeaderText = "Dirección";
                    dgvCustomers.Columns["city"].HeaderText = "Ciudad";
                    dgvCustomers.Columns["state"].HeaderText = "Estado";
                    dgvCustomers.Columns["create_date"].HeaderText = "Fecha Registro";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ConfigureGrid()
        {
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            dgvCustomers.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvCustomers.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(70, 70, 70);
            dgvCustomers.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(70, 70, 70);
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dgvCustomers.RowHeadersVisible = false;
            if (dgvCustomers.Columns["chkSelect"] != null)
            {
                dgvCustomers.Columns["chkSelect"].HeaderText = "Sel.";
                dgvCustomers.Columns["chkSelect"].Width = 50;
                dgvCustomers.Columns["chkSelect"].DisplayIndex = 0;
            }
            dgvCustomers.ScrollBars = ScrollBars.Both;
        }
        private void EnableControls(bool enable)
        {
            txtFirstName.Enabled = enable;
            txtLastName.Enabled = enable;
            txtPhone.Enabled = enable;
            txtEmail.Enabled = enable;
            txtStreet.Enabled = enable;
            txtCity.Enabled = enable;
            txtState.Enabled = enable;
            btnSave.Enabled = enable;
            btnCancel.Enabled = enable;
            btnEdit.Enabled = !enable && dgvCustomers.SelectedRows.Count > 0;
            btnNew.Enabled = !enable;
        }
        private void ResetForm()
        {
            txtCustomerId.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtStreet.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            _isEditing = false;
            EnableMaintenanceControls(false);
            btnEdit.Enabled = false;
            btnCancel.Enabled = false;
        }
        private int GetMaxCustomerId()
        {
            int maxId = 0;
            if (dgvCustomers.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvCustomers.Rows)
                {
                    if (row.Cells["customer_id"].Value != null &&
                        int.TryParse(row.Cells["customer_id"].Value.ToString(), out int currentId))
                    {
                        if (currentId > maxId) maxId = currentId;
                    }
                }
            }
            return maxId;
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            try
            {
                ResetForm();
                txtCustomerId.Text = (GetMaxCustomerId() + 1).ToString();
                EnableControls(true);
                txtFirstName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al preparar nuevo cliente: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            _isEditing = true;
            EnableMaintenanceControls(true);
            btnEdit.Enabled = false;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFirstName.Text))
                    throw new Exception("El nombre es requerido");
                if (string.IsNullOrEmpty(txtLastName.Text))
                    throw new Exception("Los apellidos son requeridos");
                if (string.IsNullOrEmpty(txtEmail.Text))
                    throw new Exception("El email es requerido");
                if (_isEditing)
                {
                    _customersBusiness.UpdateCustomer(
                        Convert.ToInt32(txtCustomerId.Text),
                        txtFirstName.Text,
                        txtLastName.Text,
                        txtPhone.Text,
                        txtEmail.Text,
                        txtStreet.Text,
                        txtCity.Text,
                        txtState.Text
                    );
                    MessageBox.Show("Cliente actualizado correctamente", "Éxito",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _customersBusiness.AddCustomer(
                        Convert.ToInt32(txtCustomerId.Text),
                        txtFirstName.Text,
                        txtLastName.Text,
                        txtPhone.Text,
                        txtEmail.Text,
                        txtStreet.Text,
                        txtCity.Text,
                        txtState.Text
                    );
                    MessageBox.Show("Cliente registrado correctamente", "Éxito",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                LoadCustomers();
                ResetForm();
                tabControl1.SelectedTab = tabPageList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DgvCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                tabControl1.SelectedTab = tabPageMaintenance;
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                txtCustomerId.Text = row.Cells["customer_id"].Value.ToString();
                txtFirstName.Text = row.Cells["first_name"].Value.ToString();
                txtLastName.Text = row.Cells["last_name"].Value.ToString();
                txtPhone.Text = row.Cells["phone"]?.Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["email"].Value.ToString();
                txtStreet.Text = row.Cells["street"]?.Value?.ToString() ?? "";
                txtCity.Text = row.Cells["city"]?.Value?.ToString() ?? "";
                txtState.Text = row.Cells["state"]?.Value?.ToString() ?? "";
                _isEditing = false;
                EnableMaintenanceControls(false);
                btnEdit.Enabled = true;
                btnCancel.Enabled = true;
            }
        }
        private void EnableMaintenanceControls(bool enable)
        {
            txtFirstName.Enabled = enable;
            txtLastName.Enabled = enable;
            txtPhone.Enabled = enable;
            txtEmail.Enabled = enable;
            txtStreet.Enabled = enable;
            txtCity.Enabled = enable;
            txtState.Enabled = enable;
            btnSave.Enabled = enable;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            tabControl1.SelectedTab = tabPageList;
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchCustomers();
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchCustomers();
        }
        private void SearchCustomers()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();

                if (string.IsNullOrEmpty(searchText))
                {
                    LoadCustomers();
                    return;
                }
                DataTable allCustomers = _customersBusiness.GetCustomers();
                DataView dv = allCustomers.DefaultView;
                dv.RowFilter = $"first_name LIKE '%{searchText}%' OR last_name LIKE '%{searchText}%'";
                dgvCustomers.DataSource = dv.ToTable();
                lblTotalRecords.Text = $"Total: {dgvCustomers.Rows.Count} registros";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en búsqueda: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnDeleteSelected_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedIds = (from DataGridViewRow row in dgvCustomers.Rows
                                   where row.Cells["chkSelect"].Value != null &&
                                         (bool)row.Cells["chkSelect"].Value == true
                                   select Convert.ToInt32(row.Cells["customer_id"].Value)).ToList();
                if (selectedIds.Count == 0)
                {
                    MessageBox.Show("Seleccione al menos un cliente para eliminar", "Advertencia",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show($"¿Está seguro de eliminar {selectedIds.Count} cliente(s)?", "Confirmar",
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (int id in selectedIds)
                    {
                        _customersBusiness.DeleteCustomer(id);
                    }
                    MessageBox.Show("Clientes eliminados correctamente", "Éxito",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomers();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = dgvCustomers.SelectedRows.Count > 0 && !btnSave.Enabled;
        }
        private void DgvCustomers_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dgvCustomers.HorizontalScrollingOffset = e.NewValue;
            }
        }
    }
}