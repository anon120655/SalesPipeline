using SalesPipeline.Utils.Resources.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface ILoggerRepo
	{
		Task SaveLog(RequestResponseLogModel logs);
	}
}
