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
		public void CreateRecordWithNoType()
		{
			var db=new QcouchDb();
			var api=db.couchApi;

			db.CreateNew();
			api.Add(
				Guid.NewGuid(), 
				new{ 
					name="no description",
					wait_time_min=0
				}
			);

			CheckForbidden(db,"The 'type' field is required.");
		}

		private void CheckForbidden(QcouchDb db, string reason)
		{
			Assert.That(db.couchApi.Responce.Code, Is.EqualTo(HttpStatusCode.Forbidden));
			var responceTextFromJson=JObject.Parse(db.couchApi.Responce.Text);
			var errorTxt=responceTextFromJson["error"].ToString();
			var reasonTxt=responceTextFromJson["reason"].ToString();
			Assert.That(errorTxt, Is.EqualTo("forbidden"));
			Assert.That(reasonTxt, Is.EqualTo(reason));
		}
	}
}

