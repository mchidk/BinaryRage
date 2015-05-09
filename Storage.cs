using System;
using System.Linq;
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

                lock (Cache.LockObject)
                {
                    //remove object from cache
                    Cache.CacheDic.Remove(filelocation + key);
                }
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
			return File.ReadAllBytes(GetExactFileLocation(key, filelocation));
		}

		public static bool ExistingStorageCheck(string key, string filelocation)
		{
			return File.Exists(GetExactFileLocation(key, filelocation));
		}

		public static string GetExactFileLocation(string key, string filelocation)
		{
			return Path.Combine(
				filelocation,
				Path.Combine(Key.Splitkey(key).ToArray()),
				key + DB_EXTENTION);
		}

		public static byte[] GetFromStorageWithKnownFileLocation(string filelocation)
		{
			return File.ReadAllBytes(filelocation);
		}



	}
}