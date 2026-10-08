using System;
using System.Drawing;
using System.Windows.Forms;

namespace PedidosApp
{
    public partial class FrmInicio : Form
    {
        public FrmInicio()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {
            lblBienvenida.Text = "BIENVENIDO";
            lblBienvenida.ForeColor = Color.FromArgb(255,255, 255);
        }

        private void timerAnimacion_Tick(object sender, EventArgs e)
        {
            if (lblBienvenida.ForeColor.A < 255)
            {
                lblBienvenida.ForeColor = Color.FromArgb(
                    lblBienvenida.ForeColor.A + 5,
                    lblBienvenida.ForeColor);
            }
            else
            {
                timerAnimacion.Stop();
            }
        }
    }
}