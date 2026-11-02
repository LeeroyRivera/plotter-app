using Plotter.Core;
using System.Formats.Asn1;

namespace Plotter.Usb
{
    // Esta clase implementa la interfaz IPlotterLink para proporcionar una conexión con el dispositivo CH375 a través de la librería nativa CH375DLL.DLL.
    public sealed class Ch375Link : IPlotterLink
    {

        /*
         Este id es el identificador usb esperado para el dispositivo CH375. Se utiliza para verificar que el dispositivo conectado es efectivamente un CH375 y no otro dispositivo USB.
        VID (Vendor ID) es un identificador único asignado a cada fabricante de dispositivos USB por la USB Implementers Forum (USB-IF). En este caso, el VID 4348 corresponde a WCH, el fabricante del CH375.
        PID (Product ID) es un identificador único asignado a cada producto de un fabricante. En este caso, el PID 5537 corresponde al CH375.
         */
        private const uint ExpectedUsbId = 0x55374348; //VID 4348 PID 5537
        private const int MaxChunk = 4096; // Tamaño máximo de datos que se pueden enviar o recibir en una sola operación de lectura o escritura.
        private const int UsbPacket = 64; // Tamaño de paquete USB estándar para dispositivos de baja velocidad.
        private const uint TimeoutMs = 3000; // Tiempo de espera en milisegundos para las operaciones USB.

        private readonly uint _index;
        private bool _open;
        private readonly Queue<byte> _pending = new(); // bytes leidos de mas en una lectura anterior.

        public Ch375Link(uint index) => _index = index;

        public bool IsOpen => _open;

        public void Open() {
            if (_open) return;

            nint handle = Ch375Native.CH375OpenDevice(_index);
            if (handle == -1) throw new IOException($"Plotter {_index}: No se pudo abrir el dispositivo CH375."); //

            try
            {
                if (!Ch375Native.CH375SetExclusive(_index, 1))
                {
                    throw new IOException($"Plotter {_index}: No se pudo establecer el accesso exclusivo para el dispositivo CH375."); // Esto asegura que ningún otro proceso pueda acceder al dispositivo mientras está en uso.
                }

                uint id = Ch375Native.CH375GetUsbID(_index);
                if (id != ExpectedUsbId)
                {
                    throw new IOException($"Plotter {_index}: El dispositivo conectado no es un plotter (ID 0x{id:X8})."); // Id esperado: 0x55374348
                }

                if (!Ch375Native.CH375SetTimeout(_index, TimeoutMs, TimeoutMs))
                {
                    throw new IOException($"Plotter {_index}: No se pudo establecer el tiempo de espera para el dispositivo CH375.");
                }

            }
            catch 
            {
                Ch375Native.CH375CloseDevice(_index);
                throw;
            }

            _pending.Clear();
            _open = true;

        }

        public void Close()
        {
            if (!_open) return;
            Ch375Native.CH375CloseDevice(_index);
            _pending.Clear();
            _open = false;
        }

        public void Dispose() => Close();

        public void Write(byte[] data)
        {
            EnsureOpen();

            int offset = 0; // offset es la poscicion actual en el array de datos que se está enviando.
            byte[] chunk = new byte[MaxChunk]; // chunk es un buffer temporal para almacenar los datos que se enviarán en cada operación de escritura.
            while (offset < data.Length) //
            { 
                int size = Math.Min(MaxChunk, data.Length - offset); // size es la cantidad de bytes que se enviarán en esta operación de escritura. Se asegura de no exceder el tamaño máximo permitido (MaxChunk) y de no leer más allá del final del array de datos.
                Array.Copy(data, offset, chunk, 0, size); // Se copia "data" desde la posición "offset" hasta "chunk".

                uint len = (uint)size; // len es la cantidad de bytes que se enviarán en esta operación de escritura. Se pasa por referencia a la función CH375WriteData para que pueda actualizarse con la cantidad real de bytes enviados.
                if (!Ch375Native.CH375WriteData(_index, chunk, ref len) || len != size)
                    throw new IOException($"Plotter {_index}: fallo al enviar datos al dispositivo CH375. Bytes enviados: {len}, esperados: {size}, posición: {offset}");

                offset += size;
            }
        }

        public byte[] Read(int count)
        {
            EnsureOpen();

            byte[] result = new byte[count];
            int filled = 0; // filled es la cantidad de bytes que se han leído y almacenado en el array "result" hasta el momento.

            while (filled < count && _pending.Count > 0)
                result[filled++] = _pending.Dequeue(); // Se llenan los bytes pendientes de la lectura anterior.

            byte[] tmp = new byte[UsbPacket];
            while (filled < count)
            {
                uint len = UsbPacket; // len es la cantidad de bytes que se intentarán leer en esta operación de lectura. Se pasa por referencia a la función CH375ReadData para que pueda actualizarse con la cantidad real de bytes leídos.
                if (!Ch375Native.CH375ReadData(_index, tmp, ref len) || len == 0)
                    throw new IOException($"Plotter {_index}: fallo al leer datos del dispositivo CH375. Bytes leídos: {len}, posición: {filled} de {count} bytes");

                for (int i = 0; i < len; i++)
                {
                    if (filled < count) result[filled++] = tmp[i]; // Se llenan los bytes leídos en el array "result" hasta alcanzar la cantidad solicitada.
                    else _pending.Enqueue(tmp[i]); // Si se han leído más bytes de los solicitados, se almacenan en la cola de pendientes para futuras lecturas.
                }
                    
            }
            return result;
        }

        private void EnsureOpen()
        {
            if (!_open) throw new InvalidOperationException($"Plotter {_index}: El dispositivo CH375 no está abierto.");
        }
    }
}
