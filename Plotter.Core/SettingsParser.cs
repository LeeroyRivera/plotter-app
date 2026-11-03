using System.Buffers.Binary;
using System.Text;

namespace Plotter.Core;

/// Convierte la respuesta de Msg_Read en PlotterSettings.
public static class SettingsParser
{
    public const int UsbReplyLength = 95;
    private const int FirmwareLength = 22;
    private const int BlockLength = 72;

    public static PlotterSettings ParseUsb(byte[] reply)
    {
        if (reply.Length != UsbReplyLength)                                   
            throw new FormatException($"Msg_Read: se esperaban {UsbReplyLength} bytes, llegaron {reply.Length}.");
        if (reply[0] != 0x5E)
            throw new FormatException($"Msg_Read: primer byte 0x{reply[0]:X2}, se esperaba 0x5E.");

        string firmware = Encoding.ASCII.GetString(reply, 1, FirmwareLength);  
        if (!firmware.StartsWith("Msg_"))
            throw new FormatException($"Msg_Read: cabecera inesperada '{firmware}'.");

        ReadOnlySpan<byte> b = reply.AsSpan(1 + FirmwareLength, BlockLength); // 72 bytes de datos de configuración del plotter, en little-endian (USB, o red tras invertir los pares). 
        return ParseBlock(firmware.Substring(4), b); // firmware.Substring(4) elimina el prefijo "Msg_" de la cadena de firmware, dejando solo la parte relevante del nombre del firmware.                          
    }

    // El bloque de 72 bytes ya en little-endian (USB, o red tras invertir los pares).
    private static PlotterSettings ParseBlock(string firmware, ReadOnlySpan<byte> b) // (4)
    {
        return new PlotterSettings(
            Firmware: firmware,
            FormatMarker: b[0],
            SprayCount: b[1],
            FirstSpray: b[2],
            PrintStyle: b[3],
            LineWidthPx: b[4],
            Overlap12: b[5],
            Overlap23: b[6],
            Overlap34: b[7],
            Unknown8: I32(b, 8),
            Unknown12: I32(b, 12),
            Unknown16: BinaryPrimitives.ReadUInt32LittleEndian(b.Slice(16, 4)),
            PlotterWidthMm: I32(b, 20),
            UsageCounter: I32(b, 24),
            Shift12: I32(b, 28),
            Shift23: I32(b, 32),
            Shift34: I32(b, 36),
            PaperFeedErrorMm: F64(b, 40),
            BiDirShiftMm: F64(b, 48),
            OneMeterXmm: F64(b, 56),
            OneMeterYmm: F64(b, 64));
    }

    private static int I32(ReadOnlySpan<byte> b, int o) => BinaryPrimitives.ReadInt32LittleEndian(b.Slice(o, 4));
    private static double F64(ReadOnlySpan<byte> b, int o) => BinaryPrimitives.ReadDoubleLittleEndian(b.Slice(o, 8));
}