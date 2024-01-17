using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class ProcessSales : IProcessSales
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public ProcessSales(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<ProcessSaleCustom> GetById(Guid id)
		{
			var query = await _repo.Context.ProcessSales
				.Include(x => x.ProcessSale_Sections.OrderBy(s => s.SequenceNo))
				.ThenInclude(x => x.ProcessSale_Section_Items.OrderBy(s => s.SequenceNo))
				.ThenInclude(x => x.ProcessSale_Section_ItemOptions.OrderBy(s => s.SequenceNo))
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<ProcessSaleCustom>(query);
		}

		public async Task<ProcessSaleCustom> Update(ProcessSaleCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var processSales = await _repo.Context.ProcessSales.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (processSales != null)
				{
					processSales.UpdateDate = _dateNow;
					processSales.UpdateBy = model.CurrentUserId;
					processSales.SequenceNo = model.SequenceNo;
					processSales.Name = model.Name;
					_db.Update(processSales);
					await _db.SaveAsync();

					//Update Status To Delete All
					var processSaleSections = _repo.Context.ProcessSale_Sections.Where(x => x.ProcessSaleId == processSales.Id).ToList();
					if (processSaleSections.Count > 0)
					{
						foreach (var section in processSaleSections)
						{
							var processSaleSectionItems = _repo.Context.ProcessSale_Section_Items.Where(x => x.PSaleSectionId == section.Id).ToList();
							if (processSaleSectionItems.Count > 0)
							{
								foreach (var section_item in processSaleSectionItems)
								{
									var processSaleSectionItemOptions = _repo.Context.ProcessSale_Section_ItemOptions.Where(x => x.PSaleSectionItemId == section_item.Id).ToList();
									if (processSaleSectionItemOptions.Count > 0)
									{
										foreach (var section_item_option in processSaleSectionItemOptions)
										{
											section_item_option.Status = StatusModel.Delete;
										}
										await _db.SaveAsync();
									}
									section_item.Status = StatusModel.Delete;
								}
								await _db.SaveAsync();
							}
							section.Status = StatusModel.Delete;
						}
						await _db.SaveAsync();
					}

					if (model.ProcessSale_Sections != null)
					{
						foreach (var section in model.ProcessSale_Sections)
						{
							var processSaleSection = await _repo.Context.ProcessSale_Sections
																   .FirstOrDefaultAsync(x => x.ProcessSaleId == processSales.Id && x.Id == section.Id);

							if (processSaleSection != null && section.Status == StatusModel.Active)
							{
								processSaleSection.Status = section.Status;
								processSaleSection.SequenceNo = section.SequenceNo;
								processSaleSection.Name = section.Name;
								processSaleSection.ShowAlways = section.ShowAlways;
								_db.Update(processSaleSection);
							}
							else if (section.Status == StatusModel.Active)
							{
								processSaleSection = new();
								processSaleSection.Status = StatusModel.Active;
								processSaleSection.ProcessSaleId = processSales.Id;
								processSaleSection.SequenceNo = section.SequenceNo;
								processSaleSection.Name = section.Name;
								processSaleSection.ShowAlways = section.ShowAlways;
								await _db.InsterAsync(processSaleSection);
							}
							await _db.SaveAsync();

							if (processSaleSection != null && section.ProcessSale_Section_Items != null)
							{
								foreach (var section_item in section.ProcessSale_Section_Items)
								{
									var processSaleSectionItem = await _repo.Context.ProcessSale_Section_Items
																			 .Where(x => x.PSaleSectionId == processSaleSection.Id && x.Id == section_item.Id)
																			 .FirstOrDefaultAsync();
									if (processSaleSectionItem != null && section_item.Status == StatusModel.Active)
									{
										processSaleSectionItem.Status = section_item.Status;
										processSaleSectionItem.SequenceNo = section_item.SequenceNo;
										processSaleSectionItem.ItemLabel = section_item.ItemLabel;
										processSaleSectionItem.ItemType = section_item.ItemType;
										processSaleSectionItem.Required = section_item.Required;
										_db.Update(processSaleSectionItem);
									}
									else if (section_item.Status == StatusModel.Active)
									{
										processSaleSectionItem = new();
										processSaleSectionItem.Status = StatusModel.Active;
										processSaleSectionItem.PSaleSectionId = processSaleSection.Id;
										processSaleSectionItem.SequenceNo = section_item.SequenceNo;
										processSaleSectionItem.ItemLabel = section_item.ItemLabel;
										processSaleSectionItem.ItemType = section_item.ItemType;
										processSaleSectionItem.Required = section_item.Required;
										await _db.InsterAsync(processSaleSectionItem);
									}
									await _db.SaveAsync();

									if (processSaleSectionItem != null && section_item.ProcessSale_Section_ItemOptions != null)
									{
										foreach (var section_item_option in section_item.ProcessSale_Section_ItemOptions)
										{
											var processSaleSectionItemOption = await _repo.Context.ProcessSale_Section_ItemOptions
																							.Where(x => x.PSaleSectionItemId == processSaleSectionItem.Id && x.Id == section_item_option.Id)
																							.FirstOrDefaultAsync();
											if (processSaleSectionItemOption != null && section_item_option.Status == StatusModel.Active)
											{
												processSaleSectionItemOption.Status = section_item_option.Status;
												processSaleSectionItemOption.OptionLabel = section_item_option.OptionLabel;
												processSaleSectionItemOption.ShowSectionId = section_item_option.ShowSectionId;
												_db.Update(processSaleSectionItemOption);
											}
											else if (section_item_option.Status == StatusModel.Active)
											{
												processSaleSectionItemOption = new();
												processSaleSectionItemOption.Status = StatusModel.Active;
												processSaleSectionItemOption.PSaleSectionItemId = processSaleSectionItem.Id;
												processSaleSectionItemOption.SequenceNo = section_item_option.SequenceNo;
												processSaleSectionItemOption.OptionLabel = section_item_option.OptionLabel;
												processSaleSectionItemOption.DefaultValue = section_item_option.DefaultValue;
												processSaleSectionItemOption.ShowSectionId = section_item_option.ShowSectionId;
												await _db.InsterAsync(processSaleSectionItemOption);
											}
											await _db.SaveAsync();
										}
									}

								}
							}
						}
					}

					_transaction.Commit();
				}

				return _mapper.Map<ProcessSaleCustom>(processSales);
			}
		}

		public async Task<PaginationView<List<ProcessSaleCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.ProcessSales
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.SequenceNo)
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

			return new PaginationView<List<ProcessSaleCustom>>()
			{
				Items = _mapper.Map<List<ProcessSaleCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<ProcessSale_ReplyCustom> CreateReply(ProcessSale_ReplyCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				string? _fullNameUser = string.Empty;
				var users = await _repo.Context.Users.FirstOrDefaultAsync(x => x.Id == model.CurrentUserId);
				if (users != null)
					_fullNameUser = users.FullName;

				string? _processSaleName = string.Empty;
				var processSales = await _repo.Context.ProcessSales.FirstOrDefaultAsync(x => x.Id == model.ProcessSaleId);
				if (processSales != null)
					_processSaleName = processSales.Name;

				ProcessSale_Reply saleReply = new();
				saleReply.Status = StatusModel.Active;
				saleReply.CreateDate = _dateNow;
				saleReply.CreateBy = model.CurrentUserId;
				saleReply.CreateByName = _fullNameUser;
				saleReply.UpdateDate = _dateNow;
				saleReply.UpdateBy = model.CurrentUserId;
				saleReply.UpdateByName = _fullNameUser;
				saleReply.ProcessSaleId = model.ProcessSaleId;
				saleReply.ProcessSaleName = _processSaleName;
				await _db.InsterAsync(saleReply);
				await _db.SaveAsync();

				if (model.ProcessSale_Reply_Sections?.Count > 0)
				{
					foreach (var reply_section in model.ProcessSale_Reply_Sections)
					{
						if (reply_section.IsSave)
						{
							ProcessSale_Reply_Section saleReplySection = new();
							saleReplySection.Status = StatusModel.Active;
							saleReplySection.PSaleReplyId = saleReply.Id;
							saleReplySection.PSaleSectionId = reply_section.PSaleSectionId;
							saleReplySection.Name = reply_section.Name;
							await _db.InsterAsync(saleReplySection);
							await _db.SaveAsync();

							if (reply_section.ProcessSale_Reply_Section_Items?.Count > 0)
							{
								foreach (var reply_section_item in reply_section.ProcessSale_Reply_Section_Items)
								{
									string? _itemLabel = string.Empty;
									string? _itemType = string.Empty;
									var processSaleItem = await _repo.Context.ProcessSale_Section_Items.FirstOrDefaultAsync(x => x.Id == reply_section_item.PSaleSectionItemId);
									if (processSaleItem != null)
									{
										_itemLabel = processSaleItem.ItemLabel;
										_itemType = processSaleItem.ItemType;
									}

									ProcessSale_Reply_Section_Item replySectionItem = new();
									replySectionItem.Status = StatusModel.Active;
									replySectionItem.PSaleReplySectionId = saleReplySection.Id;
									replySectionItem.PSaleSectionItemId = reply_section_item.PSaleSectionItemId;
									replySectionItem.ItemLabel = _itemLabel;
									replySectionItem.ItemType = _itemType;
									await _db.InsterAsync(replySectionItem);
									await _db.SaveAsync();

									if (reply_section_item.ProcessSale_Reply_Section_ItemValues?.Count > 0)
									{
										foreach (var reply_section_value in reply_section_item.ProcessSale_Reply_Section_ItemValues)
										{
											ProcessSale_Reply_Section_ItemValue replySectionItemValue = new();
											replySectionItemValue.Status = StatusModel.Active;
											replySectionItemValue.PSaleReplySectionItemId = replySectionItem.Id;
											replySectionItemValue.PSaleSectionItemOptionId = reply_section_value.PSaleSectionItemOptionId;
											replySectionItemValue.OptionLabel = reply_section_value.OptionLabel;
											replySectionItemValue.ReplyValue = reply_section_value.ReplyValue;
											replySectionItemValue.ReplyDate = reply_section_value.ReplyDate;
											replySectionItemValue.ReplyTime = reply_section_value.ReplyTime;
											replySectionItemValue.FileId = reply_section_value.FileId;
											replySectionItemValue.FileUrl = reply_section_value.FileUrl;
											await _db.InsterAsync(replySectionItemValue);
											await _db.SaveAsync();
										}
									}

								}
							}
						}
					}
				}

				_transaction.Commit();

				return _mapper.Map<ProcessSale_ReplyCustom>(saleReply);
			}
		}

		public Task<ProcessSale_ReplyCustom> UpdateReply(ProcessSale_ReplyCustom model)
		{
			throw new NotImplementedException();
		}

		public async Task<ProcessSale_ReplyCustom> GetReplyById(Guid id)
		{
			var query = await _repo.Context.ProcessSale_Replies
				.Include(x => x.ProcessSale_Reply_Sections).ThenInclude(x => x.ProcessSale_Reply_Section_Items).ThenInclude(x => x.ProcessSale_Reply_Section_ItemValues)
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<ProcessSale_ReplyCustom>(query);
		}

		public async Task<PaginationView<List<ProcessSale_ReplyCustom>>> GetListReply(allFilter model)
		{
			var query = _repo.Context.ProcessSale_Replies
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderByDescending(x => x.UpdateBy).ThenByDescending(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.val1))
			{
				query = query.Where(x => x.ProcessSaleName != null && x.ProcessSaleName.Contains(model.val1));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<ProcessSale_ReplyCustom>>()
			{
				Items = _mapper.Map<List<ProcessSale_ReplyCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}
