using CapaNegocio;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace CapaPresentacion.Forms
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            this.AcceptButton = btnLogin;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
               DataTable dt = new NLogin().Login(txtUsername.Text, txtPassword.Text);
                if (dt.Rows.Count > 0)
                {
                    int user_id = Convert.ToInt32(dt.Rows[0]["user_id"]);
                    string role = dt.Rows[0]["role"].ToString();
                    string fullName = dt.Rows[0]["full_name"].ToString();
                    FrmPrincipal frm = new FrmPrincipal(role)
                    {
                        idTrabajador = user_id.ToString(),
                        nombre = fullName.Split(' ')[0],
                        apellido = fullName.Contains(" ") ? fullName.Split(' ')[1] : "",
                        acceso = role
                    };

                    frm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar sesión: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            FrmChangePass frmChangePass = new FrmChangePass();
            frmChangePass.ShowDialog();
        }
    }
}