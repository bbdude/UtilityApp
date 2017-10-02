using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityApp
{
    class Program
    {
        static KeyCatch keyLog = new KeyCatch();

        static void WL(string line)
        {
            Console.WriteLine(line);
            return;
        }
        
        static void Main(string[] args)
        {
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
