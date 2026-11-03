using System.Net;
using System.Net.Sockets;
using Plotter.Core;

namespace Plotter.Net;

/// Conexión TCP con un plotter (puerto 1024), igual que PlotCenter en modo red.
public sealed class TcpLink : IPlotterLink
{
    public const int Port = 1024;   // fijo en el firmware
    private const int ConnectTimeoutMs = 2000;  // PlotCenter espera 2 s
    private const int IoTimeoutMs = 3000;   // igual que el USB

    private readonly IPAddress _ip;
    private TcpClient? _client;
    private NetworkStream? _stream;

    public TcpLink(IPAddress ip) => _ip = ip;

    public bool IsOpen => _stream != null;                                 
    public void Open()
    {
        if (IsOpen) return;

        var client = new TcpClient
        {
            NoDelay = true, // Desactiva Nagle para que los paquetes se envíen inmediatamente, reduciendo la latencia.                                          
            ReceiveTimeout = IoTimeoutMs,
            SendTimeout = IoTimeoutMs
        };

        // Intentar conectarse al plotter con un tiempo de espera
        try
        {
            using var cts = new CancellationTokenSource(ConnectTimeoutMs);   
            client.ConnectAsync(_ip, Port, cts.Token).AsTask().GetAwaiter().GetResult();
        }
        catch (OperationCanceledException)
        {
            client.Dispose();
            throw new TimeoutException($"Plotter {_ip}: no responde en el puerto {Port} (2 s).");
        }
        catch (SocketException ex)
        {
            client.Dispose();
            throw new IOException($"Plotter {_ip}: conexión rechazada ({ex.SocketErrorCode}).", ex);
        }

        _client = client;
        _stream = client.GetStream();
    }

    public void Close()
    {
        if (!IsOpen) return;
        _stream!.Dispose();
        _client!.Dispose();
        _stream = null;
        _client = null;
    }

    public void Dispose() => Close();

    public void Write(byte[] data)
    {
        EnsureOpen();
        try
        {
            _stream!.Write(data, 0, data.Length);                       
        }
        catch (IOException ex)
        {
            throw new IOException($"Plotter {_ip}: fallo al enviar {data.Length} bytes.", ex);
        }
    }

    public byte[] Read(int count)
    {
        EnsureOpen();
        byte[] result = new byte[count];
        int filled = 0;

        while (filled < count)
        {
            int n;
            try
            {
                n = _stream!.Read(result, filled, count - filled);
            }
            catch (IOException ex) when (ex.InnerException is SocketException { SocketErrorCode: SocketError.TimedOut })
            {
                throw new TimeoutException($"Plotter {_ip}: sin respuesta ({filled} de {count} bytes).");
            }

            if (n == 0)
                throw new IOException($"Plotter {_ip}: el plotter cerró la conexión ({filled} de {count} bytes).");
            filled += n;
        }
        return result;
    }

    private void EnsureOpen()
    {
        if (!IsOpen)
            throw new InvalidOperationException($"Plotter {_ip}: la conexión no está abierta.");
    }
}