using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace Qcouch
{
	public class QcouchDb
	{
		public QcouchDb (bool isSelfChecking=true)
		{
			var headers = new System.Net.WebHeaderCollection();

			const string host = "http://admin:password@127.0.0.1:5984";
			headers.Add ("Authorization: Basic YWRtaW46cGFzc3dvcmQ=");

			_couchApi = new CouchApi(host, TestDb, headers, isSelfChecking:isSelfChecking);
		}

		private const string DbBaseName = "q-couch";
		private string CleanDb { get { return string.Format("{0}-clean", DbBaseName); } }
		private string TestDb  { get { return string.Format("{0}-test", DbBaseName); } }

		public void CreateNew()
		{
			CouchApi.Delete();
			CouchApi.Create();
			CouchApi.Replicate(CleanDb,TestDb);
		}

		public List<Guid> CreateSomeRides()
		{
			var list = new List<object> {
				new{
					type="ride",
					name="base2-test",
					description="simple description",
				},
				new{
					type="ride",
					name="base-test",
					description="hello world",
				},
				new{
					type="ride",
					name="simple list description",
					description=new string[] {"hello world"},
				},
				new{
					type="ride",
					name="list description",
					description=new string[] {"hello","world"},
				},
				new{
					type="ride",
					name="no description",
				},
				new{
					type="ride",
					name="false description",
					description=false,
				},
				new{
					type="ride",
					name="true description",
					description=true,
				},
			};

			return CreateSomeRecords(list);
		}

		public void CreateSomeRideStatus(List<Guid> guids)
		{
			var _guids = guids.GetEnumerator();
			_guids.MoveNext();


			var list = new List<object> {
				new{
					type="ride-status",
					attraction_id= _guids.Current.ToString(),
					wait_time_min=6
				},
			};
			CreateSomeRecords(list);
		}


		private List<Guid> CreateSomeRecords(List<object> list)
		{
			var Result = new List<Guid>();

			foreach (var o in list) {
				var guid = Guid.NewGuid();
				CouchApi.Add (
					guid,
					o
				);
				Result.Add(guid);
			}
			return Result;
		}

		private readonly CouchApi _couchApi;
		public CouchApi CouchApi { get { return _couchApi; } }
	}
}

