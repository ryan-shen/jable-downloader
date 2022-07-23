using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace JableDownloader.Services
{
    public class TypeConverter
    {
        public static byte[] ToByteArray(string byteString)
        {
            string escapedString = Regex.Replace(byteString, @"\s", "");
            var bytes = new List<byte>();

            for (int i = 0; i < escapedString.Length; i += 2)
            {
                bytes.Add(byte.Parse($"{escapedString[i]}{escapedString[i + 1]}", NumberStyles.HexNumber));
            }

            return bytes.ToArray();
        }
    }
}
