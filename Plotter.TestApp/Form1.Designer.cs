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
        btnConectar = new Button();
        btnSettings = new Button();
        txtConfiguracion = new TextBox();
        label1 = new Label();
        SuspendLayout();
        // 
        // btnConectar
        // 
        btnConectar.Location = new Point(39, 127);
        btnConectar.Name = "btnConectar";
        btnConectar.Size = new Size(94, 29);
        btnConectar.TabIndex = 0;
        btnConectar.Text = "Conectar";
        btnConectar.UseVisualStyleBackColor = true;
        btnConectar.Click += btnConectar_Click;
        // 
        // btnSettings
        // 
        btnSettings.Location = new Point(447, 135);
        btnSettings.Name = "btnSettings";
        btnSettings.Size = new Size(204, 29);
        btnSettings.TabIndex = 1;
        btnSettings.Text = "Configuracion";
        btnSettings.UseVisualStyleBackColor = true;
        btnSettings.Click += btnSettings_Click;
        // 
        // txtConfiguracion
        // 
        txtConfiguracion.Location = new Point(326, 170);
        txtConfiguracion.Multiline = true;
        txtConfiguracion.Name = "txtConfiguracion";
        txtConfiguracion.ScrollBars = ScrollBars.Vertical;
        txtConfiguracion.Size = new Size(462, 268);
        txtConfiguracion.TabIndex = 2;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(39, 94);
        label1.Name = "label1";
        label1.Size = new Size(50, 20);
        label1.TabIndex = 3;
        label1.Text = "label1";
        label1.Click += label1_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(label1);
        Controls.Add(txtConfiguracion);
        Controls.Add(btnSettings);
        Controls.Add(btnConectar);
        Name = "Form1";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnConectar;
    private Button btnSettings;
    private TextBox txtConfiguracion;
    private Label label1;
}
