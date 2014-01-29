using System;
using Newtonsoft.Json.Linq;

namespace Qcouch
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			//new MainClass().so2();
			new TestCreate().Create();
		}



		private void so2()
		{
			var JsonObj = JObject.FromObject( new{name="bob"} );
			JsonObj.Add("type", JToken.FromObject("person"));
		}
	}
}
