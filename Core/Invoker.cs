using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public class Invoker
	{
		/// <summary>
		/// Очередь на исполнение команд.
		/// </summary>
		public Queue<ExtendedCommand> InvokeCommandQueue { get; set; }

		/// <summary>
		/// Очередь на исполнение вычислительных команд.
		/// </summary>
		public Queue<ExtendedCommand> InvokeComputationCommandQueue { get; set; }

		/// <summary>
		/// Контруктор по умолчанию.
		/// </summary>
		public Invoker()
		{
			InvokeCommandQueue = new Queue<ExtendedCommand>();
			InvokeComputationCommandQueue = new Queue<ExtendedCommand>();
		}

		/// <summary>
		/// Исполняет управляющую функцию.
		/// </summary>
		/// <param name="control_function">Управляющая функция.</param>
		/// <param name="input_params">Входной параметр.</param>
		/// <param name="output">Выходной параметр.</param>
		public void InvokeControlFunction(ExtendedControlFunction control_function, DataCell[] input_params, DataCell output = null)
		{
			// Подставляем входные параметры.
			for (int i = 0; i < input_params.Count(); i++)
			{
				control_function.TempData[i + 1] = input_params[i];
			}

			// Подставляем выходной параметр.
			if (output != null)
			{
				control_function.TempData[0] = output;
				control_function.Result = output;
			}

			InvokeCommandQueue.Enqueue(control_function.Commands[0].Extend(control_function));
		}

		/// <summary>
		/// Исполняет вычислительную функцию.
		/// </summary>
		/// <param name="command">Команда.</param>
		public void InvokeComputationFunction(ExtendedCommand command)
		{
			var computation_function = command.Function as ExtendedComputationFunction;
			if (computation_function != null)
			{
				command.Result.Value = computation_function.Function.Invoke(command.InputParams);
				command.Result.HasValue = true;

				foreach (var function in command.Result.TriggeredCommands)
				{
					InvokeCommandQueue.Enqueue(function.Extend(command.ParentFunction));
				}
			}
			else
			{
				throw new Exception("Некорректные входные данные.");
			}
		}

		/// <summary>
		/// Исполняет команду.
		/// </summary>
		/// <param name="command"></param>
		public void Invoke(ExtendedCommand command)
		{
			var computation_function = command.Function as ExtendedComputationFunction;
			if (computation_function != null)
			{
				// Если необходимо выполнить вычислительную функцию, то отправляем в отдельную очередь.
				InvokeComputationCommandQueue.Enqueue(command);
			}

			var control_function = command.Function as ExtendedControlFunction;
			if (control_function != null)
			{
				InvokeControlFunction(control_function, command.InputParams, command.Result);
			}
		}
	}
}
