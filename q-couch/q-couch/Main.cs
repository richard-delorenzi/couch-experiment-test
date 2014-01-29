using System;


namespace Qcouch
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			new TestCreate().Create();
		}
	}
}
