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

			const string host = "http://admin:password@127.0.0.1:5984";
			headers.Add ("Authorization: Basic YWRtaW46cGFzc3dvcmQ=");

			_couchApi = new CouchApi(host, testDb, headers);
		}

		private const string _dbBaseName = "q-couch";
		private string cleanDb { get { return string.Format("{0}-clean", _dbBaseName); } }
		private string testDb  { get { return string.Format("{0}-test", _dbBaseName); } }

		public void CreateNew()
		{
			couchApi.Delete();
			couchApi.Create();
			couchApi.Replicate(cleanDb,testDb);
		}

		public void CreateSomeRides ()
		{
			CreateNew();

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
				couchApi.Add (
					Guid.NewGuid(),
					o
				);
			}
		}

		private readonly CouchApi _couchApi;
		public CouchApi couchApi { get { return _couchApi; } }
	}
}

