using System;
using System.Collections.Generic;
using System.Text;

namespace JETILib
{
    public class JETIRadio
    {

        /// Return Type: int
        ///dwNumDevices: int*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_GetNumRadio")]
        public static extern int JETI_GetNumRadio(ref int dwNumDevices);


        /// Return Type: int
        ///dwDeviceNum: int
        ///dwSerial1: int*
        ///dwSerial2: int*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_GetSerialRadio")]
        public static extern int JETI_GetSerialRadio(int dwDeviceNum, ref int dwSerial1, ref int dwSerial2);


        /// Return Type: int
        ///dwDeviceNum: int
        ///dwDevice: int*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_OpenRadio")]
        public static extern int JETI_OpenRadio(int dwDeviceNum, ref int dwDevice);


        /// Return Type: int
        ///dwDevice: int
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_CloseRadio")]
        public static extern int JETI_CloseRadio(int dwDevice);


        /// Return Type: int
        ///dwDevice: int
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_Measure")]
        public static extern int JETI_Measure(int dwDevice);


        /// Return Type: int
        ///dwDevice: int
        ///boStatus: boolean*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_MeasureStatus")]
        public static extern int JETI_MeasureStatus(int dwDevice, ref bool boStatus);


        /// Return Type: int
        ///dwDevice: int
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_MeasureBreak")]
        public static extern int JETI_MeasureBreak(int dwDevice);


        /// Return Type: int
        ///dwDevice: int
        ///fSprad: double*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_SpecRad")]
        public static extern int JETI_SpecRad(int dwDevice, ref double fSprad);


        /// Return Type: int
        ///dwDevice: int
        ///fRadio: double*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_Radio")]
        public static extern int JETI_Radio(int dwDevice, ref double fRadio);


        /// Return Type: int
        ///dwDevice: int
        ///fPhoto: double*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_Photo")]
        public static extern int JETI_Photo(int dwDevice, ref float fPhoto);


        /// Return Type: int
        ///dwDevice: int
        ///fChromx: double*
        ///fChromy: double*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_Chromxy")]
        public static extern int JETI_Chromxy(int dwDevice, ref double fChromx, ref double fChromy);


        /// Return Type: int
        ///dwDevice: int
        ///fChromu: double*
        ///fChromv: double*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_Chromuv")]
        public static extern int JETI_Chromuv(int dwDevice, ref double fChromu, ref double fChromv);


        /// Return Type: int
        ///dwDevice: int
        ///fDWL: double*
        ///fPE: double*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_DWLPE")]
        public static extern int JETI_DWLPE(int dwDevice, ref double fDWL, ref double fPE);


        /// Return Type: int
        ///dwDevice: int
        ///dwCCT: int*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_CCT")]
        public static extern int JETI_CCT(int dwDevice, ref int dwCCT);


        /// Return Type: int
        ///dwDevice: int
        ///fCRI: double*
        [System.Runtime.InteropServices.DllImportAttribute("jeti_radio.dll", EntryPoint = "JETI_CRI")]
        public static extern int JETI_CRI(int dwDevice, ref double fCRI);


    }
}
