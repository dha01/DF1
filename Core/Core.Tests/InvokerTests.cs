using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using NUnit.Framework;

namespace Core.Tests
{
	[TestFixture]
	public class InvokerTests
	{
		private IExtendedFunction[] known_functions = new IExtendedFunction[]
		{
			// 0 Инициализирующая функция.
			new ExtendedComputationFunction
			{
				UniqueName = "Init",
				Function = (input) => input[0].Value
			},
			// 1 Сумма.
			new ExtendedComputationFunction
			{
				UniqueName = "SumInt32",
				Function = (input) => (int) input[0].Value + (int) input[1].Value
			},
			// 2 Умножение.
			new ExtendedComputationFunction
			{
				UniqueName = "Mul",
				Function = (input) => (int) input[0].Value*(int) input[1].Value
			}
		};


		/// <summary>
		/// (val) => { return SumInt32(val, val); };
		/// </summary>
		/// <param name="usings"></param>
		/// <returns></returns>
		public ControlFunction SumInt32(IExtendedFunction[] usings)
		{
			int init_func_id = Array.IndexOf(usings, usings.First(x => x.UniqueName.Equals("Init")));
			var init_sum_id = Array.IndexOf(usings, usings.First(x => x.UniqueName.Equals("SumInt32")));
			
			return new ControlFunction
			{
				UsingFunctionIds = new int[] { init_func_id, init_sum_id },
				Commands = new Command[]
				{
					// 0
					new Command
					{
						FunctionId = init_func_id,
						InputParamIds = new int[]{ 1 },
						TriggeredCommandIds = new int[]{ 1 },
						ResultId = 2
					},
					// 1
					new Command
					{
						FunctionId = init_sum_id,
						InputParamIds = new int[]{ 2, 2 },
						TriggeredCommandIds = new int[]{ },
						ResultId = 0
					} 
				}
			};
		}

		
		[Test]
		public void InvokeControlFunctionTest()
		{
			var invoker = new Invoker();

			var control_function = SumInt32(known_functions);
			var using_functions = new List<IExtendedFunction>(known_functions);
			var new_function = new ExtendedControlFunction(control_function.UsingFunctionIds.Select(x => known_functions[x]).ToArray(), control_function.Commands) {UniqueName = "SubSum"};
			using_functions.Add(new_function);

			var main_control_function = new ExtendedControlFunction
			(
				using_functions.ToArray(),
				// function(x)
				// {
				//    Sum(x, x);
				//    return (val) => { return val + val; }
				// }
				new Command[]
				{
					// 0
					new Command
					{
						FunctionId = Array.IndexOf(using_functions.ToArray(), using_functions.First(x => x.UniqueName.Equals("Init"))),
						TriggeredCommandIds = new []{ 1 },
						InputParamIds = new []{ 1 },
						ResultId = 2
					}, 
					// 1
					new Command
					{
						FunctionId = Array.IndexOf(using_functions.ToArray(), using_functions.First(x => x.UniqueName.Equals("SumInt32"))),
						TriggeredCommandIds = new int[]{ 2 },
						InputParamIds = new []{ 2, 2 },
						ResultId = 3
					}, 
					// 2
					new Command
					{
						FunctionId = Array.IndexOf(using_functions.ToArray(), using_functions.First(x => x.UniqueName.Equals("SubSum"))),
						TriggeredCommandIds = new int[]{ },
						InputParamIds = new []{ 3 },
						ResultId = 0
					}
				}
			);

			invoker.InvokeControlFunction(main_control_function, new [] { new DataCell(2) });

			while (true)
			{
				// Вычислительные функции могут выполнятся параллельно.
				if (invoker.InvokeComputationCommandQueue.Count > 0)
				{
					invoker.InvokeComputationFunction(invoker.InvokeComputationCommandQueue.Dequeue());
					continue;
				}

				// Управляющие функции выполняются последовательно.
				if (invoker.InvokeCommandQueue.Count > 0)
				{
					invoker.Invoke(invoker.InvokeCommandQueue.Dequeue());
					continue;
				}
				break;
			}

			Console.WriteLine(main_control_function.Result.Value);
		}
	}
}
