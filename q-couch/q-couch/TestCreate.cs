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
		#region test_good
		[Test]
		public void CreateEmpty()
		{
			new QcouchDb().CreateNew();
		}

		[Test]
		public void Create()
		{
			var db = new QcouchDb();
			db.CreateNew();
			db.CreateSomeRides();
		}

		#endregion

		#region test_bad
		[Test]
		public void CreateRecordWithNoType()
		{
			var db=new QcouchDb(isSelfChecking:false);
			var api=db.CouchApi;

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
			Assert.That(db.CouchApi.Responce.Code, Is.EqualTo(HttpStatusCode.Forbidden));
			var responceTextFromJson=JObject.Parse(db.CouchApi.Responce.Text);
			var errorTxt=responceTextFromJson["error"].ToString();
			var reasonTxt=responceTextFromJson["reason"].ToString();
			Assert.That(errorTxt, Is.EqualTo("forbidden"));
			Assert.That(reasonTxt, Is.EqualTo(reason));
		}
		#endregion
	}
}

