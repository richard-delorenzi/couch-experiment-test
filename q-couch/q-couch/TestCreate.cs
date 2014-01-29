using System;
using System.Net;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace Qcouch
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
		//[Ignore("not finished")]
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
			Assert.That(db.couchApi.Responce.Code, Is.EqualTo(HttpStatusCode.Forbidden));
			var ResponceTextFromJson = JObject.Parse(db.couchApi.Responce.Text);
			Assert.That(ResponceTextFromJson, Is.EqualTo( JObject.FromObject( new{ error="forbidden", reason="The 'type' field is required."})));
		}
	}
}

