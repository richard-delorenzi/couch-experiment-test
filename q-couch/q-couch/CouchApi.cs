using System;
using System.Net;
using Richard.Contracts;
using Richard.Json;


namespace Qcouch
{
	public class CouchApi
	{
		public CouchApi( string host, string db, System.Net.WebHeaderCollection headers, bool isSelfChecking=true)
		{
			this.host=host;
			this.db=db;
			this.IsSelfChecking=IsSelfChecking;
			rest = new Rest("application/json;charset=utf-8", headers);
		}

		public void Delete(){
			rest.Request(Rest.Method.Delete, FullUrl(null), null);
			Contract.Ensures(!IsSelfChecking || Responce.IsGood || Responce.IsNotFound);
		}

		public void Create(){
			Put(null, null);
			Contract.Ensures(!IsSelfChecking || Responce.IsGood);
		}

		public void Add(Guid id, object msg){
			Put(id.ToString(),msg);
			Contract.Ensures(!IsSelfChecking || Responce.IsGood);
		}

		public void Replicate(string from, string to)
		{
			var url=string.Format("{0}/_replicate",host);
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

		private void Put(string url, object msg)
		{
			rest.Request(Rest.Method.Put, FullUrl(url), msg.ToJsonString() );
		}

		private string FullUrl(string url)
		{
			return string.Format( (url==null)?"{0}/{1}":"{0}/{1}/{2}",host,db,url);
		}

		private readonly Rest rest;
		private readonly string host;
		private readonly string db;
	}

	public class Responce {
		public Responce(HttpStatusCode code, string text){this.Code=code; this.Text=text;}
		public HttpStatusCode Code {get; private set;}
		public string Text {get; private set;}

		public bool IsGood {
			get {
				return (Code==HttpStatusCode.OK||
				        Code==HttpStatusCode.Created||
				        Code==HttpStatusCode.Accepted||
				        Code==HttpStatusCode.NotModified);
			}
		}

		public bool IsNotFound {
			get { return Code==HttpStatusCode.NotFound; }
		}
	}

	class Rest {
		public Rest( string contentType, System.Net.WebHeaderCollection headers)
		{
			this.contentType=contentType;
			this.headers = headers;
		}

		public enum Method { Delete, Put, Get, Post };

		public void Request(Method method, string url, string msg)
		{
			var request = HttpWebRequest.Create(url);
			request.Method = method.ToString().ToUpper();
			request.Headers = headers;
			request.ContentType = contentType;

			if (msg != null)
			{
				System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
				byte[] bytes = encoding.GetBytes(msg);
				request.ContentLength = bytes.Length;
				using (var requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
			}

			string responceText;
			HttpStatusCode responceCode;

			WebResponse responce;
			{
				try {
					responce = request.GetResponse();
				} catch (System.Net.WebException e)
				{
					responce = e.Response;
				}

				responceCode = (responce as HttpWebResponse).StatusCode;
				using (var data = responce.GetResponseStream())
				using (var reader = new System.IO.StreamReader(data)) 
				{
					responceText = reader.ReadToEnd();
				}
			}
			(responce as IDisposable).Dispose();

			request.Abort(); //there is no close

			Responce = new Responce(responceCode,responceText);
		}

		public Responce Responce {
			get; private set;
		}

		private string contentType;
		private readonly System.Net.WebHeaderCollection headers;
	}

}

