using Plotter.Core;
using Plotter.Usb;

namespace Plotter.TestApp;

public partial class Form1 : Form
{
    private uint Plotter1Index; // Indice del plotter seleccionado en el combo box cbxPlotter1. cambia
    private uint Plotter2Index;
    public Form1()
    {
        InitializeComponent();
    }

    private void btnConectar1_Click(object sender, EventArgs e)
    {
        try
        {
            using var link = new Ch375Link(Plotter1Index);
            link.Open();
            MessageBox.Show("Conectado al plotter 1");
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
            string logPath1 = Path.Combine(AppContext.BaseDirectory, "logs",
                $"Plotter{Plotter1Index}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");
            string logPath2 = Path.Combine(AppContext.BaseDirectory, "logs",
                $"Plotter{Plotter2Index}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");
            using var link1 = new LoggingLink(new Ch375Link(Plotter1Index), logPath1);
            using var link2 = new LoggingLink(new Ch375Link(Plotter2Index), logPath2);

            link1.Open();
            link2.Open();

            var protocol1 = new PlotterProtocol(link1).ReadSettings();
            var protocol2 = new PlotterProtocol(link2).ReadSettings();

            txtConfiguracion1.Text = protocol1.ToString().Replace(", ", Environment.NewLine);


            txtConfiguracion2.Text = protocol2.ToString().Replace(", ", Environment.NewLine);

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }


    private void cbxPlotter1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Plotter1Index = (uint)cbxPlotter1.SelectedIndex;
    }

    private void btnConectarP2_Click(object sender, EventArgs e)
    {
        try
        {
            using var link = new Ch375Link(Plotter2Index);
            link.Open();
            MessageBox.Show("Conectado al plotter 2");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void cbxPlotter2_SelectedIndexChanged(object sender, EventArgs e)
    {
        Plotter2Index = (uint)cbxPlotter2.SelectedIndex;
    }
}
