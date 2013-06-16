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

		static public void Insert(string key, T value, string filelocation)
		{
			//How to read from Queue while writing to disk?
			SimpleObject simpleObject = new SimpleObject {Key = key, Value = value, FileLocation = filelocation};
			
			sendQueue.Add(simpleObject);
			var data = sendQueue.Take(); //this blocks if there are no items in the queue.
			
            //ThreadPool.QueueUserWorkItem
			ThreadPool.QueueUserWorkItem(state =>
			{
				//Add to cache
				Interlocked.Increment(ref Cache.counter);
				//Cache.CacheDic[key] = simpleObject;

                Cache.CacheDic.AddOrUpdate(filelocation + key, simpleObject, (k, v) => simpleObject);
				Storage.WritetoStorage(data.Key, Compress.CompressGZip(ConvertHelper.ObjectToByteArray(value)), data.FileLocation);
			});
		}

		static public void Remove(string key, string filelocation)
		{
            //File.Delete(Storage.GetExactFileLocation(key,filelocation));
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
