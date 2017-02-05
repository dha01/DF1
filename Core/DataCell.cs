using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public class DataCell
	{
		public bool HasValue { get; set; }
		public object Value { get; set; }

		public Command[] TriggeredCommands  { get; set; }

		public DataCell()
		{
			Value = null;
			HasValue = false;
		}

		public DataCell(object value)
		{
			HasValue = value == null;
			Value = value;
		}
	}
}
