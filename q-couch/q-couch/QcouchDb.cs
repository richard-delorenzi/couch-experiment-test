using System;

using System.Net;

//json
using System.ServiceModel.Web;
using System.Runtime.Serialization;

namespace qcouch
{
	public class QcouchDb
	{
		string uri = "http://admin:password@127.0.0.1:5984/q-couch";
		public void CreateNew ()
		{
			Delete();
			Create();
		}


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

