using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USBLock;

namespace USBLockCli
{
    class Program
    {
        private static void PrintUsage()
        {
            System.Console.WriteLine("{0} [/ON] [/OFF] [/TOGGLE] [/SHOW] \r\n", System.AppDomain.CurrentDomain.FriendlyName);
            System.Console.WriteLine("\t /ON    \t Enable USB removable storage");
            System.Console.WriteLine("\t /OFF   \t Disable USB removable storage");
            System.Console.WriteLine("\t /TOGGLE\t Toggle USB removable storage status");
            System.Console.WriteLine("\t /SHOW  \t Show USB removable storage status");

        }
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                PrintUsage();
            }
            else
            {
                USBStorageStatus StorageStatus = new USBStorageStatus();
                bool CurrentStatus = StorageStatus.getUSBStorageStatus();

                switch (args[0].ToLower()){
                    case "/on":
                        StorageStatus.toggleStatus(true);
                        break;
                    case "/off":
                        StorageStatus.toggleStatus(true);
                        break;
                    case "/toggle":
                        StorageStatus.toggleStatus(!CurrentStatus);
                        break;
                    case "/show":
                        if (CurrentStatus) {
                            System.Console.WriteLine("on");
                            Environment.Exit(0);
                           
                        } else
                        {
                            System.Console.WriteLine("off");
                            Environment.Exit(1);

                        }
                        break;
                    default:
                        PrintUsage();
                        break;
                }
            }
           
           
            
        }
    }
}
