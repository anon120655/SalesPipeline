using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class PreCal : IPreCal
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public PreCal(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Pre_CalCustom> Validate(Pre_CalCustom model)
		{
			var pre_Cal_Info = await _repo.Context.Pre_Cals
				.Where(x => x.Status == StatusModel.Active && x.Id != model.Id
				&& x.Master_Pre_Applicant_LoanId == model.Master_Pre_Applicant_LoanId
				&& x.Master_Pre_BusinessTypeId == model.Master_Pre_BusinessTypeId).FirstOrDefaultAsync();
			if (pre_Cal_Info != null)
			{
				throw new ExceptionCustom("ประเภทผู้ขอสินเชื่อและประเภทธุรกิจ มีในระบบแล้ว!");
			}

			return model;
		}

		public async Task<Pre_CalCustom> Create(Pre_CalCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				string? master_Pre_Applicant_LoanIdName = null;
				string? master_Pre_BusinessTypeName = null;

				if (model.Master_Pre_Applicant_LoanId.HasValue)
				{
					master_Pre_Applicant_LoanIdName = await _repo.Master_Pre_App_Loan.GetNameById(model.Master_Pre_Applicant_LoanId.Value);
				}
				if (model.Master_Pre_BusinessTypeId.HasValue)
				{
					master_Pre_BusinessTypeName = await _repo.Master_Pre_BusType.GetNameById(model.Master_Pre_BusinessTypeId.Value);
				}

				DateTime _dateNow = DateTime.Now;

				var pre_Cal = new Data.Entity.Pre_Cal();
				pre_Cal.Status = StatusModel.Active;
				pre_Cal.CreateDate = _dateNow;
				pre_Cal.CreateBy = model.CurrentUserId;
				pre_Cal.UpdateDate = _dateNow;
				pre_Cal.UpdateBy = model.CurrentUserId;
				pre_Cal.Name = $"{master_Pre_Applicant_LoanIdName},{master_Pre_BusinessTypeName}";
				pre_Cal.Master_Pre_Applicant_LoanId = model.Master_Pre_Applicant_LoanId;
				pre_Cal.Master_Pre_Applicant_LoanName = master_Pre_Applicant_LoanIdName;
				pre_Cal.Master_Pre_BusinessTypeId = model.Master_Pre_BusinessTypeId;
				pre_Cal.Master_Pre_BusinessTypeName = master_Pre_BusinessTypeName;
				pre_Cal.DisplayResultType = model.DisplayResultType;
				await _db.InsterAsync(pre_Cal);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Pre_CalCustom>(pre_Cal);
			}
		}

		public async Task<Pre_CalCustom> Update(Pre_CalCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				string? master_Pre_Applicant_LoanIdName = null;
				string? master_Pre_BusinessTypeName = null;

				if (model.Master_Pre_Applicant_LoanId.HasValue)
				{
					master_Pre_Applicant_LoanIdName = await _repo.Master_Pre_App_Loan.GetNameById(model.Master_Pre_Applicant_LoanId.Value);
				}
				if (model.Master_Pre_BusinessTypeId.HasValue)
				{
					master_Pre_BusinessTypeName = await _repo.Master_Pre_BusType.GetNameById(model.Master_Pre_BusinessTypeId.Value);
				}

				DateTime _dateNow = DateTime.Now;
				var pre_Cal = await _repo.Context.Pre_Cals.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (pre_Cal != null)
				{
					pre_Cal.UpdateDate = _dateNow;
					pre_Cal.UpdateBy = model.CurrentUserId;
					pre_Cal.Name = $"{master_Pre_Applicant_LoanIdName},{master_Pre_BusinessTypeName}";
					pre_Cal.Master_Pre_Applicant_LoanId = model.Master_Pre_Applicant_LoanId;
					pre_Cal.Master_Pre_Applicant_LoanName = master_Pre_Applicant_LoanIdName;
					pre_Cal.Master_Pre_BusinessTypeId = model.Master_Pre_BusinessTypeId;
					pre_Cal.Master_Pre_BusinessTypeName = master_Pre_BusinessTypeName;
					pre_Cal.DisplayResultType = model.DisplayResultType;
					_db.Update(pre_Cal);
					await _db.SaveAsync();
				}

				_transaction.Commit();

				return _mapper.Map<Pre_CalCustom>(pre_Cal);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Pre_Cals.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
			if (query != null)
			{
				query.UpdateDate = DateTime.Now;
				query.UpdateBy = model.userid;
				query.Status = StatusModel.Delete;
				_db.Update(query);
				await _db.SaveAsync();
			}
		}

		public async Task UpdateStatusById(UpdateModel model)
		{
			if (model != null && Boolean.TryParse(model.value, out bool parsedValue))
			{
				var _status = parsedValue ? (short)1 : (short)0;
				Guid id = Guid.Parse(model.id);
				var query = await _repo.Context.Pre_Cals.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateBy = model.userid;
					query.Status = _status;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<Pre_CalCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Pre_Cals
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Pre_CalCustom>(query);
		}

		public async Task<Pre_CalCustom> GetIncludeAllById(Guid id)
		{
			var query = await _repo.Context.Pre_Cals
										 .Include(x => x.Pre_Cal_Infos).ThenInclude(x => x.Pre_Cal_Info_Scores.OrderBy(o => o.SequenceNo))
										 .Include(x => x.Pre_Cal_Fetu_Stans).ThenInclude(x => x.Pre_Cal_Fetu_Stan_ItemOptions.OrderBy(o => o.SequenceNo))
										 .Include(x => x.Pre_Cal_Fetu_Stans).ThenInclude(x => x.Pre_Cal_Fetu_Stan_Scores.OrderBy(o => o.SequenceNo))
										 .Include(x => x.Pre_Cal_Fetu_Apps).ThenInclude(x => x.Pre_Cal_Fetu_App_Items.OrderBy(o => o.SequenceNo)).ThenInclude(x => x.Pre_Cal_Fetu_App_Item_Scores)
										 .Include(x => x.Pre_Cal_Fetu_Bus).ThenInclude(x => x.Pre_Cal_Fetu_Bus_Items.OrderBy(o => o.SequenceNo)).ThenInclude(x => x.Pre_Cal_Fetu_Bus_Item_Scores)
										 .Include(x => x.Pre_Cal_WeightFactors).ThenInclude(x => x.Pre_Cal_WeightFactor_Items.OrderBy(o => o.SequenceNo))
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Pre_CalCustom>(query);
		}

		public async Task<Pre_CalCustom> GetCalByAppBusId(Guid appid, Guid busid)
		{
			var query = await _repo.Context.Pre_Cals
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Master_Pre_Applicant_LoanId == appid && x.Master_Pre_BusinessTypeId == busid);

			if (query == null)
			{
				throw new ExceptionCustom($"ไม่พบตัวแปรคำนวณ ประเภทผู้ขอสินเชื่อ และประเภทธุรกิจที่ท่านเลือก");
			}

			var pre_Cal_Fetu_Stans = await _repo.Context.Pre_Cal_Fetu_Stans.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Pre_CalId == query.Id);
			if(pre_Cal_Fetu_Stans == null) throw new ExceptionCustom($"ข้อมูลคุณสมบัติมารตฐานไม่สมบูรณ์");

			var pre_Cal_Fetu_Apps = await _repo.Context.Pre_Cal_Fetu_Apps.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Pre_CalId == query.Id);
			if (pre_Cal_Fetu_Apps == null) throw new ExceptionCustom($"ข้อมูลคุณสมบัติตามประเภทผู้ขอไม่สมบูรณ์");

			var pre_Cal_Fetu_Bus = await _repo.Context.Pre_Cal_Fetu_Bus.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Pre_CalId == query.Id);
			if (pre_Cal_Fetu_Bus == null) throw new ExceptionCustom($"ข้อมูลคุณสมบัติตามประเภทธุรกิจไม่สมบูรณ์");

			return _mapper.Map<Pre_CalCustom>(query);
		}

		public async Task<PaginationView<List<Pre_CalCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Pre_Cals
									 .Where(x => x.Status != StatusModel.Delete)
									 .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
									 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.val1))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.val1));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Pre_CalCustom>>()
			{
				Items = _mapper.Map<List<Pre_CalCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}
