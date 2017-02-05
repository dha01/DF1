using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public class ControlFunction : IFunction
	{
		public Command[] Commands { get; set; }
		public int[] UsingFunctionIds { get; set; }
	}
}
