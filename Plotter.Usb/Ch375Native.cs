using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Plotter.Usb
{
    internal static class Ch375Native
    {
        /*
            Esta clase contiene las funciones nativas del CH375DLL.DLL, que es la librería que permite la comunicación con el dispositivo CH375.
            Traduce las funciones nativas a métodos de C# para que puedan ser utilizados en el código de la aplicación.

            Las funciones nativas son importadas mediante el atributo DllImport, que permite llamar a funciones de bibliotecas externas desde código C#.
            CallingConvention.StdCall indica que la convención de llamadas utilizada es la estándar de Windows, que es la utilizada por la mayoría de las funciones de la API de Windows.
            
                HANDLE CH375OpenDevice(ULONG iIndex);
                VOID   CH375CloseDevice(ULONG iIndex);
                BOOL   CH375SetExclusive(ULONG iIndex, ULONG iExclusive);
                BOOL   CH375SetTimeout(ULONG iIndex, ULONG iWriteTimeout, ULONG iReadTimeout);
                ULONG  CH375GetUsbID(ULONG iIndex);
                BOOL   CH375WriteData(ULONG iIndex, PVOID iBuffer, PULONG ioLength);
                BOOL   CH375ReadData(ULONG iIndex, PVOID oBuffer, PULONG ioLength);

            nint es un tipo de dato que representa un puntero o un identificador de recurso en C#. Handle es un tipo de dato que representa un identificador de recurso en C/C++. En este caso, se utiliza nint para representar el identificador del dispositivo CH375.
            uint es un tipo de dato que representa un número entero sin signo de 32 bits en C#. ULONG es un tipo de dato que representa un número entero sin signo de 32 bits en C/C++.
            ref uint es un modificador que indica que el parámetro se pasa por referencia, lo que significa que la función puede modificar el valor del parámetro y devolverlo al llamador. PULONG es un puntero a un número entero sin signo de 32 bits en C/C++.
            byte[] es un array de bytes en C#. PVOID es un puntero a un tipo de dato desconocido en C/C++. En este caso, se utiliza byte[] para representar un buffer de datos que se va a enviar o recibir del dispositivo CH375.
         */

        private const string Dll = "CH375DLL.DLL";

        [DllImport(Dll, 
            CallingConvention = CallingConvention.StdCall)]
        public static extern nint CH375OpenDevice(uint iIndex);

        [DllImport(Dll,
            CallingConvention = CallingConvention.StdCall)]
        public static extern void CH375CloseDevice(uint iIndex);

        [DllImport(Dll,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool CH375SetExclusive(uint iIndex, uint iExclusive);

        [DllImport(Dll,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool CH375SetTimeout(uint iIndex, uint iWriteTimeout, uint iReadTimeout);

        [DllImport(Dll,
            CallingConvention = CallingConvention.StdCall)]
        public static extern uint CH375GetUsbID(uint iIndex);

        [DllImport(Dll,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool CH375WriteData(uint iIndex, byte[] iBuffer, ref uint ioLength);

        [DllImport(Dll,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool CH375ReadData(uint iIndex, byte[] oBuffer, ref uint ioLength);
    }
}
