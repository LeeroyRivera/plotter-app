using System.Text;

namespace Plotter.Core;

/// Comandos del protocolo Msg_* sobre cualquier transporte.
public sealed class PlotterProtocol
{
    private readonly IPlotterLink _link;

    public PlotterProtocol(IPlotterLink link) => _link = link;

    //Segun el firmware, el plotter responde con 95 bytes (USB) o 94 bytes (red). El primer byte indica el formato: 0x5E = USB, si no = red.
    public PlotterSettings ReadSettings()
    {
        _link.Write(Encoding.ASCII.GetBytes("Msg_Read"));

        byte[] first = _link.Read(1);
        int rest = first[0] switch
        {
            0x5E => SettingsParser.UsbReplyLength - 1,      // USB: 94 más
            (byte)'M' => SettingsParser.NetworkReplyLength - 1,  // red: 93 más
            _ => throw new FormatException($"Msg_Read: primer byte inesperado 0x{first[0]:X2}.")
        };

        byte[] reply = [.. first, .. _link.Read(rest)];
        return SettingsParser.Parse(reply);
    }
}