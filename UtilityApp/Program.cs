using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilityApp
{
    class Program
    {
        static KeyCatch keyLog = new KeyCatch();
        static NotifyIcon tray = new NotifyIcon();
        static System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
        static System.Windows.Forms.MenuItem menuItem = new System.Windows.Forms.MenuItem();

        static void WL(string line)
        {
            Console.WriteLine(line);
            return;
        }

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

    }
}
