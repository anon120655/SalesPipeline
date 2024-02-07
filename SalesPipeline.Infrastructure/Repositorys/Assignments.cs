using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
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

		public async Task<bool> CheckAssignmentSaleById(Guid id)
		{
			return await _repo.Context.Assignment_Sales.AnyAsync(x => x.AssignmentId == id);
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
			var currentNumber = await _repo.Context.Assignment_Sales
											  .Include(x => x.Sale)
											  .Where(x => x.AssignmentId == id && x.Sale.StatusSaleId >= StatusSaleModel.WaitContact && x.Sale.StatusSaleId <= StatusSaleModel.CloseSale)
											  .CountAsync();
			if (currentNumber > 0)
			{
				var assignments = await _repo.Context.Assignments.FirstOrDefaultAsync(x => x.Id == id);
				if (assignments != null)
				{
					assignments.CurrentNumber = currentNumber;
					_db.Update(assignments);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<PaginationView<List<AssignmentCustom>>> GetListAutoAssign(allFilter model)
		{
			//*************** ต้องเช็คพวก สาขา จังหวัด อำเภอ เพิ่มเติม ****************

			//เรียงจากลูกค้าที่ดูแลปัจจุบัน น้อย --> มาก
			var query = _repo.Context.Assignments.Where(x => x.Status != StatusModel.Delete)
												 //.Include(x => x.Assignment_Sales).ThenInclude(x => x.Sale).ThenInclude(x => x.Customer)
												 .Include(x => x.User).ThenInclude(x => x.Branch)
												 .OrderBy(x => x.CurrentNumber).ThenBy(x=>x.CreateDate)
												 .AsQueryable();

			if (!String.IsNullOrEmpty(model.emp_id))
			{
				query = query.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(model.emp_id));
			}

			if (!String.IsNullOrEmpty(model.emp_name))
			{
				query = query.Where(x => x.EmployeeName != null && x.EmployeeName.Contains(model.emp_name));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var userAssignment = await query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();

			List<AssignmentCustom> responseItems = new();

			//ลูกค้าที่ยังไม่ถูกมอบหมาย
			var salesCustomer = await _repo.Context.Sales
				.Include(x => x.Customer)
				.Where(x => x.Status != StatusModel.Delete && !x.AssignedUserId.HasValue && x.StatusSaleId == StatusSaleModel.WaitAssign)
				.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
				.ToListAsync();

			if (salesCustomer.Count > 0 && userAssignment.Count > 0)
			{
				//แยกรายการลูกค้าที่ยังไม่ถูกมอบหมายออกเป็นส่วนเท่าๆ กัน
				var partitionCustomer = GeneralUtils.PartitionList(salesCustomer, userAssignment.Count);

				if (partitionCustomer.Length > 0)
				{
					int index_path = 0;
					foreach (var item_path in partitionCustomer)
					{
						//มอบหมายให้พนักงานเท่าๆ กัน
						var assignment = _mapper.Map<AssignmentCustom>(userAssignment[index_path]);
						assignment.Assignment_Sales = new();
						assignment.User = null;

						foreach (var item_sales in item_path)
						{
							assignment.Assignment_Sales.Add(new()
							{
								Id = Guid.NewGuid(),
								Status = StatusModel.Active,
								AssignmentId = assignment.Id,
								SaleId = item_sales.Id,
								IsActive = StatusModel.Active,
								IsSelect = true,
								IsSelectMove = false,
								Sale = _mapper.Map<SaleCustom>(item_sales)
							});
						}
						responseItems.Add(assignment);
						index_path++;
					}
				}
			}

			return new PaginationView<List<AssignmentCustom>>()
			{
				Items = responseItems,
				Pager = pager
			};
		}

		public async Task Assign(List<AssignmentCustom> model)
		{
			foreach (var item in model)
			{
				var assignments = await _repo.Context.Assignments.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == item.Id);

				var sales_select = item.Assignment_Sales?.Where(x => x.IsSelect).ToList();

				if (assignments != null && sales_select?.Count > 0)
				{
					foreach (var item_sale in sales_select)
					{
						var currentUserName = await _repo.User.GetFullNameById(item.CurrentUserId);
						var assignedUserName = await _repo.User.GetFullNameById(assignments.UserId);

						var assignmentSale = await CreateSale(new()
						{
							CreateBy = item.CurrentUserId,
							CreateByName = currentUserName,
							AssignmentId = assignments.Id,
							SaleId = item_sale.SaleId
						});

						var sales = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == item_sale.SaleId);
						if (sales != null)
						{
							sales.AssignedUserId = assignments.UserId;
							sales.AssignedUserName = assignedUserName;
							_db.Update(sales);
							await _db.SaveAsync();
						}

						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = item_sale.SaleId,
							StatusId = StatusSaleModel.WaitContact,
							CreateBy = item.CurrentUserId,
							CreateByName = currentUserName,
						});
					}

					await UpdateCurrentNumber(assignments.Id);
				}

			}
		}

	}
}
