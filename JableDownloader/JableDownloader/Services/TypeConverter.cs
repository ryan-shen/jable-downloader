using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace JableDownloader.Services
{
    /// <summary>
    /// 型別轉換器
    /// </summary>
    public class TypeConverter
    {
        /// <summary>
        /// 將 Byte String 轉換成 Byte
        /// </summary>
        /// <param name="byteString">要轉換的 Byte String</param>
        /// <example>"A3" -> 10 10 00 11</example>
        /// <returns></returns>
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
