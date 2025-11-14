using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class PreFactorController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public PreFactorController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[AllowAnonymous]
		[HttpPost("Process")]
		public async Task<IActionResult> Process(Pre_FactorCustom model)
		{
			try
			{
				var data = await _repo.PreFactor.Process(model);

				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("GetById")]
		public async Task<IActionResult> GetById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.PreFactor.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetLastProcessBySaleId")]
		public async Task<IActionResult> GetLastProcessBySaleId([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.PreFactor.GetLastProcessBySaleId(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ประเมินผู้ขอสินเชื่อ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPut("UpdateEvaluateAppLoan")]
		public async Task<IActionResult> UpdateEvaluateAppLoan(Pre_ResultCustom model)
		{
			try
			{
				var data = await _repo.PreFactor.UpdateEvaluateAppLoan(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ตารางการผ่อนสินเชื่อ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("PaymentSchedule")]
		public async Task<IActionResult> PaymentSchedule(PayScheduleFactor model)
		{
			try
			{
				var data = await _repo.PreFactor.PaymentSchedule(model);

				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ทดสอบ Function XLookup
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("XLookup_Calculator")]
		public async Task<IActionResult> XLookup_Calculator(XLookupRequest model)
		{
			try
			{
				var lookupResult = LoanCalculator.XLookupLists(model.lookupValue, model.lookUpModel, model.match_mode, model.search_mode);

				await Task.CompletedTask;

				return Ok(lookupResult);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		static void PrintAmortizationSchedule(double principal, double monthlyInterestRate, int numberOfPayments, double monthlyPayment)
		{
			double balance = principal;

			Console.WriteLine("\nงวดที่\tยอดชำระ\tดอกเบี้ย\tเงินต้น\tยอดคงเหลือ");

			for (int period = 1; period <= numberOfPayments; period++)
			{
				double interest = Math.Round(balance * monthlyInterestRate, 2);
				double principalPayment = Math.Round(monthlyPayment - interest, 2);
				balance = Math.Round(balance - principalPayment, 2);

				// แสดงรายละเอียดในแต่ละงวด
				var xxx = $"{period}\t{monthlyPayment:F2}\t{interest:F2}\t{principalPayment:F2}\t{balance:F2}";

				Console.WriteLine("ok");
			}
		}

	}
}
