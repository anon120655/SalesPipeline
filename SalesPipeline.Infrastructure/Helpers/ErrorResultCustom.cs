using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Helpers
{
	public class ErrorResultCustom : ObjectResult
	{
		private const int DefaultStatusCode = StatusCodes.Status400BadRequest;

		/// <summary>
		/// Creates a new <see cref="TimeoutExceptionObjectResult"/> instance.
		/// </summary>
		/// <param name="error">Contains the errors to be returned to the client.</param>
		public ErrorResultCustom(ErrorCustom error, Exception ex) : base(error)
		{
			string message = ex.Message?.ToString() ?? string.Empty;
			if (message.StartsWith(GeneralTxt.ErrorTxt))
			{
				StatusCode = DefaultStatusCode;
				error.Status = StatusCodes.Status400BadRequest;
			}
			else
			{
				StatusCode = StatusCodes.Status500InternalServerError;
				error.Status = StatusCodes.Status500InternalServerError;
			}

			error.Message = GeneralUtils.GetExMessage(ex);

		}
	}
}
