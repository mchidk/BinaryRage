using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BinaryRage.UnitTests
{
	[TestFixture]
	public class WaitForCompletionTests
	{
		const string DB_NAME = "WaitForCompletionTests";

		[SetUp]
		public void Setup()
		{
			if (Directory.Exists(DB_NAME))
				Directory.Delete(DB_NAME, recursive: true);
		}

		[Test]
		public void ShouldWaitForIOCompletionWhenAsked()
		{
			var m = new Model { Description = "foobar" };
			for (int i = 0; i < 10; i++)
				BinaryRage.DB<Model>.Insert("key" + i, m, DB_NAME);

			// Without calling the wait method this test will fail every time
			// with a DirectoryNotFoundException
			BinaryRage.DB<Model>.WaitForCompletion();

			var readObjects = new Dictionary<string, Model>();
			for (int i = 0; i < 10; i++)
				readObjects.Add("key" + i, BinaryRage.DB<Model>.Get("key" + i, DB_NAME));

			Assert.AreEqual(m.Description, readObjects["key0"].Description);
			Assert.AreEqual(m.Description, readObjects["key9"].Description);
		}
	}
}
