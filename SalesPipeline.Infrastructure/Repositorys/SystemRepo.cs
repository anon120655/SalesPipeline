using AutoMapper;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.ManageSystems;
using Microsoft.EntityFrameworkCore;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Customers;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class SystemRepo : ISystemRepo
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public SystemRepo(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<System_SignatureCustom> CreateSignature(System_SignatureCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var systemSignature = new Data.Entity.System_Signature()
				{
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
				};
				await _db.InsterAsync(systemSignature);
				await _db.SaveAsync();

				if (model.Files != null && model.Files.ImgBase64Only != null)
				{
					if (GeneralUtils.IsBase64String(model.Files.ImgBase64Only))
					{
						model.Files.appSet = _appSet;
						model.Files.Id = systemSignature.Id.ToString();
						model.Files.Folder = "systems\\signature";
						var uploadModel = GeneralUtils.UploadImage(model.Files);

						systemSignature.Name = model.Files.FileName;
						systemSignature.ImgUrl = uploadModel.ImageUrl;
						systemSignature.ImgThumbnailUrl = uploadModel.ImageThumbnailUrl;
						_db.Update(systemSignature);
						await _db.SaveAsync();


						var users = await _repo.Context.Users.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == model.CurrentUserId);
						if (users != null)
						{
							users.UrlSignature = systemSignature.ImgUrl;
							_db.Update(users);
							await _db.SaveAsync();
						}
					}
				}

				_transaction.Commit();

				return _mapper.Map<System_SignatureCustom>(systemSignature);
			}
		}

		public async Task<System_SignatureCustom> GetSignatureLast(int userid)
		{
			var query = await _repo.Context.System_Signatures
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.CreateBy == userid);

			return _mapper.Map<System_SignatureCustom>(query);
		}

		public async Task<System_SLACustom> CreateSLA(System_SLACustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var systemSla = new Data.Entity.System_SLA()
				{
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					StatusSlaId = model.StatusSlaId,
					NumberDays = model.NumberDays,
				};
				await _db.InsterAsync(systemSla);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<System_SLACustom>(systemSla);
			}
		}

		public async Task<System_SLACustom> UpdateSLA(System_SLACustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var systemSlas = await _repo.Context.System_SLAs.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (systemSlas != null)
				{
					systemSlas.UpdateDate = _dateNow;
					systemSlas.UpdateBy = model.CurrentUserId;
					systemSlas.StatusSlaId = model.StatusSlaId;
					systemSlas.NumberDays = model.NumberDays;
					_db.Update(systemSlas);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<System_SLACustom>(systemSlas);
			}
		}

		public async Task DeleteSLAById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.System_SLAs.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
			if (query != null)
			{
				query.UpdateDate = DateTime.Now;
				query.UpdateBy = model.userid;
				query.Status = StatusModel.Delete;
				_db.Update(query);
				await _db.SaveAsync();
			}
		}

		public async Task<System_SLACustom> GetSLAById(Guid id)
		{
			var query = await _repo.Context.System_SLAs
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<System_SLACustom>(query);
		}

		public async Task<PaginationView<List<System_SLACustom>>> GetListSLA(allFilter model)
		{
			var query = _repo.Context.System_SLAs.Where(x => x.Status != StatusModel.Delete)
												 .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<System_SLACustom>>()
			{
				Items = _mapper.Map<List<System_SLACustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}
