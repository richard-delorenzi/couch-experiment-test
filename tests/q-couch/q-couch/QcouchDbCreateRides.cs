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
				new{name="slow one", description="it is slow"},
			};
			CreateSomeRecords(list, CreateRide);
		}

		public void CreateSomeRideStatus()
		{
			var list = new List<object> {
				new{
					ride_name="fast one",
					wait_time_min=7,
					state="closed"
				},
				new{
					ride_name="slow one",
					zwait_time_min=5,
					zstate="open"
				}
			};
			CreateSomeRecords(list, CreateRideStatus);
		}
	}
}

