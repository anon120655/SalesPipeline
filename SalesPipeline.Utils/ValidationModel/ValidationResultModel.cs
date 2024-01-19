using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ValidationModel
{
	public class ValidationResultModel
	{
		public string status { get; }

		public List<ValidationError> Errors { get; }

		public ValidationResultModel(ModelStateDictionary modelState)
		{
			status = "422";
			Errors = modelState.Keys
					.SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
					.ToList();
		}
	}
}
