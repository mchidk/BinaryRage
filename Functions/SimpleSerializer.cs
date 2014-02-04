using System;
using System.Web.Script.Serialization;

namespace BinaryRage.Functions
{
	internal class SimpleSerializer
	{
		public static string Serrialize<T>(T myobj)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			string returnstring = js.Serialize(myobj);

			return returnstring;
		}
		
	}
}