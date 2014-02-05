using System;
using Newtonsoft.Json.Linq;

namespace Qcouch
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			var tests = new TestCreate();
			tests.CreateEmpty();
		}
	}
}
