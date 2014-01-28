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
			var r = new Rest(uri);
			r.Delete();
			r.Create();
		}
	}
}

