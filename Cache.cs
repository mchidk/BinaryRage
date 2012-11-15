using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryRage
{
	public class Cache
	{
		public static ConcurrentDictionary<string, SimpleObject> cacheDic = new ConcurrentDictionary<string, SimpleObject>();
		
		//todo move counter and other internal stuff? to other places
		public static int counter = 0;
	}
}
