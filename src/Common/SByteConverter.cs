using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    public static class SByteToStrinConverter
    {
        public static string ConvertToString(this byte[] fileBytes, int startIndex, int lenght)
        {
            var convertedName = string.Empty;
            using (var stream = new MemoryStream(fileBytes))
            {
                stream.Position = startIndex;
                var name = new char[lenght];
                using (BinaryReader br = new BinaryReader(stream))
                {
                    for (int i = 0; i < lenght; i++)
                    {
                        name[i] = (char)br.ReadSByte();
                    }
                    convertedName = new string(name);
                }
            }
#if DEBUG
            var second = System.Text.Encoding.UTF8.GetString(fileBytes, startIndex, lenght);
            if (second != convertedName)
            {
                throw new Exception($"strings are not equal {second} , {convertedName}");
            }
#endif
            return convertedName;
        }
    }
}
