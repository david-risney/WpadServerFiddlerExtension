using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Fiddler;

using System.IO;
using System.Net;
using System.Net.NetworkInformation;

using System.Threading;

using WpadDhcpServerLibrary;

// done: switch to .NET 2.0 library if easy/possible
// done: serialize settings in and out on shutdown and startup using FiddlerApp.Prefs
// done: log also should go to FiddlerApp.Log (?) check the wiki
// done: default to allow list with local machine only in the list
// done: fix add button
// warning for bad fiddler settings - IPv6 (?), allow remote connections (!)
// if my WPAD DHCP response comes in before my router's DHCP reponse do I win?
//  - build release
//  - perform everything possible before run between recieving DHCP request and sending response
//  - switch to using only a signle byte buffer for both request and response.
// if I fill in extra params from my router's DHCP response will my DHCP WPAD response be respected even if it comes after the router's DHCP response?
//  done: create dhcp wpad client app
//  done: tttrace the dhcp client server
//  done: debuging trace...
//    0:002> k
//    ChildEBP RetAddr  
//    0164efa4 72824de7 dhcpcore!GetSpecifiedDhcpMessage+0x1e [d:\w7rtm\minio\dhcp\client\nt\dhcpmsg.c @ 962]
//    0164f060 7282e72a dhcpcore!SendInformAndGetReplies+0x19a [d:\w7rtm\minio\dhcp\client\nt\protocol.c @ 1559]
//    0164f080 7281ab15 dhcpcore!DhcpDoInform+0x90 [d:\w7rtm\minio\dhcp\client\nt\apiimpl.c @ 785]
//    0164f0d4 728032dc dhcpcore!DhcpSendInformIfRequired+0x29a [d:\w7rtm\minio\dhcp\client\nt\apiimpl.c @ 1319]
//    0164f128 72803250 dhcpcore!RequestParamsDetailed+0x2f [d:\w7rtm\minio\dhcp\client\nt\apiimpl.c @ 1425]
//    0164f184 72803066 dhcpcore!RequestParams+0x329 [d:\w7rtm\minio\dhcp\client\nt\apiimpl.c @ 1727]
//    0164f1b8 7584fc8f dhcpcore!RpcSrvRequestParams+0x135 [d:\w7rtm\minio\dhcp\client\nt\apistub.c @ 744]
//    0164f1e4 758b4c53 rpcrt4!Invoke+0x2a [d:\w7rtm\minio\rpc\ndr20\i386\invoke.asm @ 86]
//    Looks like we can't send a followup DHCP response at least not with my home router.
//    
// what's the deal with /wspad.dat?  Should I serve it?   Send 404 to get it to stop?  What app is sending it?
namespace WpadFiddlerExtension
{
    public class FiddlerExtension : IAutoTamper
    {
        private void Server_OnException(Exception exception)
        {
            Log.Get().AddLogEntry(Log.Entry.EntryType.Error, exception.ToString(), exception);
        }

        private void Server_OnClientDhcpWpadRequest(IPEndPoint clientIpEndPoint, ref bool handleRequest)
        {
            handleRequest = ShouldHandleRequest(clientIpEndPoint);
            Log.Get().AddLogEntry(Log.Entry.EntryType.Note,
                (handleRequest ? "Replied to " : "Ignored ") + "WPAD DHCP request from " + clientIpEndPoint,
                clientIpEndPoint.Address);
        }

        private bool ShouldHandleRequest(IPEndPoint clientIpEndPoint)
        {
            bool handle = true;
            switch (Settings.Get().IPList)
            {
                case Settings.IPListValues.AllowList:
                    handle = Settings.Get().AllowIPList.Contains(clientIpEndPoint.Address);
                    break;

                case Settings.IPListValues.DenyList:
                    handle = !Settings.Get().DenyIPList.Contains(clientIpEndPoint.Address);
                    break;
            }
            return handle;
        }

        private void OnWpadMenuItemClick(object sender, EventArgs e)
        {
            Form settingDialog = new Form();
            ExtensionSettingsDialog extensionSettingsDialog = new ExtensionSettingsDialog();
            settingDialog.Controls.Add(extensionSettingsDialog);
            settingDialog.ClientSize = extensionSettingsDialog.Size;
            settingDialog.Text = "WPAD Server Settings";
            settingDialog.ShowIcon = false;
            settingDialog.ShowInTaskbar = false;
            settingDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            settingDialog.MinimizeBox = false;
            settingDialog.MaximizeBox = false;
            settingDialog.ShowDialog();
        }

        private void _AddMenuItem()
        {
            MenuItem newMenuItem = new MenuItem();
            newMenuItem.Text = "WPAD Server Settings...";
            newMenuItem.Click += new EventHandler(OnWpadMenuItemClick);

            MainMenu menu = FiddlerApplication.UI.mnuMain;
            foreach (MenuItem menuItem in menu.MenuItems)
            {
                if (menuItem.Text.Contains("Tools"))
                {
                    menuItem.MenuItems.Add(newMenuItem);
                    newMenuItem = null;
                    break;
                }
            }
            if (newMenuItem != null)
            {
                menu.MenuItems.Add(newMenuItem);
            }
        }

        private ExtensionSettingsDialog mExtensionsSettingsDialog = null;
        public void _AddFiddlerTab()
        {
            TabPage page = new TabPage("WPAD Server Settings");
            mExtensionsSettingsDialog = new ExtensionSettingsDialog();
            mExtensionsSettingsDialog.Dock = DockStyle.Fill;
            page.Controls.Add(mExtensionsSettingsDialog);
            FiddlerApplication.UI.tabsViews.TabPages.Add(page);
        }

        // Called when Fiddler User Interface is fully available
        public void OnLoad()
        {
            _Load();
            // EricLaw change - use tabs instead of a modal dialog:
            // _AddMenuItem();
            _AddFiddlerTab();
            Settings.Get().Server.OnClientDhcpWpadRequest = Server_OnClientDhcpWpadRequest;
            Settings.Get().Server.OnException = Server_OnException;

            //(new Thread(_Load)).Start();
            
        }

        private void _Load()
        {
            //Thread.Sleep(2000);
            Settings.Get().OnLoad();
        }

        // Called when Fiddler is shutting down
        public void OnBeforeUnload()
        {
            if (mExtensionsSettingsDialog != null)
            {
                mExtensionsSettingsDialog.SaveAndClose();
            }
            if (Settings.Get().Server != null)
            {
                Settings.Get().Server.Stop();
            }
        }

        // Called before the user can edit a request using the Fiddler Inspectors
        public void AutoTamperRequestBefore(Session oSession)
        {
            if (Settings.Get().PacSetting != Settings.PacSettingValues.RemotePacUrl)
            {
                if (oSession.PathAndQuery.ToLowerInvariant() == "/wpad.dat" || oSession.PathAndQuery.ToLowerInvariant().StartsWith("/wpad.dat?"))
                {
                    Log.Get().AddLogEntry(Log.Entry.EntryType.Note, "Responding to wpad.dat HTTP request from " + oSession.clientIP + ":" + oSession.clientPort);
                    oSession.utilCreateResponseAndBypassServer();
                    oSession.oResponse.headers.HTTPResponseCode = 200;
                    oSession.oResponse.headers.HTTPResponseStatus = "200 OK";
                    oSession.oResponse.headers["Content-Type"] = "application/x-ns-proxy-autoconfig";
                    oSession.oResponse.headers["Connection"] = "close";
                    oSession.oResponse.headers.Remove("Content-Length");

                    byte[] bytes = null;
                    switch (Settings.Get().PacSetting)
                    {
                        default:
                            throw new ApplicationException("Cannot access wpad.dat without the WPAD server configured for it.");

                        case Settings.PacSettingValues.LocalFiddler:
                            string host = IPGlobalProperties.GetIPGlobalProperties().HostName.ToLowerInvariant();
                            int port = Fiddler.FiddlerApplication.oProxy.ListenPort;
                            bytes = Encoding.UTF8.GetBytes("function FindProxyForURL(url, host) { return \"PROXY " + host + ":" + port + "; DIRECT\"; }");
                            break;

                        case Settings.PacSettingValues.LocalPacFile:
                            System.IO.FileStream stream = new System.IO.FileStream(Settings.Get().LocalPacFile.FullName, System.IO.FileMode.Open);
                            long length = stream.Length;
                            bytes = new byte[length];
                            stream.Read(bytes, 0, (int)length);
                            break;
                    }
                    oSession.responseBodyBytes = bytes;
                    oSession.bBufferResponse = true;
                    oSession.state = SessionStates.ReadingResponse;
                }
                else if (oSession.PathAndQuery.ToLowerInvariant() == "/wspad.dat" || oSession.PathAndQuery.ToLowerInvariant().StartsWith("/wspad.dat?"))
                {
                    Log.Get().AddLogEntry(Log.Entry.EntryType.Note, "No good response to wspad.dat HTTP request from " + oSession.clientIP + ":" + oSession.clientPort);
                    oSession.utilCreateResponseAndBypassServer();
                    oSession.oResponse.headers.HTTPResponseCode = 404;
                    oSession.oResponse.headers.HTTPResponseStatus = "404 Not Found";
                    oSession.oResponse.headers["Content-Type"] = " text/html";
                    oSession.oResponse.headers["Connection"] = "close";
                    oSession.oResponse.headers.Remove("Content-Length");

                    oSession.responseBodyBytes = Encoding.UTF8.GetBytes(
                        "I don't do wspad.dat.  I'm not sure how it works.  Maybe in a future version.  Sorry."
                        );
                    oSession.bBufferResponse = true;
                    oSession.state = SessionStates.ReadingResponse;
                }
            }
        }

        // Called after the user has had the chance to edit the request using the Fiddler Inspectors, but before the request is sent
        public void AutoTamperRequestAfter(Session oSession)
        {
        }

        // Called before the user can edit a response using the Fiddler Inspectors, unless streaming.
        public void AutoTamperResponseBefore(Session oSession)
        {
        }

        // Called after the user edited a response using the Fiddler Inspectors.  Not called when streaming.
        public void AutoTamperResponseAfter(Session oSession)
        {
        }

        // Called Fiddler returns a self-generated HTTP error (for instance DNS lookup failed, etc)
        public void OnBeforeReturningError(Session oSession)
        {
        }
    }
}