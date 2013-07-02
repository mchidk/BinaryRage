using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryRage.Functions
{
    public class ListHelper<T>
    {
        static public List<T> PushToList(List<T> genericList, T genericObject, string objectKey, string key)
        {
            //Removes all entries with this key
            genericList.RemoveAll(obj => (string)obj.GetType().GetProperty(objectKey).GetValue(obj, null) == key);

            //Add the entry
            genericList.Add(genericObject);

            return genericList;
        }

        static public bool ExistsInList(List<T> genericList, string objectKey, string key)
        {
            //Check if entry exists in list
            if (genericList.Exists(obj => (string)obj.GetType().GetProperty(objectKey).GetValue(obj, null) == key))
                return true;

            return false;
        }
    }
}
