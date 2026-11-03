using Plotter.Core;
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

    private void btnSettings_Click(object sender, EventArgs e)
    {
        try
        {
            using var link = new Ch375Link(0);
            link.Open();
            var protocol = new PlotterProtocol(link);
            PlotterSettings s = protocol.ReadSettings();
            txtConfiguracion.Text = s.ToString().Replace(", ", Environment.NewLine);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }
}
