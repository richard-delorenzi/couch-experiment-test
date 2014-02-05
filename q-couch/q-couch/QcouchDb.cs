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
			headers.Add ("Authorization: Basic YWRtaW46cGFzc3dvcmQ=");

			CouchApi = new CouchApi( 
			    createionHost: CreateionHost, 
			    baseUrl: "http://admin:password@qcouch:5984",
			    headers: headers, 
			    isSelfChecking:isSelfChecking);
		}
		private string CreateionHost { get { return "http://admin:password@127.0.0.1:5984"; } }
		private readonly System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
		private const string DbBaseName = "q-couch";
		private string CleanDb { get { return string.Format("{0}-clean", DbBaseName); } }
		private string TestDb  { get { return string.Format("{0}-test", DbBaseName); } }

		public void CreateNew()
		{
			var api = new CouchApi( 
			    createionHost: CreateionHost, 
			    baseUrl: CreateionHost+"/"+TestDb,
			    headers: headers, 
			    isSelfChecking:false);
			api.Delete();
			api.Create();
			api.Replicate(CleanDb,TestDb);
		}

		public string RideId(string rideName)
		{
			CouchApi.Get("./ride-id-by-name",rideName);
			var responce = CouchApi.Responce.Text.ToString();
			var o = JObject.Parse(responce);
			var id=o["rows"][0]["id"];
			return id.ToString();
		}

		public JObject Rides()
		{
			CouchApi.Get("./rides");
			var responce = CouchApi.Responce.Text.ToString();
			var o = JObject.Parse(responce);
			return o;
		}

		protected void CreateRideStatus(JObject o){
			CreateRecord( JObject.FromObject(new {type="ride-status", attraction_id=RideId(o.AsString("ride_name")), wait_time_min=o.AsString("wait_time_min") }));
		}

		protected void CreateRide(JObject o){
			var typeToken = JToken.FromObject("ride");
			o.Add( "type", typeToken );
			CreateRecord( o );
		}

		protected delegate void CreateMethod(JObject o);
		protected void CreateSomeRecords(List<object> list, CreateMethod create)
		{
			foreach (var o in list) {
				create(JObject.FromObject(o));
			}
		}

		private void CreateRecord(JObject o)
		{
			var guid = Guid.NewGuid();
			CouchApi.Add (
				guid,
				o
			);
		}

		public readonly CouchApi CouchApi;
	}
}

