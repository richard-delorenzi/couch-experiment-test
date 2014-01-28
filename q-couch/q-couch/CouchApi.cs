using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace qcouch
{
	public class CouchApi
	{
		public CouchApi( string host, string db, System.Net.WebHeaderCollection headers)
		{
			this.host=host;
			this.db=db;
			rest = new Rest("application/json;charset=utf-8", headers);
		}

		public Responce Delete(){
			return rest.Request(Rest.Method.Delete, fullUrl(null), null);
		}

		public Responce Create(){
			return Put(null, null);
		}

		public Responce Add(Guid id, object msg){
			return Put(id.ToString(),msg);
		}

		private Responce Put(string url, object msg)
		{
			return rest.Request(Rest.Method.Put, fullUrl(url), toJsonString(msg));
		}

		public Responce Replicate(string from, string to)
		{
			var url=string.Format("{0}/_replicate",host);
			var msg = toJsonString(new{
				   source=from,
				   target=to
			});

			return rest.Request(
				Rest.Method.Post, 
				url,
				msg
			);
		}

		private string fullUrl(string url)
		{
			return string.Format( (url==null)?"{0}/{1}":"{0}/{1}/{2}",host,db,url);
		}

		private string toJsonString(object o)
		{
			return o==null?null:JObject.FromObject(o).ToString();
		}

		private readonly Rest rest;
		private readonly string host;
		private readonly string db;
	}

	public class Responce {
		public Responce(HttpStatusCode code, string text){this.code=code; this.text=text;}
		public HttpStatusCode code {get; private set;}
		public string text {get; private set;} 
	}

	class Rest {
		public Rest( string contentType, System.Net.WebHeaderCollection headers)
		{
			this.contentType=contentType;
			this.headers = headers;
		}

		public enum Method { Delete, Put, Get, Post };

		public Responce Request(Method method, string url, string msg)
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

			return new Responce(responceCode,responceText);
		}

		private string contentType;
		private readonly System.Net.WebHeaderCollection headers;
	}
}

