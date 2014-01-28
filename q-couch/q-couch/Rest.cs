using System;

using System.Net;

namespace qcouch
{
	public class Rest
	{
		public Rest( string uri, string contextType, System.Net.WebHeaderCollection headers)
		{
			this.uri=uri;
			this.contextType=contextType;
			this.headers = headers;
		}

		public void Delete(){
			try {
				Request(Method.Delete, null, null);
			} catch {}
		}

		public void Create(){
			Request(Method.Put, null, null);
		}

		public void Put(string url, string msg)
		{
			Request(Method.Put, url, msg);
		}

		public enum Method { Delete, Put, Get, Post };

		private WebResponse Request(Method method, string url, string msg)
		{
			var request = HttpWebRequest.Create((url==null)?uri:string.Format("{0}/{1}",uri,url));
			request.Method = method.ToString().ToUpper();
			request.ContentType = contextType;
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
		private string contextType;
		private System.Net.WebHeaderCollection headers;
	}
}

