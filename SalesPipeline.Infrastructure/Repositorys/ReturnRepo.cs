using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
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

		public async Task RMToMCenter(ReturnModel model)
		{
			int countReturn = 0;
			foreach (var item in model.RM_Sale)
			{
				var sales = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Id == item.SaleId);
				if (sales != null)
				{
					if (model.CurrentUserId != sales.AssUserId)
						throw new ExceptionCustom("currentuserid not match assuserid");

					sales.AssUserId = null;
					sales.AssUserName = null;
					_db.Update(sales);
					await _db.SaveAsync();

					var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

					var reasonName = await _repo.MasterReasonReturn.GetNameById(model.Master_ReasonReturnId);

					await _repo.Sales.UpdateStatusOnly(new()
					{
						SaleId = item.SaleId,
						StatusId = StatusSaleModel.RMReturnMCenter,
						CreateBy = model.CurrentUserId,
						CreateByName = currentUserName,
						Description = reasonName
					});

					var assignment_RM_Sales = await _repo.Context.Assignment_RM_Sales
						.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.SaleId == item.SaleId);
					if (assignment_RM_Sales != null)
					{
						assignment_RM_Sales.Status = StatusModel.InActive;
						_db.Update(assignment_RM_Sales);
						await _db.SaveAsync();
					}

					countReturn++;
				}
			}

			if (countReturn > 0)
			{
				var assignmentRM = await _repo.AssignmentRM.GetByUserId(model.CurrentUserId);
				if (assignmentRM == null)
					throw new ExceptionCustom("currentuserid not match assignmentrm");

				await _repo.AssignmentRM.UpdateCurrentNumber(assignmentRM.Id);
			}
			else
			{
				throw new ExceptionCustom("sales not found.");
			}
		}

		public async Task MCenterToBranch(ReturnModel model)
		{
			foreach (var item in model.RM_Sale)
			{
				var sales = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Id == item.SaleId);
				if (sales != null)
				{
					sales.BranchId = null;
					sales.BranchName = null;
					sales.AssCenterUserId = null;
					sales.AssCenterUserName = null;
					sales.AssCenterCreateBy = null;
					sales.AssCenterDate = null;
					_db.Update(sales);
					await _db.SaveAsync();

					var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

					var reasonName = await _repo.MasterReasonReturn.GetNameById(model.Master_ReasonReturnId);

					await _repo.Sales.UpdateStatusOnly(new()
					{
						SaleId = item.SaleId,
						StatusId = StatusSaleModel.MCenterReturnBranch,
						CreateBy = model.CurrentUserId,
						CreateByName = currentUserName,
						Description = reasonName
					});

					//ไม่ต้อง update Assignment_RM_Sale เพราะยังไม่ถูกมอบหมายให้ RM

				}
			}
		}

		public Task BranchToLCenter(ReturnModel model)
		{
			throw new NotImplementedException();
		}

	}
}
