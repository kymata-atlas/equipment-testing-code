using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JETIApp
{
	class Power
	{
	    [System.Runtime.InteropServices.DllImportAttribute("powrprof.dll", EntryPoint="GetActivePwrScheme")]
		static extern  bool GetActivePwrScheme(ref int UID) ;

		[System.Runtime.InteropServices.DllImportAttribute("powrprof.dll", EntryPoint="ReadPwrScheme")]
		static extern bool ReadPwrScheme (int UID, IntPtr pPowerPolicy);


		public static bool MonitorTimeout(ref string result, ref int timeout)
		{
			int activeID=0;
			bool ret;

			ret = GetActivePwrScheme(ref activeID);
			if (ret == false)
			{
				result = "Error getting active power scheme ID unable to";
				return false;
			}

			POWER_POLICY PwrPolicy = new POWER_POLICY();

			IntPtr pPwrPolicy = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(POWER_POLICY)));

			ret = Power.ReadPwrScheme(activeID, pPwrPolicy);

			if (ret)
			{

				PwrPolicy = (POWER_POLICY)Marshal.PtrToStructure(pPwrPolicy, typeof(POWER_POLICY));
			}
			else
			{
				result = "Error retrieving active power policy details";
				return false;
			}

			// determine if we are running on battery or mains
			PowerStatus p=SystemInformation.PowerStatus;
			StringBuilder sb=new StringBuilder();

			ret = true;

			if (p.PowerLineStatus == PowerLineStatus.Offline)
			{
				timeout = (int)PwrPolicy.user.VideoTimeoutDc;
				sb.AppendLine("Warning computer is running on battery, make sure battery does not run out before calibration has finished");
				if (PwrPolicy.user.VideoTimeoutDc != 0)
					sb.AppendFormat("Power settings indicate monitor is set to power down if idle after " + PwrPolicy.user.VideoTimeoutDc.ToString() + " seconds.\nIt is recommended to disable monitor timeouts before calibration.\n");
				ret = false;
				

			}
			else if (p.PowerLineStatus==PowerLineStatus.Online)
			{
				timeout = (int)PwrPolicy.user.VideoTimeoutAc;
				if (PwrPolicy.user.VideoTimeoutAc != 0)
				{
					sb.AppendFormat("Power settings indicate monitor is set to power down if idle after " + PwrPolicy.user.VideoTimeoutAc.ToString() + " seconds.\nIt is recommended to disable monitor timeouts before calibration\n.");
					ret = false;

				}
			}
			else
			{
				timeout=-1;
				sb.AppendFormat("Warning unable to determine if PC is running on battery or mains.\nIt is recommended to disable monitor timeouts before calibration.\n");
				ret = false;
				
			}
			
			result=sb.ToString();
			return ret;

		}

		public enum POWER_ACTION : int
		{
			PowerActionNone = 0,
			PowerActionReserved,
			PowerActionSleep,
			PowerActionHibernate,
			PowerActionShutdown,
			PowerActionShutdownReset,
			PowerActionShutdownOff,
			PowerActionWarmEject
		}

		 public enum SYSTEM_POWER_STATE : int
		 {
			 PowerSystemUnspecified = 0,
			 PowerSystemWorking = 1,
			 PowerSystemSleeping1 = 2,
			 PowerSystemSleeping2 = 3,
			 PowerSystemSleeping3 = 4,
			 PowerSystemHibernate = 5,
			 PowerSystemShutdown = 6,
			 PowerSystemMaximum = 7
		 } 

		public struct POWER_ACTION_POLICY
		 {
		 public POWER_ACTION Action;
		 public uint Flags;
		 public uint EventCode;
		 } 
		
		[StructLayout(LayoutKind.Sequential)]
		public struct USER_POWER_POLICY
		{
			public uint Revision;
			public POWER_ACTION_POLICY IdleAc;
			public POWER_ACTION_POLICY IdleDc;
			public uint IdleTimeoutAc;
			public uint IdleTimeoutDc;
			public byte IdleSensitivityAc;
			public byte IdleSensitivityDc;
			public byte ThrottlePolicyAc;
			public byte ThrottlePolicyDc;
			public SYSTEM_POWER_STATE MaxSleepAc;
			public SYSTEM_POWER_STATE MaxSleepDc;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public uint[] Reserved;
			public uint VideoTimeoutAc;
			public uint VideoTimeoutDc;
			public uint SpindownTimeoutAc;
			public uint SpindownTimeoutDc;
			public bool OptimizeForPowerAc;
			public bool OptimizeForPowerDc;
			public byte FanThrottleToleranceAc;
			public byte FanThrottleToleranceDc;
			public byte ForcedThrottleAc;
			public byte ForcedThrottleDc;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MACHINE_POWER_POLICY
		{
			public byte Revision;
			public SYSTEM_POWER_STATE MinSleepAc;
			public SYSTEM_POWER_STATE MinSleepDc;
			public SYSTEM_POWER_STATE ReducedLatencySleepAc;
			public SYSTEM_POWER_STATE ReducedLatencySleepDc;
			public uint DozeTimeoutAc;
			public uint DozeTimeoutDc;
			public uint DozeS4TimeoutAc;
			public uint DozeS4TimeoutDc;
			public byte MinThrottleAc;
			public byte MinThrottleDc;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] pad1;
			public POWER_ACTION_POLICY OverThrottledAc;
			public POWER_ACTION_POLICY OverThrottledDc;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POWER_POLICY
		{
			public USER_POWER_POLICY user;
			public MACHINE_POWER_POLICY machine;
		}
	}
}
