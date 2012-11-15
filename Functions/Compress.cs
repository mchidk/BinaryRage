using System.IO;
using System.IO.Compression;

namespace BinaryRage.Functions
{
	public static class Compress
	{

		/// <summary>
		/// Compresses byte array to new byte array.
		/// </summary>
		public static byte[] CompressGZip(byte[] raw)
		{
			using (MemoryStream memory = new MemoryStream())
			{
				using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, false))
				{
					gzip.Write(raw, 0, raw.Length);
				}
				return memory.ToArray();
			}
		}

		public static byte[] DecompressGZip(byte[] gzip)
		{
			// Create a GZIP stream with decompression mode.
			// ... Then create a buffer and write into while reading from the GZIP stream.
			using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
			{
				const int size = 4096;
				byte[] buffer = new byte[size];
				using (MemoryStream memory = new MemoryStream())
				{
					int count = 0;
					do
					{
						count = stream.Read(buffer, 0, size);
						if (count > 0)
						{
							memory.Write(buffer, 0, count);
						}
					}
					while (count > 0);
					return memory.ToArray();
				}
			}
		}

	}
}