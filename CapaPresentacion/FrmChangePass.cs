using System;
using System.Data;
using System.Windows.Forms;
using CapaNegocio;
namespace CapaPresentacion.Forms
{
    public partial class FrmChangePass : Form
    {
        public FrmChangePass()
        {
            InitializeComponent();
            this.AcceptButton = btnVerify;
        }
        private void btnVerify_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new NUsers().VerifyUser(
                    Convert.ToInt32(txtUserId.Text),
                    txtEmail.Text,
                    txtFullName.Text);
                if (dt.Rows.Count > 0)
                {
                    gbNewPassword.Enabled = true;
                    btnSave.Enabled = true;
                    MessageBox.Show("Usuario verificado. Ahora puede cambiar su contraseña.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Datos de usuario no coinciden. Verifique la información.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar usuario: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtNewPassword.Text.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string result = new NUsers().ChangePassword(
                    Convert.ToInt32(txtUserId.Text),
                    txtNewPassword.Text);

                MessageBox.Show(result, "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar contraseña: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void togglePassword_CheckedChanged(object sender, EventArgs e)
        {
            txtNewPassword.PasswordChar = togglePassword.Checked ? '\0' : '•';
            txtConfirmPassword.PasswordChar = togglePassword.Checked ? '\0' : '•';
        }
    }
}