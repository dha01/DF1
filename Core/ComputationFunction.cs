using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public class ComputationFunction : IFunction
	{
		public int FunctionId { get; set; }

		public IExtendedFunction Extend(IExtendedFunction[] global_functions)
		{
			return global_functions[FunctionId];
		}
	}
}
