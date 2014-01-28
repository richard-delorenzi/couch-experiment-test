using System;

using Newtonsoft.Json.Linq;

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
			CreateNew();

			var json = toJsonString(new
			{
				type="ride",
				name="base2-test",
				description="simple description",
				wait_time_min=5
			});

			                              

			rest.Put(
				Guid.NewGuid().ToString(),
				json );
		}

		private string toJsonString(object o)
		{
			return JObject.FromObject(o).ToString();
		}

		private readonly Rest rest;
	}
}

