using System;
using System.ComponentModel;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using NLog;

namespace DhcpdToMicrosoft.Utility
{
    public static class IPAddressExtensions
    {
  
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

        public static IPAddress GetLastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < (broadcastAddress.Length -1 ); i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            broadcastAddress[broadcastAddress.Length-1] = (byte)(ipAdressBytes[broadcastAddress.Length-1] | (subnetMaskBytes[broadcastAddress.Length-1] ^ 254));
            return new IPAddress(broadcastAddress);

        }

        public static IDictionary<IPAddress, IPAddress> ExtractContiguousRanges(IEnumerable<IPAddress> IPs)
        {
            Logger logger = LogManager.GetCurrentClassLogger(); 
            IDictionary<IPAddress, IPAddress> ranges = new Dictionary<IPAddress, IPAddress>();
            HashSet<uint> IPNumbers = new HashSet<uint>();
            foreach (IPAddress address in IPs)
            {
                logger.Debug("For exclusion " + address.ToUint());
                IPNumbers.Add(address.ToUint());
            }
            IEnumerable<IEnumerable<uint>> IPNumberGroups = IPNumbers.Distinct()
                .GroupBy(num => NumberUtils.Range(num, uint.MaxValue - num + 1)
                .TakeWhile(IPNumbers.Contains).Last())
                .Where(seq => seq.Count() >= 3)
                .Select(seq => seq.OrderBy(num => num));

            foreach (IEnumerable<uint> group in IPNumberGroups)
            {
                uint low = group.Min();
                uint high = group.Max();
                IPAddress rangeLow = parseUint(low);
                IPAddress rangeHigh = parseUint(high);
                logger.Debug("Selected for exclusion: " + rangeLow + " to " + rangeHigh);
                ranges.Add(rangeLow, rangeHigh);
            }
            return ranges;

        }

        public static IPAddress GetFirstUsuableAddress(this IPAddress address)
        {
            byte[] ip = address.GetAddressBytes();
                ip[3]++;
                if (ip[3] == 0)
                {
                    ip[2]++;
                    if (ip[2] == 0)
                    {
                        ip[1]++;
                        if (ip[1] == 0)
                            ip[0]++;
                    }
                }

            return new IPAddress(ip);
        }

        public static uint ToUint(this IPAddress address)
        {
            byte[] ipBytes = address.GetAddressBytes();
            ByteConverter bConvert = new ByteConverter();
            uint ipUint = 0;

            int shift = 24; // indicates number of bits left for shifting
            foreach (byte b in ipBytes)
            {
                if (ipUint == 0)
                {
                    ipUint = (uint)bConvert.ConvertTo(b, typeof(uint)) << shift;
                    shift -= 8;
                    continue;
                }

                if (shift >= 8)
                    ipUint += (uint)bConvert.ConvertTo(b, typeof(uint)) << shift;
                else
                    ipUint += (uint)bConvert.ConvertTo(b, typeof(uint));

                shift -= 8;
            }

            return ipUint;
        }

        /* reverse byte order in array */
        public static uint reverseBytesArray(uint ip)
        {
            byte[] bytes = BitConverter.GetBytes(ip);
            bytes = bytes.Reverse().ToArray();
            return (uint)BitConverter.ToInt32(bytes, 0);
        }

        public static IPAddress parseUint(uint ip)
        {
            uint reverseUint = reverseBytesArray(ip);
            byte[] bytes = BitConverter.GetBytes(reverseUint);

            return new IPAddress(bytes);
        }

        public static IEnumerable<IPAddress> GetIPRange(IPAddress startIP, IPAddress endIP)
        {
            uint sIP = startIP.ToUint();
            uint eIP = endIP.ToUint();
            while (sIP <= eIP)
            {
                yield return new IPAddress(reverseBytesArray(sIP));
                sIP++;
            }
        }

        public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }


        public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
        {
            IPAddress network1 = address.GetNetworkAddress(subnetMask);
            IPAddress network2 = address2.GetNetworkAddress(subnetMask);

            return network1.Equals(network2);
        }
    }
}
