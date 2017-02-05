using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class Command
    {
		public int ResultId { get; set; }
		public int FunctionId { get; set; }
		public int[] InputParamIds { get; set; }
		public int[] TriggeredCommandIds { get; set; }
		public ExtendedCommand Extend(ExtendedControlFunction parent_control_function)
		{
			parent_control_function.TempData[ResultId].TriggeredCommands = TriggeredCommandIds.Select(x => parent_control_function.Commands[x]).ToArray();

			return new ExtendedCommand
			{
				FunctionId = FunctionId,
				InputParamIds = InputParamIds,
				TriggeredCommandIds = TriggeredCommandIds,

				ParentFunction = parent_control_function,
				Result = parent_control_function.TempData[ResultId],
				Function = parent_control_function.UsingFunctions[FunctionId],
				InputParams = InputParamIds.Select(x => parent_control_function.TempData[x]).ToArray(),
				TriggeredCommands = parent_control_function.TempData[ResultId].TriggeredCommands
			};
		}
    }
}
