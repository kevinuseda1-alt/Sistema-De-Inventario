using CapaNegocio;
using FontAwesome.Sharp;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace CapaPresentacion.Forms
{
    public partial class FrmUsers : Form
    {
        private string _currentRole;
        private bool _showCheckboxes = false;
        private bool _isEditing = false;

        public FrmUsers(string userRole)
        {
            InitializeComponent();
            _currentRole = userRole;
            InitializeIcons();
            CheckAdminAccess();
            DisableControls();
        }
        private void InitializeIcons()
        {
            btnNew.IconChar = IconChar.PlusCircle;
            btnEdit.IconChar = IconChar.Edit;
            btnSave.IconChar = IconChar.Save;
            btnCancel.IconChar = IconChar.TimesCircle;
            btnDelete.IconChar = IconChar.Trash;
            btnSearch.IconChar = IconChar.Search;
            btnPrint.IconChar = IconChar.Print;
        }
        private void CheckAdminAccess()
        {
            if (_currentRole != "admin")
            {
                this.Close();
                return;
            }
            LoadUsers();
        }
        private void DisableControls()
        {
            txtUsername.Enabled = false;
            txtFullName.Enabled = false;
            txtPassword.Enabled = false;
            txtEmail.Enabled = false;
            cbRole.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnNew.Enabled = true;
        }
        private void EnableControlsForEdit()
        {
            txtUsername.Enabled = true;
            txtFullName.Enabled = true;
            txtPassword.Enabled = true;
            txtEmail.Enabled = true;
            cbRole.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnEdit.Enabled = false;
        }
        private void LoadUsers()
        {
            try
            {
                dgvUsers.Columns.Clear();
                dgvUsers.DataSource = new NUsers().ListarUsuarios();

                if (chkDelete.Checked && !dgvUsers.Columns.Contains("chkSelect"))
                {
                    DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn();
                    chkColumn.HeaderText = "Sel.";
                    chkColumn.Name = "chkSelect";
                    chkColumn.ReadOnly = false;
                    dgvUsers.Columns.Insert(0, chkColumn);
                }

                FormatDataGridView();
                lblTotalRecords.Text = $"Total registros: {dgvUsers.Rows.Count}";
                CheckSelectedUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FormatDataGridView()
        {
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BackgroundColor = Color.FromArgb(60, 60, 60);
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvUsers.DefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvUsers.DefaultCellStyle.ForeColor = Color.White;
            dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 80, 80);
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.ScrollBars = ScrollBars.Both;
            HideUnwantedColumns();
            RenameColumns();
            string[] columnsToHide = { "is_active", "created_date" };
            foreach (string columnName in columnsToHide)
            {
                if (dgvUsers.Columns[columnName] != null)
                {
                    dgvUsers.Columns[columnName].Visible = false;
                }
            }
            if (_showCheckboxes)
            {
                AddCheckboxColumn();
            }
        }
        private void HideUnwantedColumns()
        {
            string[] columnsToHide = { "is_active", "created_date", "last_login_date" };
            foreach (string columnName in columnsToHide)
            {
                if (dgvUsers.Columns[columnName] != null)
                {
                    dgvUsers.Columns[columnName].Visible = false;
                }
            }
        }
        private void RenameColumns()
        {
            RenameColumnIfExists("user_id", "ID");
            RenameColumnIfExists("username", "Usuario");
            RenameColumnIfExists("full_name", "Nombre Completo");
            RenameColumnIfExists("email", "Correo Electrónico");
            RenameColumnIfExists("role", "Rol");
        }
        private void RenameColumnIfExists(string columnName, string newHeaderText)
        {
            if (dgvUsers.Columns[columnName] != null)
            {
                dgvUsers.Columns[columnName].HeaderText = newHeaderText;
            }
        }
        private void AddCheckboxColumn()
        {
            if (dgvUsers.Columns.Contains("Seleccionar")) return;
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Seleccionar",
                Name = "Seleccionar",
                Width = 70,
                ReadOnly = false
            };
            dgvUsers.Columns.Insert(0, checkBoxColumn);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new NUsers().SearchUsers(txtSearch.Text);
                dgvUsers.DataSource = dt;
                FormatDataGridView();
                lblTotalRecords.Text = $"Total registros: {dt.Rows.Count}";
                CheckSelectedUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectedTab = tabPageMaintenance;
                ClearFields();
                int siguienteId = new NUsers().ObtenerSiguienteId();
                txtUserId.Text = siguienteId.ToString();
                EnableControlsForEdit();
                _isEditing = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al preparar nuevo usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                try
                {
                    if (!int.TryParse(txtUserId.Text, out int userId))
                    {
                        MessageBox.Show("ID de usuario inválido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string result;
                    if (_isEditing)
                    {
                        result = new NUsers().UpdateUser(
                            userId,
                            txtUsername.Text,
                            txtFullName.Text,
                            txtEmail.Text,
                            cbRole.SelectedItem.ToString(),
                            true);
                        if (!string.IsNullOrEmpty(txtPassword.Text))
                        {
                            new NUsers().ChangePassword(userId, txtPassword.Text);
                        }
                    }
                    else
                    {
                        result = new NUsers().CreateUser(
                            txtUsername.Text,
                            txtPassword.Text,
                            txtFullName.Text,
                            txtEmail.Text,
                            cbRole.SelectedItem.ToString(),
                            out userId);
                    }
                    MessageBox.Show(result, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                    tabControl1.SelectedTab = tabPageList;
                    DisableControls();
                    _isEditing = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                cbRole.SelectedItem == null)
            {
                MessageBox.Show("Todos los campos son obligatorios (excepto contraseña para edición)", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!_isEditing && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("La contraseña es obligatoria para nuevos usuarios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            txtEmail.Clear();
            cbRole.SelectedIndex = -1;
        }
        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    tabControl1.SelectedTab = tabPageMaintenance;
                    DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                    txtUserId.Text = row.Cells["user_id"].Value.ToString();
                    txtUsername.Text = row.Cells["username"].Value.ToString();
                    txtFullName.Text = row.Cells["full_name"].Value.ToString();
                    txtEmail.Text = row.Cells["email"].Value.ToString();
                    cbRole.SelectedItem = row.Cells["role"].Value.ToString();
                    txtPassword.Clear();
                    DisableControls();
                    btnEdit.Enabled = true;
                    _isEditing = false;
                    dgvUsers.EndEdit();
                    CheckSelectedUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageList;
            ClearFields();
            DisableControls();
            _isEditing = false;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserId.Text))
            {
                EnableControlsForEdit();
                _isEditing = true;
            }
        }
        private void CheckSelectedUsers()
        {
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!dgvUsers.Columns.Contains("chkSelect"))
            {
                MessageBox.Show("Active primero el modo de eliminación", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var selectedRows = dgvUsers.Rows
                .Cast<DataGridViewRow>()
                .Where(row => row.Cells["chkSelect"].Value != null &&
                              Convert.ToBoolean(row.Cells["chkSelect"].Value))
                .ToList();

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un usuario para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"¿Eliminar {selectedRows.Count} usuarios seleccionados?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in selectedRows)
                {
                    if (row.Cells["user_id"].Value != null)
                    {
                        int userId = Convert.ToInt32(row.Cells["user_id"].Value);
                        string result = new NUsers().DeleteUser(userId);
                    }
                }
                MessageBox.Show("Usuarios eliminados correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
            }
        }
        private void chkDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDelete.Checked)
            {
                if (!dgvUsers.Columns.Contains("chkSelect"))
                {
                    DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn();
                    chkColumn.HeaderText = "Sel.";
                    chkColumn.Name = "chkSelect";
                    chkColumn.ReadOnly = false;
                    dgvUsers.Columns.Insert(0, chkColumn);
                }
            }
            else
            {
                if (dgvUsers.Columns.Contains("chkSelect"))
                {
                    dgvUsers.Columns.Remove("chkSelect");
                }
            }
            CheckSelectedUsers();
        }
        private void dgvUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                CheckSelectedUsers();
            }
        }

    }
}