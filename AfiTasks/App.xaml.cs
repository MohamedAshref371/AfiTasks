using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace AfiTasks
{
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon trayIcon;
        private List<string[]> tasks;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            trayIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = AfiTasks.Properties.Resources.board,
                Visible = true,
                Text = "AfiTasks"
            };

            if (!System.IO.File.Exists("Tasks.txt"))
            {
                MessageBox.Show("ملف Tasks.txt غير موجود", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                ExitApp();
            }

            string[] arr = System.IO.File.ReadAllLines("Tasks.txt").Select(s => s.Trim()).Where(s => s != "").ToArray();
            tasks = new List<string[]>();
            List<string> current = new List<string>();

            foreach (var item in arr)
            {
                if (item.Contains("***"))
                {
                    if (current.Count > 0) tasks.Add(current.ToArray());
                    current.Clear();
                }
                else
                    current.Add(item);
            }

            if (current.Count > 0) tasks.Add(current.ToArray());

            var menu = new System.Windows.Forms.ContextMenuStrip();
            for (int i = 0; i < tasks.Count; i++)
            {
                int idx = i;
                menu.Items.Add(tasks[i][0], null, (s, ev) => RunTask(idx));
            }
            menu.Items.Add("خروج", null, (s, ev) => ExitApp());
            trayIcon.ContextMenuStrip = menu;
        }

        private void RunTask(int i)
        {
            MainWindow mw = new MainWindow(tasks[i], i % 2);
            mw.Show();
        }

        private void ExitApp()
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
            Current.Shutdown();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
        }
    }
}
