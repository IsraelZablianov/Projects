using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;

namespace MailSenderService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public long dwServiceType;
        public ServiceState dwCurrentState;
        public long dwControlsAccepted;
        public long dwWin32ExitCode;
        public long dwServiceSpecificExitCode;
        public long dwCheckPoint;
        public long dwWaitHint;
    };

    public partial class MailSenderService : ServiceBase
    {
        private EventLog mailServiceEventLog;
        private int eventId = 0;

        public MailSenderService(string[] args)
        {
            InitializeComponent();
            var eventSourceName = "MailServiceSource";
            var logName = "MailServiceNewLog";
            if (args.Count() > 0)
                eventSourceName = args[0];
            if (args.Count() > 1)
                logName = args[1];
            mailSenderEventLog = new EventLog();
            if (!EventLog.SourceExists(eventSourceName))
                EventLog.CreateEventSource(eventSourceName, logName);
            mailSenderEventLog.Source = eventSourceName;
            mailSenderEventLog.Log = logName;
        }


        protected override void OnStart(string[] args)
        {
            var serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(ServiceHandle, ref serviceStatus);

            mailSenderEventLog.WriteEntry("--In OnStart--");

            // Set up a timer to trigger every minute.
            var timer = new Timer { Interval = 60000 };// 60 seconds  
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(ServiceHandle, ref serviceStatus);


        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            try
            {
                var auctionsScedualJob = new AuctionsScedualJob();
                auctionsScedualJob.DeleteEndedAuctions();
            }
            catch (Exception ex)
            {
                mailSenderEventLog.WriteEntry(ex.Message, EventLogEntryType.Information, eventId++);

            }
        }

        protected override void OnStop()
        {
            mailSenderEventLog.WriteEntry("In onStop.");
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

    }
}
