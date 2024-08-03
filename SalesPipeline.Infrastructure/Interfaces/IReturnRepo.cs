using SalesPipeline.Utils.Resources.Assignments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IReturnRepo
	{
		/// <summary>
		/// พนักงาน RM ส่งคืน ผู้จัดการศูนย์
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task RMToCenBranch(ReturnModel model);
		/// <summary>
		/// ผู้จัดการศูนย์ ส่งคืน สำนักงานใหญ่
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task CenBranchToLoan(ReturnModel model);
		/// <summary>
		/// กิจการสาขาภาค ส่งคืน ศูนย์ธุรกิจสินเชื่อ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task BranchRegToLoan(ReturnModel model);
	}
}
