using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using WpadDhcpServerLibrary;

/*
 * On resetting IE's WPAD info for testing out the server:

In my test automation, I do a few things:
-	Clear the SWPAD cache (internet settings\wpad)
-	Clear the TIF to make sure that wpad.dat isn’t cached
-	Call InternetSetOption to set autoproxy on. At least on Vista, this seems to invalidate the cached wpad state in the connection settings reg key.

Actually, I primarily wrote this automation for Vista, so I’m not sure if this alone is sufficient to invalidate wpad on win7. Deleting the connection settings reg key, then calling InternetSetOption to turn on autoproxy is certainly going to work.
 * 
 * 
 *
 * {
                AutoProxyHelper.SetAutoDetectSettings();
                swpadRegHelpers.ResetWpadRegState();
                swpadRegHelpers.SetWpadWaitTime();
                swpadRegHelpers.SetWpadDetectType(2); // 2 = DNS Only

                NativeMethods.InternetSetOptionA(IntPtr.Zero, (int)HIOption.INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
 * }

 *             public static void ResetWpadRegState()
            {
                RemoveSavedConnections();
                RemoveSwpadZones();
                DeleteWaitTime();
                DeleteDetectType();
            }
 * 
        public static void SetAutoDetectSettings(WinInetSafeHandle handle)
        {
            Log.Trace("Setting autoproxy settings to \"auto-detect\"");
        
            INTERNET_PER_CONN_OPTION[] ConOptions = new INTERNET_PER_CONN_OPTION[1];

            ConOptions[0] = new INTERNET_PER_CONN_OPTION();
            ConOptions[0].dwOption = (Int32)INTERNET_PER_CONN_OPTION_FLAGS.INTERNET_PER_CONN_FLAGS;
            ConOptions[0].Value.dwValue = (Int32)PER_CONN_FLAGS.PROXY_TYPE_AUTO_DETECT;

            SetPerConnectionOptions(handle, ConOptions);
        }

*/

namespace WpadDhcpServerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            bool needsHelp = true;
            Program program = new Program();

            if (args.Length == 2)
            {
                switch (args[0])
                {
                    case "--pacUrl":
                        program.SetPacUrl(args[1]);
                        needsHelp = false;
                        break;

                    default:
                        break;
                }
            }
            
            if (!needsHelp)
            {
                program.Run();
            }
            else
            {
                Console.Out.WriteLine(
                    "Runs a simple WPAD DHCP server.\n" +
                    "Usage:\n" +
                    "\tWpadDhcpServer --pacUrl [PAC URL]\n" +
                    "Example:\n" +
                    "\tWpadDhcpServer --pacUrl \"http://192.168.1.122:9870/wpad.dat\"");
            }
        }

        protected WpadDhcpServer sServer;

        public Program()
        {
            sServer = new WpadDhcpServer();
            sServer.OnClientDhcpWpadRequest = OnClientDhcpWpadRequest;
            sServer.OnException = OnException;
        }

        public void SetPacUrl(string pacUrl)
        {
            sServer.PacUri = new Uri(pacUrl);
        }

        public void Run()
        {
            sServer.Start();
            Console.Out.WriteLine("Now listening for WPAD DHCP requests.  Press enter to end.");
            Console.In.ReadLine();
            sServer.Stop();
            Console.Out.WriteLine("Terminating...");
        }

        protected void OnClientDhcpWpadRequest(IPEndPoint ipEndPoint, ref bool handle)
        {
            handle = true;
            Console.Out.WriteLine(
                "DHCP Request:\t" + 
                ipEndPoint.Address + ":" + ipEndPoint.Port);
        }

        protected void OnException(Exception exception)
        {
            Console.Out.WriteLine("Server error: " + exception.ToString());
            Console.Out.WriteLine("Restarting server...");
            sServer.Start();
        }
    }
}