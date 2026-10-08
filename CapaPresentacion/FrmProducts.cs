using FontAwesome.Sharp;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CapaNegocio;
namespace CapaPresentacion.Forms
{
    public partial class FrmProducts : Form
    {
        private bool isEditMode = false;
        private bool isNewMode = false;
        private DataTable categories;
        public FrmProducts()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.ClientSize = new Size(570, 452);
        }
        private void FrmProducts_Load(object sender, EventArgs e)
        {
            ConfigureStyles();
            LoadProducts();
            LoadCategories();
            ResetForm();
            DisableFormControls();
            UpdateRecordCount();
        }
        private void ConfigureStyles()
        {
            this.ForeColor = Color.White;
            this.BackColor = Color.FromArgb(40, 40, 40);
            dgvProducts.BackgroundColor = Color.FromArgb(60, 60, 60);
            dgvProducts.GridColor = Color.Gray;
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvProducts.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvProducts.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.RowsDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvProducts.RowsDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 80, 80);
            dgvProducts.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            dgvProducts.RowTemplate.Height = 60;
            tabControl1.BackColor = Color.FromArgb(40, 40, 40);
            tabPageList.BackColor = Color.FromArgb(40, 40, 40);
            tabPageMaintenance.BackColor = Color.FromArgb(40, 40, 40);
            ConfigureFontAwesomeButton(btnNew, "Nuevo", IconChar.FileAlt);
            ConfigureFontAwesomeButton(btnEdit, "Editar", IconChar.Edit);
            ConfigureFontAwesomeButton(btnSave, "Guardar", IconChar.Save);
            ConfigureFontAwesomeButton(btnDelete, "Eliminar", IconChar.Trash);
            ConfigureFontAwesomeButton(btnCancel, "Cancelar", IconChar.TimesCircle);
            ConfigureFontAwesomeButton(btnSearch, "Buscar", IconChar.Search);
            ConfigureFontAwesomeButton(btnPrint, "Imprimir", IconChar.Print);
            ConfigureFontAwesomeButton(btnUploadImage, "Cargar Imagen", IconChar.Upload);
            ConfigureFontAwesomeButton(btnRemoveImage, "Quitar Imagen", IconChar.TrashAlt);
            lblTotalRecords.ForeColor = Color.White;
            lblTotalRecords.BackColor = Color.Transparent;
            lblTotalRecords.TextAlign = ContentAlignment.MiddleLeft;
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
            btn.AutoSize = true;
            btn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
        private void LoadProducts()
        {
            dgvProducts.DataSource = NProducts.Mostrar();
            if (dgvProducts.Columns.Contains("product_id"))
                dgvProducts.Columns["product_id"].HeaderText = "ID";
            if (dgvProducts.Columns.Contains("product_name"))
                dgvProducts.Columns["product_name"].HeaderText = "Nombre";
            if (dgvProducts.Columns.Contains("model_year"))
                dgvProducts.Columns["model_year"].HeaderText = "Año";
            if (dgvProducts.Columns.Contains("price"))
                {
                    dgvProducts.Columns["price"].HeaderText = "Precio (C$)";
                    dgvProducts.Columns["price"].DefaultCellStyle.Format = "C2";
                    dgvProducts.Columns["price"].DefaultCellStyle.FormatProvider = CapaPresentacion.Moneda.Cultura;
                }
            if (dgvProducts.Columns.Contains("imagen"))
                dgvProducts.Columns["imagen"].HeaderText = "Imagen";
            if (dgvProducts.Columns.Contains("category_id"))
                dgvProducts.Columns["category_id"].HeaderText = "Categoría ID";
            if (dgvProducts.Columns.Contains("create_date"))
                dgvProducts.Columns["create_date"].HeaderText = "Fecha Creación";
            AddCheckBoxColumn();
            dgvProducts.ClearSelection();
            AjustarEstiloImagenes();
            UpdateRecordCount();
            dgvProducts.ReadOnly = false;
        }
        private void AddCheckBoxColumn()
        {
            if (dgvProducts.Columns.Contains("Select"))
            {
                dgvProducts.Columns.Remove("Select");
            }
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "Sel.";
            checkBoxColumn.Name = "Select";
            checkBoxColumn.Width = 40;
            dgvProducts.Columns.Insert(0, checkBoxColumn);
        }
        private void AjustarEstiloImagenes()
        {
            if (dgvProducts.Columns.Contains("imagen"))
            {
                DataGridViewImageColumn imgCol = (DataGridViewImageColumn)dgvProducts.Columns["imagen"];
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }
        }
        private void LoadCategories()
        {
            categories = NCategories.Mostrar();
            cbCategory.DataSource = categories;
            cbCategory.DisplayMember = "category_name";
            cbCategory.ValueMember = "category_id";
            cbCategory.BackColor = Color.FromArgb(60, 60, 60);
            cbCategory.ForeColor = Color.White;
            cbCategory.FlatStyle = FlatStyle.Flat;
        }
        private void ResetForm()
        {
            txtProductId.Text = "";
            txtProductName.Text = "";
            numModelYear.Value = DateTime.Now.Year;
            numPrice.Value = 0;
            pbImage.Image = null;
            if (cbCategory.Items.Count > 0)
                cbCategory.SelectedIndex = 0;
            dtpCreateDate.Value = DateTime.Now;
            btnSave.Text = "Guardar";
            isEditMode = false;
            isNewMode = false;
            txtProductName.Focus();
        }
        private void EnableFormControls()
        {
            txtProductName.Enabled = true;
            txtProductName.BackColor = Color.FromArgb(60, 60, 60);
            numModelYear.Enabled = true;
            numModelYear.BackColor = Color.FromArgb(60, 60, 60);
            numPrice.Enabled = true;
            numPrice.BackColor = Color.FromArgb(60, 60, 60);
            cbCategory.Enabled = true;
            dtpCreateDate.Enabled = true;
            btnUploadImage.Enabled = true;
            btnRemoveImage.Enabled = true;
        }
        private void DisableFormControls()
        {
            txtProductName.Enabled = false;
            txtProductName.BackColor = Color.FromArgb(50, 50, 50);
            numModelYear.Enabled = false;
            numModelYear.BackColor = Color.FromArgb(50, 50, 50);
            numPrice.Enabled = false;
            numPrice.BackColor = Color.FromArgb(50, 50, 50);
            cbCategory.Enabled = false;
            dtpCreateDate.Enabled = false;
            btnUploadImage.Enabled = false;
            btnRemoveImage.Enabled = false;
        }
        private void UpdateRecordCount()
        {
            int count = dgvProducts.Rows.Count;
            lblTotalRecords.Text = $"Total de registros: {count}";
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isEditMode && !isNewMode) return;
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Ingrese el nombre del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                byte[] imageBytes = null;
                if (pbImage.Image != null)
                {
                    Bitmap bmp = new Bitmap(pbImage.Image);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imageBytes = ms.ToArray();
                        bmp.Dispose();
                    }
                }
                if (isEditMode)
                {
                    int id = Convert.ToInt32(txtProductId.Text);
                    string rpta = NProducts.Editar(
                        id,
                        txtProductName.Text,
                        (short)numModelYear.Value,
                        numPrice.Value,
                        imageBytes,
                        Convert.ToInt32(cbCategory.SelectedValue),
                        dtpCreateDate.Value
                    );
                    if (rpta == "OK")
                        MessageBox.Show("Producto actualizado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Error al actualizar: " + rpta, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (isNewMode)
                {
                    int idGenerado = 0;
                    string rpta = NProducts.Insertar(
                        txtProductName.Text,
                        (short)numModelYear.Value,
                        numPrice.Value,
                        imageBytes,
                        Convert.ToInt32(cbCategory.SelectedValue),
                        dtpCreateDate.Value,
                        ref idGenerado
                    );
                    if (rpta == "OK")
                        MessageBox.Show($"Producto guardado. ID: {idGenerado}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Error al guardar: " + rpta, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadProducts();
                ResetForm();
                DisableFormControls();
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
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.Cells["Select"].Value != null && Convert.ToBoolean(row.Cells["Select"].Value))
                {
                    hasSelection = true;
                    break;
                }
            }
            if (!hasSelection)
            {
                MessageBox.Show("Seleccione al menos un producto para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("¿Está seguro de eliminar los productos seleccionados?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool allDeleted = true;
                    string errors = "";

                    foreach (DataGridViewRow row in dgvProducts.Rows)
                    {
                        if (row.Cells["Select"].Value != null && Convert.ToBoolean(row.Cells["Select"].Value))
                        {
                            int id = Convert.ToInt32(row.Cells["product_id"].Value);
                            string rpta = NProducts.Eliminar(id);
                            if (rpta != "OK")
                            {
                                allDeleted = false;
                                errors += $"Error al eliminar producto ID {id}: {rpta}\n";
                            }
                        }
                    }
                    if (allDeleted)
                    {
                        MessageBox.Show("Productos eliminados correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Algunos productos no se pudieron eliminar:\n" + errors, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
                txtProductId.Text = row.Cells["product_id"].Value.ToString();
                txtProductName.Text = row.Cells["product_name"].Value.ToString();
                numModelYear.Value = Convert.ToInt16(row.Cells["model_year"].Value);
                numPrice.Value = Convert.ToDecimal(row.Cells["price"].Value);
                if (row.Cells["imagen"].Value != DBNull.Value && row.Cells["imagen"].Value != null)
                {
                    try
                    {
                        byte[] imageData = (byte[])row.Cells["imagen"].Value;
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            pbImage.Image = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar la imagen: " + ex.Message);
                        pbImage.Image = null;
                    }
                }
                else
                {
                    pbImage.Image = null;
                }
                cbCategory.SelectedValue = Convert.ToInt32(row.Cells["category_id"].Value);
                dtpCreateDate.Value = Convert.ToDateTime(row.Cells["create_date"].Value);
                DisableFormControls();
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
                    dgvProducts.DataSource = NProducts.Buscar(searchText);
                    AddCheckBoxColumn();
                }
                else
                {
                    LoadProducts();
                }
                UpdateRecordCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            ResetForm();
            EnableFormControls();
            isNewMode = true;
            isEditMode = false;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            DisableFormControls();
            tabControl1.SelectedTab = tabPageList;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProductId.Text))
            {
                EnableFormControls();
                isEditMode = true;
                isNewMode = false;
            }
            else
            {
                MessageBox.Show("No hay un producto seleccionado para editar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        pbImage.Image = Image.FromStream(fs);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            pbImage.Image = null;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                FrmReporteProducto frm = new FrmReporteProducto();
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el reporte: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}