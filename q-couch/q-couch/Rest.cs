using System;

using System.Net;

namespace qcouch
{
	public class Rest
	{
		public Rest( string uri)
		{
			this.uri=uri;
		}

		private string uri;

		public void Delete(){
			var request = HttpWebRequest.Create(uri);
			request.Method = "DELETE";
			request.ContentType = "application/json;charset=utf-8";
			var headers = new System.Net.WebHeaderCollection();
			headers.Add ("Authorization: Basic YWRtaW46cGFzc3dvcmQ=");
			request.Headers = headers;
			try {
				var responce = request.GetResponse();
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
	}
}

