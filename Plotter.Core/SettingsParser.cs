using System.Buffers.Binary;
using System.Text;

namespace Plotter.Core;

/// Convierte la respuesta de Msg_Read en PlotterSettings (formato USB o red).
public static class SettingsParser
{
    public const int UsbReplyLength = 95;   // 0x5E + "Msg_ParaNew Version3.3" + 72
    public const int NetworkReplyLength = 94;
    private const int FirmwareLength = 22;
    private const int BlockLength = 72;

    /// Elige el formato por el primer byte: 0x5E = USB, si no = red.
    public static PlotterSettings Parse(byte[] reply) =>
        reply.Length > 0 && reply[0] == 0x5E ? ParseUsb(reply) : ParseNetwork(reply);

    // ------------------------------------------------------------------ USB

    public static PlotterSettings ParseUsb(byte[] reply)
    {
        if (reply.Length != UsbReplyLength)
            throw new FormatException($"Msg_Read (USB): se esperaban {UsbReplyLength} bytes, llegaron {reply.Length}.");
        if (reply[0] != 0x5E)
            throw new FormatException($"Msg_Read (USB): primer byte 0x{reply[0]:X2}, se esperaba 0x5E.");

        string firmware = ReadFirmware(reply, 1);
        ReadOnlySpan<byte> b = reply.AsSpan(1 + FirmwareLength, BlockLength); // bloque little-endian
        return ParseUsbBlock(firmware, b);
    }

    // Offsets del bloque por USB.
    private static PlotterSettings ParseUsbBlock(string firmware, ReadOnlySpan<byte> b)
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
            Unknown16: U32(b, 16),
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

    // ------------------------------------------------------------------ Red

    public static PlotterSettings ParseNetwork(byte[] reply)
    {
        if (reply.Length != NetworkReplyLength)
            throw new FormatException($"Msg_Read (red): se esperaban {NetworkReplyLength} bytes, llegaron {reply.Length}.");

        string firmware = ReadFirmware(reply, 0);

        // Copia del bloque: lo vamos a modificar y la respuesta original debe quedar intacta.
        byte[] b = reply.AsSpan(FirmwareLength, BlockLength).ToArray();

        // Por red cada palabra de 16 bits llega con los bytes invertidos.
        for (int i = 0; i < b.Length; i += 2)
            (b[i], b[i + 1]) = (b[i + 1], b[i]);

        return ParseNetworkBlock(firmware, b);
    }

    // Offsets del bloque por red, tras invertir los pares 
    private static PlotterSettings ParseNetworkBlock(string firmware, ReadOnlySpan<byte> b)
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
            Unknown16: U32(b, 16),
            PlotterWidthMm: I32(b, 20),
            UsageCounter: U16(b, 24),          // uint16 por red
            Shift12: I32(b, 26),               // 2 bytes antes que por USB
            Shift23: I32(b, 30),
            Shift34: I32(b, 34),
            // 38–39: relleno
            PaperFeedErrorMm: F64(b, 40),
            BiDirShiftMm: F64(b, 48),
            OneMeterXmm: F64(b, 56),
            OneMeterYmm: F64(b, 64));
    }


    // Lee "Msg_ParaNew Version3.3" desde 'start', comprueba el prefijo y lo quita.
    private static string ReadFirmware(byte[] reply, int start)
    {
        string text = Encoding.ASCII.GetString(reply, start, FirmwareLength);
        if (!text.StartsWith("Msg_"))
            throw new FormatException($"Msg_Read: cabecera inesperada '{text}'.");
        return text.Substring(4);
    }

    private static int I32(ReadOnlySpan<byte> b, int o) => BinaryPrimitives.ReadInt32LittleEndian(b.Slice(o, 4));
    private static uint U32(ReadOnlySpan<byte> b, int o) => BinaryPrimitives.ReadUInt32LittleEndian(b.Slice(o, 4));
    private static ushort U16(ReadOnlySpan<byte> b, int o) => BinaryPrimitives.ReadUInt16LittleEndian(b.Slice(o, 2));
    private static double F64(ReadOnlySpan<byte> b, int o) => BinaryPrimitives.ReadDoubleLittleEndian(b.Slice(o, 8));
}
