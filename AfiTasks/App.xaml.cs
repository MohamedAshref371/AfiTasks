using AfiTasks.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

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

            //var tasksMenu = new System.Windows.Forms.ToolStripMenuItem("قائمة المهام");
            //for (int i = 0; i < tasks.Count; i++)
            //{
            //    int idx = i;
            //    tasksMenu.DropDownItems.Add(tasks[i][0], null, (s, ev) => RunTask(idx));
            //}

            var menu = new System.Windows.Forms.ContextMenuStrip();
            for (int i = 0; i < tasks.Count; i++)
            {
                int idx = i;
                menu.Items.Add(tasks[i][0], null, (s, ev) => RunTask(idx));
            }
            menu.Items.Add("خروج", null, (s, ev) => ExitApp());
            trayIcon.ContextMenuStrip = menu;

            // دبل كليك على الأيقونة
            //trayIcon.DoubleClick += (s, ev) => ShowMainWindow();

            // افتح النافذة الأساسية أول مرة
            //ShowMainWindow();
        }

        private void RunTask(int i)
        {
            MainWindow mw = new MainWindow(tasks[i], i % 2);
            mw.Show();
        }

        //private void ShowMainWindow()
        //{
        //    if (Current.MainWindow == null)
        //    {
        //        Current.MainWindow = new MainWindow();
        //        Current.MainWindow.Closed += (s, e) => Current.MainWindow = null;
        //    }

        //    if (!Current.MainWindow.IsVisible)
        //        Current.MainWindow.Show();

        //    if (Current.MainWindow.WindowState == WindowState.Minimized)
        //        Current.MainWindow.WindowState = WindowState.Normal;

        //    Current.MainWindow.Activate();
        //}

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
