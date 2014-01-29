using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using Richard.Json;

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

		public string RideId(string rideName)
		{
			CouchApi.Get("_design/couch-experiment/_view/ride-id-by-name",rideName);
			var responce = CouchApi.Responce.Text.ToString();
			var o = JObject.Parse(responce);
			var id=o["rows"][0]["id"];
			return id.ToString();
		}

		protected void CreateRideStatus(JObject o){
			CreateRecord( new {type="ride-status", attraction_id=RideId(o.AsString("ride_name")), wait_time_min=o.AsString("wait_time_min") });
		}

		protected void CreateRide(JObject o){
			var desc=o.AsString("description");
			CreateRecord( new {type="ride", name=o.AsString("name"), description= (desc==null)?null:desc });
		}

		protected delegate void CreateMethod(JObject o);
		protected void CreateSomeRecords(List<object> list, CreateMethod create)
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

