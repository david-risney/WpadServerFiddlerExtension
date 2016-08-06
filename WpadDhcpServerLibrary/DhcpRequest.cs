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
    class DhcpRequest : DhcpMessage
    {
        private bool mParsed = false;

        public bool IsValidWpadRequest
        {
            get { return mParsed; }
        }

        public DhcpRequest() { }

        public void SetData(byte[] dhcpRequestBytes)
        {
            mParsed = _Parse(dhcpRequestBytes);
        }

        private bool _ValidateWpadDhcpRequest()
        {
            // They must have requested Wpad
            bool valid = _IsParamSet(DhcpOptionValue.WpadServer);
            if (valid)
            {
                valid = false;
                if (Op == 1)
                {
                    ArrayPart part;
                    part.length = part.offset = 0;

                    _GetOptionData(DhcpOptionValue.DHCPMessageTYPE, ref part);
                    valid = part.length == 1 && mFullMsg[part.offset] == (byte)DhcpMessageType.DHCPINFORM;
                }
            }

            return valid;
        }

        private bool _Parse(byte[] data)
        {
            bool parsed = false;

            try
            {
                mFullMsg = data;
                parsed = _ValidateWpadDhcpRequest();
            }
            catch
            {
                parsed = false;
            }

            return parsed;
        }
    }
}