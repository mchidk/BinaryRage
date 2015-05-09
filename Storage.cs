using System;
using System.Linq;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace BinaryRage
{
	internal static class Storage
	{
		private const string DB_EXTENTION = ".odb";

		private static string createDirectoriesBasedOnKeyAndFilelocation(string key, string filelocation)
		{
			string pathSoFar = "";
			foreach (var folder in GetFolders(key, filelocation))
			{
				try
				{
					pathSoFar = Path.Combine(pathSoFar, folder);
					if (!Directory.Exists(pathSoFar))
						Directory.CreateDirectory(pathSoFar);
				}
				catch (Exception)
				{

				}
			}
			return pathSoFar;
		}

		public static void WritetoStorage(string key, byte[] value, string filelocation)
		{
			//create folders
			string dirstructure = createDirectoriesBasedOnKeyAndFilelocation(key, filelocation);

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
				Path.Combine(GetFolders(key, filelocation).ToArray()),
				key + DB_EXTENTION);
		}

		public static byte[] GetFromStorageWithKnownFileLocation(string filelocation)
		{
			return File.ReadAllBytes(filelocation);
		}

		private static IEnumerable<string> GetFolders(string key, string filelocation)
		{
			yield return filelocation;
			foreach (var folder in Key.Splitkey(key))
				yield return folder;
		}

	}
}