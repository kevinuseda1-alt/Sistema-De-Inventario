using FontAwesome.Sharp;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaNegocio;
namespace CapaPresentacion.Forms
{
    public partial class FrmCategories : Form
    {
        private bool isEditMode = false;
        public FrmCategories()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(40, 40, 40);
        }
        private void FrmCategories_Load(object sender, EventArgs e)
        {
            ConfigureStyles();
            LoadCategories();
            ResetForm();
        }
        private void ConfigureStyles()
        {
            dgvCategories.BackgroundColor = Color.FromArgb(60, 60, 60);
            dgvCategories.GridColor = Color.Gray;
            dgvCategories.BorderStyle = BorderStyle.None;
            dgvCategories.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvCategories.EnableHeadersVisualStyles = false;
            dgvCategories.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvCategories.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCategories.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvCategories.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCategories.RowsDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvCategories.RowsDefaultCellStyle.ForeColor = Color.White;
            dgvCategories.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 80, 80);
            dgvCategories.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            ConfigureFontAwesomeButton(btnNew, "Nuevo", IconChar.PlusCircle);
            ConfigureFontAwesomeButton(btnEdit, "Editar", IconChar.Edit);
            ConfigureFontAwesomeButton(btnSave, "Guardar", IconChar.Save);
            ConfigureFontAwesomeButton(btnDelete, "Eliminar", IconChar.Trash);
            ConfigureFontAwesomeButton(btnCancel, "Cancelar", IconChar.TimesCircle);
            ConfigureFontAwesomeButton(btnSearch, "Buscar", IconChar.Search);
            txtCategoryName.BackColor = Color.FromArgb(60, 60, 60);
            txtCategoryName.ForeColor = Color.White;
            txtCategoryId.BackColor = Color.FromArgb(60, 60, 60);
            txtCategoryId.ForeColor = Color.White;
            txtSearch.BackColor = Color.FromArgb(60, 60, 60);
            txtSearch.ForeColor = Color.White;
            tabControl1.BackColor = Color.FromArgb(40, 40, 40);
            tabPageList.BackColor = Color.FromArgb(40, 40, 40);
            tabPageMaintenance.BackColor = Color.FromArgb(40, 40, 40);
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
            btn.BackColor = Color.FromArgb(70, 70, 70);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.Gray;
            btn.FlatAppearance.BorderSize = 1;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btn.Padding = new Padding(5, 2, 5, 2);
            btn.Cursor = Cursors.Hand;
            btn.AutoSize = true;
            btn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
        private void LoadCategories()
        {
            dgvCategories.DataSource = NCategories.Mostrar();
            if (dgvCategories.Columns.Contains("category_id"))
                dgvCategories.Columns["category_id"].HeaderText = "ID";
            if (dgvCategories.Columns.Contains("category_name"))
                dgvCategories.Columns["category_name"].HeaderText = "Nombre Categoría";
            if (!dgvCategories.Columns.Contains("Select"))
            {
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                checkBoxColumn.HeaderText = "Sel.";
                checkBoxColumn.Name = "Select";
                checkBoxColumn.Width = 40;
                dgvCategories.Columns.Insert(0, checkBoxColumn);
            }
            foreach (DataGridViewColumn col in dgvCategories.Columns)
            {
                if (col.Name == "Select")
                    col.ReadOnly = false;
                else
                    col.ReadOnly = true;
            }
            dgvCategories.ClearSelection();
        }
        private void SetControlsEnabled(bool enabled)
        {
            txtCategoryName.Enabled = enabled;
            txtCategoryId.Enabled = false;
            btnSave.Enabled = enabled;
            btnEdit.Enabled = !enabled && dgvCategories.SelectedRows.Count > 0;
        }
        private void ResetForm()
        {
            txtCategoryId.Text = "";
            txtCategoryName.Text = "";
            btnSave.Text = "Guardar";
            isEditMode = false;
            SetControlsEnabled(false);
            btnEdit.Enabled = false;
        }
        private void dgvCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCategories.Columns[e.ColumnIndex].Name == "Select")
            {
                dgvCategories.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Ingrese el nombre de la categoría", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (isEditMode)
                {
                    int id = Convert.ToInt32(txtCategoryId.Text);
                    string rpta = NCategories.Editar(id, txtCategoryName.Text);
                    if (rpta == "OK")
                        MessageBox.Show("Categoría actualizada correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Error al actualizar: " + rpta, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int idGenerado = 0;
                    string rpta = NCategories.Insertar(txtCategoryName.Text, ref idGenerado);

                    if (rpta == "OK")
                        MessageBox.Show($"Categoría guardada. ID: {idGenerado}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Error al guardar: " + rpta, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadCategories();
                ResetForm();
                tabControl1.SelectedTab = tabPageList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool hasSelection = false;
            foreach (DataGridViewRow row in dgvCategories.Rows)
            {
                if (row.Cells["Select"].Value != null && Convert.ToBoolean(row.Cells["Select"].Value))
                {
                    hasSelection = true;
                    break;
                }
            }
            if (!hasSelection)
            {
                MessageBox.Show("Seleccione al menos una categoría para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("¿Está seguro de eliminar las categorías seleccionadas?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool allDeleted = true;
                    string errors = "";
                    foreach (DataGridViewRow row in dgvCategories.Rows)
                    {
                        if (row.Cells["Select"].Value != null && Convert.ToBoolean(row.Cells["Select"].Value))
                        {
                            int id = Convert.ToInt32(row.Cells["category_id"].Value);
                            string rpta = NCategories.Eliminar(id);
                            if (rpta != "OK")
                            {
                                allDeleted = false;
                                errors += $"Error al eliminar categoría ID {id}: {rpta}\n";
                            }
                        }
                    }
                    if (allDeleted)
                    {
                        MessageBox.Show("Categorías eliminadas correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Algunas categorías no se pudieron eliminar:\n" + errors, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    LoadCategories();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void dgvCategories_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtCategoryId.Text = dgvCategories.Rows[e.RowIndex].Cells["category_id"].Value.ToString();
                txtCategoryName.Text = dgvCategories.Rows[e.RowIndex].Cells["category_name"].Value.ToString();
                isEditMode = true;
                SetControlsEnabled(false);
                btnEdit.Enabled = true;
                tabControl1.SelectedTab = tabPageMaintenance;
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    DataTable dt = NCategories.Mostrar();
                    DataView dv = new DataView(dt);
                    dv.RowFilter = $"category_name LIKE '%{searchText}%'";
                    dgvCategories.DataSource = dv.ToTable();
                }
                else
                {
                    LoadCategories();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetControlsEnabled(true);
            tabControl1.SelectedTab = tabPageMaintenance;
            txtCategoryName.Focus();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            tabControl1.SelectedTab = tabPageList;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            SetControlsEnabled(true);
            btnEdit.Enabled = false;
        }
        private void dgvCategories_SelectionChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = dgvCategories.SelectedRows.Count > 0;
        }
    }
}