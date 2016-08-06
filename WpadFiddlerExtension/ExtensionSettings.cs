using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

using WpadDhcpServerLibrary;

namespace WpadFiddlerExtension
{
    class Settings
    {
        private const string _prefPrefix = "net.deletethis.wpadserver.";
        private const string _defaultListValue = "$default$";

        static Settings sSettings = new Settings();
        public static Settings Get() { return sSettings; }

        public bool WpadDhcpServerOn = false;
        public WpadDhcpServer Server = new WpadDhcpServer();

        public Settings()
        {
        }

        ~Settings()
        {
            SaveSettings();
        }

        public void OnLoad()
        {
            _LoadSettings();
        }

        private void _LoadSettings()
        {
            PacSetting = (PacSettingValues)Fiddler.FiddlerApplication.Prefs.GetInt32Pref(_prefPrefix + "PacSetting", (int)PacSettingValues.LocalFiddler);
            IPList = (IPListValues)Fiddler.FiddlerApplication.Prefs.GetInt32Pref(_prefPrefix + "IPList", (int)IPListValues.AllowList);
            AllowIPList = _GetListFromString(Fiddler.FiddlerApplication.Prefs.GetStringPref(_prefPrefix + "AllowIPList", _defaultListValue));
            DenyIPList = _GetListFromString(Fiddler.FiddlerApplication.Prefs.GetStringPref(_prefPrefix + "DenyIPList", ""));

            string remotePacUrlStr = Fiddler.FiddlerApplication.Prefs.GetStringPref(_prefPrefix + "RemotePacUrl", "");
            RemotePacUrl = remotePacUrlStr == "" ? null : new Uri(remotePacUrlStr);

            string localPacFileStr = Fiddler.FiddlerApplication.Prefs.GetStringPref(_prefPrefix + "LocalPacFile", "");
            LocalPacFile = localPacFileStr == "" ? null : new FileInfo(localPacFileStr);

            // EricLaw wants - Always start as off:
            Running = false; //  Fiddler.FiddlerApplication.Prefs.GetBoolPref(_prefPrefix + "Running", true);
        }

        public void SaveSettings()
        {
            Fiddler.FiddlerApplication.Prefs.SetInt32Pref(_prefPrefix + "PacSetting", (int)PacSetting);
            Fiddler.FiddlerApplication.Prefs.SetInt32Pref(_prefPrefix + "IPList", (int)IPList);
            Fiddler.FiddlerApplication.Prefs.SetStringPref(_prefPrefix + "AllowIPList", _GetStringFromList(AllowIPList));
            Fiddler.FiddlerApplication.Prefs.SetStringPref(_prefPrefix + "DenyIPList", _GetStringFromList(DenyIPList));
            Fiddler.FiddlerApplication.Prefs.SetStringPref(_prefPrefix + "RemotePacUrl", RemotePacUrl == null ? "" : RemotePacUrl.ToString());
            Fiddler.FiddlerApplication.Prefs.SetStringPref(_prefPrefix + "LocalPacFile", LocalPacFile == null ? "" : LocalPacFile.FullName);
            Fiddler.FiddlerApplication.Prefs.SetBoolPref(_prefPrefix + "Running", Running);
        }

        public void ClearSettings()
        {
            Fiddler.FiddlerApplication.Prefs.RemovePref(_prefPrefix + "PacSetting");
            Fiddler.FiddlerApplication.Prefs.RemovePref(_prefPrefix + "IPList");
            Fiddler.FiddlerApplication.Prefs.RemovePref(_prefPrefix + "AllowIPList");
            Fiddler.FiddlerApplication.Prefs.RemovePref(_prefPrefix + "DenyIPList");
            Fiddler.FiddlerApplication.Prefs.RemovePref(_prefPrefix + "RemotePacUrl");
            Fiddler.FiddlerApplication.Prefs.RemovePref(_prefPrefix + "LocalPacFile");
            Fiddler.FiddlerApplication.Prefs.RemovePref(_prefPrefix + "Running");
        }

        private void _GetLocalIPAddress(ref IPAddress ipAddr, ref IPAddress ipMask)
        {
            foreach (NetworkInterface netInt in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInt.OperationalStatus == OperationalStatus.Up &&
                    netInt.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    netInt.GetIPv4Statistics().BytesReceived > 0)
                {
                    foreach (UnicastIPAddressInformation ipAddrInfo in netInt.GetIPProperties().UnicastAddresses)
                    {
                        if (ipAddrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddr = ipAddrInfo.Address;
                            ipMask = ipAddrInfo.IPv4Mask;
                        }
                    }
                }
            }
        }

        private List<IPAddress> _GetLocalIPAddresses()
        {
            List<IPAddress> ipAddresses = new List<IPAddress>();
            foreach (NetworkInterface netInt in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInt.OperationalStatus == OperationalStatus.Up &&
                    netInt.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    netInt.GetIPv4Statistics().BytesReceived > 0)
                {
                    foreach (UnicastIPAddressInformation ipAddrInfo in netInt.GetIPProperties().UnicastAddresses)
                    {
                        if (ipAddrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddresses.Add(ipAddrInfo.Address);
                        }
                    }
                }
            }
            return ipAddresses;
        }

        private List<IPAddress> _GetListFromString(string ipListStr)
        {
            List<IPAddress> ipList = new List<IPAddress>();
            if (ipListStr == _defaultListValue)
            {
                //IPAddress ipAddr = null, ipMask = null;
                //_GetLocalIPAddress(ref ipAddr, ref ipMask);
                //ipList.Add(ipAddr);

                List<IPAddress> localIpAddrs = _GetLocalIPAddresses();
                ipList.AddRange(localIpAddrs);
            }
            else
            {
                string[] entries = ipListStr.Split(';');
                foreach (string entry in entries)
                {
                    if (entry != null && entry.Trim() != "")
                    {
                        ipList.Add(IPAddress.Parse(entry));
                    }
                }
            }
            return ipList;
        }

        private string _GetStringFromList(List<IPAddress> ipList)
        {
            string ipListStr = "";
            for (int idx = 0; idx < ipList.Count; ++idx)
            {
                ipListStr += ipList[idx].ToString();
                if (idx + 1 < ipList.Count)
                {
                    ipListStr += ";";
                }
            }
            return ipListStr;
        }

        public enum IPListValues
        {
            NoList,
            AllowList,
            DenyList
        };
        public IPListValues IPList = IPListValues.NoList;
        public List<IPAddress> AllowIPList = new List<IPAddress>();
        public List<IPAddress> DenyIPList = new List<IPAddress>();

        public enum PacSettingValues
        {
            LocalFiddler,
            RemotePacUrl,
            LocalPacFile
        };
        private PacSettingValues mPacSetting = PacSettingValues.LocalFiddler;
        public PacSettingValues PacSetting
        {
            get { return mPacSetting; }
            set
            {
                mPacSetting = value;
                switch (mPacSetting)
                {
                    case PacSettingValues.RemotePacUrl:
                        if (RemotePacUrl == null)
                            throw new ApplicationException("RemotePacUrl must be set to use the PacSettingValues.RemotePacUrl value for PacSetting.");
                        Server.PacUri = RemotePacUrl;
                        break;

                    default:
                        string host = IPGlobalProperties.GetIPGlobalProperties().HostName.ToLowerInvariant();
                        int port = Fiddler.FiddlerApplication.oProxy.ListenPort;
                        Server.PacUri = new Uri("http://" + host + ":" + port + "/wpad.dat");
                        break;
                }
            }
        }

        public bool Running
        {
            get { return Server.Running; }
            set
            {
                if (value && !Server.Running)
                {
                    Log.Get().AddLogEntry(Log.Entry.EntryType.Note, "Starting WPAD DHCP server.");
                    Server.Start();
                }
                else if (!value && Server.Running)
                {
                    Log.Get().AddLogEntry(Log.Entry.EntryType.Note, "Stopping WPAD DHCP server.");
                    Server.Stop();
                }
            }
        }

        public Uri RemotePacUrl = null;
        public FileInfo LocalPacFile = null;
    }
}