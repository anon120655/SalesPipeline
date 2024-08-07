using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Assignments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class ReturnRepo : IReturnRepo
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public ReturnRepo(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		/// <summary>
		/// พนักงาน RM ส่งคืน ผู้จัดการศูนย์
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task RMToCenBranch(ReturnModel model)
		{
			int countReturn = 0;
			foreach (var item in model.ListSale)
			{
				if (Guid.TryParse(item.ID, out Guid saleId))
				{
					var sales = await _repo.Context.Sales.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == saleId);
					if (sales != null)
					{
						sales.AssUser = null;
						sales.AssCenterUser = null;

						if (model.CurrentUserId != sales.AssUserId)
							throw new ExceptionCustom("currentuserid not match assuserid");

						await _repo.Sales.CreateReturn(new()
						{
							CurrentUserId = model.CurrentUserId,
							CustomerId = sales.CustomerId,
							CompanyName = sales.CompanyName,
							SaleId = sales.Id,
							StatusSaleId = sales.StatusSaleId,
							StatusSaleName = sales.StatusSaleName,
							StatusDescription = sales.StatusDescription,
							Master_BusinessTypeId = sales.Customer.Master_BusinessTypeId,
							Master_BusinessTypeName = sales.Customer.Master_BusinessTypeName,
							Master_LoanTypeId = sales.Customer.Master_LoanTypeId,
							Master_LoanTypeName = sales.Customer.Master_LoanTypeName,
							AssUserId = sales.AssUserId,
							AssUserName = sales.AssUserName,
						});

						//Name ต่างๆ ใช้ตอนแสดงผลว่าใครส่งกลับ แต่ id ต้อง clear ออกเพื่อคำนวณ UpdateCurrentNumber
						sales.AssUserId = null;
						//sales.AssUserName = null;
						_db.Update(sales);
						await _db.SaveAsync();

						var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

						var reasonName = await _repo.MasterReasonReturn.GetNameById(model.Master_ReasonReturnId);

						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = saleId,
							StatusId = StatusSaleModel.RMReturnMCenter,
							CreateBy = model.CurrentUserId,
							CreateByName = currentUserName,
							Description = reasonName
						});

						var assignment_RM_Sales = await _repo.Context.Assignment_RM_Sales
							.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.SaleId == saleId);
						if (assignment_RM_Sales != null)
						{
							assignment_RM_Sales.Status = StatusModel.InActive;
							_db.Update(assignment_RM_Sales);
							await _db.SaveAsync();
						}

						countReturn++;
					}
				}
			}

			if (countReturn > 0)
			{
				var assignmentRM = await _repo.AssignmentRM.GetByUserId(model.CurrentUserId);
				if (assignmentRM == null)
					throw new ExceptionCustom("currentuserid not match assignmentrm");

				await _repo.AssignmentRM.UpdateCurrentNumber(assignmentRM.UserId);
			}
			else
			{
				throw new ExceptionCustom("sales not found.");
			}
		}

		/// <summary>
		/// ผู้จัดการศูนย์ ส่งคืน สำนักงานใหญ่
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task CenBranchToLoan(ReturnModel model)
		{
			int countReturn = 0;
			foreach (var item in model.ListSale)
			{
				if (Guid.TryParse(item.ID, out Guid saleId))
				{
					var sales = await _repo.Context.Sales.FindAsync(saleId);
					if (sales != null)
					{
						sales.AssUser = null;
						sales.AssCenterUser = null;

						var isView = await _repo.Sales.IsViewSales(saleId, model.CurrentUserId);
						if (!isView)
						{
							throw new ExceptionCustom("currentuserid not match assuserid");
						}
						//if (model.CurrentUserId != sales.AssCenterUserId)
						//	throw new ExceptionCustom("currentuserid not match assuserid");

						//Name ต่างๆ ใช้ตอนแสดงผลว่าใครส่งกลับ แต่ id ต้อง clear ออกเพื่อคำนวณ UpdateCurrentNumber
						sales.AssUserId = null;
						//sales.AssUserName = null;
						sales.AssCenterUserId = null;
						//sales.AssCenterUserName = null;
						//sales.AssCenterCreateBy = null;
						//sales.AssCenterDate = null;
						_db.Update(sales);
						await _db.SaveAsync();

						var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

						var reasonName = await _repo.MasterReasonReturn.GetNameById(model.Master_ReasonReturnId);

						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = saleId,
							StatusId = StatusSaleModel.MCenterReturnLoan,
							CreateBy = model.CurrentUserId,
							CreateByName = currentUserName,
							Description = reasonName
						});

						countReturn++;

					}
				}
			}

			if (countReturn > 0)
			{
				//var assignmentCenter = await _repo.AssignmentCenter.GetByUserId(model.CurrentUserId);
				//if (assignmentCenter == null || !assignmentCenter.BranchId.HasValue)
				//	throw new ExceptionCustom("currentuserid not match assignmentCenter");

				//await _repo.AssignmentCenter.UpdateCurrentNumber(assignmentCenter.BranchId.Value);
			}
			else
			{
				throw new ExceptionCustom("sales not found.");
			}

			//ไม่ต้อง update Assignment_RM_Sale เพราะยังไม่ถูกมอบหมายให้ RM
			//หรือถ้าถูกมอบหมายแล้ว ตอน RM ส่งคืนจะถูก update แล้ว

		}

		/// <summary>
		/// กิจการสาขาภาค ส่งคืน ศูนย์ธุรกิจสินเชื่อ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task BranchRegToLoan(ReturnModel model)
		{
			int countReturn = 0;
			foreach (var item in model.ListSale)
			{
				if (Guid.TryParse(item.ID, out Guid saleId))
				{
					var sales = await _repo.Context.Sales.FindAsync(saleId);
					if (sales != null)
					{
						sales.AssUser = null;
						sales.AssCenterUser = null;

						//Name ต่างๆ ใช้ตอนแสดงผลว่าใครส่งกลับ แต่ id ต้อง clear ออกเพื่อคำนวณ UpdateCurrentNumber
						sales.Master_Branch_RegionId = null;
						//sales.Master_Department_BranchName = null;
						sales.ProvinceId = null;
						//sales.ProvinceName = null;
						sales.BranchId = null;
						//sales.BranchName = null;
						sales.AssUserId = null;
						//sales.AssUserName = null;
						sales.AssCenterUserId = null;
						//sales.AssCenterUserName = null;
						//sales.AssCenterCreateBy = null;
						//sales.AssCenterDate = null;
						_db.Update(sales);
						await _db.SaveAsync();

						var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

						var reasonName = await _repo.MasterReasonReturn.GetNameById(model.Master_ReasonReturnId);

						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = saleId,
							StatusId = StatusSaleModel.BranchReturnLCenter,
							CreateBy = model.CurrentUserId,
							CreateByName = currentUserName,
							Description = reasonName
						});

						countReturn++;

					}
				}
			}

			//ไม่ต้อง update Assignments เพราะถูก update ตอนส่งคืนจากผู้จัดการศูนย์แล้ว

		}

	}
}
