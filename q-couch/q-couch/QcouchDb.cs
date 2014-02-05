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
			    urlHostPart: HostPart, 
			    urlDbPart: ".",
			    headers: headers, 
			    isSelfChecking:isSelfChecking);
		}
		private string HostPart { get { return HostPartFromDomainName("qcouch"); } }
		private string CreateionHostPart { get { return HostPartFromDomainName("127.0.0.1"); } }
		private string HostPartFromDomainName(string domainNamePart){
			return string.Format("http://admin:password@{0}:5984",domainNamePart);
		}
		private readonly System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
		private const string DbBaseName = "q-couch";
		private string CleanDb { get { return string.Format("{0}-clean", DbBaseName); } }
		private string TestDb  { get { return string.Format("{0}-test", DbBaseName); } }

		public void CreateNew()
		{
			//can't use api from constructor untill db exists and has rewrite rules.
			var api = new CouchApi( 
			    urlHostPart: CreateionHostPart, 
			    urlDbPart: TestDb,
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

