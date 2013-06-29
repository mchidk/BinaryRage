using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BinaryRage
{
	public static class Key
	{

		/// <summary>
		/// Hex string lookup table. Faster to have them in a string array
		/// </summary>
		private static readonly string[] HexStringTable = new string[]
{
    "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F",
    "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F",
    "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F",
    "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F",
    "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F",
    "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F",
    "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F",
    "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F",
    "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F",
    "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F",
    "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF",
    "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF",
    "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF",
    "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF",
    "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF",
    "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF"
};

		private static IEnumerable<string> SplitByLength(this string str, int maxLength)
		{
			if (maxLength < 1)
				maxLength = 1;

			for (int index = 0; index < str.Length; index += maxLength)
			{
				yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
			}
		}

		/// <summary>
		/// Returns a hex string representation of an array of bytes.
		/// </summary>
		/// <param name="value">The array of bytes.</param>
		/// <returns>A hex string representation of the array of bytes.</returns>
		private static string ToHex(this byte[] value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (value != null)
				foreach (byte b in value)
					stringBuilder.Append(HexStringTable[b]);

			return stringBuilder.ToString();
		}

		private static string generateFastUniqueKey()
		{
			const int size = 5;
			const string a = "abcdefghijklmnopqrstuvwxyz1234567890";
			char[] chars = a.ToCharArray();
			byte[] data = new byte[1];
			RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
			crypto.GetNonZeroBytes(data);
			data = new byte[size];
			crypto.GetNonZeroBytes(data);
			StringBuilder result = new StringBuilder(size);
			foreach (byte b in data)
			{
				result.Append(chars[b%(chars.Length)]);
			}

			return result.ToString();
		}

		internal static IEnumerable<string> Splitkey(string key)
		{
			return key.SplitByLength(key.Length / 4);
		}

		public static string GenerateUniqueKey()
		{
			TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
			return generateFastUniqueKey() + ((int)span.TotalSeconds - 1332978141);
		}

		public static int CalculateChecksum(string inputString)
		{
			return inputString.Aggregate(0, (current, c) => current + c);
		}

		public static string GenerateMD5Hash(string input)
		{
			MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
			byte[] bs = Encoding.UTF8.GetBytes(input);
			bs = x.ComputeHash(bs);

			return ToHex(bs);
		}

	}
}