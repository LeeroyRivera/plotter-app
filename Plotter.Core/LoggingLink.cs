using System.Text;

namespace Plotter.Core;

/// Envuelve un IPlotterLink y apunta logs en un archivo cada operación.
public sealed class LoggingLink : IPlotterLink
{
    private const int MaxHexBytes = 32;              

    private readonly IPlotterLink _inner;             
    private readonly StreamWriter _log;

    // Crea un LoggingLink que envuelve otro IPlotterLink y escribe un log de todas las operaciones en el archivo logPath.
    public LoggingLink(IPlotterLink inner, string logPath)
    {
        _inner = inner;
        Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
        _log = new StreamWriter(logPath, append: true) { AutoFlush = true }; 
    }

    public bool IsOpen => _inner.IsOpen;

    public void Open() => Do("OPEN", () => _inner.Open());
    public void Close() => Do("CLOSE", () => _inner.Close());

    public void Write(byte[] data)
    {
        Do(null, () => _inner.Write(data));
        Line("W", data);                             
    }

    public byte[] Read(int count)
    {
        byte[] data = Array.Empty<byte>();
        Do(null, () => data = _inner.Read(count));
        Line("R", data);
        return data;
    }

    public void Dispose()
    {
        _inner.Dispose();                             
        _log.WriteLine($"{Now()}  CLOSE");
        _log.Dispose();
    }


    // Ejecuta una acción y registra el resultado en el log. Si la acción lanza una excepción, se registra el error y se vuelve a lanzar la excepción.
    private void Do(string? label, Action action)   
    {
        try
        {
            action();
            if (label != null) _log.WriteLine($"{Now()}  {label}");
        }
        catch (Exception ex)
        {
            _log.WriteLine($"{Now()}  ERR  {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }

    // Escribe una línea en el log con la dirección (W o R), la longitud de los datos, los bytes en hexadecimal y su representación ASCII.
    private void Line(string dir, byte[] data)
    {
        int n = Math.Min(data.Length, MaxHexBytes);
        string hex = Convert.ToHexString(data, 0, n);                       // "4D73675F…"
        string spaced = string.Join(" ", Enumerable.Range(0, n).Select(i => hex.Substring(i * 2, 2)));
        string ascii = new string(data.Take(n).Select(b => b is >= 32 and < 127 ? (char)b : '.').ToArray());
        string more = data.Length > n ? " …" : "";
        _log.WriteLine($"{Now()}  {dir} {data.Length,6}  {spaced}{more}  {ascii}");
    }

    private static string Now() => DateTime.Now.ToString("HH:mm:ss.fff");
}