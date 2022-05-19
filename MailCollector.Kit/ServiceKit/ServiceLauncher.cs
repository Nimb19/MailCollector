using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MailCollector.Kit.ServiceKit
{
    public static class ServiceLauncher
    {
        private const int WmQuit = 0x12;
        private const int WmClose = 0x10;
        private const int StdOutputHandle = -11;

        public static void ReadMessages()
        {
            var message = new Message();
            while (true)
                while (GetMessage(ref message, IntPtr.Zero, 0, 0))
                {
                    if (message.SystemMessage == WmQuit || message.SystemMessage == WmClose)
                        return;
                    TranslateMessage(ref message);
                    DispatchMessage(ref message);
                }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        public static bool IsService()
        {
            return GetStdHandle(StdOutputHandle) == IntPtr.Zero;
        }

        public static bool IsSystemProcess()
        {
            return !IsService() && !IsUserProcess();
        }

        public static bool IsUserProcess()
        {
            return Environment.UserInteractive;
        }

        public static void SetServiceStatus(ServiceState state)
        {
            if (!IsService())
                return;
            var status = new ServiceStatus { currentState = (int)state };
            SetServiceStatus(Process.GetCurrentProcess().Handle, ref status);
        }

        #region WinApi

        private struct PointApi
        {
#pragma warning disable 169
#pragma warning disable 649
            public int X;
            public int Y;
#pragma warning restore 649
#pragma warning restore 169
        }

        private struct Message
        {
#pragma warning disable 169
#pragma warning disable 649
            public IntPtr Hwmd;
            public int SystemMessage;
            public IntPtr WParam;
            public IntPtr LParam;
            public uint Time;
            public PointApi Pt;
#pragma warning restore 649
#pragma warning restore 169
        }

        [DllImport("user32.dll")]
        private static extern bool GetMessage(ref Message lpMessage, IntPtr handle, uint mMsgFilterInMain,
            uint mMsgFilterMax);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool TranslateMessage(ref Message lpMessage);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int DispatchMessage(ref Message lpMessage);

        /// <summary>
        ///     Set service status
        /// </summary>
        //[DllImport("ADVAPI32.DLL", EntryPoint = "SetServiceStatus")]
        //public static extern bool SetServiceStatus(IntPtr hServiceStatus, ref ServiceStatus lpServiceStatus);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int SetServiceStatus(IntPtr hServiceStatus, ref ServiceStatus lpServiceStatus);

        #endregion
    }

    public enum ServiceState
    {
        Unknown = -1, // The state cannot be (has not been) retrieved.
        NotFound = 0, // The service is not known on the host server.
        Stopped = 1,
        StartPending = 2,
        StopPending = 3,
        Running = 4,
        ContinuePending = 5,
        PausePending = 6,
        Paused = 7
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int serviceType;
        public int currentState;
        public int controlsAccepted;
        public int win32ExitCode;
        public int serviceSpecificExitCode;
        public int checkPoint;
        public int waitHint;
    }
}
