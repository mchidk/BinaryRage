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
  using System.IO.Compression;

  static public class DB
  {
    static BlockingCollection<SimpleObject> sendQueue = new BlockingCollection<SimpleObject>();

    static readonly object LockObject = new object();

    static public void Insert<T>(string key, T value, string filelocation, CompressionLevel compressionLevel)
    {
      Interlocked.Increment(ref Cache.counter);
      SimpleObject simpleObject = new SimpleObject { Key = key, Value = value, FileLocation = filelocation };

      sendQueue.Add(simpleObject);
      var data = sendQueue.Take(); //this blocks if there are no items in the queue.

      //Add to cache
      lock (Cache.LockObject)
      {
        Cache.CacheDic[filelocation + key] = simpleObject;
      }

      ThreadPool.QueueUserWorkItem(
        state =>
          {
            lock (Cache.LockObject)
            {
              Storage.WritetoStorage(
                data.Key,
                Compress.CompressGZip(ConvertHelper.ObjectToByteArray(value), compressionLevel),
                data.FileLocation);
            }
          });
    }

    static public void Insert<T>(string key, T value, string filelocation)
    {
      Insert(key, value, filelocation, CompressionLevel.Optimal);
    }

    static public void Remove(string key, string filelocation)
    {
      lock (Cache.LockObject)
      {
        Cache.CacheDic.Remove(filelocation + key);
      }

      lock (DB.LockObject)
      {
        File.Delete(Storage.GetExactFileLocation(key, filelocation));
      }
    }

    static public T Get<T>(string key, string filelocation)
    {
      //Try getting the object from cache first
      lock (Cache.LockObject)
      {
        SimpleObject simpleObjectFromCache;
        if (Cache.CacheDic.TryGetValue(filelocation + key, out simpleObjectFromCache)) return (T)simpleObjectFromCache.Value;
      }

      //Get from disk
      lock (DB.LockObject)
      {
        byte[] compressGZipData = Compress.DecompressGZip(Storage.GetFromStorage(key, filelocation));
        T umcompressedObject = (T)ConvertHelper.ByteArrayToObject(compressGZipData);
        return umcompressedObject;
      }
    }

    static public string GetJSON<T>(string key, string filelocation)
    {
      return SimpleSerializer.Serrialize(Get<T>(key, filelocation));
    }

    static public bool Exists(string key, string filelocation)
    {
      return Storage.ExistingStorageCheck(key, filelocation);
    }

    static public void WaitForCompletion()
    {
      while (Cache.counter > 0)
      {
        Thread.Sleep(10);
      }
    }
  }
}
