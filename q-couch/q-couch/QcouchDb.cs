using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace Qcouch
{
	public class QcouchDb
	{
		public QcouchDb (bool isSelfChecking=true)
		{
			var headers = new System.Net.WebHeaderCollection();

			const string host = "http://admin:password@127.0.0.1:5984";
			headers.Add ("Authorization: Basic YWRtaW46cGFzc3dvcmQ=");

			_couchApi = new CouchApi(host, TestDb, headers, isSelfChecking:isSelfChecking);
		}

		private const string DbBaseName = "q-couch";
		private string CleanDb { get { return string.Format("{0}-clean", DbBaseName); } }
		private string TestDb  { get { return string.Format("{0}-test", DbBaseName); } }

		public void CreateNew()
		{
			CouchApi.Delete();
			CouchApi.Create();
			CouchApi.Replicate(CleanDb,TestDb);
		}

		public void CreateSomeRides()
		{
			var list= new List<object>{
				new{name="base2-test", description="simple description"},
				new{name="base-test",  description="hello world"},
				new{
					name="simple list description",
					description=new string[] {"hello world"}
				},
				new{
					name="list description",
					description=new string[] {"hello","world"}
				},
				new{name="no description"},
				new{name="false description", description=false},
				new{name="true description", description=true},
			};
			CreateSomeRecords(list, CreateRide);
		}

		public void CreateSomeRideStatus()
		{
			var list = new List<object> {
				new{
					ride_name="base-test",
					wait_time_min=6
				},
			};
			CreateSomeRecords(list, CreateRideStatus);
		}

		public string RideId(string rideName)
		{
			CouchApi.Get("_design/couch-experiment/_view/ride-id-by-name",rideName);
			var responce = CouchApi.Responce.Text.ToString();
			var o = JObject.Parse(responce);
			var id=o["rows"][0]["id"];
			return id.ToString();
		}

		private void CreateRideStatus(JObject o){
			CreateRecord( new {type="ride-status", attraction_id=RideId(o["ride_name"].ToString()), wait_time_min=o["wait_time_min"].ToString() });
		}

		private void CreateRide(JObject o){
			var desc=o["description"];
			CreateRecord( new {type="ride", name=o["name"].ToString(), description= (desc==null)?null:desc.ToString() });
		}

		private delegate void CreateMethod(JObject o);
		private void CreateSomeRecords(List<object> list, CreateMethod create)
		{
			foreach (var o in list) {
				create(JObject.FromObject(o));
			}
		}

		private void CreateRecord(object o)
		{
			var guid = Guid.NewGuid();
			CouchApi.Add (
				guid,
				o
			);
		}

		private readonly CouchApi _couchApi;
		public CouchApi CouchApi { get { return _couchApi; } }
	}
}

