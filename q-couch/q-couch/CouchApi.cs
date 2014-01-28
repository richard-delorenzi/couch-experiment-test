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

		public void Delete(){
			try {
				rest.Request(Rest.Method.Delete, fullUrl(null), null);
			} catch {}
		}

		public void Create(){
			Put(null, null);
		}

		public void Add(Guid id, object msg){
			Put(id.ToString(),msg);
		}

		private void Put(string url, object msg)
		{
			rest.Request(Rest.Method.Put, fullUrl(url), toJsonString(msg));
		}

		public void Replicate(string from, string to)
		{
			var url=string.Format("{0}/_replicate",host);
			var msg = toJsonString(new{
				   source=from,
				   target=to
			});

			rest.Request(
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

	class Rest {
		public Rest( string contentType, System.Net.WebHeaderCollection headers)
		{
			this.contentType=contentType;
			this.headers = headers;
		}

		public enum Method { Delete, Put, Get, Post };

		public WebResponse Request(Method method, string url, string msg)
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

			var responce = request.GetResponse();
			request.Abort(); //there is no close
			return responce;
		}

		private string contentType;
		private readonly System.Net.WebHeaderCollection headers;
	}
}

