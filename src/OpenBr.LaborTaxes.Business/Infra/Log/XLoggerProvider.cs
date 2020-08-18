using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Business.Infra.Log
{
    public class XLoggerProvider : LoggerProvider
    {

        bool terminated;
        internal XLoggerOptions Settings { get; private set; }
        ConcurrentQueue<LogEntry> InfoQueue = new ConcurrentQueue<LogEntry>();

        public XLoggerProvider(IOptionsMonitor<XLoggerOptions> Settings) : this(Settings.CurrentValue)
        {
            SettingsChangeToken = Settings.OnChange(settings => {
                this.Settings = settings;
            });
        }

        public XLoggerProvider(XLoggerOptions settings)
        {
            this.Settings = settings;
            ThreadProc();
        }

        void WriteLogLine()
        {
            LogEntry info = null;
            if (InfoQueue.TryDequeue(out info))
            {
                string json = JsonSerializer.Serialize(info, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNameCaseInsensitive = true, WriteIndented = true });
                Console.WriteLine($"JSON Log: {json}");
            }
        }

        void ThreadProc()
        {
            Task.Run(() => {

                while (!terminated)
                {
                    try
                    {
                        WriteLogLine();
                        System.Threading.Thread.Sleep(100);
                    }
                    catch // (Exception ex)
                    {
                    }
                }
            });
        }

        public override bool IsEnabled(LogLevel logLevel)
        {

            bool Result = 
                logLevel != LogLevel.None
                && Settings.LogLevel != LogLevel.None
                && Convert.ToInt32(logLevel) >= Convert.ToInt32(Settings.LogLevel);

            return Result;

        }

        public override void WriteLog(LogEntry info)
        {
            InfoQueue.Enqueue(info);
        }

        #region IDisposable Support

        protected override void Dispose(bool disposing)
        {
            terminated = true;
            base.Dispose(disposing);
        }

        #endregion

    }
}
