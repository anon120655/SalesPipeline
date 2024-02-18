using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
	public class AssignmentCenter : IAssignmentCenter
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public AssignmentCenter(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<bool> CheckAssignmentByUserId(int id)
		{
			return await _repo.Context.Assignments.AnyAsync(x => x.UserId == id);
		}

		public async Task<AssignmentCustom> Create(AssignmentCustom model)
		{
			if (await CheckAssignmentByUserId(model.UserId))
				throw new ExceptionCustom("assignment duplicate user");

			if (string.IsNullOrEmpty(model.EmployeeName))
			{
				model.EmployeeName = await _repo.User.GetFullNameById(model.UserId);
			}

			var assignment = new Data.Entity.Assignment();
			assignment.Status = StatusModel.Active;
			assignment.CreateDate = DateTime.Now;
			assignment.Code = model.Code;
			assignment.Name = model.Name;
			assignment.UserId = model.UserId;
			assignment.EmployeeId = model.EmployeeId;
			assignment.EmployeeName = model.EmployeeName;
			assignment.RMNumber = model.RMNumber ?? 0;
			assignment.CurrentNumber = model.CurrentNumber ?? 0;
			await _db.InsterAsync(assignment);
			await _db.SaveAsync();

			return _mapper.Map<AssignmentCustom>(assignment);
		}

	}
}
