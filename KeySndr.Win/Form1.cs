using System;
using System.Diagnostics;
using System.Windows.Forms;
using KeySndr.Base;
using KeySndr.Base.Providers;
using KeySndr.Win.Events;
using KeySndr.Win.Providers;

namespace KeySndr.Win
{
    public partial class Form1 : Form
    {
        public delegate void StringEntryDelegate(string text);
        public delegate void ExceptionEntryDelegate(string text, Exception e);

        private readonly KeySndrApp app;
        private LoggingProvider loggingProvider;

        public Form1(KeySndrApp a)
        {
            app = a;
            InitializeComponent();
            SetupNotifyIcon();
            SetupLogging();
            SetupInterface();
            WriteWelcome();
        }

        private void WriteWelcome()
        {
            AddEntry("KeySndr Win version " + AssembyInfo.GetAssemblyVersion(typeof(Form1)).ToString());
            AddEntry("KeySndr Base version " + AssembyInfo.GetAssemblyVersion(typeof(KeySndrApp)).ToString());

            var appConfigProvider = ObjectFactory.GetProvider<IAppConfigProvider>();
            var appConfig = appConfigProvider.AppConfig;
            AddEntry($"Admin interface at http://localhost:{appConfig.LastPort}/manage/index.html");
        }

        private void SetupInterface()
        {
            Text += $" {AssembyInfo.GetAssemblyVersion(typeof (Form1))}";
            Text += $" ({AssembyInfo.GetAssemblyVersion(typeof (KeySndrApp))})";
        }

        private void SetupLogging()
        {
            loggingProvider = (LoggingProvider)ObjectFactory.GetProvider<ILoggingProvider>();
            loggingProvider.OnLogEntry += OnLogEntry;
        }

        private void OnLogEntry(object sender, LogEntryEventArgs logEntryEventArgs)
        {
            var args = logEntryEventArgs as ErrorLogEntryEventArgs;
            if (args == null)
            {
                AddEntry(logEntryEventArgs.Message);
                return;
            }
            AddErrorLogEntry(args.Message, args.Exception);
        }

        private void SetupNotifyIcon()
        {
            notifyIcon.Text = Text;
            notifyIcon.Visible = true;
        }

        private void AddEntry(string m)
        {
            if (textBoxLog.InvokeRequired)
            {
                textBoxLog.BeginInvoke(new StringEntryDelegate(AddEntry), m);
                return;
            }
            textBoxLog.AppendText(m);
            textBoxLog.AppendText(Environment.NewLine);
        }

        private void AddErrorLogEntry(string m, Exception e)
        {
            if (textBoxLog.InvokeRequired)
            {
                textBoxLog.BeginInvoke(new ExceptionEntryDelegate(AddErrorLogEntry), m, e);
                return;
            }
            textBoxLog.AppendText(m);
            textBoxLog.AppendText(e.Message);
            textBoxLog.AppendText(e.StackTrace);
            textBoxLog.AppendText(Environment.NewLine);
        }

        private void Exit()
        {
            loggingProvider.OnLogEntry -= OnLogEntry;
            app.StopAll();
        }

        private void ReloadAll()
        {
            app.ReloadAll();
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            ReloadAll();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadAll();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void reloadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReloadAll();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Exit();
        }

        private void buttonOpenAdmin_Click(object sender, EventArgs e)
        {
            OpenAdmin();
        }

        private void openAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAdmin();
        }

        private void OpenAdmin()
        {
            var appConfigProvider = ObjectFactory.GetProvider<IAppConfigProvider>();
            var appConfig = appConfigProvider.AppConfig;
            var sInfo = new ProcessStartInfo($"http://localhost:{appConfig.LastPort}/manage/index.html");
            Process.Start(sInfo);
        }
    }
}
