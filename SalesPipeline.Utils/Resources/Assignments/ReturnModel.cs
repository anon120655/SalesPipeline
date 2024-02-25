using Microsoft.AspNetCore.Mvc.ApplicationModels;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Utils.Resources.Assignments
{
	public class ReturnModel : CommonModel
	{
        public Guid Master_ReasonReturnId { get; set; }
		public List<Assignment_RM_SaleCustom> RM_Sale { get; set; } = null!;
	}
}
