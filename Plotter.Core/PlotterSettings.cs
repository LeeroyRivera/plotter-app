
namespace Plotter.Core
{
    public sealed record PlotterSettings(
        string Firmware,
        byte FormatMarker,
        int SprayCount,
        int FirstSpray,
        int PrintStyle,
        int LineWidthPx,
        int Overlap12, int Overlap23, int Overlap34,
        int Unknown8, int Unknown12, uint Unknown16,
        int PlotterWidthMm,
        int UsageCounter,
        int Shift12, int Shift23, int Shift34,
        double PaperFeedErrorMm,
        double BiDirShiftMm,
        double OneMeterXmm,
        double OneMeterYmm);
}
