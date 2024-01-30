using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class Assignments : IAssignments
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public Assignments(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<PaginationView<List<AssignmentCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Assignments.Where(x => x.Status != StatusModel.Delete)
												 .Include(x => x.Assignment_Sales)
												 .Include(x => x.User)
												 .OrderByDescending(x => x.CreateDate)
												 .AsQueryable();

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<AssignmentCustom>>()
			{
				Items = _mapper.Map<List<AssignmentCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<AssignmentCustom> Create(AssignmentCustom model)
		{
			if (string.IsNullOrEmpty(model.EmployeeName))
			{
				model.EmployeeName = await _repo.User.GetFullNameById(model.UserId);
			}

			var assignment = new Data.Entity.Assignment();
			assignment.Status = StatusModel.Active;
			assignment.CreateDate = DateTime.Now;
			assignment.UserId = model.UserId;
			assignment.EmployeeId = model.EmployeeId;
			assignment.EmployeeName = model.EmployeeName;
			assignment.CurrentNumber = model.CurrentNumber ?? 0;
			await _db.InsterAsync(assignment);
			await _db.SaveAsync();

			return _mapper.Map<AssignmentCustom>(assignment);
		}

		public async Task<Assignment_SaleCustom> CreateSale(Assignment_SaleCustom model)
		{
			if (string.IsNullOrEmpty(model.CreateByName))
			{
				model.CreateByName = await _repo.User.GetFullNameById(model.CreateBy);
			}

			var assignment_Sale = new Data.Entity.Assignment_Sale();
			assignment_Sale.Status = StatusModel.Active;
			assignment_Sale.CreateDate = DateTime.Now;
			assignment_Sale.CreateBy = model.CreateBy;
			assignment_Sale.CreateByName = model.CreateByName;
			assignment_Sale.AssignmentId = model.AssignmentId;
			assignment_Sale.IsActive = StatusModel.Active;
			assignment_Sale.SaleId = model.SaleId;
			assignment_Sale.Description = model.Description;
			await _db.InsterAsync(assignment_Sale);
			await _db.SaveAsync();

			return _mapper.Map<Assignment_SaleCustom>(assignment_Sale);
		}

		public async Task<bool> CheckAssignmentByUserId(int id)
		{
			return await _repo.Context.Assignments.AnyAsync(x => x.UserId == id);
		}

		public async Task<AssignmentCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Assignments
				.Include(x => x.Assignment_Sales)
				.Include(x => x.User)
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<AssignmentCustom>(query);
		}

		public async Task<AssignmentCustom> GetByUserId(int id)
		{
			var query = await _repo.Context.Assignments
				.Include(x => x.Assignment_Sales)
				.Include(x => x.User)
				.Where(x => x.UserId == id).FirstOrDefaultAsync();
			return _mapper.Map<AssignmentCustom>(query);
		}

		public async Task UpdateCurrentNumber(Guid id)
		{
			var assignment_Sales = await _repo.Context.Assignment_Sales
											  .Include(x => x.Sale)
											  .Where(x => x.AssignmentId == id && x.Sale.StatusSaleId >= StatusSaleModel.WaitContact && x.Sale.StatusSaleId <= StatusSaleModel.CloseSale)
											  .ToListAsync();
			if (assignment_Sales.Count > 0)
			{
				var assignments = await _repo.Context.Assignments.FirstOrDefaultAsync(x => x.Id == id);
				if (assignments != null)
				{
					assignments.CurrentNumber = assignment_Sales.Count;
					_db.Update(assignments);
					await _db.SaveAsync();
				}
			}
		}

	}
}
