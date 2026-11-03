namespace Plotter.TestApp;

/// Cómo se conecta un plotter.
public enum Transport
{
    Usb,
    Red
}

/// Configuración de un plotter: nombre y cómo se llega a él.
/// Usb → se usa UsbIndex (0, 1…). Red → se usa Ip (por ejemplo "192.168.3.116").
public sealed record PlotterConfig(
    string Name,
    Transport Transport,
    uint UsbIndex = 0,
    string? Ip = null);
