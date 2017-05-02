using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace QuidMind.CloRoFeel.LcdManagerModule.LcdPaginator
{
    static class ParralaxLcdHelperExtension
    {
        static byte[] LCD_BacklightOn = new byte[] { 0x11 };
        static byte[] LCD_BacklightOff = new byte[] { 0x12 };

        //static byte[] LCD_ClearScreen = new byte[] { 0x80, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,0x80 };
        static byte[] LCD_ClearScreen = new byte[] { 0x0c };
        static byte[] LCD_ClearLine0 = new byte[] { 0x80, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x80 };
        static byte[] LCD_ClearLine1 = new byte[] { 0x94, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x94 };

        static byte[] LCD_Line0Start = new byte[] { 0x80 };
        static byte[] LCD_Line1Start = new byte[] { 0x94 };

        static byte[] LCD_Off = new byte[] { 0x15 };
        static byte[] LCD_On_NoCursor_NoBlink = new byte[] { 0x16 };
        static byte[] LCD_On_NoCursor_Blink = new byte[] { 0x17 };
        static byte[] LCD_On_Cursor_NoBlink = new byte[] { 0x18 };
        static byte[] LCD_On_Cursor_Blink = new byte[] { 0x19 };

        static public SerialPort SwitchOff(this SerialPort lcdComPort)
        { SendByteArray(lcdComPort, LCD_Off); return lcdComPort; }

        static public SerialPort SwitchOn_NoCursor_NoBlink(this SerialPort lcdComPort)
        { SendByteArray(lcdComPort, LCD_On_NoCursor_NoBlink); return lcdComPort; }

        static public SerialPort SwitchOn_NoCursor_Blink(this SerialPort lcdComPort)
        { SendByteArray(lcdComPort, LCD_On_NoCursor_Blink); return lcdComPort; }

        static public SerialPort SwitchOn_Cursor_NoBlink(this SerialPort lcdComPort)
        { SendByteArray(lcdComPort, LCD_On_Cursor_NoBlink); return lcdComPort; }

        static public SerialPort SwitchOn_Cursor_Blink(this SerialPort lcdComPort)
        { SendByteArray(lcdComPort, LCD_On_Cursor_Blink); return lcdComPort; }


        static public SerialPort BacklightOn(this SerialPort lcdComPort)
        { SendByteArray(lcdComPort, LCD_BacklightOn); return lcdComPort; }

        static public SerialPort BacklightOff(this SerialPort lcdComPort)
        { SendByteArray(lcdComPort, LCD_BacklightOff); return lcdComPort; }

        static public SerialPort ClearScreen(this SerialPort lcdComPort)
        {
            SendByteArray(lcdComPort, LCD_ClearScreen);
            System.Threading.Thread.Sleep(6);
            return lcdComPort;
        }

        static public SerialPort ClearLine0(this SerialPort lcdComPort)
        { SendByteArray(lcdComPort, LCD_ClearLine0); return lcdComPort; }

        static public SerialPort ClearLine1(this SerialPort lcdComPort)
        { SendByteArray(lcdComPort, LCD_ClearLine1); return lcdComPort; }

        static public SerialPort GoToLine0(this  SerialPort lcdComPort)
        { SendByteArray(lcdComPort, LCD_Line0Start); return lcdComPort; }

        static public SerialPort GoToLine1(this  SerialPort lcdComPort)
        { SendByteArray(lcdComPort, LCD_Line1Start); return lcdComPort; }

        static public SerialPort WriteLineEx(this  SerialPort lcdComPort, string msg)
        {
            lcdComPort.Write(msg);
            lcdComPort.BaseStream.Flush();
            return lcdComPort;
        }

        static public void SendByteArray(SerialPort com, byte[] array)
        {
            com.Write(array, 0, array.Length);
            com.BaseStream.Flush();
        }
    }

}
