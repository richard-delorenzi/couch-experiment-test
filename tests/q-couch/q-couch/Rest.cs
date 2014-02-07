using System;
using System.Net;

namespace Qcouch
{
	public class Rest {
		public Rest( string contentType, string acceptType, System.Net.WebHeaderCollection headers)
		{
			this.contentType=contentType;
			this.acceptType=acceptType;
			this.headers = headers;
		}

		public enum Method { Delete, Put, Get, Post };

		public void Request(Method method, string url, string msg)
		{
			var request = HttpWebRequest.Create(url) as HttpWebRequest;
			request.Method = method.ToString().ToUpper();
			request.Headers = headers;
			request.ContentType = contentType;
			request.Accept= acceptType;

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
		private string acceptType;
		private readonly System.Net.WebHeaderCollection headers;
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
}

