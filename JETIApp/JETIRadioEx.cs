using System;
using System.Collections.Generic;
using System.Text;

namespace JETILib
{
	class JETIRadioEx
	{

		public partial class NativeMethods
		{

			/// Return Type: DWORD->unsigned int
			///dwNumDevices: DWORD*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_GetNumRadioEx")]
			public static extern uint JETI_GetNumRadioEx(ref uint dwNumDevices);


			/// Return Type: DWORD->unsigned int
			///dwDeviceNum: DWORD->unsigned int
			///dwSerial1: DWORD*
			///dwSerial2: DWORD*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_GetSerialRadioEx")]
			public static extern uint JETI_GetSerialRadioEx(uint dwDeviceNum, ref uint dwSerial1, ref uint dwSerial2);


			/// Return Type: DWORD->unsigned int
			///dwDeviceNum: DWORD->unsigned int
			///dwDevice: DWORD*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_OpenRadioEx")]
			public static extern uint JETI_OpenRadioEx(uint dwDeviceNum, ref uint dwDevice);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_CloseRadioEx")]
			public static extern uint JETI_CloseRadioEx(uint dwDevice);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			///dwTint: DWORD->unsigned int
			///bAver: BYTE->unsigned char
			///dwStep: DWORD->unsigned int
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_MeasureEx")]
			public static extern uint JETI_MeasureEx(uint dwDevice, uint dwTint, byte bAver, uint dwStep);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			///boStatus: BOOL*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_MeasureStatusEx")]
			public static extern uint JETI_MeasureStatusEx(uint dwDevice, ref int boStatus);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_MeasureBreakEx")]
			public static extern uint JETI_MeasureBreakEx(uint dwDevice);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			///dwBeg: DWORD->unsigned int
			///dwEnd: DWORD->unsigned int
			///fSprad: FLOAT*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_SpecRadEx")]
			public static extern uint JETI_SpecRadEx(uint dwDevice, uint dwBeg, uint dwEnd, ref float fSprad);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			///dwBeg: DWORD->unsigned int
			///dwEnd: DWORD->unsigned int
			///fRadio: FLOAT*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_RadioEx")]
			public static extern uint JETI_RadioEx(uint dwDevice, uint dwBeg, uint dwEnd, ref float fRadio);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			///dwBeg: DWORD->unsigned int
			///dwEnd: DWORD->unsigned int
			///fPhoto: FLOAT*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_PhotoEx")]
			public static extern uint JETI_PhotoEx(uint dwDevice, uint dwBeg, uint dwEnd, ref float fPhoto);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			///dwBeg: DWORD->unsigned int
			///dwEnd: DWORD->unsigned int
			///fChromx: FLOAT*
			///fChromy: FLOAT*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_ChromxyEx")]
			public static extern uint JETI_ChromxyEx(uint dwDevice, uint dwBeg, uint dwEnd, ref float fChromx, ref float fChromy);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			///dwBeg: DWORD->unsigned int
			///dwEnd: DWORD->unsigned int
			///fChromu: FLOAT*
			///fChromv: FLOAT*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_ChromuvEx")]
			public static extern uint JETI_ChromuvEx(uint dwDevice, uint dwBeg, uint dwEnd, ref float fChromu, ref float fChromv);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			///dwBeg: DWORD->unsigned int
			///dwEnd: DWORD->unsigned int
			///fDWL: FLOAT*
			///fPE: FLOAT*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_DWLPEEx")]
			public static extern uint JETI_DWLPEEx(uint dwDevice, uint dwBeg, uint dwEnd, ref float fDWL, ref float fPE);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			///dwBeg: DWORD->unsigned int
			///dwEnd: DWORD->unsigned int
			///dwCCT: DWORD*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_CCTEx")]
			public static extern uint JETI_CCTEx(uint dwDevice, uint dwBeg, uint dwEnd, ref uint dwCCT);


			/// Return Type: DWORD->unsigned int
			///dwDevice: DWORD->unsigned int
			///dwCCT: DWORD->unsigned int
			///fCRI: FLOAT*
			[System.Runtime.InteropServices.DllImportAttribute("jeti_radio_ex.dll", EntryPoint = "JETI_CRIEx")]
			public static extern uint JETI_CRIEx(uint dwDevice, uint dwCCT, ref float fCRI);

		}

	}
}
