using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class ExtendedCommand : Command
    {
		public DataCell Result { get; set; }
		public IExtendedFunction Function { get; set; }
		public DataCell[] InputParams { get; set; }
		public Command[] TriggeredCommands { get; set; }

		public ExtendedControlFunction ParentFunction { get; set; }
    }
}
