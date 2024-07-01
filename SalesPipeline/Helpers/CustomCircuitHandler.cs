using Microsoft.AspNetCore.Components.Server.Circuits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Helpers
{
	public class CustomCircuitHandler : CircuitHandler
	{
		public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
		{
			Console.WriteLine("Circuit opened.");
			return Task.CompletedTask;
		}

		public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
		{
			Console.WriteLine("Connection up.");
			return Task.CompletedTask;
		}

		public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
		{
			Console.WriteLine("Connection down.");
			return Task.CompletedTask;
		}

		public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
		{
			Console.WriteLine("Circuit closed.");
			return Task.CompletedTask;
		}
	}
}
