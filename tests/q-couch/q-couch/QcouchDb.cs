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
			    urlHostPart: "http://admin:password@127.0.0.1:5984", 
			    urlDbPart: "q-couch-test",
			    headers: headers, 
			    isSelfChecking:isSelfChecking);
		}

		private string Rewrite( string docName)
		{
			return string.Format("_design/couch-experiment/_rewrite/{0}",docName);
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
			CouchApi.Get(Rewrite("ride-id-by-name"),rideName);
			var responce = CouchApi.Responce.Text.ToString();
			var o = JObject.Parse(responce);
			var id=o["rows"][0]["id"];
			return id.ToString();
		}

		public JObject Rides()
		{
			CouchApi.Get(Rewrite("rides"));
			var responce = CouchApi.Responce.Text.ToString();
			var o = JObject.Parse(responce);
			return o;
		}

		public JObject WaitTimeModifiers()
		{
			CouchApi.Get(Rewrite("waitTimeModifiers"));
			var responce = CouchApi.Responce.Text.ToString();
			var o = JObject.Parse(responce);
			return o;
		}

		protected void CreateRideStatus(JObject o){
			var attraction_id=RideId(o.AsString("ride_name"));
			var idToken = JToken.FromObject(attraction_id);
			o.Add("attraction_id", idToken);
			o.Remove("ride_name");

			CreateDoc("ride-status", o);
		}

		protected void CreateRide(JObject o){
			CreateDoc("ride", o);
		}

		protected void CreateWaitTimeModifiers(JObject o){
			CreateDoc("wait_time_modifier", o);
		}

		private void CreateDoc(string type, JObject o)
		{
			var typeToken = JToken.FromObject(type);
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
		private readonly System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
	}
}

