using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IPLookup.API.InMemoryDataBase
{
    public static class SByteToStrinConverter
    {
        public static string ConvertToString(byte[] bytes, int startIndex, int lenght)
        {
            var convertedName = string.Empty;
            using (var stream = new MemoryStream(bytes))
            {
                stream.Position = startIndex;
                var name = new char[lenght];
                using (BinaryReader br = new BinaryReader(stream))
                {
                    for (int i = 0; i < lenght; i++)
                    {
                        name[i] = Convert.ToChar(br.ReadSByte());
                    }
                    convertedName = new string(name);
                }
            }
            return convertedName;
        }
    }
}
