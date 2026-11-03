using Plotter.Usb;

namespace Plotter.TestApp;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void btnConectar_Click(object sender, EventArgs e)
    {
        try
        {
            using var link = new Ch375Link(0);
            link.Open();
            MessageBox.Show("Conectado al plotter 0");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }
}
