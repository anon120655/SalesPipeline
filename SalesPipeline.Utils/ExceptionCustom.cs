using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils
{
	[Serializable]
	public class ExceptionCustom : Exception
	{
		public ExceptionCustom() { }

		public ExceptionCustom(string? name)
			: base($"{GeneralTxt.ErrorTxt}{name}")
		{

		}
	}
}
