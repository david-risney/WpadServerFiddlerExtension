using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace WpadDhcpServerLibrary
{
    public class WpadDhcpServer
    {
        public Uri PacUri
        {
            set
            {
                if (Encoding.UTF8.GetBytes(value.ToString()).Length > 0xFF)
                {
                    throw new ApplicationException("WPAD URL must not be longer than 0xFF characters.");
                }
                mPacUri = value;
            }
            get { return mPacUri; }
        }

        public bool Running { get { return mRunning; } }

        public delegate void ClientDhcpWpadRequest(IPEndPoint clientIpEndPoint, ref bool handleRequest);
        public ClientDhcpWpadRequest OnClientDhcpWpadRequest;

        public delegate void ExceptionHandler(Exception exception);
        public ExceptionHandler OnException;

        public delegate void RunningChangedHandler(bool running);
        public RunningChangedHandler OnRunningChanged;

        private Uri mPacUri = null;
        private UdpClient mUdpClient = null;
        private bool mDone = false;
        private bool mRunning = false;

        public void Start()
        {
            _ValidateParameters();

            if (!mRunning)
            {
                mDone = false;
                (new Thread(_Start)).Start();
            }
        }

        public void Stop()
        {
            if (mRunning)
            {
                mDone = true;
                if (mUdpClient != null)
                {
                    mUdpClient.Close();
                }
            }
        }

        private IPAddress _GetLocalIPAddress()
        {
            IPAddress ipAddr = null;

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
                            break;
                        }
                    }
                }
            }

            return ipAddr;
        }

        private void _ValidateParameters()
        {
            if (mPacUri == null)
            {
                throw new ApplicationException("WPAD URL required.");
            }
        }

        private void _Start()
        {
            try
            {
                mRunning = true;
                if (OnRunningChanged != null)
                    OnRunningChanged(mRunning);

                IPAddress ipAddr = _GetLocalIPAddress();
                
                while (!mDone)
                {
                    mUdpClient = new UdpClient(new IPEndPoint(IPAddress.Any, DhcpMessage.sServerPort));
                    IPEndPoint clientEndPoint = null;
                    byte[] data = mUdpClient.Receive(ref clientEndPoint);
                    _OnDataReceived(mUdpClient, data, clientEndPoint, ipAddr);
                    mUdpClient.Close();
                    mUdpClient = null;
                }
                mRunning = false;
                if (OnRunningChanged != null)
                    OnRunningChanged(mRunning);
            }
            catch (Exception e)
            {
                mRunning = false;
                Stop();
                if (OnRunningChanged != null)
                    OnRunningChanged(mRunning);
                if (OnException != null)
                    OnException(e);
            }
        }

        private DhcpRequest mRequest = new DhcpRequest();

        private void _OnDataReceived(UdpClient udpServer, byte[] data, IPEndPoint clientEndPoint, IPAddress ipAddr)
        {
            mRequest.SetData(data);

            if (mRequest.IsValidWpadRequest)
            {
                bool handle = true;
                if (OnClientDhcpWpadRequest != null)
                    OnClientDhcpWpadRequest(clientEndPoint, ref handle);

                if (handle)
                {
                    RespondToRequest(udpServer, clientEndPoint, ipAddr, mRequest);
                }
            }
        }

        private DhcpResponse mResponse = new DhcpResponse();

        private void RespondToRequest(
            UdpClient udpClient,
            IPEndPoint clientEndPoint,
            IPAddress ipAddr,
            DhcpRequest request)
        {
            mResponse.Fill(request);
            mResponse.ApplyWpadOption(PacUri, ipAddr);

            byte[] responseData = mResponse.ToByteArray();
            udpClient.Send(responseData, responseData.Length, clientEndPoint);
        }
    }
}