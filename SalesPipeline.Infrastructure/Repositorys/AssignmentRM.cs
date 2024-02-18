using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class AssignmentRM : IAssignmentRM
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public AssignmentRM(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Assignment_RMCustom> Create(Assignment_RMCustom model)
		{
			if (await CheckAssignmentByUserId(model.UserId))
				throw new ExceptionCustom("assignmentRM duplicate user");

			if (string.IsNullOrEmpty(model.EmployeeName))
			{
				model.EmployeeName = await _repo.User.GetFullNameById(model.UserId);
			}

			var assignment_RM = new Data.Entity.Assignment_RM();
			assignment_RM.Status = StatusModel.Active;
			assignment_RM.CreateDate = DateTime.Now;
			assignment_RM.UserId = model.UserId;
			assignment_RM.EmployeeId = model.EmployeeId;
			assignment_RM.EmployeeName = model.EmployeeName;
			assignment_RM.CurrentNumber = model.CurrentNumber ?? 0;
			await _db.InsterAsync(assignment_RM);
			await _db.SaveAsync();

			return _mapper.Map<Assignment_RMCustom>(assignment_RM);
		}

		public async Task<Assignment_RM_SaleCustom> CreateSale(Assignment_RM_SaleCustom model)
		{
			if (string.IsNullOrEmpty(model.CreateByName))
			{
				model.CreateByName = await _repo.User.GetFullNameById(model.CreateBy);
			}

			var assignment_RM_Sale = new Data.Entity.Assignment_RM_Sale();
			assignment_RM_Sale.Status = StatusModel.Active;
			assignment_RM_Sale.CreateDate = DateTime.Now;
			assignment_RM_Sale.CreateBy = model.CreateBy;
			assignment_RM_Sale.CreateByName = model.CreateByName;
			assignment_RM_Sale.AssignmentRMId = model.AssignmentRMId;
			assignment_RM_Sale.IsActive = StatusModel.Active;
			assignment_RM_Sale.SaleId = model.SaleId;
			assignment_RM_Sale.Description = model.Description;
			await _db.InsterAsync(assignment_RM_Sale);
			await _db.SaveAsync();

			return _mapper.Map<Assignment_RM_SaleCustom>(assignment_RM_Sale);
		}

		public async Task<bool> CheckAssignmentByUserId(int id)
		{
			return await _repo.Context.Assignment_RMs.AnyAsync(x => x.UserId == id);
		}

		public async Task<bool> CheckAssignmentSaleById(Guid id)
		{
			return await _repo.Context.Assignment_RM_Sales.AnyAsync(x => x.AssignmentRMId == id);
		}

		public async Task<Assignment_RMCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Assignment_RMs
				.Include(x => x.Assignment_RM_Sales)
				.Include(x => x.User)
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<Assignment_RMCustom>(query);
		}

		public async Task<Assignment_RMCustom> GetByUserId(int id)
		{
			var query = await _repo.Context.Assignment_RMs
				.Include(x => x.Assignment_RM_Sales)
				.Include(x => x.User)
				.Where(x => x.UserId == id).FirstOrDefaultAsync();
			return _mapper.Map<Assignment_RMCustom>(query);
		}

		public async Task UpdateCurrentNumber(Guid id)
		{
			var currentNumber = await _repo.Context.Assignment_RM_Sales
											  .Include(x => x.Sale)
											  .Where(x => x.AssignmentRMId == id && x.Status == StatusModel.Active && x.IsActive == StatusModel.Active
													&& x.Sale.StatusSaleId >= StatusSaleModel.WaitContact && x.Sale.StatusSaleId <= StatusSaleModel.CloseSale)
											  .CountAsync();
			if (currentNumber > 0)
			{
				var assignment_RMs = await _repo.Context.Assignment_RMs.FirstOrDefaultAsync(x => x.Id == id);
				if (assignment_RMs != null)
				{
					assignment_RMs.CurrentNumber = currentNumber;
					_db.Update(assignment_RMs);
					await _db.SaveAsync();
				}
				//**** ต้อง update ผู้จัดการศูนย์ด้วย
				//var assignments = await _repo.Context.Assignments.FirstOrDefaultAsync(x => x.Id == id);
				//if (assignments != null)
				//{
				//	assignments.CurrentNumber = currentNumber;
				//	_db.Update(assignments);
				//	await _db.SaveAsync();
				//}
			}
		}

		public async Task<PaginationView<List<Assignment_RMCustom>>> GetListAutoAssign(allFilter model)
		{
			//*************** ต้องเช็คพวก สาขา จังหวัด อำเภอ เพิ่มเติม ****************

			//รูปแบบการมอบหมายตามเกณฑ์
			//1. เรียงจากลูกค้าที่ดูแลปัจจุบัน น้อย --> มาก
			//2. หาลูกค้าที่ยังไม่ถูกมอบหมาย
			//3. แยกรายการลูกค้าที่ยังไม่ถูกมอบหมายออกเป็นส่วนเท่าๆ กัน
			//4. มอบหมายให้พนักงานเท่าๆ กัน  (พนักงานที่ดูแลลูกค้าน้อยสุดจะถูกมอบหมายก่อนเรียงลำดับไปเรื่อยๆ)

			Guid? assignmentId = null;
			if (model.assigncenter.HasValue)
			{
				var assignments = await _repo.Context.Assignments.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.UserId == model.assigncenter);
				if (assignments != null)
				{
					assignmentId = assignments.Id;
				}
			}

			//เรียงจากลูกค้าที่ดูแลปัจจุบัน น้อย --> มาก
			var query = _repo.Context.Assignment_RMs.Where(x => x.Status != StatusModel.Delete)
												 .Include(x => x.User).ThenInclude(x => x.Branch)
												 .OrderBy(x => x.CurrentNumber).ThenBy(x => x.CreateDate)
												 .AsQueryable();

			if (assignmentId.HasValue)
			{
				query = query.Where(x=>x.AssignmentId == assignmentId);
			}

			if (!String.IsNullOrEmpty(model.emp_id))
			{
				query = query.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(model.emp_id));
			}

			if (!String.IsNullOrEmpty(model.emp_name))
			{
				query = query.Where(x => x.EmployeeName != null && x.EmployeeName.Contains(model.emp_name));
			}

			if (!String.IsNullOrEmpty(model.province))
			{
				//ยังไม่ confirm เรื่องจังหวัดและอำเภอที่ดูแล
			}

			if (!String.IsNullOrEmpty(model.amphur))
			{
				//ยังไม่ confirm เรื่องจังหวัดและอำเภอที่ดูแล
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var userAssignment = await query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();

			List<Assignment_RMCustom> responseItems = new();

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
						var assignment_RM = _mapper.Map<Assignment_RMCustom>(userAssignment[index_path]);
						assignment_RM.Assignment_RM_Sales = new();
						assignment_RM.Tel = assignment_RM.User?.Tel;
						assignment_RM.ProvinceName = assignment_RM.User?.Branch?.Name;
						assignment_RM.AmphurName = "-";

						foreach (var item_sales in item_path)
						{
							assignment_RM.Assignment_RM_Sales.Add(new()
							{
								Id = Guid.NewGuid(),
								Status = StatusModel.Active,
								AssignmentRMId = assignment_RM.Id,
								SaleId = item_sales.Id,
								IsActive = StatusModel.Active,
								IsSelect = true,
								IsSelectMove = false,
								Sale = _mapper.Map<SaleCustom>(item_sales)
							});
						}

						assignment_RM.User = null;
						responseItems.Add(assignment_RM);
						index_path++;
					}
				}
			}

			return new PaginationView<List<Assignment_RMCustom>>()
			{
				Items = responseItems,
				Pager = pager
			};
		}

		public async Task<PaginationView<List<Assignment_RMCustom>>> GetListRM(allFilter model)
		{
			var query = _repo.Context.Assignment_RMs.Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.CurrentNumber).ThenBy(x => x.CreateDate)
												 .AsQueryable();

			if (!String.IsNullOrEmpty(model.emp_id))
			{
				query = query.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(model.emp_id));
			}

			if (!String.IsNullOrEmpty(model.emp_name))
			{
				query = query.Where(x => x.EmployeeName != null && x.EmployeeName.Contains(model.emp_name));
			}

			if (!String.IsNullOrEmpty(model.province))
			{
				//ยังไม่ confirm เรื่องจังหวัดและอำเภอที่ดูแล
			}

			if (!String.IsNullOrEmpty(model.amphur))
			{
				//ยังไม่ confirm เรื่องจังหวัดและอำเภอที่ดูแล
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Assignment_RMCustom>>()
			{
				Items = _mapper.Map<List<Assignment_RMCustom>>(await items.ToListAsync()),
				Pager = pager
			};

		}

		public async Task Assign(List<Assignment_RMCustom> model)
		{
			foreach (var item in model)
			{
				var assignment_RM = await _repo.Context.Assignment_RMs.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == item.Id);

				var sales_select = item.Assignment_RM_Sales?.Where(x => x.IsSelect).ToList();

				if (assignment_RM != null && sales_select?.Count > 0)
				{
					foreach (var item_sale in sales_select)
					{
						var currentUserName = await _repo.User.GetFullNameById(item.CurrentUserId);
						var assignedUserName = await _repo.User.GetFullNameById(assignment_RM.UserId);

						var assignmentRMSale = await CreateSale(new()
						{
							CreateBy = item.CurrentUserId,
							CreateByName = currentUserName,
							AssignmentRMId = assignment_RM.Id,
							SaleId = item_sale.SaleId
						});

						var sales = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == item_sale.SaleId);
						if (sales != null)
						{
							sales.AssignedUserId = assignment_RM.UserId;
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

					await UpdateCurrentNumber(assignment_RM.Id);
				}

			}
		}

		public async Task AssignChange(AssignChangeModel model)
		{
			var assignments_RM_sales = await _repo.Context.Assignment_RM_Sales
				.Include(s => s.AssignmentRM)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.IsActive == StatusModel.Active && x.SaleId == model.Original.Id);

			if (assignments_RM_sales == null) throw new ExceptionCustom("Assignment_RM something went wrong!");

			assignments_RM_sales.Status = StatusModel.InActive;
			assignments_RM_sales.IsActive = StatusModel.InActive;
			assignments_RM_sales.AssignmentRM.CurrentNumber = assignments_RM_sales.AssignmentRM.CurrentNumber - 1;
			_db.Update(assignments_RM_sales);
			await _db.SaveAsync();

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);
			var assignedUserName = await _repo.User.GetFullNameById(model.New.UserId);

			var assignmentSale = await CreateSale(new()
			{
				CreateBy = model.CurrentUserId,
				CreateByName = currentUserName,
				AssignmentRMId = model.New.Id,
				SaleId = model.Original.Id
			});

			var sales = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.Original.Id);
			if (sales != null)
			{
				sales.AssignedUserId = model.New.UserId;
				sales.AssignedUserName = assignedUserName;
				_db.Update(sales);
				await _db.SaveAsync();
			}

			await UpdateCurrentNumber(model.New.Id);

		}

	}
}
