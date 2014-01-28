using System;

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
			var headers = new System.Net.WebHeaderCollection();
			headers.Add ("Authorization: Basic YWRtaW46cGFzc3dvcmQ=");
			var r = new Rest(uri,"application/json;charset=utf-8", headers);
			r.Delete();
			r.Create();
		}
	}
}

