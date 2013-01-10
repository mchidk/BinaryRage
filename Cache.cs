using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryRage
{
	internal class Cache
	{
		public static ConcurrentDictionary<string, SimpleObject> CacheDic = new ConcurrentDictionary<string, SimpleObject>();
		public static int counter = 0;
	}
}
