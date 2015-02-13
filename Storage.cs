using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace BinaryRage
{
	internal static class Storage
	{
		private const string DB_EXTENTION = ".odb";

		private static string createDirectoriesBasedOnKeyAndFilelocation(string filelocation,string key)
		{
			try
			{
				if (!Directory.Exists(filelocation))
					Directory.CreateDirectory(filelocation);
			}
			catch (Exception)
			{
				
			}

			var keyArray = Key.Splitkey(key);
			
			string keypath = "";
			foreach (var k in keyArray)
			{
				try
				{
					keypath += Path.DirectorySeparatorChar + k;
					if (!Directory.Exists(filelocation + keypath))
						Directory.CreateDirectory(filelocation + keypath);
				}
				catch (Exception)
				{
					
				}
			}
			return filelocation + keypath;
		}

		public static void WritetoStorage(string key, byte[] value, string filelocation)
		{
			//create folders
			string dirstructure = createDirectoriesBasedOnKeyAndFilelocation(filelocation, key);

			//Write the file to it's location
			try
			{
                File.WriteAllBytes(dirstructure + Path.DirectorySeparatorChar + key + DB_EXTENTION, value);
				
				//remove object from cache
				SimpleObject tmpSimpleObject;
                Cache.CacheDic.TryRemove(filelocation + key, out tmpSimpleObject);
            }
			catch (Exception)
			{
				
			}

			Interlocked.Decrement(ref Cache.counter);

            //Calculate pause based on amount of bytes
            Thread.Sleep(value.Length / 100);
		}

		public static byte[] GetFromStorage(string key, string filelocation)
		{
			var keyArray = Key.Splitkey(key);

			string dirstructure = "";
			foreach (var k in keyArray)
                dirstructure += Path.DirectorySeparatorChar + k;

            byte[] bytes = File.ReadAllBytes(filelocation + Path.DirectorySeparatorChar + dirstructure + Path.DirectorySeparatorChar + key + DB_EXTENTION);
			return bytes;
		}

		public static bool ExistingStorageCheck(string key, string filelocation)
		{
			var keyArray = Key.Splitkey(key);

			string dirstructure = "";
			foreach (var k in keyArray)
                dirstructure += Path.DirectorySeparatorChar + k;

            return File.Exists(filelocation + Path.DirectorySeparatorChar + dirstructure + Path.DirectorySeparatorChar + key + DB_EXTENTION);
		}

		public static string GetExactFileLocation(string key, string filelocation)
		{
			var keyArray = Key.Splitkey(key);

			string dirstructure = "";
			foreach (var k in keyArray)
                dirstructure += Path.DirectorySeparatorChar + k;

            return filelocation + Path.DirectorySeparatorChar + dirstructure + Path.DirectorySeparatorChar + key + DB_EXTENTION;
		}

		public static byte[] GetFromStorageWithKnownFileLocation(string filelocation)
		{
			byte[] bytes = File.ReadAllBytes(filelocation);
			return bytes;
		}



	}
}