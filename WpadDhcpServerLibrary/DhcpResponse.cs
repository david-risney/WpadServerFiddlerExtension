using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;

namespace WpadDhcpServerLibrary
{
    class DhcpResponse : DhcpMessage
    {
        public void ApplyWpadOption(Uri pacUri, IPAddress localAddress)
        {
            if (localAddress != null && localAddress.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                localAddress = null;
            }

            byte[] wpadUrlEncoded = Encoding.UTF8.GetBytes(pacUri.ToString());
            mFullMsg[sOp.offset] = 2;

            int optionsSize =
                1 + 1 + 1 +                                 // MessageType, size, ack
                (localAddress != null ? 1 + 1 + 4 : 0) +    // Server Id, length, address
                1 + 1 + wpadUrlEncoded.Length + 1 +         // Wpad, size, Wpad URL, NULL terminator
                1;                                          // Terminating byte

            if (sOptions.offset + optionsSize > mFullMsg.Length)
            {
                Array.Resize(ref mFullMsg, sOptions.offset + optionsSize);
            }

            int optionIdx = sOptions.offset;
            mFullMsg[optionIdx++] = (byte)DhcpOptionValue.DHCPMessageTYPE;
            mFullMsg[optionIdx++] = 1;
            mFullMsg[optionIdx++] = (byte)DhcpMessageType.DHCPACK;

            if (localAddress != null)
            {
                mFullMsg[optionIdx++] = (byte)DhcpOptionValue.ServerIdentifier;
                mFullMsg[optionIdx++] = 4;
                Array.Copy(localAddress.GetAddressBytes(), 0, mFullMsg, optionIdx, 4);
                optionIdx += 4;
            }

            mFullMsg[optionIdx++] = (byte)DhcpOptionValue.WpadServer;
            mFullMsg[optionIdx++] = (byte)(wpadUrlEncoded.Length + 1);
            Array.Copy(wpadUrlEncoded, 0, mFullMsg, optionIdx, wpadUrlEncoded.Length);
            optionIdx += wpadUrlEncoded.Length;
            mFullMsg[optionIdx++] = 0;

            mFullMsg[optionIdx++] = 0xFF;
        }

        public byte[] ToByteArray() { return mFullMsg; }
    }
}