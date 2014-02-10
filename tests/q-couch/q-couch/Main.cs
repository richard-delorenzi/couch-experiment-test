using System;
using Newtonsoft.Json.Linq;

namespace Qcouch
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			NUnit.Gui.AppEntry.Main(
				new string[] {
				System.Reflection.Assembly.GetExecutingAssembly().Location, 
				"-run"
			});
		}
	}
}
