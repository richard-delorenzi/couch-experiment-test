using System;
using System.Collections.Generic;

namespace Qcouch
{
	public class QcouchDbCreateRides : QcouchDb
	{
		public void CreateSomeRides()
		{
			var list= new List<object>{
				new{name="fast one", description="it is fast"},
			};
			CreateSomeRecords(list, CreateRide);
		}

		public void CreateSomeRideStatus()
		{
			var list = new List<object> {
				new{
					ride_name="fast one",
					wait_time_min=6
				},
			};
			CreateSomeRecords(list, CreateRideStatus);
		}
	}
}

