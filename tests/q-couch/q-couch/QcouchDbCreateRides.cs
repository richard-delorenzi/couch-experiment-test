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
					wait_time_min=11,
					state="closed"
				},
				new{
					ride_name="slow one",
					wait_time_min=17,
					state="open"
				}
			};
			CreateSomeRecords(list, CreateRideStatus);
		}

		public void CreateSomeWaitTimeModifiers()
		{
			var list = new List<object> {
				new{
					name="bronze",
					percentage=100,
					forground_colour="bronze",
					background_colour="white"
				},
				new{
					name="silver",
					percentage=50,
					forground_colour="silver",
					background_colour="black"
				},
				new {
					name="gold",
					percentage=5,
					forground_colour="gold",
					background_colour="white"
				},
				new {
					name="instant",
					percentage=0,
					forground_colour="green",
					background_colour="white"
				}
			};
			CreateSomeRecords(list, CreateWaitTimeModifiers);

		}
	}
}

