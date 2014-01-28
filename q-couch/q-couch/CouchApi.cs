using System;

using System.Net;

namespace qcouch
{
	public class CouchApi
	{
		public CouchApi( string uri, string contentType, System.Net.WebHeaderCollection headers)
		{
			rest = new Rest(uri, contentType, headers);
		}

		public void Delete(){
			try {
				rest.Request(Rest.Method.Delete, null, null);
			} catch {}
		}

		public void Create(){
			rest.Request(Rest.Method.Put, null, null);
		}

		public void Put(string url, string msg)
		{
			rest.Request(Rest.Method.Put, url, msg);
		}

		private readonly Rest rest;
	}

	class Rest {
		public Rest( string uri, string contentType, System.Net.WebHeaderCollection headers)
		{
			this.uri=uri;
			this.contentType=contentType;
			this.headers = headers;
		}

		public enum Method { Delete, Put, Get, Post };

		public WebResponse Request(Method method, string url, string msg)
		{
			var request = HttpWebRequest.Create((url==null)?uri:string.Format("{0}/{1}",uri,url));
			request.Method = method.ToString().ToUpper();
			request.ContentType = contentType;
			request.Headers = headers;

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

		private string uri;
		private string contentType;
		private readonly System.Net.WebHeaderCollection headers;
	}
}

