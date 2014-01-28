using System;
using System.Collections.Generic;

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

			CouchApi = new CouchApi(uri, contentType, headers);
		}

		public void CreateNew()
		{
			CouchApi.Delete();
			CouchApi.Create();
		}

		public void CreateSomeRides ()
		{
			CreateNew ();



			var list = new List<object> {
				new{
					type="ride",
					name="base2-test",
					description="simple description",
					wait_time_min=5
				},
				new{
					type="ride",
					name="base-test",
					description="hello world",
					wait_time_min=2
				},
				new{
					type="ride",
					name="simple list description",
					description=new string[] {"hello world"},
					wait_time_min=3
				},
				new{
					type="ride",
					name="list description",
					description=new string[] {"hello","world"},
					wait_time_min=4
				},
				new{
					type="ride",
					name="no description",
					wait_time_min=0
				},
				new{
					type="ride",
					name="false description",
					description=false,
					wait_time_min=5
				},
				new{
					type="ride",
					name="true description",
					description=true,
					wait_time_min=6
				},
			};

			foreach (var o in list) {
				CouchApi.Put (
					Guid.NewGuid().ToString(),
					toJsonString(o)
				);
			}
		}

		private string toJsonString(object o)
		{
			return JObject.FromObject(o).ToString();
		}

		private readonly CouchApi CouchApi;
	}
}

