using System;
using System.Collections.Generic;

namespace Qcouch
{
	public class QcouchDbCreateVariousRideRecords : QcouchDb
	{
		 List<object> list= new List<object>{
				new{name="base2-test", description="simple description"},
				new{name="base-test",  description="hello world"},
				new{
					name="simple list description",
					description=new string[] {"hello world"}
				},
				new{
					name="list description",
					description=new string[] {"hello","world"}
				},
				new{name="no description"},
				new{name="false description", description=false},
				new{name="true description", description=true},
			};

		public void CreateSomeRides()
		{
			CreateSomeRecords(list, CreateRide);
		}

		public int NumberOfRidesCreated {get{return list.Count;}}
	}
}

