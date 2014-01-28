using System;
using NUnit;
using NUnit.Framework;

namespace qcouch
{
	[TestFixture]
	public class TestCreate
	{
		[Test]
		public void CreateEmpty()
		{
			new QcouchDb().CreateNew();
		}

		[Test]
		public void Create()
		{
			new QcouchDb().Create();
		}
	}
}

