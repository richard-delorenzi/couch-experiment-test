using System;

//json
using System.ServiceModel.Web;
using System.Runtime.Serialization;

namespace qcouch
{
	public class QcouchDb
	{
		public QcouchDb ()
		{
			var headers = new System.Net.WebHeaderCollection();

			const string uri = "http://admin:password@127.0.0.1:5984/q-couch";
			const string contentType = "application/json;charset=utf-8";
			headers.Add ("Authorization: Basic YWRtaW46cGFzc3dvcmQ=");

			rest = new Rest(uri, contentType, headers);
		}

		public void CreateNew()
		{
			rest.Delete();
			rest.Create();
		}

		public void Create()
		{
			//CreateNew();

			rest.Put(
				Guid.NewGuid().ToString(),
				"{'type': 'ride', 'name': 'base2-test', 'description': 'simple discription', 'wait_time_min': 5}".Replace("'","\"")
				);
		}

		private readonly Rest rest;
	}
}

