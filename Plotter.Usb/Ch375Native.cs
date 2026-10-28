using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Plotter.Usb
{
    internal static class Ch375Native
    {
        private const string Dll = "CH375DLL.DLL";

        [DllImport(Dll, 
            CallingConvention = CallingConvention.StdCall)]
        public static extern int CH3750OpenDevice(uint iIndex);
    }
}
