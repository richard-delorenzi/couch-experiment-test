using System;

using System.Net;

namespace qcouch
{
	public class Rest
	{
		public Rest( string uri, string contentType, System.Net.WebHeaderCollection headers)
		{
			this.uri=uri;
			this.contextType=contextType;
			this.headers = headers;
		}

		public void Delete(){
			try {
				Request (Method.Delete);
			} catch {}
		}

		public void Create(){
			var request = HttpWebRequest.Create(uri);
			request.Method = "PUT";
			request.ContentType = "application/json;charset=utf-8";
			var headers = new System.Net.WebHeaderCollection();
			headers.Add ("Authorization: Basic YWRtaW46cGFzc3dvcmQ=");
			request.Headers = headers;
			var responce = request.GetResponse();
		}

		public enum Method { Delete, Put, Get, Post };

		private WebResponse Request(Method method)
		{
			var request = HttpWebRequest.Create(uri);
			request.Method = method.ToString().ToUpper();
			request.ContentType = contextType;
			request.Headers = headers;
			var responce = request.GetResponse();
			return responce;
		}

		private string uri;
		private string contextType;
		System.Net.WebHeaderCollection headers;
	}
}

