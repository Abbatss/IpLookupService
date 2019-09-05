using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class IPExtentions
    {
        public static string ToIpString(this uint ipUint)
        {
            var res = BitConverter.GetBytes(ipUint);
            var ipstring = $"{res[0].ToString()}.{res[1].ToString()}.{res[2].ToString()}.{res[3].ToString()}";
            return ipstring;
        }
        public static uint ConvertFromIpStringToUint(this string ipString)
        {
            var ipBytes = ipString.Split('.');
            if (ipBytes.Length != 4)
            {
                throw new InvalidOperationException("ipString has wrong format when convert to ip");
            }
            if (byte.TryParse(ipBytes[0], out var firstByte)
                && byte.TryParse(ipBytes[1], out var secondByte)
                && byte.TryParse(ipBytes[2], out var thirdByte)
                && byte.TryParse(ipBytes[3], out var fouthByte)
                )
            {
                return BitConverter.ToUInt32(new byte[] { firstByte, secondByte, thirdByte, fouthByte }, 0);
            }
            throw new InvalidOperationException("ipString has wrong format when convert to ip");
        }
        public static bool IsValidIp(this string ipString)
        {

            var ipBytes = ipString.Split('.');
            if (ipBytes.Length != 4)
            {
                return false;
            }
            return byte.TryParse(ipBytes[0], out var firstByte)
                || byte.TryParse(ipBytes[1], out var secondByte)
                || byte.TryParse(ipBytes[2], out var thirdByte)
                || byte.TryParse(ipBytes[3], out var fouthByte);
        }
    }
}
