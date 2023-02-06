using System;
using System.Collections.Generic;
using System.Text;

namespace JETILib
{
    public class JETICore
    {

        /// JETI_SUCCESS -> 0x00000000
        public const int JETI_SUCCESS = 0;

        /// JETI_ERROR_OPEN_PORT -> 0x00000002
        public const int JETI_ERROR_OPEN_PORT = 2;

        /// JETI_ERROR_PORT_SETTING -> 0x00000003
        public const int JETI_ERROR_PORT_SETTING = 3;

        /// JETI_ERROR_BUFFER_SIZE -> 0x00000004
        public const int JETI_ERROR_BUFFER_SIZE = 4;

        /// JETI_ERROR_PURGE -> 0x00000005
        public const int JETI_ERROR_PURGE = 5;

        /// JETI_ERROR_TIMEOUT_SETTING -> 0x00000006
        public const int JETI_ERROR_TIMEOUT_SETTING = 6;

        /// JETI_ERROR_SEND -> 0x00000007
        public const int JETI_ERROR_SEND = 7;

        /// JETI_TIMEOUT -> 0x00000008
        public const int JETI_TIMEOUT = 8;

        /// JETI_ERROR_RECEIVE -> 0x0000000A
        public const int JETI_ERROR_RECEIVE = 10;

        /// JETI_ERROR_NAK -> 0x0000000B
        public const int JETI_ERROR_NAK = 11;

        /// JETI_ERROR_CONVERT -> 0x0000000C
        public const int JETI_ERROR_CONVERT = 12;

        /// JETI_ERROR_PARAMETER -> 0x0000000D
        public const int JETI_ERROR_PARAMETER = 13;

        /// JETI_BUSY -> 0x0000000E
        public const int JETI_BUSY = 14;

        /// JETI_CHECKSUM_ERROR -> 0x00000011
        public const int JETI_CHECKSUM_ERROR = 17;

        /// JETI_INVALID_STEPWIDTH -> 0x00000012
        public const int JETI_INVALID_STEPWIDTH = 18;

        /// JETI_INVALID_NUMBER -> 0x00000013
        public const int JETI_INVALID_NUMBER = 19;

        /// JETI_NOT_CONNECTED -> 0x00000014
        public const int JETI_NOT_CONNECTED = 20;

        /// JETI_INVALID_HANDLE -> 0x00000015
        public const int JETI_INVALID_HANDLE = 21;
        /// Return Type: int
    ///dwNumDevices: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetNumDevices")]
public static extern  int JETI_GetNumDevices(ref int dwNumDevices) ;

    
    /// Return Type: int
    ///dwDeviceNum: int
    ///dwSerial1: int*
    ///dwSerial2: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetSerialDevice")]
public static extern  int JETI_GetSerialDevice(int dwDeviceNum, ref int dwSerial1, ref int dwSerial2) ;

    
    /// Return Type: int
    ///dwDeviceNum: int
    ///dwDevice: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_OpenDevice")]
public static extern  int JETI_OpenDevice(int dwDeviceNum, ref int dwDevice) ;

    
    /// Return Type: int
    ///dwDevice: int
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CloseDevice")]
public static extern  int JETI_CloseDevice(int dwDevice) ;

    
    /// Return Type: int
    ///dwDevice: int
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_Reset")]
public static extern  int JETI_Reset(int dwDevice) ;

    
    /// Return Type: int
    ///dwDevice: int
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_Break")]
public static extern  int JETI_Break(int dwDevice) ;

    
    /// Return Type: int
    ///dwDevice: int
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_InitMeasure")]
public static extern  int JETI_InitMeasure(int dwDevice) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///boStatus: boolean*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_MeasureStatusCore")]
public static extern  int JETI_MeasureStatusCore(int dwDevice, ref bool boStatus) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwPixel: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetPixel")]
public static extern  int JETI_GetPixel(int dwDevice, ref int dwPixel) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///fFit: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetFit")]
public static extern  int JETI_GetFit(int dwDevice, ref double fFit) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwSDelay: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetSDelay")]
public static extern  int JETI_GetSDelay(int dwDevice, ref int dwSDelay) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///boLaserStat: boolean*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetLaserStat")]
public static extern  int JETI_GetLaserStat(int dwDevice, ref bool boLaserStat) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///boLaserStat: boolean
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_SetLaserStat")]
public static extern  int JETI_SetLaserStat(int dwDevice, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.I1)] bool boLaserStat) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///boShutterStat: boolean*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetShutterStat")]
public static extern  int JETI_GetShutterStat(int dwDevice, ref bool boShutterStat) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///boShutterStat: boolean
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_SetShutterStat")]
public static extern  int JETI_SetShutterStat(int dwDevice, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.I1)] bool boShutterStat) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///bMeasHead: short*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetMeasHead")]
public static extern  int JETI_GetMeasHead(int dwDevice, ref short bMeasHead) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///bDarkmode: short*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetDarkmodeConf")]
public static extern  int JETI_GetDarkmodeConf(int dwDevice, ref short bDarkmode) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///bDarkmode: short
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_SetDarkmodeConf")]
public static extern  int JETI_SetDarkmodeConf(int dwDevice, short bDarkmode) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///bExpmode: short*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetExposureConf")]
public static extern  int JETI_GetExposureConf(int dwDevice, ref short bExpmode) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///bExpmode: short
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_SetExposureConf")]
public static extern  int JETI_SetExposureConf(int dwDevice, short bExpmode) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///bPrevFunc: short*
    ///bConfFunc: short*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetFunctionConf")]
public static extern  int JETI_GetFunctionConf(int dwDevice, ref short bPrevFunc, ref short bConfFunc) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///bFunction: short
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_SetFunctionConf")]
public static extern  int JETI_SetFunctionConf(int dwDevice, short bFunction) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwPrevTint: int*
    ///dwConfTint: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetTintConf")]
public static extern  int JETI_GetTintConf(int dwDevice, ref int dwPrevTint, ref int dwConfTint) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwTint: int
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_SetTintConf")]
public static extern  int JETI_SetTintConf(int dwDevice, int dwTint) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///bAver: short*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetAverConf")]
public static extern  int JETI_GetAverConf(int dwDevice, ref short bAver) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///bAver: short
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_SetAverConf")]
public static extern  int JETI_SetAverConf(int dwDevice, short bAver) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///bAdaptmode: short*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetAdaptConf")]
public static extern  int JETI_GetAdaptConf(int dwDevice, ref short bAdaptmode) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///bAdaptmode: short
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_SetAdaptConf")]
public static extern  int JETI_SetAdaptConf(int dwDevice, short bAdaptmode) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int*
    ///dwEnd: int*
    ///dwStep: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_GetWranConf")]
public static extern  int JETI_GetWranConf(int dwDevice, ref int dwBeg, ref int dwEnd, ref int dwStep) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///dwStep: int
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_SetWranConf")]
public static extern  int JETI_SetWranConf(int dwDevice, int dwBeg, int dwEnd, int dwStep) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwDark: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchDark")]
public static extern  int JETI_FetchDark(int dwDevice, ref int dwDark) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwLight: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchLight")]
public static extern  int JETI_FetchLight(int dwDevice, ref int dwLight) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwRefer: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchRefer")]
public static extern  int JETI_FetchRefer(int dwDevice, ref int dwRefer) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwTransRefl: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchTransRefl")]
public static extern  int JETI_FetchTransRefl(int dwDevice, ref int dwTransRefl) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///fSprad: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchSprad")]
public static extern  int JETI_FetchSprad(int dwDevice, ref double fSprad) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///fRadio: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchRadio")]
public static extern  int JETI_FetchRadio(int dwDevice, ref double fRadio) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///fPhoto: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchPhoto")]
public static extern  int JETI_FetchPhoto(int dwDevice, ref double fPhoto) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///fChromx: double*
    ///Chromy: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchChromxy")]
public static extern  int JETI_FetchChromxy(int dwDevice, ref double fChromx, ref double Chromy) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///fChromu: double*
    ///fChromv: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchChromuv")]
public static extern  int JETI_FetchChromuv(int dwDevice, ref double fChromu, ref double fChromv) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///fDWL: double*
    ///fPE: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchDWLPE")]
public static extern  int JETI_FetchDWLPE(int dwDevice, ref double fDWL, ref double fPE) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwCCT: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchCCT")]
public static extern  int JETI_FetchCCT(int dwDevice, ref int dwCCT) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///fCRI: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_FetchCRI")]
public static extern  int JETI_FetchCRI(int dwDevice, ref double fCRI) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fStep: double
    ///fDark: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcLintDark")]
public static extern  int JETI_CalcLintDark(int dwDevice, int dwBeg, int dwEnd, double fStep, ref double fDark) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fStep: double
    ///fDark: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcSplinDark")]
public static extern  int JETI_CalcSplinDark(int dwDevice, int dwBeg, int dwEnd, double fStep, ref double fDark) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fStep: double
    ///fLight: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcLintLight")]
public static extern  int JETI_CalcLintLight(int dwDevice, int dwBeg, int dwEnd, double fStep, ref double fLight) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fStep: double
    ///fLight: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcSplinLight")]
public static extern  int JETI_CalcSplinLight(int dwDevice, int dwBeg, int dwEnd, double fStep, ref double fLight) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fStep: double
    ///fRefer: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcLintRefer")]
public static extern  int JETI_CalcLintRefer(int dwDevice, int dwBeg, int dwEnd, double fStep, ref double fRefer) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fStep: double
    ///fRefer: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcSplinRefer")]
public static extern  int JETI_CalcSplinRefer(int dwDevice, int dwBeg, int dwEnd, double fStep, ref double fRefer) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fStep: double
    ///fTransRefl: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcLintTransRefl")]
public static extern  int JETI_CalcLintTransRefl(int dwDevice, int dwBeg, int dwEnd, double fStep, ref double fTransRefl) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fStep: double
    ///fTransRefl: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcSplinTransRefl")]
public static extern  int JETI_CalcSplinTransRefl(int dwDevice, int dwBeg, int dwEnd, double fStep, ref double fTransRefl) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fRadio: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcRadio")]
public static extern  int JETI_CalcRadio(int dwDevice, int dwBeg, int dwEnd, ref double fRadio) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fPhoto: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcPhoto")]
public static extern  int JETI_CalcPhoto(int dwDevice, int dwBeg, int dwEnd, ref double fPhoto) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fChromx: double*
    ///fChromy: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcChromxy")]
public static extern  int JETI_CalcChromxy(int dwDevice, int dwBeg, int dwEnd, ref double fChromx, ref double fChromy) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fChromu: double*
    ///fChromv: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcChromuv")]
public static extern  int JETI_CalcChromuv(int dwDevice, int dwBeg, int dwEnd, ref double fChromu, ref double fChromv) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///fDWL: double*
    ///fPE: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcDWLPE")]
public static extern  int JETI_CalcDWLPE(int dwDevice, int dwBeg, int dwEnd, ref double fDWL, ref double fPE) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwBeg: int
    ///dwEnd: int
    ///dwCCT: int*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcCCT")]
public static extern  int JETI_CalcCCT(int dwDevice, int dwBeg, int dwEnd, ref int dwCCT) ;

    
    /// Return Type: int
    ///dwDevice: int
    ///dwCCT: int
    ///fCRI: double*
    [System.Runtime.InteropServices.DllImportAttribute("jeti_core.dll", EntryPoint="JETI_CalcCRI")]
public static extern  int JETI_CalcCRI(int dwDevice, int dwCCT, ref double fCRI) ;


    }
}
