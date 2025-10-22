namespace StroboTech_Visualize
{
    partial class Form1
    {
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.Button btnLimpar;
        private OxyPlot.WindowsForms.PlotView plotGrafico;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label lblPeakAcc;
        private System.Windows.Forms.Label lblRmsAcc;
        private System.Windows.Forms.Label lblStdDev;
        private System.Windows.Forms.Label lblVRms;
        private System.Windows.Forms.Label lblFreqDom;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            btnAdicionar = new Button();
            btnLimpar = new Button();
            plotGrafico = new OxyPlot.WindowsForms.PlotView();
            panelInfo = new Panel();
            legendPanel = new Panel();
            colorX = new Panel();
            lblX = new Label();
            colorY = new Panel();
            lblY = new Label();
            colorZ = new Panel();
            lblZ = new Label();
            lblPeakAcc = new Label();
            lblRmsAcc = new Label();
            lblStdDev = new Label();
            lblVRms = new Label();
            lblFreqDom = new Label();

            // -------------------- Barra de Status --------------------
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();

            statusStrip.Items.Add(lblStatus);
            statusStrip.Dock = DockStyle.Bottom;
            statusStrip.BackColor = Color.FromArgb(240, 240, 240);
            lblStatus.Text = "Grupo de Projeto Alfas - https://alfasweb.com.br - 2025";
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            lblStatus.ForeColor = Color.Black;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            panelInfo.SuspendLayout();
            legendPanel.SuspendLayout();
            SuspendLayout();

            int larguraBotao = 50;
            int espacamento = 10;
            int totalLargura = 3 * larguraBotao + 2 * espacamento;
            int posXInicial = (panelInfo.Width - totalLargura) / 2;
            int posY = 455;

            // -------------------- Botões --------------------
            btnAdicionar.Text = "Adicionar CSV";
            btnAdicionar.Size = new Size(120, 40);
            btnAdicionar.Location = new Point(10, 10);
            btnAdicionar.FlatStyle = FlatStyle.Flat;
            btnAdicionar.BackColor = Color.FromArgb(52, 152, 219);
            btnAdicionar.ForeColor = Color.White;
            btnAdicionar.Click += btnAdicionar_Click;

            btnLimpar.Text = "Limpar Dados";
            btnLimpar.Size = new Size(120, 40);
            btnLimpar.Location = new Point(140, 10);
            btnLimpar.FlatStyle = FlatStyle.Flat;
            btnLimpar.BackColor = Color.FromArgb(231, 76, 60);
            btnLimpar.ForeColor = Color.White;
            btnLimpar.Click += btnLimpar_Click;

            // -------------------- Gráfico --------------------
            plotGrafico.Location = new Point(12, 12);
            plotGrafico.Size = new Size(720, 670);
            plotGrafico.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            plotGrafico.PanCursor = Cursors.Hand;
            plotGrafico.ZoomRectangleCursor = Cursors.Cross;
            plotGrafico.Model = new OxyPlot.PlotModel { Padding = new OxyPlot.OxyThickness(10) };

            // -------------------- Painel lateral --------------------
            panelInfo.Size = new Size(336, 670);
            panelInfo.Location = new Point(744, 12);
            panelInfo.BackColor = Color.FromArgb(245, 245, 245);
            panelInfo.BorderStyle = BorderStyle.FixedSingle;
            panelInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            panelInfo.Padding = new Padding(10);

            panelInfo.Controls.Add(btnAdicionar);
            panelInfo.Controls.Add(btnLimpar);

            // -------------------- Legenda --------------------
            legendPanel.Location = new Point(10, 60);
            legendPanel.Size = new Size(310, 100);

            colorX.BackColor = Color.Red;
            colorX.Size = new Size(20, 20);
            colorX.Location = new Point(10, 10);
            lblX.Text = "Eixo X";
            lblX.Location = new Point(40, 10);
            lblX.AutoSize = true;

            colorY.BackColor = Color.Green;
            colorY.Size = new Size(20, 20);
            colorY.Location = new Point(10, 40);
            lblY.Text = "Eixo Y";
            lblY.Location = new Point(40, 40);
            lblY.AutoSize = true;

            colorZ.BackColor = Color.Blue;
            colorZ.Size = new Size(20, 20);
            colorZ.Location = new Point(10, 70);
            lblZ.Text = "Eixo Z";
            lblZ.Location = new Point(40, 70);
            lblZ.AutoSize = true;

            legendPanel.Controls.AddRange(new Control[] { colorX, lblX, colorY, lblY, colorZ, lblZ });
            panelInfo.Controls.Add(legendPanel);

            // -------------------- Linha divisória --------------------
            var linhaDivisoria = new Panel();
            linhaDivisoria.BackColor = Color.Gray;
            linhaDivisoria.Height = 2;
            linhaDivisoria.Width = panelInfo.Width - 20;
            linhaDivisoria.Location = new Point(10, 400);
            panelInfo.Controls.Add(linhaDivisoria);

            // -------------------- Botão Reset Zoom --------------------
            var btnResetZoom = new Button();
            btnResetZoom.Text = "Reset Zoom";
            btnResetZoom.Size = new Size(100, 35);
            btnResetZoom.Location = new Point(10, 410);
            btnResetZoom.FlatStyle = FlatStyle.Flat;
            btnResetZoom.BackColor = Color.FromArgb(52, 152, 219);
            btnResetZoom.ForeColor = Color.White;
            btnResetZoom.Click += BtnResetZoom_Click;
            panelInfo.Controls.Add(btnResetZoom);

            // -------------------- Botões X/Y/Z --------------------
            var btnX = new Button();
            btnX.Text = "X";
            btnX.Size = new Size(50, 35);
            btnX.Location = new Point(posXInicial, posY);
            btnX.FlatStyle = FlatStyle.Flat;
            btnX.BackColor = Color.Red;
            btnX.ForeColor = Color.White;
            btnX.Click += (s, e) => FiltrarEixo("X");
            panelInfo.Controls.Add(btnX);

            var btnY = new Button();
            btnY.Text = "Y";
            btnY.Size = new Size(50, 35);
            btnY.Location = new Point(posXInicial + larguraBotao + espacamento, posY);
            btnY.FlatStyle = FlatStyle.Flat;
            btnY.BackColor = Color.Green;
            btnY.ForeColor = Color.White;
            btnY.Click += (s, e) => FiltrarEixo("Y");
            panelInfo.Controls.Add(btnY);

            var btnZ = new Button();
            btnZ.Text = "Z";
            btnZ.Size = new Size(50, 35);
            btnZ.Location = new Point(posXInicial + 2 * (larguraBotao + espacamento), posY);
            btnZ.FlatStyle = FlatStyle.Flat;
            btnZ.BackColor = Color.Blue;
            btnZ.ForeColor = Color.White;
            btnZ.Click += (s, e) => FiltrarEixo("Z");
            panelInfo.Controls.Add(btnZ);

            // -------------------- Métricas --------------------
            ConfigureMetricLabel(lblPeakAcc, "Pico Aceleração: 0.00 mm/s²", Color.DarkRed, 180);
            ConfigureMetricLabel(lblRmsAcc, "RMS Aceleração: 0.00 mm/s²", Color.DarkBlue, 220);
            ConfigureMetricLabel(lblStdDev, "Desvio Padrão: 0.00 mm/s", Color.Purple, 260);
            ConfigureMetricLabel(lblVRms, "Velocidade RMS: 0.00 mm/s", Color.DarkGreen, 300);
            ConfigureMetricLabel(lblFreqDom, "Frequência Dom.: 0.00 Hz", Color.OrangeRed, 340);

            panelInfo.Controls.AddRange(new Control[] { lblPeakAcc, lblRmsAcc, lblStdDev, lblVRms, lblFreqDom });

            // -------------------- Form principal --------------------
            ClientSize = new Size(1080, 720);
            Controls.Add(plotGrafico);
            Controls.Add(panelInfo);
            Controls.Add(statusStrip); // <- adiciona a barra de status
            Text = "Strobotech Visualize - V1.0";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            panelInfo.ResumeLayout(false);
            legendPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        // -------------------- Atualizar status --------------------
        private void AtualizarStatus(string mensagem)
        {
            lblStatus.Text = mensagem;
            statusStrip.Refresh();
        }

        // -------------------- Função auxiliar para labels --------------------
        private void ConfigureMetricLabel(Label lbl, string text, Color cor, int top)
        {
            lbl.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lbl.ForeColor = cor;
            lbl.Text = text;
            lbl.Location = new Point(10, top);
            lbl.Size = new Size(300, 30);
            lbl.AutoSize = false;
        }

        private Panel legendPanel;
        private Panel colorX;
        private Label lblX;
        private Panel colorY;
        private Label lblY;
        private Panel colorZ;
        private Label lblZ;
    }
}
