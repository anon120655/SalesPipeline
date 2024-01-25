using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class MasterController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public MasterController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		/// <summary>
		/// ฝ่ายธุรกิจสินเชื่อ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet("GetDivLoans")]
		public async Task<IActionResult> GetDivLoans([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterDivLoan.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ประเภทสินเชื่อ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet("GetLoanType")]
		public async Task<IActionResult> GetLoanType([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterLoanTypes.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// เหตุผลการส่งคืน
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet("GetReasonReturns")]
		public async Task<IActionResult> GetReasonReturns([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterReasonReturn.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// SLA การดำเนินการ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet("GetSLAOperations")]
		public async Task<IActionResult> GetSLAOperations([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterSLAOperation.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ผลผลิต
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet("GetYields")]
		public async Task<IActionResult> GetYields([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterYield.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ห่วงโซ่
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet("GetChains")]
		public async Task<IActionResult> GetChains([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterChain.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ขนาดธุรกิจ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet("GetBusinessSize")]
		public async Task<IActionResult> GetBusinessSize([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterBusinessSize.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ประเภทธุรกิจ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet("GetBusinessType")]
		public async Task<IActionResult> GetBusinessType([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterBusinessType.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ช่องทางการติดต่อ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet("GetContactChannel")]
		public async Task<IActionResult> GetContactChannel([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterContactChannel.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ISICCode
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet("GetISICCode")]
		public async Task<IActionResult> GetISICCode([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterISICCode.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		///  สถานะการขาย
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet("GetStatusSale")]
		public async Task<IActionResult> GetStatusSale([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterStatusSale.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		///  จังหวัด
		/// </summary>
		/// <returns></returns>
		[HttpGet("GetProvince")]
		public async Task<IActionResult> GetProvince()
		{
			try
			{
				var response = await _repo.Thailand.GetProvince();

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// อำเภอ
		/// </summary>
		/// <param name="provinceID"></param>
		/// <returns></returns>
		[HttpGet("GetAmphur")]
		public async Task<IActionResult> GetAmphur([FromQuery] int provinceID)
		{
			try
			{
				var response = await _repo.Thailand.GetAmphur(provinceID);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ตำบล
		/// </summary>
		/// <param name="provinceID"></param>
		/// <param name="amphurID"></param>
		/// <returns></returns>
		[HttpGet("GetTambol")]
		public async Task<IActionResult> GetTambol([FromQuery] int provinceID, int amphurID)
		{
			try
			{
				var response = await _repo.Thailand.GetTambol(provinceID, amphurID);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("Positions")]
		public async Task<IActionResult> Positions([FromQuery] allFilter model)
		{
			try
			{
				return Ok(await _repo.Master.Positions(model));
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("Departments")]
		public async Task<IActionResult> Departments([FromQuery] allFilter model)
		{
			try
			{
				return Ok(await _repo.Master.Departments(model));
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("Regions")]
		public async Task<IActionResult> Regions([FromQuery] allFilter model)
		{
			try
			{
				return Ok(await _repo.Master.Regions(model));
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("Branchs")]
		public async Task<IActionResult> Branchs([FromQuery] allFilter model)
		{
			try
			{
				return Ok(await _repo.Master.Branchs(model));
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("MenuItem")]
		public async Task<IActionResult> MenuItem([FromQuery] allFilter model)
		{
			try
			{
				return Ok(await _repo.Master.MenuItem(model));
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("CreateDivBranch")]
		public async Task<IActionResult> CreateDivBranch(Master_Division_BranchCustom model)
		{
			try
			{
				var data = await _repo.MasterDivBranch.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateDivBranch")]
		public async Task<IActionResult> UpdateDivBranch(Master_Division_BranchCustom model)
		{
			try
			{
				var data = await _repo.MasterDivBranch.Update(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteDivBranchById")]
		public async Task<IActionResult> DeleteDivBranchById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.MasterDivBranch.DeleteById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateStatusDivBranchById")]
		public async Task<IActionResult> UpdateStatusDivBranchById(UpdateModel model)
		{
			try
			{
				await _repo.MasterDivBranch.UpdateStatusById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetDivBranchById")]
		public async Task<IActionResult> GetDivBranchById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.MasterDivBranch.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetDivBranchs")]
		public async Task<IActionResult> GetDivBranchs([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.MasterDivBranch.GetBranchs(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		//ฝ่ายธุรกิจสินเชื่อ
		[HttpPost("CreateDivLoans")]
		public async Task<IActionResult> CreateDivLoans(Master_Division_LoanCustom model)
		{
			try
			{
				var data = await _repo.MasterDivLoan.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateDivLoans")]
		public async Task<IActionResult> UpdateDivLoans(Master_Division_LoanCustom model)
		{
			try
			{
				var data = await _repo.MasterDivLoan.Update(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteDivLoansById")]
		public async Task<IActionResult> DeleteDivLoansById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.MasterDivLoan.DeleteById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateStatusDivLoansById")]
		public async Task<IActionResult> UpdateStatusDivLoansById(UpdateModel model)
		{
			try
			{
				await _repo.MasterDivLoan.UpdateStatusById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetDivLoansById")]
		public async Task<IActionResult> GetDivLoansById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.MasterDivLoan.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		//ประเภทสินเชื่อ
		[HttpPost("CreateLoanType")]
		public async Task<IActionResult> CreateLoanType(Master_LoanTypeCustom model)
		{
			try
			{
				var data = await _repo.MasterLoanTypes.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateLoanType")]
		public async Task<IActionResult> UpdateLoanType(Master_LoanTypeCustom model)
		{
			try
			{
				var data = await _repo.MasterLoanTypes.Update(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteLoanTypeById")]
		public async Task<IActionResult> DeleteLoanTypeById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.MasterLoanTypes.DeleteById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateStatusLoanTypeById")]
		public async Task<IActionResult> UpdateStatusLoanTypeById(UpdateModel model)
		{
			try
			{
				await _repo.MasterLoanTypes.UpdateStatusById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetLoanTypeById")]
		public async Task<IActionResult> GetLoanTypeById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.MasterLoanTypes.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		//เหตุผลการส่งคืน
		[HttpPost("CreateReasonReturn")]
		public async Task<IActionResult> CreateReasonReturn(Master_ReasonReturnCustom model)
		{
			try
			{
				var data = await _repo.MasterReasonReturn.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateReasonReturn")]
		public async Task<IActionResult> UpdateReasonReturn(Master_ReasonReturnCustom model)
		{
			try
			{
				var data = await _repo.MasterReasonReturn.Update(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteReasonReturnById")]
		public async Task<IActionResult> DeleteReasonReturnById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.MasterReasonReturn.DeleteById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateStatusReasonReturnById")]
		public async Task<IActionResult> UpdateStatusReasonReturnById(UpdateModel model)
		{
			try
			{
				await _repo.MasterReasonReturn.UpdateStatusById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetReasonReturnById")]
		public async Task<IActionResult> GetReasonReturnById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.MasterReasonReturn.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		//SLA การดำเนินการ
		[HttpPost("CreateSLAOpe")]
		public async Task<IActionResult> CreateSLAOpe(Master_SLAOperationCustom model)
		{
			try
			{
				var data = await _repo.MasterSLAOperation.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateSLAOpe")]
		public async Task<IActionResult> UpdateSLAOpe(Master_SLAOperationCustom model)
		{
			try
			{
				var data = await _repo.MasterSLAOperation.Update(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteSLAOpeById")]
		public async Task<IActionResult> DeleteSLAOpeById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.MasterSLAOperation.DeleteById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateStatusSLAOpeById")]
		public async Task<IActionResult> UpdateStatusSLAOpeById(UpdateModel model)
		{
			try
			{
				await _repo.MasterSLAOperation.UpdateStatusById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetSLAOpeById")]
		public async Task<IActionResult> GetSLAOpeById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.MasterSLAOperation.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		//ผลผลิต
		[HttpPost("CreateYield")]
		public async Task<IActionResult> CreateYield(Master_YieldCustom model)
		{
			try
			{
				var data = await _repo.MasterYield.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateYield")]
		public async Task<IActionResult> UpdateYield(Master_YieldCustom model)
		{
			try
			{
				var data = await _repo.MasterYield.Update(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteYieldById")]
		public async Task<IActionResult> DeleteYieldById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.MasterYield.DeleteById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateStatusYieldById")]
		public async Task<IActionResult> UpdateStatusYieldById(UpdateModel model)
		{
			try
			{
				await _repo.MasterYield.UpdateStatusById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetYieldById")]
		public async Task<IActionResult> GetYieldById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.MasterYield.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		//ห่วงโซ่
		[HttpPost("CreateChain")]
		public async Task<IActionResult> CreateChain(Master_ChainCustom model)
		{
			try
			{
				var data = await _repo.MasterChain.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateChain")]
		public async Task<IActionResult> UpdateChain(Master_ChainCustom model)
		{
			try
			{
				var data = await _repo.MasterChain.Update(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteChainById")]
		public async Task<IActionResult> DeleteChainById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.MasterChain.DeleteById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateStatusChainById")]
		public async Task<IActionResult> UpdateStatusChainById(UpdateModel model)
		{
			try
			{
				await _repo.MasterChain.UpdateStatusById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetChainById")]
		public async Task<IActionResult> GetChainById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.MasterChain.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}


	}
}
