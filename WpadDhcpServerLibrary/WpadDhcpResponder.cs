using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace WpadDhcpServerLibrary
{
    class WpadDhcpResponder
    {
        public static void RespondToRequest(
            WpadDhcpServer wpadServer,
            UdpClient udpClient,
            IPEndPoint clientEndPoint,
            IPAddress ipAddr,
            DhcpRequest request)
        {
            DhcpResponse response = new DhcpResponse();
            byte[] responseData = null;

            response.Fill(request);
            response.ApplyWpadOption(wpadServer.PacUri, ipAddr);

            responseData = response.ToByteArray();
            udpClient.Send(responseData, responseData.Length, clientEndPoint);
        }
    }
}