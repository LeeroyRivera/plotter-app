
namespace Plotter.Core
{
    public interface IPlotterLink : IDisposable
    {
        void Open();
        void Close();

        void Write(byte[] data);

        byte[] Read(int count);

        bool IsOpen { get; }

    }
}
