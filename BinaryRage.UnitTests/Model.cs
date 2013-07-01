using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryRage.UnitTests
{
	[Serializable]
	public class Model
	{
		public string Title { get; set; }
		public string ThumbUrl { get; set; }
		public string Description { get; set; }
		public float Price { get; set; }
	}
}