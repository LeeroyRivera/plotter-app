namespace Plotter.TestApp;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        btnConectarP1 = new Button();
        btnSettings = new Button();
        txtConfiguracion1 = new TextBox();
        label1 = new Label();
        cbxPlotter1 = new ComboBox();
        label2 = new Label();
        cbxPlotter2 = new ComboBox();
        btnConectarP2 = new Button();
        txtConfiguracion2 = new TextBox();
        SuspendLayout();
        // 
        // btnConectarP1
        // 
        btnConectarP1.Location = new Point(99, 49);
        btnConectarP1.Name = "btnConectarP1";
        btnConectarP1.Size = new Size(94, 29);
        btnConectarP1.TabIndex = 0;
        btnConectarP1.Text = "Conectar";
        btnConectarP1.UseVisualStyleBackColor = true;
        btnConectarP1.Click += btnConectar1_Click;
        // 
        // btnSettings
        // 
        btnSettings.Location = new Point(303, 135);
        btnSettings.Name = "btnSettings";
        btnSettings.Size = new Size(204, 29);
        btnSettings.TabIndex = 1;
        btnSettings.Text = "Configuracion";
        btnSettings.UseVisualStyleBackColor = true;
        btnSettings.Click += btnSettings_Click;
        // 
        // txtConfiguracion1
        // 
        txtConfiguracion1.Location = new Point(12, 170);
        txtConfiguracion1.Multiline = true;
        txtConfiguracion1.Name = "txtConfiguracion1";
        txtConfiguracion1.ScrollBars = ScrollBars.Vertical;
        txtConfiguracion1.Size = new Size(280, 268);
        txtConfiguracion1.TabIndex = 2;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(28, 18);
        label1.Name = "label1";
        label1.Size = new Size(65, 20);
        label1.TabIndex = 3;
        label1.Text = "Plotter 1";
        // 
        // cbxPlotter1
        // 
        cbxPlotter1.FormattingEnabled = true;
        cbxPlotter1.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" });
        cbxPlotter1.Location = new Point(99, 15);
        cbxPlotter1.Name = "cbxPlotter1";
        cbxPlotter1.Size = new Size(151, 28);
        cbxPlotter1.TabIndex = 4;
        cbxPlotter1.SelectedIndexChanged += cbxPlotter1_SelectedIndexChanged;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(503, 23);
        label2.Name = "label2";
        label2.Size = new Size(65, 20);
        label2.TabIndex = 5;
        label2.Text = "Plotter 2";
        // 
        // cbxPlotter2
        // 
        cbxPlotter2.FormattingEnabled = true;
        cbxPlotter2.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" });
        cbxPlotter2.Location = new Point(574, 20);
        cbxPlotter2.Name = "cbxPlotter2";
        cbxPlotter2.Size = new Size(151, 28);
        cbxPlotter2.TabIndex = 6;
        cbxPlotter2.SelectedIndexChanged += cbxPlotter2_SelectedIndexChanged;
        // 
        // btnConectarP2
        // 
        btnConectarP2.Location = new Point(574, 54);
        btnConectarP2.Name = "btnConectarP2";
        btnConectarP2.Size = new Size(94, 29);
        btnConectarP2.TabIndex = 7;
        btnConectarP2.Text = "Conectar";
        btnConectarP2.UseVisualStyleBackColor = true;
        btnConectarP2.Click += btnConectarP2_Click;
        // 
        // txtConfiguracion2
        // 
        txtConfiguracion2.Location = new Point(508, 181);
        txtConfiguracion2.Multiline = true;
        txtConfiguracion2.Name = "txtConfiguracion2";
        txtConfiguracion2.ScrollBars = ScrollBars.Vertical;
        txtConfiguracion2.Size = new Size(280, 268);
        txtConfiguracion2.TabIndex = 8;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(txtConfiguracion2);
        Controls.Add(btnConectarP2);
        Controls.Add(cbxPlotter2);
        Controls.Add(label2);
        Controls.Add(cbxPlotter1);
        Controls.Add(label1);
        Controls.Add(txtConfiguracion1);
        Controls.Add(btnSettings);
        Controls.Add(btnConectarP1);
        Name = "Form1";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnConectarP1;
    private Button btnSettings;
    private TextBox txtConfiguracion1;
    private Label label1;
    private ComboBox cbxPlotter1;
    private Label label2;
    private ComboBox cbxPlotter2;
    private Button btnConectarP2;
    private TextBox txtConfiguracion2;
}
