using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using BinaryRage.Functions;

namespace BinaryRage
{
	static public class DB<T>
	{
		static BlockingCollection<SimpleObject> sendQueue = new BlockingCollection<SimpleObject>();
		private static int writeCounter;

		static public void WaitForCompletion()
		{
			while (writeCounter > 0)
				Thread.Sleep(10);
		}
        
		static public void Insert(string key, T value, string filelocation)
		{
			Interlocked.Increment(ref writeCounter);

			SimpleObject simpleObject = new SimpleObject {Key = key, Value = value, FileLocation = filelocation};
			
			sendQueue.Add(simpleObject);
			var data = sendQueue.Take(); //this blocks if there are no items in the queue.
			
            //ThreadPool.QueueUserWorkItem
			ThreadPool.QueueUserWorkItem(state =>
			{
				//Add to cache
				Interlocked.Increment(ref Cache.counter);
                Cache.CacheDic[filelocation + key] = simpleObject;
				Storage.WritetoStorage(data.Key, Compress.CompressGZip(ConvertHelper.ObjectToByteArray(value)), data.FileLocation);

				Interlocked.Decrement(ref writeCounter);
			});
		}

		static public void Remove(string key, string filelocation)
		{
            if (!Cache.CacheDic.IsEmpty)
            {
                SimpleObject value;
                Cache.CacheDic.TryRemove(filelocation + key, out value);
            }

            File.Delete(Storage.GetExactFileLocation(key, filelocation));
		}

		static public T Get(string key, string filelocation)
		{
			//Try getting the object from cache first
			if (!Cache.CacheDic.IsEmpty)
			{
				SimpleObject simpleObjectFromCache;
                if (Cache.CacheDic.TryGetValue(filelocation + key, out simpleObjectFromCache))
					return (T) simpleObjectFromCache.Value;
			}
			
			byte[] compressGZipData = Compress.DecompressGZip(Storage.GetFromStorage(key, filelocation));
			T umcompressedObject = (T)ConvertHelper.ByteArrayToObject(compressGZipData);
			return umcompressedObject;
		}

		static public bool Exists(string key, string filelocation)
		{
			return Storage.ExistingStorageCheck(key, filelocation);
		}

	}
}
