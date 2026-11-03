using Plotter.Core;
using Plotter.Net;
using Plotter.Usb;
using System.Net;

namespace Plotter.TestApp;

/// Crea el IPlotterLink adecuado (USB o red) a partir de la configuración de un plotter.
public static class LinkFactory
{
    public static IPlotterLink Create(PlotterConfig cfg) => cfg.Transport switch
    {
        Transport.Usb => new Ch375Link(cfg.UsbIndex),
        Transport.Red => new TcpLink(ParseIp(cfg)),
        _ => throw new ArgumentOutOfRangeException(nameof(cfg), $"Transporte desconocido: {cfg.Transport}")
    };

    // Valida la IP con un mensaje claro en lugar del error genérico de IPAddress.Parse.
    private static IPAddress ParseIp(PlotterConfig cfg)
    {
        if (string.IsNullOrWhiteSpace(cfg.Ip) || !IPAddress.TryParse(cfg.Ip.Trim(), out var ip))
            throw new ArgumentException($"{cfg.Name}: IP no válida '{cfg.Ip}'.");
        return ip;
    }
}
