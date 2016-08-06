using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.IO;

namespace WpadDhcpServerLibrary
{
    class DhcpMessage
    {
        public const int sServerPort = 67;  // Port the server sends and receives on
        public const int sClientPort = 68;  // Port the client sends and recieves on

        protected enum DhcpMessageType
        {
            DHCPDISCOVER = 1,
            DHCPOFFER = 2,
            DHCPREQUEST = 3,
            DHCPDECLINE = 4,
            DHCPACK = 5,
            DHCPNAK = 6,
            DHCPRELEASE = 7,
            DHCPINFORM = 8
        };

        protected enum DhcpOptionValue  //refer to the rfc2132.txt for vendor specific info
        {
            SubnetMask = 1,
            TimeOffset = 2,
            Router = 3,
            TimeServer = 4,
            NameServer = 5,
            DomainNameServer = 6,
            LogServer = 7,
            CookieServer = 8,
            LPRServer = 9,
            ImpressServer = 10,
            ResourceLocServer = 11,
            HostName = 12,
            BootFileSize = 13,
            MeritDump = 14,
            DomainName = 15,
            SwapServer = 16,
            RootPath = 17,
            ExtensionsPath = 18,
            IpForwarding = 19,
            NonLocalSourceRouting = 20,
            PolicyFilter = 21,
            MaximumDatagramReAssemblySize = 22,
            DefaultIPTimeToLive = 23,
            PathMTUAgingTimeout = 24,
            PathMTUPlateauTable = 25,
            InterfaceMTU = 26,
            AllSubnetsAreLocal = 27,
            BroadcastAddress = 28,
            PerformMaskDiscovery = 29,
            MaskSupplier = 30,
            PerformRouterDiscovery = 31,
            RouterSolicitationAddress = 32,
            StaticRoute = 33,
            TrailerEncapsulation = 34,
            ARPCacheTimeout = 35,
            EthernetEncapsulation = 36,
            TCPDefaultTTL = 37,
            TCPKeepaliveInterval = 38,
            TCPKeepaliveGarbage = 39,
            NetworkInformationServiceDomain = 40,
            NetworkInformationServers = 41,
            NetworkTimeProtocolServers = 42,
            VendorSpecificInformation = 43,
            NetBIOSoverTCPIPNameServer = 44,
            NetBIOSoverTCPIPDatagramDistributionServer = 45,
            NetBIOSoverTCPIPNodeType = 46,
            NetBIOSoverTCPIPScope = 47,
            XWindowSystemFontServer = 48,
            XWindowSystemDisplayManager = 49,
            RequestedIPAddress = 50,
            IPAddressLeaseTime = 51,
            OptionOverload = 52,
            DHCPMessageTYPE = 53,
            ServerIdentifier = 54,
            ParameterRequestList = 55,
            Message = 56,
            MaximumDHCPMessageSize = 57,
            RenewalTimeValue_T1 = 58,
            RebindingTimeValue_T2 = 59,
            Vendorclassidentifier = 60,
            ClientIdentifier = 61,
            NetworkInformationServicePlusDomain = 64,
            NetworkInformationServicePlusServers = 65,
            TFTPServerName = 66,
            BootfileName = 67,
            MobileIPHomeAgent = 68,
            SMTPServer = 69,
            POP3Server = 70,
            NNTPServer = 71,
            DefaultWWWServer = 72,
            DefaultFingerServer = 73,
            DefaultIRCServer = 74,
            StreetTalkServer = 75,
            STDAServer = 76,
            WpadServer = 252,
            END_Option = 255
        };

        protected struct Part
        {
            public Part(int o, int s) { offset = o; size = s; }
            public int offset;
            public int size;
            public int next { get { return offset + size; } }
        };

        protected byte[] mFullMsg = null;

        protected byte[] Slice(Part part) {
            int size = part.size;
            if (part.size == -1)
            {
                size = mFullMsg.Length - part.offset;
            }
            byte[] result = new byte[size];
            Array.Copy(mFullMsg, part.offset, result, 0, size);
            return result;
        }

        protected static Part sOp = new Part(0, 1); // Op code:   1 = bootRequest, 2 = BootReply
        protected byte Op { get { return mFullMsg[sOp.offset]; } }

        protected static Part sHtype = new Part(sOp.next, 1); // Hardware Address Type: 1 = 10MB ethernet
        protected byte Htype { get { return mFullMsg[sHtype.offset]; } }

        protected static Part sHlen = new Part(sHtype.next, 1); // hardware address length: length of MACID
        protected byte Hlen { get { return mFullMsg[sHlen.offset]; } }

        protected static Part sHops = new Part(sHlen.next, 1); // Hw options
        protected byte Hops { get { return mFullMsg[sHops.offset]; } }

        protected static Part sXid = new Part(sHops.next, 4); // transaction id (5)
        protected byte[] Xid { get { return Slice(sXid); } }

        protected static Part sSecs = new Part(sXid.next, 2); // elapsed time from trying to boot (3)
        protected byte[] Secs { get { return Slice(sSecs); } }

        protected static Part sFlags = new Part(sSecs.next, 2); // flags (3)
        protected byte[] Flags { get { return Slice(sFlags); } }

        protected static Part sCiaddr = new Part(sFlags.next, 4); // client IP (5)
        protected byte[] Ciaddr { get { return Slice(sCiaddr); } }

        protected static Part sYiaddr = new Part(sCiaddr.next, 4); // your client IP (5)
        protected byte[] Yiaddr { get { return Slice(sYiaddr); } }

        protected static Part sSiaddr = new Part(sYiaddr.next, 4); // Server IP  (5)
        protected byte[] Siaddr { get { return Slice(sSiaddr); } }

        protected static Part sGiaddr = new Part(sSiaddr.next, 4); // relay agent IP (5)
        protected byte[] Giaddr { get { return Slice(sGiaddr); } }

        protected static Part sChaddr = new Part(sGiaddr.next, 16); // Client HW address (16)
        protected byte[] Chaddr { get { return Slice(sChaddr); } }

        protected static Part sSname = new Part(sChaddr.next, 64); // Optional server host name (64)
        protected byte[] Sname { get { return Slice(sSname); } }

        protected static Part sFile = new Part(sSname.next, 128); // Boot file name (128)
        protected byte[] File { get { return Slice(sFile); } }

        protected static Part sCookie = new Part(sFile.next, 4); // Magic cookie (4)
        protected byte[] Cookie { get { return Slice(sCookie); } }

        protected static Part sOptions = new Part(sCookie.next, -1); // options (rest)
        protected byte[] Options { get { return Slice(sOptions); } }

        public void Fill(DhcpMessage source)
        {
            mFullMsg = source.mFullMsg;
            source.mFullMsg = null;
        }

        public struct ArrayPart
        {
            public int offset;
            public int length;
            public int end { get { return offset + length; } }
        }

        // The options data contains a series of entries
        // Each entry starts with a byte with an ID     corresponding to the
        // DhcpOptionValue enum.  The next byte is the length of the rest
        // of that entry.  The rest of the entry contains data specific to
        // that option ID.  The total length of the entry is two plus the
        // length stored in the entry number of bytes.
        protected void _GetOptionData(DhcpOptionValue dhcpOption, ref ArrayPart part)
        {
            part.offset = 0;
            part.length = 0;

            try
            {
                for (int optionsIdx = sOptions.offset; optionsIdx < mFullMsg.Length; )
                {
                    byte curId = mFullMsg[optionsIdx];
                    byte curLength = mFullMsg[optionsIdx + 1];

                    if (((byte)dhcpOption) == curId)
                    {
                        part.offset = optionsIdx + 2;
                        part.length = curLength;
                        break;
                    }

                    optionsIdx += 1 + 1 + curLength;
                }
            }
            catch
            {
            }
        }

        protected bool _IsParamSet(DhcpMessage.DhcpOptionValue parameter)
        {
            // They must have requested Wpad
            bool set = false;
            ArrayPart part;
            part.length = part.offset = 0;

            _GetOptionData(DhcpOptionValue.ParameterRequestList, ref part);
            if (part.length != 0)
            {
                for (int parameterIdx = part.offset;
                    parameterIdx < part.end; ++parameterIdx)
                {
                    if (mFullMsg[parameterIdx] == (byte)parameter)
                    {
                        set = true;
                        break;
                    }
                }
            }
            return set;
        }
   }
}