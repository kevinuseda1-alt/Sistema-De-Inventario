namespace PedidosApp
{
    partial class FrmDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Panel panelClientes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalClientes;
        private System.Windows.Forms.Panel panelProductos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTotalProductos;
        private System.Windows.Forms.Panel panelVentas;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblVentasMes;
        private System.Windows.Forms.Panel panelPedidos;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblPedidosPendientes;
        private System.Windows.Forms.Panel panelInfoEmpresa;
        private System.Windows.Forms.Label lblEmpresa;
        private System.Windows.Forms.Label lblPropietario;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblHorario;
        private System.Windows.Forms.Button btnActualizar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelStats = new System.Windows.Forms.Panel();
            this.panelPedidos = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.lblPedidosPendientes = new System.Windows.Forms.Label();
            this.panelVentas = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblVentasMes = new System.Windows.Forms.Label();
            this.panelProductos = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotalProductos = new System.Windows.Forms.Label();
            this.panelClientes = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalClientes = new System.Windows.Forms.Label();
            this.panelInfoEmpresa = new System.Windows.Forms.Panel();
            this.lblHorario = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.lblDireccion = new System.Windows.Forms.Label();
            this.lblPropietario = new System.Windows.Forms.Label();
            this.lblEmpresa = new System.Windows.Forms.Label();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.panelPedidos.SuspendLayout();
            this.panelVentas.SuspendLayout();
            this.panelProductos.SuspendLayout();
            this.panelClientes.SuspendLayout();
            this.panelInfoEmpresa.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTitulo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(554, 50);
            this.panel1.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(15, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(183, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "PANEL DE CONTROL";
            // 
            // panelStats
            // 
            this.panelStats.Controls.Add(this.panelPedidos);
            this.panelStats.Controls.Add(this.panelVentas);
            this.panelStats.Controls.Add(this.panelProductos);
            this.panelStats.Controls.Add(this.panelClientes);
            this.panelStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStats.Location = new System.Drawing.Point(0, 50);
            this.panelStats.Name = "panelStats";
            this.panelStats.Padding = new System.Windows.Forms.Padding(10);
            this.panelStats.Size = new System.Drawing.Size(554, 180);
            this.panelStats.TabIndex = 1;
            // 
            // panelPedidos
            // 
            this.panelPedidos.Controls.Add(this.label7);
            this.panelPedidos.Controls.Add(this.lblPedidosPendientes);
            this.panelPedidos.Location = new System.Drawing.Point(420, 10);
            this.panelPedidos.Name = "panelPedidos";
            this.panelPedidos.Size = new System.Drawing.Size(120, 160);
            this.panelPedidos.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label7.Location = new System.Drawing.Point(10, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 15);
            this.label7.TabIndex = 1;
            this.label7.Text = "Stock bajo";
            // 
            // lblPedidosPendientes
            // 
            this.lblPedidosPendientes.AutoSize = true;
            this.lblPedidosPendientes.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblPedidosPendientes.Location = new System.Drawing.Point(10, 40);
            this.lblPedidosPendientes.Name = "lblPedidosPendientes";
            this.lblPedidosPendientes.Size = new System.Drawing.Size(50, 37);
            this.lblPedidosPendientes.TabIndex = 0;
            this.lblPedidosPendientes.Text = "12";
            // 
            // panelVentas
            // 
            this.panelVentas.Controls.Add(this.label5);
            this.panelVentas.Controls.Add(this.lblVentasMes);
            this.panelVentas.Location = new System.Drawing.Point(290, 10);
            this.panelVentas.Name = "panelVentas";
            this.panelVentas.Size = new System.Drawing.Size(120, 160);
            this.panelVentas.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.Location = new System.Drawing.Point(10, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Valor inventario";
            // 
            // lblVentasMes
            // 
            this.lblVentasMes.AutoSize = true;
            this.lblVentasMes.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblVentasMes.Location = new System.Drawing.Point(10, 40);
            this.lblVentasMes.Name = "lblVentasMes";
            this.lblVentasMes.Size = new System.Drawing.Size(80, 25);
            this.lblVentasMes.TabIndex = 0;
            this.lblVentasMes.Text = "C$ 0.00";
            // 
            // panelProductos
            // 
            this.panelProductos.Controls.Add(this.label3);
            this.panelProductos.Controls.Add(this.lblTotalProductos);
            this.panelProductos.Location = new System.Drawing.Point(150, 10);
            this.panelProductos.Name = "panelProductos";
            this.panelProductos.Size = new System.Drawing.Size(120, 160);
            this.panelProductos.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.Location = new System.Drawing.Point(10, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Total Productos";
            // 
            // lblTotalProductos
            // 
            this.lblTotalProductos.AutoSize = true;
            this.lblTotalProductos.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotalProductos.Location = new System.Drawing.Point(10, 40);
            this.lblTotalProductos.Name = "lblTotalProductos";
            this.lblTotalProductos.Size = new System.Drawing.Size(50, 37);
            this.lblTotalProductos.TabIndex = 0;
            this.lblTotalProductos.Text = "89";
            // 
            // panelClientes
            // 
            this.panelClientes.Controls.Add(this.label1);
            this.panelClientes.Controls.Add(this.lblTotalClientes);
            this.panelClientes.Location = new System.Drawing.Point(10, 10);
            this.panelClientes.Name = "panelClientes";
            this.panelClientes.Size = new System.Drawing.Size(120, 160);
            this.panelClientes.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Proveedores";
            // 
            // lblTotalClientes
            // 
            this.lblTotalClientes.AutoSize = true;
            this.lblTotalClientes.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotalClientes.Location = new System.Drawing.Point(10, 40);
            this.lblTotalClientes.Name = "lblTotalClientes";
            this.lblTotalClientes.Size = new System.Drawing.Size(67, 37);
            this.lblTotalClientes.TabIndex = 0;
            this.lblTotalClientes.Text = "125";
            // 
            // panelInfoEmpresa
            // 
            this.panelInfoEmpresa.Controls.Add(this.lblHorario);
            this.panelInfoEmpresa.Controls.Add(this.lblEmail);
            this.panelInfoEmpresa.Controls.Add(this.lblTelefono);
            this.panelInfoEmpresa.Controls.Add(this.lblDireccion);
            this.panelInfoEmpresa.Controls.Add(this.lblPropietario);
            this.panelInfoEmpresa.Controls.Add(this.lblEmpresa);
            this.panelInfoEmpresa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfoEmpresa.Location = new System.Drawing.Point(0, 230);
            this.panelInfoEmpresa.Name = "panelInfoEmpresa";
            this.panelInfoEmpresa.Padding = new System.Windows.Forms.Padding(20);
            this.panelInfoEmpresa.Size = new System.Drawing.Size(554, 183);
            this.panelInfoEmpresa.TabIndex = 2;
            // 
            // lblHorario
            // 
            this.lblHorario.AutoSize = true;
            this.lblHorario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHorario.Location = new System.Drawing.Point(20, 140);
            this.lblHorario.Name = "lblHorario";
            this.lblHorario.Size = new System.Drawing.Size(137, 15);
            this.lblHorario.TabIndex = 5;
            this.lblHorario.Text = "Horario: L-V 9:00-18:00";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEmail.Location = new System.Drawing.Point(20, 110);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(144, 15);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "Email: info@bikestore.com";
            // 
            // lblTelefono
            // 
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTelefono.Location = new System.Drawing.Point(20, 80);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(151, 15);
            this.lblTelefono.TabIndex = 3;
            this.lblTelefono.Text = "Teléfono: +1 234 567 890";
            // 
            // lblDireccion
            // 
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDireccion.Location = new System.Drawing.Point(20, 50);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(131, 15);
            this.lblDireccion.TabIndex = 2;
            this.lblDireccion.Text = "Av. Principal 123, Ciudad";
            // 
            // lblPropietario
            // 
            this.lblPropietario.AutoSize = true;
            this.lblPropietario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPropietario.Location = new System.Drawing.Point(20, 30);
            this.lblPropietario.Name = "lblPropietario";
            this.lblPropietario.Size = new System.Drawing.Size(111, 15);
            this.lblPropietario.TabIndex = 1;
            this.lblPropietario.Text = "Josué Claros Roca";
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.AutoSize = true;
            this.lblEmpresa.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblEmpresa.Location = new System.Drawing.Point(15, 10);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(85, 21);
            this.lblEmpresa.TabIndex = 0;
            this.lblEmpresa.Text = "BIKE STORE";
            // 
            // btnActualizar
            // 
            this.btnActualizar.BackColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.btnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnActualizar.Location = new System.Drawing.Point(450, 380);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(90, 25);
            this.btnActualizar.TabIndex = 3;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // FrmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 413);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.panelInfoEmpresa);
            this.Controls.Add(this.panelStats);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelStats.ResumeLayout(false);
            this.panelPedidos.ResumeLayout(false);
            this.panelPedidos.PerformLayout();
            this.panelVentas.ResumeLayout(false);
            this.panelVentas.PerformLayout();
            this.panelProductos.ResumeLayout(false);
            this.panelProductos.PerformLayout();
            this.panelClientes.ResumeLayout(false);
            this.panelClientes.PerformLayout();
            this.panelInfoEmpresa.ResumeLayout(false);
            this.panelInfoEmpresa.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}