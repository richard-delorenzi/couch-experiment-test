using System;
using System.Collections.Generic;
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
			new QcouchDb().CreateSomeRides();
		}

		[Test]
		public void CreateBadRides()
		{
			var db= new QcouchDb();
			var api = db.couchApi;

			db.CreateNew();
			api.Add(
				Guid.NewGuid(), 
				new{ //no type
					name="no description",
					wait_time_min=0
				});

		}
	}
}

