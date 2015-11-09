using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
/*
*
* Find out license for this file.
* Copied from somewhere in the internet.
* Original might be this VB project http://www.codeproject.com/Articles/117657/InputManager-library-Track-user-input-and-simulate?fid=1591687&fr=76
*
*/
namespace KeySndr.InputManager
{
    public class Keyboard
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SendInput(int cInputs, ref INPUT pInputs, int cbSize);
        private struct INPUT
        {
            public uint dwType;
            public MOUSEKEYBDHARDWAREINPUT mkhi;
        }
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public uint dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }
        [StructLayout(LayoutKind.Explicit)]
        private struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public uint dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }
        const UInt32 INPUT_MOUSE = 0;
        const int INPUT_KEYBOARD = 1;
        const int INPUT_HARDWARE = 2;
        const UInt32 KEYEVENTF_EXTENDEDKEY = 0x01;
        const UInt32 KEYEVENTF_KEYUP = 0x02;
        const UInt32 KEYEVENTF_UNICODE = 0x04;
        const UInt32 KEYEVENTF_SCANCODE = 0x08;
        const UInt32 XBUTTON1 = 0x01;
        const UInt32 XBUTTON2 = 0x02;
        const UInt32 MOUSEEVENTF_MOVE = 0x01;
        const UInt32 MOUSEEVENTF_LEFTDOWN = 0x02;
        const UInt32 MOUSEEVENTF_LEFTUP = 0x04;
        const UInt32 MOUSEEVENTF_RIGHTDOWN = 0x08;
        const UInt32 MOUSEEVENTF_RIGHTUP = 0x10;
        const UInt32 MOUSEEVENTF_MIDDLEDOWN = 0x20;
        const UInt32 MOUSEEVENTF_MIDDLEUP = 0x40;
        const UInt32 MOUSEEVENTF_XDOWN = 0x80;
        const UInt32 MOUSEEVENTF_XUP = 0x100;
        const UInt32 MOUSEEVENTF_WHEEL = 0x800;
        const UInt32 MOUSEEVENTF_VIRTUALDESK = 0x4000;
        const UInt32 MOUSEEVENTF_ABSOLUTE = 0x8000;

        [DllImport("user32.dll", CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern UInt32 MapVirtualKey(UInt32 uCode, MapVirtualKeyMapTypes uMapType);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern UInt32 MapVirtualKeyEx(UInt32 uCode, MapVirtualKeyMapTypes uMapType, IntPtr dwhkl);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        public enum MapVirtualKeyMapTypes : uint
        {
            /// <summary>uCode is a virtual-key code and is translated into a scan code.
            /// If it is a virtual-key code that does not distinguish between left- and
            /// right-hand keys, the left-hand scan code is returned.
            /// If there is no translation, the function returns 0.
            /// </summary>
            /// <remarks></remarks>
            MAPVK_VK_TO_VSC = 0x00,

            /// <summary>uCode is a scan code and is translated into a virtual-key code that
            /// does not distinguish between left- and right-hand keys. If there is no
            /// translation, the function returns 0.
            /// </summary>
            /// <remarks></remarks>
            MAPVK_VSC_TO_VK = 0x01,

            /// <summary>uCode is a virtual-key code and is translated into an unshifted
            /// character value in the low-order word of the return value. Dead keys (diacritics)
            /// are indicated by setting the top bit of the return value. If there is no
            /// translation, the function returns 0.
            /// </summary>
            /// <remarks></remarks>
            MAPVK_VK_TO_CHAR = 0x02,

            /// <summary>Windows NT/2000/XP: uCode is a scan code and is translated into a
            /// virtual-key code that distinguishes between left- and right-hand keys. If
            /// there is no translation, the function returns 0.
            /// </summary>
            /// <remarks></remarks>
            MAPVK_VSC_TO_VK_EX = 0x03,

            /// <summary>Not currently documented
            /// </summary>
            /// <remarks></remarks>
            MAPVK_VK_TO_VSC_EX = 0x04,
        } ///Enum

        private static ScanKey GetScanKey(Keys VKey)
        {
            uint ScanCode = MapVirtualKey((uint)VKey, MapVirtualKeyMapTypes.MAPVK_VK_TO_VSC);
            bool Extended = (VKey == Keys.RMenu) || (VKey == Keys.RControlKey) || (VKey == Keys.Left) || (VKey == Keys.Right) || (VKey == Keys.Up) || (VKey == Keys.Down) || (VKey == Keys.Home) || (VKey == Keys.Delete) || (VKey == Keys.PageUp) || (VKey == Keys.PageDown) || (VKey == Keys.End) || (VKey == Keys.Insert) || (VKey == Keys.NumLock) || (VKey == Keys.PrintScreen) || (VKey == Keys.Divide);
            return new ScanKey(ScanCode, Extended);
        }

        private struct ScanKey
        {
            public uint ScanCode;
            public bool Extended;
            public ScanKey(uint sCode, Boolean ex/* = false*/)
            {
                ScanCode = sCode;
                Extended = ex;
            }
        }
        /// <summary>
        /// Sends shortcut keys (key down and up) signals.
        /// </summary>
        /// <param name="kCode">The array of keys to send as a shortcut.</param>
        /// <param name="Delay">The delay in milliseconds between the key down and up events.</param>
        /// <remarks></remarks>
        public static void ShortcutKeys(Keys[] kCode, int Delay /*= 0*/)
        {
            KeyPressStruct KeysPress = new KeyPressStruct(kCode, Delay);
            Thread t = new Thread(new ParameterizedThreadStart(KeyPressThread));
            t.Start(KeysPress);
        }
        /// <summary>
        /// Sends a key down signal.
        /// </summary>
        /// <param name="kCode">The virtual keycode to send.</param>
        /// <remarks></remarks>
        public static void KeyDown(Keys kCode)
        {
            ScanKey sKey = GetScanKey(kCode);
            INPUT input = new INPUT();
            input.dwType = INPUT_KEYBOARD;
            input.mkhi.ki = new KEYBDINPUT();
            input.mkhi.ki.wScan = (short)sKey.ScanCode;
            input.mkhi.ki.dwExtraInfo = IntPtr.Zero;
            input.mkhi.ki.dwFlags = KEYEVENTF_SCANCODE | (sKey.Extended ? KEYEVENTF_EXTENDEDKEY : 0);
            int cbSize = Marshal.SizeOf(typeof(INPUT));
            SendInput(1, ref input, cbSize);
        }
        /// <summary>
        /// Sends a key up signal.
        /// </summary>
        /// <param name="kCode">The virtual keycode to send.</param>
        /// <remarks></remarks>
        public static void KeyUp(Keys kCode)
        {
            ScanKey sKey = GetScanKey(kCode);
            INPUT input = new INPUT();
            input.dwType = INPUT_KEYBOARD;
            input.mkhi.ki = new KEYBDINPUT();
            input.mkhi.ki.wScan = (short)sKey.ScanCode;
            input.mkhi.ki.dwExtraInfo = IntPtr.Zero;
            input.mkhi.ki.dwFlags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP | (sKey.Extended ? KEYEVENTF_EXTENDEDKEY : 0);
            int cbSize = Marshal.SizeOf(typeof(INPUT));
            SendInput(1, ref input, cbSize);
        }
        /// <summary>
        /// Sends a key press signal (key down and up).
        /// </summary>
        /// <param name="kCode">The virtual keycode to send.</param>
        /// <param name="Delay">The delay to set between the key down and up commands.</param>
        /// <remarks></remarks>
        public static void KeyPress(Keys kCode, int Delay /*= 0*/)
        {
            Keys[] SendKeys = new Keys[] { kCode };
            KeyPressStruct KeysPress = new KeyPressStruct(SendKeys, Delay);
            Thread t = new Thread(new ParameterizedThreadStart(KeyPressThread));
            t.Start(KeysPress);
        }
        private static void KeyPressThread(object obj)
        {
            KeyPressStruct KeysP = (KeyPressStruct)obj;
            foreach (Keys k in KeysP.Keys)
            {
                KeyDown(k);
            }
            if (KeysP.Delay > 0) { Thread.Sleep(KeysP.Delay); }
            foreach (Keys k in KeysP.Keys)
            {
                KeyUp(k);
            }
        }
        private struct KeyPressStruct
        {
            public Keys[] Keys;
            public int Delay;
            public KeyPressStruct(Keys[] KeysToPress, int DelayTime /*= 0*/)
            {
                Keys = KeysToPress;
                Delay = DelayTime;
            }
        }
    }
    /// <summary>
    /// Provides methods to send keyboard input. The keys are being sent virtually and cannot be used with DirectX.
    /// </summary>
    /// <remarks></remarks>
    public class VirtualKeyboard
    {
        #region "API Declaring"
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
               CharSet = CharSet.Unicode, EntryPoint = "keybd_event",
               ExactSpelling = true, SetLastError = true)]
        public static extern bool keybd_event(UInt32 bVk, UInt32 bScan, UInt32 dwFlags, UInt32 dwExtraInfo);
        const UInt32 KEYEVENTF_EXTENDEDKEY = 0x01;
        const UInt32 KEYEVENTF_KEYUP = 0x02;
        #endregion

        /// <summary>
        /// Sends shortcut keys (key down and up) signals.
        /// </summary>
        /// <param name="kCode">The array of keys to send as a shortcut.</param>
        /// <param name="Delay">The delay in milliseconds between the key down and up events.</param>
        /// <remarks></remarks>
        public static void ShortcutKeys(Keys[] kCode, int Delay /*= 0*/)
        {
            KeyPressStruct KeyPress = new KeyPressStruct(kCode, Delay);
            Thread t = new Thread(new ParameterizedThreadStart(KeyPressThread));
            t.Start(KeyPress);
        }
        /// <summary>
        /// Sends a key down signal.
        /// </summary>
        /// <param name="kCode">The virtual keycode to send.</param>
        /// <remarks></remarks>
        public static void KeyDown(Keys kCode)
        {
            keybd_event((UInt32)kCode, 0, 0, 0);
        }
        /// <summary>
        /// Sends a key up signal.
        /// </summary>
        /// <param name="kCode">The virtual keycode to send.</param>
        /// <remarks></remarks>
        public static void KeyUp(Keys kCode)
        {
            keybd_event((UInt32)kCode, 0, KEYEVENTF_KEYUP, 0);
        }
        /// <summary>
        /// Sends a key press signal (key down and up).
        /// </summary>
        /// <param name="kCode">The virtual key code to send.</param>
        /// <param name="Delay">The delay to set between the key down and up commands.</param>
        /// <remarks></remarks>
        public static void KeyPress(Keys kCode, int Delay /*= 0*/)
        {
            Keys[] SendKeys = new Keys[] { kCode };
            KeyPressStruct KeyPress = new KeyPressStruct(SendKeys, Delay);
            Thread t = new Thread(new ParameterizedThreadStart(KeyPressThread));
            t.Start(KeyPress);
        }
        public static void KeyPressThread(object obj)
        {
            KeyPressStruct KeysP = (KeyPressStruct)obj;
            foreach (Keys k in KeysP.Keys)
            {
                KeyDown(k);
            }
            if (KeysP.Delay > 0) Thread.Sleep(KeysP.Delay);
            foreach (Keys k in KeysP.Keys)
            {
                KeyUp(k);
            }
        }
        private struct KeyPressStruct
        {
            public Keys[] Keys;
            public int Delay;
            public KeyPressStruct(Keys[] KeysToPress, int DelayTime /*= 0*/)
            {
                Keys = KeysToPress;
                Delay = DelayTime;
            }
        }
    }
    /// <summary>
    /// Provides methods to send mouse input that also works in DirectX games.
    /// </summary>
    /// <remarks></remarks>
    public class Mouse
    {
        #region "APIs"
        [DllImport("user32.dll", CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int GetSystemMetrics(int smIndex);
        private const int SM_SWAPBUTTON = 23;
        #region "SendInput"
        [DllImport("user32.dll", CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SendInput(int cInputs, ref INPUT pInputs, int cbSize);
        private struct INPUT
        {
            public uint dwType;
            public MOUSEKEYBDHARDWAREINPUT mkhi;
        }
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }
        [StructLayout(LayoutKind.Explicit)]
        private struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public uint dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        const UInt32 INPUT_MOUSE = 0;
        const int INPUT_KEYBOARD = 1;
        const int INPUT_HARDWARE = 2;
        const UInt32 KEYEVENTF_EXTENDEDKEY = 0x1;
        const UInt32 KEYEVENTF_KEYUP = 0x2;
        const UInt32 KEYEVENTF_UNICODE = 0x4;
        const UInt32 KEYEVENTF_SCANCODE = 0x8;
        const UInt32 XBUTTON1 = 0x1;
        const UInt32 XBUTTON2 = 0x2;
        const UInt32 MOUSEEVENTF_MOVE = 0x1;
        const UInt32 MOUSEEVENTF_LEFTDOWN = 0x2;
        const UInt32 MOUSEEVENTF_LEFTUP = 0x4;
        const UInt32 MOUSEEVENTF_RIGHTDOWN = 0x8;
        const UInt32 MOUSEEVENTF_RIGHTUP = 0x10;
        const UInt32 MOUSEEVENTF_MIDDLEDOWN = 0x20;
        const UInt32 MOUSEEVENTF_MIDDLEUP = 0x40;
        const UInt32 MOUSEEVENTF_XDOWN = 0x80;
        const UInt32 MOUSEEVENTF_XUP = 0x100;
        const UInt32 MOUSEEVENTF_WHEEL = 0x800;
        const UInt32 MOUSEEVENTF_VIRTUALDESK = 0x4000;
        const UInt32 MOUSEEVENTF_ABSOLUTE = 0x8000;
        #endregion
        #endregion

        public enum MouseButtons
        {
            LeftDown = 0x2,
            LeftUp = 0x4,
            RightDown = 0x8,
            RightUp = 0x10,
            MiddleDown = 0x20,
            MiddleUp = 0x40,
            Absolute = 0x8000,
            Wheel = 0x800,
        }
        public enum MouseKeys
        {
            Left = -1,
            Right = -2,
            Middle = -3,
        }
        public enum ScrollDirection
        {
            Up = 120,
            Down = -120,
        }

        /// <summary>
        /// Returns true if mouse buttons are swapped
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsLeftHanded
        {
            get
            {
                try
                {
                    return (GetSystemMetrics(SM_SWAPBUTTON) == 1);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Sends a mouse button signal. To send a scroll use the Scroll method.
        /// </summary>
        /// <param name="mButton">The button to send.</param>
        /// <remarks></remarks>
        public static void SendButton(MouseButtons mButton)
        {
            INPUT input = new INPUT();
            input.dwType = INPUT_MOUSE;
            input.mkhi.mi = new MOUSEINPUT();
            input.mkhi.mi.dwExtraInfo = IntPtr.Zero;
            input.mkhi.mi.dwFlags = (uint)mButton;
            input.mkhi.mi.dx = 0;
            input.mkhi.mi.dy = 0;
            int cbSize = Marshal.SizeOf(typeof(INPUT));
            SendInput(1, ref input, cbSize);
        }
        /// <summary>
        /// Sends a mouse press signal (down and up).
        /// </summary>
        /// <param name="mKey">The key to press.</param>
        /// <param name="Delay">The delay to set between the events.</param>
        /// <remarks></remarks>
        public static void PressButton(MouseKeys mKey, int Delay /*= 0*/)
        {
            ButtonDown(mKey);
            if (Delay > 0) System.Threading.Thread.Sleep(Delay);
            ButtonUp(mKey);
        }
        /// <summary>
        /// Send a mouse button down signal.
        /// </summary>
        /// <param name="mKey">The mouse key to send as mouse button down.</param>
        /// <remarks></remarks>
        public static void ButtonDown(MouseKeys mKey)
        {
            switch (mKey)
            {
                case MouseKeys.Left:
                    SendButton(MouseButtons.LeftDown);
                    return;
                case MouseKeys.Right:
                    SendButton(MouseButtons.RightDown);
                    return;
                case MouseKeys.Middle:
                    SendButton(MouseButtons.MiddleDown);
                    return;
            }
        }
        /// <summary>
        /// Send a mouse button up signal.
        /// </summary>
        /// <param name="mKey">The mouse key to send as mouse button up.</param>
        /// <remarks></remarks>
        public static void ButtonUp(MouseKeys mKey)
        {
            switch (mKey)
            {
                case MouseKeys.Left:
                    SendButton(MouseButtons.LeftUp);
                    return;
                case MouseKeys.Right:
                    SendButton(MouseButtons.RightUp);
                    return;
                case MouseKeys.Middle:
                    SendButton(MouseButtons.MiddleUp);
                    return;
            }
        }
        /// <summary>
        /// Moves the mouse to a certain location on the screen.
        /// </summary>
        /// <param name="X">The x location to move the mouse.</param>
        /// <param name="Y">The y location to move the mouse</param>
        /// <remarks></remarks>
        public static void Move(int X, int Y)
        {
            INPUT input = new INPUT();
            input.dwType = INPUT_MOUSE;
            input.mkhi.mi = new MOUSEINPUT();
            input.mkhi.mi.dwExtraInfo = IntPtr.Zero;
            input.mkhi.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE;
            input.mkhi.mi.dx = X * (65535 / Screen.PrimaryScreen.Bounds.Width);
            input.mkhi.mi.dy = Y * (65535 / Screen.PrimaryScreen.Bounds.Height);
            int cbSize = Marshal.SizeOf(typeof(INPUT));
            SendInput(1, ref input, cbSize);
        }
        /// <summary>
        /// Moves the mouse to a location relative to the current one.
        /// </summary>
        /// <param name="X">The amount of pixels to move the mouse on the x axis.</param>
        /// <param name="Y">The amount of pixels to move the mouse on the y axis.</param>
        /// <remarks></remarks>
        public static void MoveRelative(int X, int Y)
        {
            INPUT input = new INPUT();
            input.dwType = INPUT_MOUSE;
            input.mkhi.mi = new MOUSEINPUT();
            input.mkhi.mi.dwExtraInfo = IntPtr.Zero;
            input.mkhi.mi.dwFlags = MOUSEEVENTF_MOVE;
            input.mkhi.mi.dx = X;
            input.mkhi.mi.dy = Y;
            int cbSize = Marshal.SizeOf(typeof(INPUT));
            SendInput(1, ref input, cbSize);
        }
        /// <summary>
        /// Sends a scroll signal with a specific direction to scroll.
        /// </summary>
        /// <param name="Direction">The direction to scroll.</param>
        /// <remarks></remarks>
        public static void Scroll(ScrollDirection Direction)
        {
            INPUT input = new INPUT();
            input.dwType = INPUT_MOUSE;
            input.mkhi.mi = new MOUSEINPUT();
            input.mkhi.mi.dwExtraInfo = IntPtr.Zero;
            input.mkhi.mi.dwFlags = (uint)MouseButtons.Wheel;
            input.mkhi.mi.mouseData = (int)Direction;
            input.mkhi.mi.dx = 0;
            input.mkhi.mi.dy = 0;
            int cbSize = Marshal.SizeOf(typeof(INPUT));
            SendInput(1, ref input, cbSize);
        }
    }
    /// <summary>
    /// Creates a low level keyboard hook.
    /// </summary>
    /// <remarks></remarks>
    public class KeyboardHook
    {
        #region "Constants and API's Declaring"
        //This is Constants
        private const int HC_ACTION = 0;
        private const int WH_KEYBOARD_LL = 13;
        private const uint WM_KEYDOWN = 0x100;
        private const uint WM_KEYUP = 0x101;
        private const uint WM_SYSKEYDOWN = 0x104;
        private const uint WM_SYSKEYUP = 0x105;
        private const uint WM_KEYLAST = 0x108;
        //Key press events
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scancode;
            public uint flags;
            public int time;
            public int dwExtraInfo;
        }
        //API Functions
        [DllImport("user32.dll", CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall, SetLastError = true, EntryPoint = "SetWindowsHookExA")]
        private static extern int SetWindowsHookEx(int idHook,
        KeyboardProcDelegate lpfn,
        int hmod,
        int dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int CallNextHookEx(int hHook, int nCode, uint wParam, ref KBDLLHOOKSTRUCT lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int UnhookWindowsHookEx(int hHook);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lib);
        // Keyboard Delegates
        public delegate int KeyboardProcDelegate(int nCode, uint wParam, ref KBDLLHOOKSTRUCT lParam);
        public delegate void KeyDownEventHandler(int nCode);
        public delegate void KeyUpEventHandler(int nCode);

        // KeyPress events
        public static event KeyDownEventHandler KeyDown/*(int vkCode)*/;
        public static event KeyUpEventHandler KeyUp/*(int vkCode)*/;

        //The identifyer for our KeyHook
        private static int KeyHook;
        //KeyHookDelegate
        private static KeyboardProcDelegate KeyHookDelegate;
        #endregion

        private static int KeyboardProc(int nCode, uint wParam, ref KBDLLHOOKSTRUCT lParam)
        {
            //If it's a keyboard state event...
            if (nCode == HC_ACTION)
            {
                switch (wParam)
                {
                    case WM_KEYDOWN:
                    case WM_SYSKEYDOWN: //If it's a keydown event...
                        if (KeyDown != null) KeyDown(lParam.vkCode);
                        break;
                    case WM_KEYUP:
                    case WM_SYSKEYUP: //If it's a keyup event... 
                        if (KeyUp != null) KeyUp(lParam.vkCode);
                        break;
                }
            }
            //Next
            return CallNextHookEx(KeyHook, nCode, wParam, ref lParam);
        }
        /// <summary>
        /// Installs low level keyboard hook. This hook raises events every time a keyboard event occured.
        /// </summary>
        /// <remarks></remarks>
        public static void InstallHook()
        {
            KeyHookDelegate = new KeyboardProcDelegate(KeyboardProc);
            if (Environment.Version.Major >= 4)
            {
                IntPtr hInstance = LoadLibrary("user32.dll");
                KeyHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyHookDelegate, hInstance.ToInt32(), 0);
            }
            else
            {
                KeyHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyHookDelegate, Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32(), 0);
            }
            //KeyHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyHookDelegate, Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32(), 0);
        }
        /// <summary>
        /// Uninstalls the low level keyboard hook. Hooks events are stopped from being raised.
        /// </summary>
        /// <remarks></remarks>
        public static void UninstallHook()
        {
            UnhookWindowsHookEx(KeyHook);
        }

        ~KeyboardHook()
        {
            //On close it UnHooks the Hook
            UnhookWindowsHookEx(KeyHook);
        }
    }
    /// <summary>
    /// Creates a low level mouse hook.
    /// </summary>
    /// <remarks></remarks>
    public class MouseHook
    {
        #region "Constants and API's Declaring"
        public enum MouseEvents
        {
            LeftDown = 0x201,
            LeftUp = 0x202,
            LeftDoubleClick = 0x203,
            RightDown = 0x204,
            RightUp = 0x205,
            RightDoubleClick = 0x206,
            MiddleDown = 0x207,
            MiddleUp = 0x208,
            MiddleDoubleClick = 0x209,
            MouseScroll = 0x20A,
        }
        public enum MouseWheelEvents
        {
            ScrollUp = 7864320,
            ScrollDown = -7864320,
        }

        //Constants
        private const int HC_ACTION = 0;
        private const int WH_MOUSE_LL = 14;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int WM_MOUSEWHEEL = 0x20A;

        //Mouse Structures
        public struct POINT
        {
            public int x;
            public int y;
        }
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public int mouseData;
            private uint flags;
            private int time;
            private uint dwExtraInfo;
        }

        //API Functions
        [DllImport("user32.dll", CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall, SetLastError = true, EntryPoint = "SetWindowsHookExA")]
        private static extern int SetWindowsHookEx(int idHook, MouseProcDelegate lpfn, int hmod, int dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref MSLLHOOKSTRUCT lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int UnhookWindowsHookEx(int hHook);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lib);

        // Mouse Delegate
        public delegate int MouseProcDelegate(int nCode, int wParam, ref MSLLHOOKSTRUCT lParam);
        public delegate void MouseMoveEventHandler(POINT ptLocat);
        public delegate void MouseEventEventHandler(MouseEvents nComEventde);
        public delegate void WheelEventHandler(MouseWheelEvents wEvent);

        // Mouse events
        public static event MouseMoveEventHandler MouseMove;//(ByVal ptLocat As POINT)
        public static event MouseEventEventHandler MouseEvent;//(ByVal mEvent As MouseEvents)
        public static event WheelEventHandler WheelEvent;//(ByVal wEvent As MouseWheelEvents)

        //The identifyer for our MouseHook
        private static int MouseEventHook;
        //MouseHookDelegate
        private static MouseProcDelegate MouseHookDelegate;
        #endregion

        /// <summary>
        /// Installs low level mouse hook. This hook raises events every time a mouse event occured.
        /// </summary>
        /// <remarks></remarks>
        public static void InstallHook()
        {
            MouseHookDelegate = new MouseProcDelegate(MouseProc);
            if (Environment.Version.Major >= 4)
            {
                IntPtr hInstance = LoadLibrary("user32.dll");
                MouseEventHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookDelegate, hInstance.ToInt32(), 0);
            }
            else
            {
                MouseEventHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookDelegate, Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32(), 0);
            }
            //MouseEventHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookDelegate, Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32(), 0);
        }
        /// <summary>
        /// Uninstalls the low level mouse hook. Hooks events are stopped from being raised.
        /// </summary>
        /// <remarks></remarks>
        public static void UninstallHook()
        {
            UnhookWindowsHookEx(MouseEventHook);
        }

        private static int MouseProc(int nCode, int wParam, ref MSLLHOOKSTRUCT lParam)
        {
            //If it is a Mouse event
            if (nCode == HC_ACTION)
            {
                if (wParam == WM_MOUSEMOVE)
                {
                    //If the mouse is moving...
                    if (MouseMove != null) MouseMove(lParam.pt);
                }
                else if (wParam == WM_LBUTTONDOWN || wParam == WM_LBUTTONUP || wParam == WM_LBUTTONDBLCLK || wParam == WM_RBUTTONDOWN || wParam == WM_RBUTTONUP || wParam == WM_RBUTTONDBLCLK || wParam == WM_MBUTTONDOWN || wParam == WM_MBUTTONUP || wParam == WM_MBUTTONDBLCLK)
                {
                    //If a mouse button event happend...
                    if (MouseEvent != null) MouseEvent((MouseEvents)wParam);
                }
                else if (wParam == WM_MOUSEWHEEL)
                {
                    //If the wheel moved...
                    if (WheelEvent != null) WheelEvent((MouseWheelEvents)lParam.mouseData);
                }
            }
            //Calls the next hook chain
            return CallNextHookEx(MouseEventHook, nCode, wParam, ref lParam);
        }
        ~MouseHook()
        {
            UnhookWindowsHookEx(MouseEventHook);
        }
    }
}
