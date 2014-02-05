using System;
using System.Net;
using Richard.Contracts;
using Richard.Json;
using Newtonsoft.Json.Linq;


namespace Qcouch
{
	public class CouchApi
	{
		public CouchApi( string urlHostPart, string urlDbPart, System.Net.WebHeaderCollection headers, bool isSelfChecking=true)
		{
			this.urlHostPart=urlHostPart;
			this.urlDbPart=urlDbPart;
			this.IsSelfChecking=IsSelfChecking;
			rest = new Rest("application/json", "application/json", headers);
		}

		public void Delete(){
			rest.Request(Rest.Method.Delete, FullUrl(""), null);
			Contract.Ensures(!IsSelfChecking || Responce.IsGood || Responce.IsNotFound);
		}

		public void Create(){
			Put(url:"", msg:null);
			Contract.Ensures(!IsSelfChecking || Responce.IsGood);
		}

		public void Add(Guid id, JObject msg){
			Put("db/" +id.ToString(),msg);
			Contract.Ensures(!IsSelfChecking || Responce.IsGood);
		}

		public void Replicate(string from, string to)
		{
			var url=string.Format("{0}/_replicate",urlHostPart);
			var msg = new{
				   source=from,
				   target=to
			}.ToJsonString();

			rest.Request(
				Rest.Method.Post, 
				url,
				msg
			);
			Contract.Ensures(!IsSelfChecking || Responce.IsGood);
		}

		public void Get(string designName, string key=null)
		{
			var url = FullUrl(designName);
			if (key != null){
				url = string.Format("{0}?key=\"{1}\"",url,key);
			}
			rest.Request(Rest.Method.Get, url, null);
		}

		public Responce Responce 
		{
			get {return rest.Responce;} 
		}

		public bool IsSelfChecking {get; private set;}

		private void Put(string url, JObject msg)
		{
			rest.Request(Rest.Method.Put, FullUrl(url), msg.ToJsonString() );
		}

		private string FullUrl(string url)
		{
			return string.Format( "{0}/{1}/{2}",urlHostPart, urlDbPart??"", url??"" );
		}

		private readonly Rest rest;
		private readonly string urlHostPart;
		private readonly string urlDbPart;
	}
}

