using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public class ExtendedControlFunction : ControlFunction, IExtendedFunction
	{
		public string UniqueName { get; set; }
		
		public DataCell[] TempData { get; set; }
		public DataCell Result { get; set; }
		public IExtendedFunction[] UsingFunctions { get; set; }

		public ExtendedControlFunction(IExtendedFunction[] using_functions, Command[] commands)
		{
			var result_data = new DataCell
			{
				HasValue = false,
				Value = null
			};
			
			UsingFunctions = using_functions;
			Commands = commands;
			Result = result_data;
			
			var max_temp_data_index = Commands.Max(x => x.ResultId);
			var tmp_data = new List<DataCell>(max_temp_data_index) { result_data };

			while (tmp_data.Count <= max_temp_data_index)
			{
				tmp_data.Add(new DataCell
				{
					Value = null,
					HasValue = false
				});
			}

			TempData = tmp_data.ToArray();
		}
	}
}
