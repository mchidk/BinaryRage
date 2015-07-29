using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRage.UnitTests
{
	[TestFixture]
	public class StorageLocationTests
	{
		[Test]
		public void ExactFileLocations()
		{
			AssertPath(Storage.GetExactFileLocation("TheKey", "location"),
				"location", "T", "h", "e", "K", "e", "y", "TheKey.odb");

			AssertPath(Storage.GetExactFileLocation("OtherKey", "location2"),
				"location2", "Ot", "he", "rK", "ey", "OtherKey.odb");
		}

		private void AssertPath(string actualPath, params string[] parts)
		{
			Assert.AreEqual(Path.Combine(parts), actualPath);
		}
	}
}
