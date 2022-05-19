using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MailCollector.Kit.ServiceKit
{
    public static class ServiceInstaller
    {
        private const int StandardRightsRequired = 0xF0000;
        private const int ServiceWin32OwnProcess = 0x00000010;

        public static void Uninstall(string serviceName)
        {
            var scm = OpenScManager(ScmAccessRights.AllAccess);

            try
            {
                var service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Service not installed.");

                try
                {
                    StopService(service);
                    if (!DeleteService(service))
                        throw new ApplicationException("Could not delete service " + Marshal.GetLastWin32Error());
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        public static bool ServiceIsInstalled(string serviceName)
        {
            var scm = OpenScManager(ScmAccessRights.Connect);

            try
            {
                var service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus);

                if (service == IntPtr.Zero)
                    return false;

                CloseServiceHandle(service);
                return true;
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        public static void Install(string serviceName, string displayName, string fileName)
        {
            var scm = OpenScManager(ScmAccessRights.AllAccess);
            try
            {
                var service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);

                if (service == IntPtr.Zero)
                    service = CreateService(scm, serviceName, displayName, ServiceAccessRights.AllAccess,
                        ServiceWin32OwnProcess, ServiceBootFlag.AutoStart, ServiceError.Normal, fileName, null,
                        IntPtr.Zero, null, null, null);

                if (service == IntPtr.Zero)
                    throw new ApplicationException("Failed to install service.");
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        public static void StartService(string serviceName)
        {
            var scm = OpenScManager(ScmAccessRights.Connect);

            try
            {
                var service = OpenService(scm, serviceName,
                    ServiceAccessRights.QueryStatus | ServiceAccessRights.Start);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Could not open service.");

                try
                {
                    StartService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        public static void StopService(string serviceName)
        {
            var scm = OpenScManager(ScmAccessRights.Connect);

            try
            {
                var service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Stop);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Could not open service.");

                try
                {
                    StopService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        private static void StartService(IntPtr service)
        {
            StartService(service, 0, 0);
            var changedStatus = WaitForServiceStatus(service, ServiceState.StartPending, ServiceState.Running);
            if (!changedStatus)
                throw new ApplicationException("Unable to start service");
        }

        private static void StopService(IntPtr service)
        {
            var status = new ServiceStatus();
            ControlService(service, ServiceControl.Stop, status);
            var changedStatus = WaitForServiceStatus(service, ServiceState.StopPending, ServiceState.Stopped);
            if (!changedStatus)
                throw new ApplicationException("Unable to stop service");
        }

        public static ServiceState GetServiceStatus(string serviceName)
        {
            var scm = OpenScManager(ScmAccessRights.Connect);

            try
            {
                var service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus);
                if (service == IntPtr.Zero)
                    return ServiceState.NotFound;

                try
                {
                    return GetServiceStatus(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        private static ServiceState GetServiceStatus(IntPtr service)
        {
            var status = new ServiceStatus();

            if (QueryServiceStatus(service, status) == 0)
                throw new ApplicationException("Failed to query service status.");

            return status.DwCurrentState;
        }

        private static bool WaitForServiceStatus(IntPtr service, ServiceState waitStatus, ServiceState desiredStatus)
        {
            var status = new ServiceStatus();

            QueryServiceStatus(service, status);
            if (status.DwCurrentState == desiredStatus) return true;

            var dwStartTickCount = Environment.TickCount;
            var dwOldCheckPoint = status.DwCheckPoint;

            while (status.DwCurrentState == waitStatus)
            {
                // Do not wait longer than the wait hint. A good interval is
                // one tenth the wait hint, but no less than 1 second and no
                // more than 10 seconds.

                var dwWaitTime = status.DwWaitHint / 10;

                if (dwWaitTime < 1000) dwWaitTime = 1000;
                else if (dwWaitTime > 10000) dwWaitTime = 10000;

                Thread.Sleep(dwWaitTime);

                // Check the status again.

                if (QueryServiceStatus(service, status) == 0) break;

                if (status.DwCheckPoint > dwOldCheckPoint)
                {
                    // The service is making progress.
                    dwStartTickCount = Environment.TickCount;
                    dwOldCheckPoint = status.DwCheckPoint;
                }
                else
                {
                    if (Environment.TickCount - dwStartTickCount > status.DwWaitHint) break;
                }
            }

            return status.DwCurrentState == desiredStatus;
        }

        private static IntPtr OpenScManager(ScmAccessRights rights)
        {
            var scm = OpenScManager(null, null, rights);
            if (scm == IntPtr.Zero)
                throw new ApplicationException("Could not connect to service control manager.");

            return scm;
        }

        #region OpenSCManager

        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode,
            SetLastError = true)]
        private static extern IntPtr OpenScManager(string machineName, string databaseName,
            ScmAccessRights dwDesiredAccess);

        #endregion

        #region OpenService

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr OpenService(IntPtr hScManager, string lpServiceName,
            ServiceAccessRights dwDesiredAccess);

        #endregion

        #region CreateService

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateService(IntPtr hScManager, string lpServiceName, string lpDisplayName,
            ServiceAccessRights dwDesiredAccess, int dwServiceType, ServiceBootFlag dwStartType,
            ServiceError dwErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId,
            string lpDependencies, string lp, string lpPassword);

        #endregion

        #region CloseServiceHandle

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseServiceHandle(IntPtr hScObject);

        #endregion

        #region QueryServiceStatus

        [DllImport("advapi32.dll")]
        private static extern int QueryServiceStatus(IntPtr hService, ServiceStatus lpServiceStatus);

        #endregion

        #region DeleteService

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteService(IntPtr hService);

        #endregion

        #region ControlService

        [DllImport("advapi32.dll")]
        private static extern int ControlService(IntPtr hService, ServiceControl dwControl,
            ServiceStatus lpServiceStatus);

        #endregion

        #region StartService

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int StartService(IntPtr hService, int dwNumServiceArgs, int lpServiceArgVectors);

        #endregion

        [StructLayout(LayoutKind.Sequential)]
        private class ServiceStatus
        {
#pragma warning disable 649
#pragma warning disable 169
            public ServiceState DwCurrentState;
            public int DwServiceType;
            public int DwControlsAccepted;
            public int DwWin32ExitCode;
            public int DwServiceSpecificExitCode;
            public int DwCheckPoint;
            public int DwWaitHint;
#pragma warning restore 169
#pragma warning restore 649
        }

        [Flags]
        public enum ScmAccessRights
        {
            Connect = 0x0001,
            CreateService = 0x0002,
            EnumerateService = 0x0004,
            Lock = 0x0008,
            QueryLockStatus = 0x0010,
            ModifyBootConfig = 0x0020,
            StandardRightsRequired = 0xF0000,

            AllAccess = StandardRightsRequired | Connect | CreateService |
                        EnumerateService | Lock | QueryLockStatus | ModifyBootConfig
        }

        public enum ServiceControl
        {
            Stop = 0x00000001,
            Pause = 0x00000002,
            Continue = 0x00000003,
            Interrogate = 0x00000004,
            Shutdown = 0x00000005,
            ParamChange = 0x00000006,
            NetBindAdd = 0x00000007,
            NetBindRemove = 0x00000008,
            NetBindEnable = 0x00000009,
            NetBindDisable = 0x0000000A
        }

        [Flags]
        public enum ServiceAccessRights
        {
            QueryConfig = 0x1,
            ChangeConfig = 0x2,
            QueryStatus = 0x4,
            EnumerateDependants = 0x8,
            Start = 0x10,
            Stop = 0x20,
            PauseContinue = 0x40,
            Interrogate = 0x80,
            UserDefinedControl = 0x100,
            Delete = 0x00010000,
            StandardRightsRequired = 0xF0000,

            AllAccess = StandardRightsRequired | QueryConfig | ChangeConfig |
                        QueryStatus | EnumerateDependants | Start | Stop | PauseContinue |
                        Interrogate | UserDefinedControl
        }

        public enum ServiceError
        {
            Ignore = 0x00000000,
            Normal = 0x00000001,
            Severe = 0x00000002,
            Critical = 0x00000003
        }

        public enum ServiceBootFlag
        {
            Start = 0x00000000,
            SystemStart = 0x00000001,
            AutoStart = 0x00000002,
            DemandStart = 0x00000003,
            Disabled = 0x00000004
        }
    }
}