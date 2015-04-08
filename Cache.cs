using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryRage
{
    internal class Cache
    {
        public static object LockObject = new object();
        public static Dictionary<string, SimpleObject> CacheDic = new Dictionary<string, SimpleObject>();
        public static int counter = 0;
    }
}
