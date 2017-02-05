using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public class ExtendedComputationFunction : ComputationFunction, IExtendedFunction
	{
		public string UniqueName { get; set; }
		public Func< DataCell[], object> Function { get; set; }

		public DataCell Result { get; set; }
	}
}
