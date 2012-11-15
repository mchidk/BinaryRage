using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinaryRage.Functions
{
	public class ConvertHelper
	{

		public static byte[] ObjectToByteArray(object obj)
		{
			if (obj == null)
				return null;
		
			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();

			using (ms)
			{
				bf.Serialize(ms, obj);	
			}

			obj = null;

			return ms.ToArray();
		}

		public static Object ByteArrayToObject(byte[] arrBytes)
		{
			MemoryStream memStream = new MemoryStream();
			BinaryFormatter binForm = new BinaryFormatter();
			memStream.Write(arrBytes, 0, arrBytes.Length);
			memStream.Seek(0, SeekOrigin.Begin);
			Object obj = (Object)binForm.Deserialize(memStream);
			return obj;
		}
	}
}