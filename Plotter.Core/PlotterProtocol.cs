using System.Text;

namespace Plotter.Core;

/// Comandos del protocolo Msg_* sobre cualquier transporte.
public sealed class PlotterProtocol
{
    private readonly IPlotterLink _link;

    public PlotterProtocol(IPlotterLink link) => _link = link;             

    public PlotterSettings ReadSettings()
    {
        _link.Write(Encoding.ASCII.GetBytes("Msg_Read"));                     // El comando Msg_Read solicita al plotter que envíe su configuración actual.
        byte[] reply = _link.Read(SettingsParser.UsbReplyLength);             // El plotter responde con 95 bytes, que incluyen la cabecera "Msg_" y los 72 bytes de datos de configuración del plotter. 
        return SettingsParser.ParseUsb(reply);
    }
}