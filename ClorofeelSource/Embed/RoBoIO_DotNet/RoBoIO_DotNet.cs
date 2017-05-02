using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace RoBoIO_DotNet
{
	public class RoBoIO
	{
		//RC SERVO LIB

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_InUse();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_Initialize(UInt32 usedchannels);
		public const int RCSERVO_USENOCHANNEL = (0);
		public const int RCSERVO_USECHANNEL0 = (1);
		public const int RCSERVO_USECHANNEL1 = (1 << 1);
		public const int RCSERVO_USECHANNEL2 = (1 << 2);
		public const int RCSERVO_USECHANNEL3 = (1 << 3);
		public const int RCSERVO_USECHANNEL4 = (1 << 4);
		public const int RCSERVO_USECHANNEL5 = (1 << 5);
		public const int RCSERVO_USECHANNEL6 = (1 << 6);
		public const int RCSERVO_USECHANNEL7 = (1 << 7);
		public const int RCSERVO_USECHANNEL8 = (1 << 8);
		public const int RCSERVO_USECHANNEL9 = (1 << 9);
		public const int RCSERVO_USECHANNEL10 = (1 << 10);
		public const int RCSERVO_USECHANNEL11 = (1 << 11);
		public const int RCSERVO_USECHANNEL12 = (1 << 12);
		public const int RCSERVO_USECHANNEL13 = (1 << 13);
		public const int RCSERVO_USECHANNEL14 = (1 << 14);
		public const int RCSERVO_USECHANNEL15 = (1 << 15);
		public const int RCSERVO_USECHANNEL16 = (1 << 16);
		public const int RCSERVO_USECHANNEL17 = (1 << 17);
		public const int RCSERVO_USECHANNEL18 = (1 << 18);
		public const int RCSERVO_USECHANNEL19 = (1 << 19);
		public const int RCSERVO_USECHANNEL20 = (1 << 20);
		public const int RCSERVO_USECHANNEL21 = (1 << 21);
		public const int RCSERVO_USECHANNEL22 = (1 << 22);
		public const int RCSERVO_USECHANNEL23 = (1 << 23);
		public const int ERROR_RCSERVO_INUSE = (ERR_NOERROR + 400);

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_Close();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_SetServo(int channel, UInt32 servono);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_SetServos(UInt32 channels, UInt32 servono);
		public const int RCSERVO_SERVO_DEFAULT = (0x00);  //conservative setting for most servos with pulse feedback
		public const int RCSERVO_SERVO_DEFAULT_NOFB = (0x01);  //conservative setting for most servos without pulse feedback
		public const int RCSERVO_KONDO_KRS786 = (0x11);
		public const int RCSERVO_KONDO_KRS788 = (0x12);
		public const int RCSERVO_KONDO_KRS78X = (0x13);
		public const int RCSERVO_KONDO_KRS4014 = (0x14);
		public const int RCSERVO_HITEC_HSR8498 = (0x22);
		public const int ERROR_RCSERVO_UNKNOWNSERVO = (ERR_NOERROR + 461);
		public const int ERROR_RCSERVO_WRONGCHANNEL = (ERR_NOERROR + 462);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_SetServoType(int channel, UInt32 type, UInt32 fbmethod);
		public const int RCSERVO_SV_FEEDBACK = (1);     //servo with pulse feedback
		public const int RCSERVO_SV_NOFEEDBACK = (2);     //servo without pulse feedback
		public const int RCSERVO_FB_SAFEMODE = (0);
		public const int RCSERVO_FB_FASTMODE = (1);     //much faster than safe mode but could cause servo's shake
		public const int RCSERVO_FB_DENOISE = (1 << 1);  //use denoise filter; more accurate but very slow

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_SetServoParams1(int channel, UInt32 period, UInt32 minduty, UInt32 maxduty);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_SetServoParams2(int channel, UInt32 invalidduty, UInt32 minlowphase);
		public const int ERROR_RCSERVO_INVALIDPARAMS = (ERR_NOERROR + 463);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_SetReadFBParams1(int channel, UInt32 initdelay, UInt32 lastdelay, UInt32 maxwidth);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_SetReadFBParams2(int channel, UInt32 resolution, long offset);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_SetReadFBParams3(int channel, int maxfails, int filterwidth);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_SetCmdPulse(int channel, int cmd, UInt32 duty);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_SetPlayModeCMD(int channel, int cmd);
		public const int RCSERVO_CMD_POWEROFF = (0);
		public const int RCSERVO_CMD1 = (1);
		public const int RCSERVO_CMD2 = (2);
		public const int RCSERVO_CMD3 = (3);
		public const int RCSERVO_CMD4 = (4);
		public const int RCSERVO_CMD5 = (5);
		public const int RCSERVO_CMD6 = (6);
		public const int RCSERVO_CMD7 = (7);

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_EnterCaptureMode();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 rcservo_ReadPosition(int channel, int cmd);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 rcservo_ReadPositionDN(int channel, int cmd); //for backward compatibility to RoBoIO library 1.1

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_ReadPositions(UInt32 channels, int cmd, [In, Out] UInt32[] width);

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_ReadPositionsDN(UInt32 channels, int cmd, [In, Out] UInt32[] width); 

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_EnableMPOS();

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_DisableMPOS();

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_EnterPlayMode();

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_EnterPlayMode_NOFB([In, Out] UInt32[] width);

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_EnterPlayMode_HOME([In, Out] UInt32[] width);

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_SetFPS(int fps);

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_SetAction([In, Out] UInt32[] width, UInt32 playtime);

		[DllImport("RoBoIO.dll")]
		public static extern int rcservo_PlayAction();

		[DllImport("RoBoIO.dll")]
		public static extern int rcservo_PlayActionMix([In, Out] Int32[] mixwidth);
		public const int RCSERVO_PLAYEND = (0);
		public const int RCSERVO_PLAYING = (1);
		public const int RCSERVO_PAUSED = (2);
		public const int RCSERVO_MIXWIDTH_POWEROFF = (0x7fffff00);
		public const int RCSERVO_MIXWIDTH_CMD1 = (0x7fffff01);
		public const int RCSERVO_MIXWIDTH_CMD2 = (0x7fffff02);
		public const int RCSERVO_MIXWIDTH_CMD3 = (0x7fffff03);
		public const int RCSERVO_MIXWIDTH_CMD4 = (0x7fffff04);
		public const int RCSERVO_MIXWIDTH_CMD5 = (0x7fffff05);
		public const int RCSERVO_MIXWIDTH_CMD6 = (0x7fffff06);
		public const int RCSERVO_MIXWIDTH_CMD7 = (0x7fffff07);

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_PauseAction();

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_ReleaseAction();

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_StopAction();

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_MoveTo([In, Out] UInt32[] width, UInt32 playtime);

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_MoveToMix([In, Out] UInt32[] width, UInt32 playtime, [In, Out] Int32[] mixwidth);

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_EnterPWMMode();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_SendPWMPulses(int channel, UInt32 period, UInt32 duty, UInt32 count);
		public const int ERROR_RCSERVO_PWMFAIL = (ERR_NOERROR + 430);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool rcservo_IsPWMCompleted(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_Outp(int channel, int value);

		[DllImport("RoBoIO.dll")]
		public static extern int rcservo_Inp(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void rcservo_Outps(UInt32 channels, UInt32 value);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 rcservo_Inps(UInt32 channels);

		//AD79X8 LIB

		public const int AD79x8_READFAIL = (0x7fff);
		public const int AD7908_READFAIL = (AD79x8_READFAIL);
		public const int AD7918_READFAIL = (AD79x8_READFAIL);
		public const int AD7928_READFAIL = (AD79x8_READFAIL);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 ad79x8_RawRead();
		public const int ERROR_ADCFAIL = (ERR_NOERROR + 310);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad79x8_RawWrite(UInt32 val);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7908_WriteCTRLREG(UInt32 ctrlreg);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad79x8_InUse();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad79x8_Initialize(int channel, int range, int coding);
		public const int AD79x8MODE_RANGE_VREF = (0);
		public const int AD79x8MODE_RANGE_2VREF = (1);
		public const int AD79x8MODE_CODING_UNSIGNED = (0);
		public const int AD79x8MODE_CODING_SIGNED = (1);
		public const int ERROR_ADC_NOSPI = (ERR_NOERROR + 331);
		public const int ERROR_ADC_INUSE = (ERR_NOERROR + 332);
		public const int ERROR_ADC_WRONGCHANNEL = (ERR_NOERROR + 311);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad79x8_Close();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad79x8_ChangeChannel(int channel, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad79x8_InitializeMCH(byte usedchannels, int range, int coding);
		public const int AD79x8_USECHANNEL0 = (1 << 7);
		public const int AD79x8_USECHANNEL1 = (1 << 6);
		public const int AD79x8_USECHANNEL2 = (1 << 5);
		public const int AD79x8_USECHANNEL3 = (1 << 4);
		public const int AD79x8_USECHANNEL4 = (1 << 3);
		public const int AD79x8_USECHANNEL5 = (1 << 2);
		public const int AD79x8_USECHANNEL6 = (1 << 1);
		public const int AD79x8_USECHANNEL7 = (1 << 0);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad79x8_ChangeChannels(byte usedchannels, int range, int coding);

		[DllImport("RoBoIO.dll")]
		public static extern int ad7908_ReadChannel(int channel, int range, int coding);  //don't use in batch mode
		public const int AD7908MODE_RANGE_VREF = (AD79x8MODE_RANGE_VREF);
		public const int AD7908MODE_RANGE_2VREF = (AD79x8MODE_RANGE_2VREF);
		public const int AD7908MODE_CODING_255 = (AD79x8MODE_CODING_UNSIGNED);
		public const int AD7908MODE_CODING_127 = (AD79x8MODE_CODING_SIGNED);

		[DllImport("RoBoIO.dll")]
		public static extern int ad7908_Read();
		public const int AD7908_USECHANNEL0 = (AD79x8_USECHANNEL0);
		public const int AD7908_USECHANNEL1 = (AD79x8_USECHANNEL1);
		public const int AD7908_USECHANNEL2 = (AD79x8_USECHANNEL2);
		public const int AD7908_USECHANNEL3 = (AD79x8_USECHANNEL3);
		public const int AD7908_USECHANNEL4 = (AD79x8_USECHANNEL4);
		public const int AD7908_USECHANNEL5 = (AD79x8_USECHANNEL5);
		public const int AD7908_USECHANNEL6 = (AD79x8_USECHANNEL6);
		public const int AD7908_USECHANNEL7 = (AD79x8_USECHANNEL7);

		[DllImport("RoBoIO.dll", EntryPoint = "ad7908_ReadMCH")]
		private static extern IntPtr csharp_ad7908_ReadMCH();
		public static int[] ad7908_ReadMCH()
		{
			int[] b = new int[32];
			IntPtr a;
			a = csharp_ad7908_ReadMCH();
			for (int i = 0; i < 32; i++)
			{
				b[i] = Marshal.ReadInt32(a, i * Marshal.SizeOf(typeof(Int32)));
			}
			return b;
		}

		[DllImport("RoBoIO.dll")]
		public static extern int ad7918_ReadChannel(int channel, int range, int coding);  //don't use in batch mode
		public const int AD7918MODE_RANGE_VREF = (AD79x8MODE_RANGE_VREF);
		public const int AD7918MODE_RANGE_2VREF = (AD79x8MODE_RANGE_2VREF);
		public const int AD7918MODE_CODING_1023 = (AD79x8MODE_CODING_UNSIGNED);
		public const int AD7918MODE_CODING_511 = (AD79x8MODE_CODING_SIGNED);

		[DllImport("RoBoIO.dll")]
		public static extern int ad7918_Read();
		public const int AD7918_USECHANNEL0 = (AD79x8_USECHANNEL0);
		public const int AD7918_USECHANNEL1 = (AD79x8_USECHANNEL1);
		public const int AD7918_USECHANNEL2 = (AD79x8_USECHANNEL2);
		public const int AD7918_USECHANNEL3 = (AD79x8_USECHANNEL3);
		public const int AD7918_USECHANNEL4 = (AD79x8_USECHANNEL4);
		public const int AD7918_USECHANNEL5 = (AD79x8_USECHANNEL5);
		public const int AD7918_USECHANNEL6 = (AD79x8_USECHANNEL6);
		public const int AD7918_USECHANNEL7 = (AD79x8_USECHANNEL7);

		[DllImport("RoBoIO.dll", EntryPoint = "ad7918_ReadMCH")]
		public static extern IntPtr csharp_ad7918_ReadMCH();
		public static int[] ad7918_ReadMCH()
		{
			int[] b = new int[32];
			IntPtr a;
			a = csharp_ad7918_ReadMCH();
			for (int i = 0; i < 32; i++)
			{
				b[i] = Marshal.ReadInt32(a, i * Marshal.SizeOf(typeof(Int32)));
			}
			return b;
		}

		[DllImport("RoBoIO.dll")]
		public static extern int ad7928_ReadChannel(int channel, int range, int coding);  //don't use in continuous mode
		public const int AD7928MODE_RANGE_VREF = (AD79x8MODE_RANGE_VREF);
		public const int AD7928MODE_RANGE_2VREF = (AD79x8MODE_RANGE_2VREF);
		public const int AD7928MODE_CODING_4095 = (AD79x8MODE_CODING_UNSIGNED);
		public const int AD7928MODE_CODING_2047 = (AD79x8MODE_CODING_SIGNED);

		[DllImport("RoBoIO.dll")]
		public static extern int ad7928_Read();
		public const int AD7928_USECHANNEL0 = (AD79x8_USECHANNEL0);
		public const int AD7928_USECHANNEL1 = (AD79x8_USECHANNEL1);
		public const int AD7928_USECHANNEL2 = (AD79x8_USECHANNEL2);
		public const int AD7928_USECHANNEL3 = (AD79x8_USECHANNEL3);
		public const int AD7928_USECHANNEL4 = (AD79x8_USECHANNEL4);
		public const int AD7928_USECHANNEL5 = (AD79x8_USECHANNEL5);
		public const int AD7928_USECHANNEL6 = (AD79x8_USECHANNEL6);
		public const int AD7928_USECHANNEL7 = (AD79x8_USECHANNEL7);

		[DllImport("RoBoIO.dll", EntryPoint = "ad7928_ReadMCH")]
		public static extern IntPtr csharp_ad7928_ReadMCH();
		public static int[] ad7928_ReadMCH()
		{
			int[] b = new int[32];
			IntPtr a;
			a = csharp_ad7928_ReadMCH();
			for (int i = 0; i < 32; i++)
			{
				b[i] = Marshal.ReadInt32(a, i * Marshal.SizeOf(typeof(Int32)));
			}
			return b;
		}

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7908_InUse();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7918_InUse();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7928_InUse();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7908_Initialize(int channel, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7918_Initialize(int channel, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7928_Initialize(int channel, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7908_Close();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7918_Close();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7928_Close();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7908_ChangeChannel(int channel, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7918_ChangeChannel(int channel, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7928_ChangeChannel(int channel, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7908_InitializeMCH(byte usedchannels, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7918_InitializeMCH(byte usedchannels, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7928_InitializeMCH(byte usedchannels, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7908_CloseMCH();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7918_CloseMCH();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7928_CloseMCH();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7908_ChangeChannels(byte usedchannels, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7918_ChangeChannels(byte usedchannels, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ad7928_ChangeChannels(byte usedchannels, int range, int coding);

		public const int ADC_READFAIL     = (AD79x8_READFAIL);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 adc_Read();
		public const int ADCMODE_RANGE_VREF      = (AD79x8MODE_RANGE_VREF);
		public const int ADCMODE_RANGE_2VREF     = (AD79x8MODE_RANGE_2VREF);
		public const int ADCMODE_UNSIGNEDCODING  = (AD79x8MODE_CODING_UNSIGNED);
		public const int ADCMODE_SIGNEDCODING    = (AD79x8MODE_CODING_SIGNED);
		
		[DllImport("RoBoIO.dll")]
		public static extern UInt32 adc_ReadChannel (int channel, int range, int coding);
		public const int ADC_USECHANNEL0     = (AD79x8_USECHANNEL0);
		public const int ADC_USECHANNEL1     = (AD79x8_USECHANNEL1);
		public const int ADC_USECHANNEL2     = (AD79x8_USECHANNEL2);
		public const int ADC_USECHANNEL3     = (AD79x8_USECHANNEL3);
		public const int ADC_USECHANNEL4     = (AD79x8_USECHANNEL4);
		public const int ADC_USECHANNEL5     = (AD79x8_USECHANNEL5);
		public const int ADC_USECHANNEL6     = (AD79x8_USECHANNEL6);
		public const int ADC_USECHANNEL7     = (AD79x8_USECHANNEL7);
			
		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool adc_InUse();
		
		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool adc_Initialize(int channel, int range, int coding);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool adc_Close();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool adc_ChangeChannel(int channel, int range, int coding);
		
		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool adc_InitializeMCH (byte usedchannels, int range, int coding);
		
		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool adc_CloseMCH();
		
		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool adc_ChangeChannels(byte usedchannels, int range, int coding);

		[DllImport("RoBoIO.dll")]
		public static extern int adc_ReadCH(int channel);

		[DllImport("RoBoIO.dll", EntryPoint = "adc_ReadMCH")]
		public static extern IntPtr csharp_adc_ReadMCH();
		public static int[] adc_ReadMCH()
		{
			int[] b = new int[32];
			IntPtr a;
			a = csharp_adc_ReadMCH();
			for (int i = 0; i < 32; i++)
			{
				b[i] = Marshal.ReadInt32(a, i * Marshal.SizeOf(typeof(Int32)));
			}
			return b;
		}

		//COMMON LIB

		[DllImport("RoBoIO.dll")]
		public static extern void roboio_SetRBVer(int ver);
		public const int RB_100b1 = (98);    //early RB-100 with 32-channel PWM (& TTL COM1) for interal use
		public const int RB_100b2 = (99);    //early RB-100 with RTS-controlled COM4
		public const int RB_100 = (100);   //officially released RB-100
		public const int RB_110 = (110);
		public const int RB_111 = (111);
		public const int ERR_NOERROR = (0);

		[DllImport("RoBoIO.dll")]
		public static extern int roboio_GetErrCode();

		[DllImport("RoBoIO.dll")]
		public static extern string roboio_GetErrMsg();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool err_SetLogFile(string logfile);

		[DllImport("RoBoIO.dll")]
		public static extern void err_CloseLogFile();

		[DllImport("RoBoIO.dll")]
		public static extern int roboio_GetRBVer();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 timer_nowtime();

		[DllImport("RoBoIO.dll")]
		public static extern void delay_ms(UInt32 delaytime);

		[DllImport("RoBoIO.dll")]
		public static extern void delay_us(UInt32 delaytime);

		//I2C LIB

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c_InUse();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c_Initialize2(UInt32 devs, int i2c0irq, int i2c1irq);
		public const int I2C_USEMODULE0 = (1 << 0);
		public const int I2C_USEMODULE1 = (1 << 1);
		public const int ERROR_I2C_INUSE = (ERR_NOERROR + 600);
		public const int ERROR_I2C_INITFAIL = (ERR_NOERROR + 602);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_Close();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c_SetSpeed(int dev, int mode, UInt32 bps);
		public const int I2CMODE_HIGHSPEED = (2);
		public const int I2CMODE_AUTO = (3);
		public const int ERROR_I2CWRONGUSAGE = (ERR_NOERROR + 630);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cmaster_Start(int dev, byte addr, byte rwbit);
		public const int ERROR_I2CARLOSS = (ERR_NOERROR + 621);
		public const int ERROR_I2CACKERR = (ERR_NOERROR + 622);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cmaster_Write(int dev, byte val);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cmaster_WriteLast(int dev, byte val);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2cmaster_Read1(int dev, bool secondlast);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2cmaster_ReadLast(int dev);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cmaster_StartN(int dev, byte addr, byte rwbit, int count);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cmaster_WriteN(int dev, byte val);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2cmaster_ReadN(int dev);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cmaster_SetRestart(int dev, byte addr, byte rwbit);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cmaster_SetRestartN(int dev, byte rwbit, int count);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2cslave_Listen(int dev);
		public const int I2CSLAVE_NOTHING = (0);
		public const int I2CSLAVE_START = (1);
		public const int I2CSLAVE_END = (2);
		public const int I2CSLAVE_WAITING = (3);
		public const int I2CSLAVE_WRITEREQUEST = (4);
		public const int I2CSLAVE_READREQUEST = (5);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cslave_Write(int dev, byte val);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2cslave_Read(int dev);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c_Initialize(int i2cirq);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c0_SetSpeed(int mode, UInt32 bps);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c1_SetSpeed(int mode, UInt32 bps);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0master_Start(byte addr, byte rwbit);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1master_Start(byte addr, byte rwbit);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0master_Write(byte val);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1master_Write(byte val);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0master_WriteLast(byte val);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1master_WriteLast(byte val);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c0master_Read();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c1master_Read();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c0master_ReadSecondLast();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c1master_ReadSecondLast();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c0master_ReadLast();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c1master_ReadLast();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0master_StartN(byte addr, byte rwbit, int count);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1master_StartN(byte addr, byte rwbit, int count);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0master_WriteN(byte val);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1master_WriteN(byte val);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c0master_ReadN();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c1master_ReadN();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0master_SetRestart(byte addr, byte rwbit);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1master_SetRestart(byte addr, byte rwbit);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0master_SetRestartN(byte rwbit, int count);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1master_SetRestartN(byte rwbit, int count);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c0slave_Listen();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c1slave_Listen();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0slave_Write(byte val);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1slave_Write(byte val);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c0slave_Read();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c1slave_Read();

		//I2CDX LIB

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_SetBaseAddress(UInt32 baseaddr);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 i2c_SetDefaultBaseAddress();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_SetIRQ(int i2c0irq, int i2c1irq);
		public const int I2CIRQ1 = (8);   //don't want to use enum:p
		public const int I2CIRQ3 = (2);
		public const int I2CIRQ4 = (4);
		public const int I2CIRQ5 = (5);
		public const int I2CIRQ6 = (7);
		public const int I2CIRQ7 = (6);
		public const int I2CIRQ9 = (1);
		public const int I2CIRQ10 = (3);
		public const int I2CIRQ11 = (9);
		public const int I2CIRQ12 = (11);
		public const int I2CIRQ14 = (13);
		public const int I2CIRQ15 = (15);
		public const int I2CIRQ_DISABLE = (0);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c_Reset(int dev);
		public const int ERROR_I2CFAIL = (ERR_NOERROR + 620);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_EnableNoiseFilter(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_DisableNoiseFilter(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_EnableStandardHSM(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_DisableStandardHSM(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_EnableINT(int dev, byte i2cints);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_DisableINT(int dev, byte i2cints);
		public const int I2CINT_SLAVEWREQ = (0x80);
		public const int I2CINT_RXRDY = (0x40);
		public const int I2CINT_TXDONE = (0x20);
		public const int I2CINT_ACKERR = (0x10);
		public const int I2CINT_ARLOSS = (0x08);
		public const int I2CINT_SLAVESTOPPED = (0x04);
		public const int I2CINT_ALL = (0xfc);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_SetCLKREG(int dev, int mode, byte prescale1, byte prescale2);
		public const int I2CMODE_STANDARD = (0);
		public const int I2CMODE_FAST = (1);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_ClearSTAT(int dev, byte i2cstats);
		public const int I2CSTAT_SLAVEWREQ = (0x80);
		public const int I2CSTAT_RXRDY = (0x40);
		public const int I2CSTAT_TXDONE = (0x20);
		public const int I2CSTAT_ACKERR = (0x10);
		public const int I2CSTAT_ARLOSS = (0x08);
		public const int I2CSTAT_SLAVESTOPPED = (0x04);
		public const int I2CSTAT_ALL = (0xfc);

		[DllImport("RoBoIO.dll")]
		public static extern byte i2c_ReadStatREG(int dev);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c_IsMaster(int dev);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c_CheckBusBusy(int dev);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c_CheckTXDone(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_ClearTXDone(int dev);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c_CheckRXRdy(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_ClearRXRdy(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c_WriteDataREG(int dev, byte val);

		[DllImport("RoBoIO.dll")]
		public static extern byte i2c_ReadDataREG(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2cmaster_SetStopBit(int dev);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cmaster_CheckStopBit(int dev);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cmaster_CheckARLoss(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2cmaster_ClearARLoss(int dev);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cmaster_CheckAckErr(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2cmaster_ClearAckErr(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2cmaster_WriteAddrREG(int dev, byte addr, byte rwbit);
		public const int I2C_WRITE = (0);
		public const int I2C_READ = (1);

		[DllImport("RoBoIO.dll")]
		public static extern void i2cslave_EnableACK(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2cslave_EnableNAK(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2cslave_SetAddr(int dev, byte addr);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2cslave_CheckSlaveWREQ(int dev);

		[DllImport("RoBoIO.dll")]
		public static extern void i2cslave_ClearSlaveWREQ(int dev);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0_Reset();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1_Reset();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0_EnableNoiseFilter();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1_EnableNoiseFilter();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0_DisableNoiseFilter();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1_DisableNoiseFilter();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0_EnableStandardHSM();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1_EnableStandardHSM();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0_DisableStandardHSM();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1_DisableStandardHSM();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0_EnableINT(byte i2cints);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1_EnableINT(byte i2cints);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0_DisableINT(byte i2cints);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1_DisableINT(byte i2cints);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0_SetCLKREG(int mode, byte prescale1, byte prescale2);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1_SetCLKREG(int mode, byte prescale1, byte prescale2);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0_ClearSTAT(byte i2cstats);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1_ClearSTAT(byte i2cstats);

		[DllImport("RoBoIO.dll")]
		public static extern byte i2c0_ReadStatREG();

		[DllImport("RoBoIO.dll")]
		public static extern byte i2c1_ReadStatREG();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0_IsMaster();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1_IsMaster();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0_CheckBusBusy();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1_CheckBusBusy();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0_CheckTXDone();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1_CheckTXDone();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0_ClearTXDone();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1_ClearTXDone();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0_CheckRXRdy();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1_CheckRXRdy();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0_ClearRXRdy();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1_ClearRXRdy();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0_WriteDataREG(byte val);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1_WriteDataREG(byte val);

		[DllImport("RoBoIO.dll")]
		public static extern byte i2c0_ReadDataREG();

		[DllImport("RoBoIO.dll")]
		public static extern byte i2c1_ReadDataREG();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0master_SetStopBit();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1master_SetStopBit();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0master_CheckStopBit();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1master_CheckStopBit();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0master_CheckARLoss();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1master_CheckARLoss();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0master_ClearARLoss();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1master_ClearARLoss();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0master_CheckAckErr();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1master_CheckAckErr();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0master_ClearAckErr();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1master_ClearAckErr();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0master_WriteAddrREG(byte addr, byte rwbit);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1master_WriteAddrREG(byte addr, byte rwbit);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0slave_EnableACK();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1slave_EnableACK();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0slave_EnableNAK();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1slave_EnableNAK();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0slave_SetAddr(byte addr);

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1slave_SetAddr(byte addr);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c0slave_CheckSlaveWREQ();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool i2c1slave_CheckSlaveWREQ();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c0slave_ClearSlaveWREQ();

		[DllImport("RoBoIO.dll")]
		public static extern void i2c1slave_ClearSlaveWREQ();

		//IO LIB

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool io_InUse();

		[DllImport("RoBoIO.dll")]
		public static extern int  io_Init();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool io_init(); //only for backward compatibility to RoBoIO v1.0
		public const int ERROR_IOINITFAIL		= (ERR_NOERROR + 100);
		public const int ERROR_IOSECTIONFULL	= (ERR_NOERROR + 101);
		public const int ERROR_CPUUNSUPPORTED	= (ERR_NOERROR + 102);

		[DllImport("RoBoIO.dll")]
		public static extern void io_Close(int section);

		[DllImport("RoBoIO.dll")]
		public static extern void io_close(); //only for backward compatibility to RoBoIO v1.0

		[DllImport("RoBoIO.dll")]
		public static extern void io_outpdw(UInt32 addr, UInt32 val);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 io_inpdw(UInt32 addr);

		[DllImport("RoBoIO.dll")]
		public static extern void io_outpw(UInt32 addr, UInt32 val);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 io_inpw(UInt32 addr);

		[DllImport("RoBoIO.dll")]
		public static extern void io_outpb(UInt32 addr, byte val);

		[DllImport("RoBoIO.dll")]
		public static extern byte io_inpb(UInt32 addr);

		[DllImport("RoBoIO.dll")]
		public static extern void io_SetISASpeed(int mode);
		public const int ISAMODE_NORMAL	           = (0);
		public const int ISAMODE_HIGHSPEED	       = (1);

		[DllImport("RoBoIO.dll")]
		public static extern int io_CpuID();
		public const int CPU_UNSUPPORTED	       = (0);
		public const int CPU_VORTEX86SX		       = (10);
		public const int CPU_VORTEX86DX_1	       = (21);
		public const int CPU_VORTEX86DX_2	       = (22);
		public const int CPU_VORTEX86DX_3	       = (23);
		public const int CPU_VORTEX86DX_UNKNOWN    = (24);
		public const int CPU_VORTEX86MX            = (30);
		public const int CPU_VORTEX86MX_PLUS       = (33);

		[DllImport("RoBoIO.dll")]
		public static extern void io_EnableIRQ();

		[DllImport("RoBoIO.dll")]
		public static extern void io_DisableIRQ();

		[DllImport("RoBoIO.dll")]
		public static extern byte read_sb_regb(byte idx);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 read_sb_regw(byte idx);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 read_sb_reg(byte idx);

		[DllImport("RoBoIO.dll")]
		public static extern void write_sb_regb(byte idx, byte val);

		[DllImport("RoBoIO.dll")]
		public static extern void write_sb_regw(byte idx, UInt32 val); //idx must = 0 (mod 2)

		[DllImport("RoBoIO.dll")]
		public static extern void write_sb_reg(byte idx, UInt32 val);  //idx must = 0 (mod 4)

		[DllImport("RoBoIO.dll")]
		public static extern byte read_nb_regb(byte idx);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 read_nb_regw(byte idx);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 read_nb_reg(byte idx);

		[DllImport("RoBoIO.dll")]
		public static extern void write_nb_regb(byte idx, byte val);

		[DllImport("RoBoIO.dll")]
		public static extern void write_nb_regw(byte idx, UInt32 val); //idx must = 0 (mod 2)

		[DllImport("RoBoIO.dll")]
		public static extern void write_nb_reg(byte idx, UInt32 val);  //idx must = 0 (mod 4)

		//PWM LIB

		[DllImport("RoBoIO.dll")]
		public static extern int pwm_NumCh();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool pwm_InUse();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool pwm_Initialize(UInt32 baseaddr, int clkmode, int irqmode);
		public const int ERROR_PWM_INUSE = (ERR_NOERROR + 300);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_Close();

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetBaseClock(int clock);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool pwm_SetPulse(int channel, UInt32 period, UInt32 duty);		//resolution: 1us

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool pwm_SetPulse_10MHZ(int channel, UInt32 period, UInt32 duty);	//resolution: 0.1us

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool pwm_SetPulse_50MHZ(int channel, UInt32 period, UInt32 duty);	//resolution: 20ns
		public const int ERROR_PWM_WRONGCHANNEL = (ERR_NOERROR + 310);
		public const int ERROR_PWM_INVALIDPULSE = (ERR_NOERROR + 311);
		public const int ERROR_PWM_CLOCKMISMATCH = (ERR_NOERROR + 312);

		//PWMDX LIB

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetBaseAddress(UInt32 baseaddr);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 pwm_SetDefaultBaseAddress();

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SelectClock(int clock);
		public const int PWMCLOCK_10MHZ = (0); //don't want to use enum:p
		public const int PWMCLOCK_50MHZ = (1);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetIRQ(int pwmirq);
		public const int PWMIRQ1 = (8);   //don't want to use enum:p
		public const int PWMIRQ3 = (2);
		public const int PWMIRQ4 = (4);
		public const int PWMIRQ5 = (5);
		public const int PWMIRQ6 = (7);
		public const int PWMIRQ7 = (6);
		public const int PWMIRQ9 = (1);
		public const int PWMIRQ10 = (3);
		public const int PWMIRQ11 = (9);
		public const int PWMIRQ12 = (11);
		public const int PWMIRQ14 = (13);
		public const int PWMIRQ15 = (15);
		public const int PWMIRQ_DISABLE = (0);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_EnablePin(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_DisablePin(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_EnableMultiPin(UInt32 channels);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_DisableMultiPin(UInt32 channels);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_Lock(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_Unlock(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_LockMulti(UInt32 channels);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_UnlockMulti(UInt32 channels);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_EnableINT(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_DisableINT(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_EnableMultiINT(UInt32 channels);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_DisableMultiINT(UInt32 channels);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_ClearMultiFLAG(UInt32 channels);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 pwm_ReadMultiFLAG(UInt32 channels);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_EnablePWM(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_DisablePWM(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_EnableMultiPWM(UInt32 channels);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_DisableMultiPWM(UInt32 channels);

		[DllImport("RoBoIO.dll")]
		public static extern int pwm_ReadCountingMode(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetCountingMode(int channel, int mode);
		public const int PWM_COUNT_MODE = (0);  //don't want to use enum:p
		public const int PWM_CONTINUE_MODE = (1);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetWaveform(int channel, int mode);
		public const int PWM_WAVEFORM_NORMAL = (0);  //don't want to use enum:p
		public const int PWM_WAVEFORM_INVERSE = (1);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetPulseCount(int channel, UInt32 count);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetCtrlREG(int channel, UInt32 creg);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetHREG(int channel, UInt32 hreg);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetLREG(int channel, UInt32 lreg);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 pwm_ReadPulseCount(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 pwm_ReadCtrlREG(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 pwm_ReadHREG(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 pwm_ReadLREG(int channel);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetSyncREG(UInt32 sreg);

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetINTREG(UInt32 ireg);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 pwm_ReadSyncREG();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 pwm_ReadINTREG();

		[DllImport("RoBoIO.dll")]
		public static extern void pwm_SetPWMSettingREG(UInt32 psreg);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 pwm_ReadPWMSettingREG();

		//SPI LIB

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_InUse();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_Initialize(int clkmode);
		public const int SPICLK_21400KHZ = (0);
		public const int SPICLK_18750KHZ = (1);
		public const int SPICLK_15000KHZ = (2);
		public const int SPICLK_12500KHZ = (3);
		public const int SPICLK_10000KHZ = (4);
		public const int SPICLK_10714KHZ = (5);
		public const int SPICLK_11538KHZ = (6);
		public const int SPICLK_13636KHZ = (7);
		public const int SPICLK_16666KHZ = (8);
		public const int SPICLK_25000KHZ = (9);
		public const int SPICLK_30000KHZ = (10);
		public const int SPICLK_37000KHZ = (11);
		public const int SPICLK_50000KHZ = (12);
		public const int SPICLK_75000KHZ = (13);
		public const int SPICLK_150000KHZ = (14);
		
		public const int ERROR_SPI_INUSE = (ERR_NOERROR + 200);
		public const int ERROR_SPI_INITFAIL = (ERR_NOERROR + 202);

		[DllImport("RoBoIO.dll")]
		public static extern void spi_Close();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_EnableSS();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_DisableSS();
		
		public const int ERROR_SPI_UNSUPPORTED = (ERR_NOERROR + 220);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_InitializeSW(int mode, UInt32 clkdelay);
		
		[DllImport("RoBoIO.dll")]
		public static extern void spi_CloseSW();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_Write(byte val);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_WriteFlush(byte val);
		
		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_FIFOFlush();
		
		[DllImport("RoBoIO.dll")]
		public static extern UInt32 spi_Read();
		
		[DllImport("RoBoIO.dll")]
		public static extern UInt32 spi_Read2();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 spi_ReadStrict();
		
		[DllImport("RoBoIO.dll")]
		public static extern UInt32 spi_Exchange(byte val);
		
		//SPIDX LIB

		[DllImport("RoBoIO.dll")]
		public static extern void spi_SetBaseAddress(UInt32 baseaddr);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 spi_SetDefaultBaseAddress();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_SetControlREG(byte ctrlreg);
		public const int ERROR_SPIBUSY = (ERR_NOERROR + 210);

		[DllImport("RoBoIO.dll")]
		public static extern void spi_SetErrorREG(byte errreg);

		[DllImport("RoBoIO.dll")]
		public static extern byte spi_ReadControlREG();

		[DllImport("RoBoIO.dll")]
		public static extern byte spi_ReadStatusREG();

		[DllImport("RoBoIO.dll")]
		public static extern byte spi_ReadCSREG();

		[DllImport("RoBoIO.dll")]
		public static extern byte spi_ReadErrorREG();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_EnableFIFO();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_DisableFIFO();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_SetClockDivisor(byte clkdiv);
		public const int ERROR_SPIWRONGCLOCK = (ERR_NOERROR + 211);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_InputReady();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_OutputFIFOEmpty();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_OutputFIFOFull();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spi_Busy();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spierror_SPIBusy();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spierror_InputOverlap();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spierror_FIFOUnderrun();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spierror_FIFOOverrun();

		[DllImport("RoBoIO.dll")]
		public static extern void spierror_ClearSPIBusy();

		[DllImport("RoBoIO.dll")]
		public static extern void spierror_ClearInputOverlap();

		[DllImport("RoBoIO.dll")]
		public static extern void spierror_ClearFIFOUnderrun();

		[DllImport("RoBoIO.dll")]
		public static extern void spierror_ClearFIFOOverrun();

		[DllImport("RoBoIO.dll")]
		public static extern void spi_ClearErrors();

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spidx_Write(byte val);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spidx_WriteFlush(byte val);
		public const int ERROR_SPIFIFOFULL = (ERR_NOERROR + 214);
		public const int ERROR_SPIFIFOFAIL = (ERR_NOERROR + 215);

		[DllImport("RoBoIO.dll")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool spidx_FIFOFlush();
		//   public const int ERROR_SPIFIFOFAIL      = (ERR_NOERROR + 215);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 spidx_Read();
		//   public const int ERROR_SPIFIFOFAIL      = (ERR_NOERROR + 215);
		public const int ERROR_SPIREADFAIL = (ERR_NOERROR + 219);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 spidx_Read2();

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 spidx_ReadStrict(); //avoid to read overdue data in SPI input buffer
		//   public const int ERROR_SPIREADFAIL      = (ERR_NOERROR + 219);

		[DllImport("RoBoIO.dll")]
		public static extern UInt32 spisw_Exchange(byte val, int mode, UInt32 delay);
		public const int SPIMODE_CPOL0 = (0x00);
		public const int SPIMODE_CPOL1 = (0x04);
		public const int SPIMODE_SMOD0 = (0x00);
		public const int SPIMODE_SMOD1 = (0x02);
		public const int SPIMODE_CPHA0 = (0x00);
		public const int SPIMODE_CPHA1 = (0x01);
		public const int ERROR_SPISW_INVALIDMODE = (ERR_NOERROR + 230);

	}
}
