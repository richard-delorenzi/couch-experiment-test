using System;
using System.Net;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using Richard.String;

namespace Qcouch
{
	[TestFixture]
	public class TestWaitTimeModifiers
	{
		QcouchDbCreateRides db;

		[SetUp]
		public void Init(){
			db = new QcouchDbCreateRides();
			db.CreateNew();
			db.CreateSomeRides();
			db.CreateSomeRideStatus();
			db.CreateSomeWaitTimeModifiers();
		}

		[Test]
		public void Setup(){}

		[Test]
		public void WaitTimeModView(){
			var waitTimeModifiersQuery = db.WaitTimeModifiers();
			Assert.That(waitTimeModifiersQuery["total_rows"].ToString(), Is.EqualTo("4"));
			var rows = waitTimeModifiersQuery["rows"];
			foreach (var row in rows){
				Assert.That(row["value"]["type"].ToString(), Is.EqualTo("wait_time_modifier"));
			}
		}

		[Test]
		[Category("debug")]
		public void RideListWaitTimeModifiers() {
			var ridesQuery=db.Rides();
			var rides=ridesQuery["rides"];

			foreach (var ride in rides) {
				var waitTimes= ride["wait_time_min"];
				Assert.That(waitTimes, Is.Not.Null, "Ride list has wait time modifiers");

				var names=new List<string>{"instant", "gold", "silver", "bronze"};
				var namesEnumerator=names.GetEnumerator();
				foreach (var mod in waitTimes) {
					namesEnumerator.MoveNext();
					var expectedValue="0";
					var expectedName=namesEnumerator.Current;
					var name=mod["name"].ToString();
					Assert.That(name, Is.EqualTo(expectedName));
				}
			}
		}
	}
	[TestFixture]
	public class TestCreate
	{
		#region rides_of_different_types
		[Test]
		public void CreateEmpty(){
			new QcouchDb().CreateNew();
		}

		private QcouchDbCreateVariousRideRecords DbWithVariousRideRecords()
		{
			var db = new QcouchDbCreateVariousRideRecords();
			db.CreateNew();
			db.CreateSomeRides();
			return db;
		}

		[Test]
		public void Create(){
			DbWithVariousRideRecords();
		}

		[Test]
		public void RideHasGuid(){
			var db = DbWithVariousRideRecords();

			var id=db.RideId("base2-test");
			Guid tmp;
			Assert.That(Guid.TryParse(id,out tmp), Is.True);
		}

		[Test]
		public void ReadRideList(){
			var db = DbWithVariousRideRecords();
			var ridesQuery = db.Rides();
			var rides=ridesQuery["rides"];
			var count=0;

			foreach ( var ride in rides )
			{
				Assert.That(ride["name"].Type, Is.EqualTo(JTokenType.String));
				count++;
			}
			Assert.That(count, Is.EqualTo(db.NumberOfRidesCreated));
		}

#if false // integrated tested ignores "explicit" and "category"
		[Test]
		public void HasIfdef(){
			var db = DbWithVariousRideRecords();
			var ridesQuery = db.Rides();

			foreach ( var ride in ridesQuery["rides"] )
			{
				var ifdefAttrSet = new HashSet<string>();
				var AttrSet = new HashSet<string>();
				foreach (var child in ride.Children()) //child is JToken
				{
					var childName =  ( child as JProperty ).Name; //assert that child is also a JProperty, and get its name
					var childBaseName = childName.WithStripedLeaderOf("ifdef_");
					(childBaseName.HadLeader?ifdefAttrSet:AttrSet).Add(childBaseName.Text);
				}
				Assert.That(ifdefAttrSet, Is.EqualTo(AttrSet));
			}
		}
#endif

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
				JObject.FromObject(new{ 
					name="no description",
					wait_time_min=0
				})
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

 