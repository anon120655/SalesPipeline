using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Sales;
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
				.Include(x => x.ProcessSale_Sections.Where(x => x.Status == StatusModel.Active).OrderBy(s => s.SequenceNo))
				.ThenInclude(x => x.ProcessSale_Section_Items.Where(x => x.Status == StatusModel.Active).OrderBy(s => s.SequenceNo))
				.ThenInclude(x => x.ProcessSale_Section_ItemOptions.Where(x => x.Status == StatusModel.Active).OrderBy(s => s.SequenceNo))
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
								processSaleSection.Id = section.Id;
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
										processSaleSectionItem.ShowType = section_item.ShowType;
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
										processSaleSectionItem.ShowType = section_item.ShowType;
										await _db.InsterAsync(processSaleSectionItem);
									}
									await _db.SaveAsync();

									if (processSaleSectionItem != null && section_item.ProcessSale_Section_ItemOptions != null)
									{
										foreach (var section_item_option in section_item.ProcessSale_Section_ItemOptions)
										{
											if (section_item_option.Master_ListId.HasValue)
											{
												section_item_option.OptionLabel = _repo.Context.Master_Lists
																					   .FirstOrDefault(x => x.Id == section_item_option.Master_ListId)?.Name;
											}


											var processSaleSectionItemOption = await _repo.Context.ProcessSale_Section_ItemOptions
																							.Where(x => x.PSaleSectionItemId == processSaleSectionItem.Id && x.Id == section_item_option.Id)
																							.FirstOrDefaultAsync();
											if (processSaleSectionItemOption != null && section_item_option.Status == StatusModel.Active)
											{
												processSaleSectionItemOption.Status = section_item_option.Status;
												processSaleSectionItemOption.OptionLabel = section_item_option.OptionLabel;
												processSaleSectionItemOption.ShowSectionId = section_item_option.ShowSectionId;
												processSaleSectionItemOption.Master_ListId = section_item_option.Master_ListId;
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
												processSaleSectionItemOption.Master_ListId = section_item_option.Master_ListId;
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

		public async Task<Sale_ReplyCustom> CreateReply(Sale_ReplyCustom model)
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
				if (processSales == null) throw new ExceptionCustom("ProcessSaleId not found.");
				_processSaleName = processSales.Name;

				if (processSales.Code == ProcessSaleCodeModel.Contact)
				{
					if (model.Sale_Contact == null) throw new ExceptionCustom("Sale_Contact model not found.");

					model.Sale_Contact.SaleId = model.SaleId;
					model.Sale_Contact.CurrentUserId = model.CurrentUserId;
					await CreateContact(model.Sale_Contact);
				}
				else if (processSales.Code == ProcessSaleCodeModel.Meet)
				{
					if (model.Sale_Meet == null) throw new ExceptionCustom("Sale_Meet model not found.");

					model.Sale_Meet.SaleId = model.SaleId;
					model.Sale_Meet.CurrentUserId = model.CurrentUserId;
					await CreateMeet(model.Sale_Meet);
				}
				else if (processSales.Code == ProcessSaleCodeModel.Document)
				{
					if (model.Sale_Document == null) throw new ExceptionCustom("Sale_Document model not found.");

					model.Sale_Document.SaleId = model.SaleId;
					model.Sale_Document.CurrentUserId = model.CurrentUserId;
					await CreateDocument(model.Sale_Document);
				}
				else if (processSales.Code == ProcessSaleCodeModel.Result)
				{
					if (model.Sale_Result == null) throw new ExceptionCustom("Sale_Result model not found.");

					model.Sale_Result.SaleId = model.SaleId;
					model.Sale_Result.CurrentUserId = model.CurrentUserId;
					await CreateResult(model.Sale_Result);
				}
				else if (processSales.Code == ProcessSaleCodeModel.CloseSale && model.Sale_Close_Sale != null)
				{
					if (model.Sale_Close_Sale == null) throw new ExceptionCustom("Sale_Close_Sale model not found.");

					model.Sale_Close_Sale.SaleId = model.SaleId;
					model.Sale_Close_Sale.CurrentUserId = model.CurrentUserId;
					await CreateCloseSale(model.Sale_Close_Sale);
				}

				Sale_Reply saleReply = new();
				saleReply.Status = StatusModel.Active;
				saleReply.CreateDate = _dateNow;
				saleReply.CreateBy = model.CurrentUserId;
				saleReply.CreateByName = _fullNameUser;
				saleReply.UpdateDate = _dateNow;
				saleReply.UpdateBy = model.CurrentUserId;
				saleReply.UpdateByName = _fullNameUser;
				saleReply.SaleId = model.SaleId;
				saleReply.ProcessSaleId = model.ProcessSaleId;
				saleReply.ProcessSaleName = _processSaleName;
				await _db.InsterAsync(saleReply);
				await _db.SaveAsync();

				if (model.Sale_Reply_Sections?.Count > 0)
				{
					foreach (var reply_section in model.Sale_Reply_Sections)
					{
						if (reply_section.IsSave)
						{
							string? _replySectionName = string.Empty;
							var processSale_Section = await _repo.Context.ProcessSale_Sections.FirstOrDefaultAsync(x => x.Id == reply_section.PSaleSectionId);
							if (processSale_Section == null) throw new ExceptionCustom("PSaleSectionId id not found.");
							_replySectionName = processSale_Section.Name;

							Sale_Reply_Section saleReplySection = new();
							saleReplySection.Status = StatusModel.Active;
							saleReplySection.SaleReplyId = saleReply.Id;
							saleReplySection.PSaleSectionId = reply_section.PSaleSectionId;
							saleReplySection.Name = _replySectionName;
							await _db.InsterAsync(saleReplySection);
							await _db.SaveAsync();

							if (reply_section.Sale_Reply_Section_Items?.Count > 0)
							{
								foreach (var reply_section_item in reply_section.Sale_Reply_Section_Items)
								{
									string? _itemLabel = string.Empty;
									string? _itemType = string.Empty;
									var processSaleItem = await _repo.Context.ProcessSale_Section_Items.FirstOrDefaultAsync(x => x.Id == reply_section_item.PSaleSectionItemId);
									if (processSaleItem != null)
									{
										_itemLabel = processSaleItem.ItemLabel;
										_itemType = processSaleItem.ItemType;
									}

									Sale_Reply_Section_Item replySectionItem = new();
									replySectionItem.Status = StatusModel.Active;
									replySectionItem.SaleReplySectionId = saleReplySection.Id;
									replySectionItem.PSaleSectionItemId = reply_section_item.PSaleSectionItemId;
									replySectionItem.ItemLabel = _itemLabel;
									replySectionItem.ItemType = _itemType;
									await _db.InsterAsync(replySectionItem);
									await _db.SaveAsync();

									if (reply_section_item.Sale_Reply_Section_ItemValues?.Count > 0)
									{
										foreach (var reply_section_value in reply_section_item.Sale_Reply_Section_ItemValues)
										{
											string? _optionLabel = reply_section_value.OptionLabel;
											string? _replyName = reply_section_value.ReplyName;

											if (_itemType == FieldTypes.Dropdown && reply_section_value.PSaleSectionItemOptionId != Guid.Empty)
											{
												var pSection_ItemOption = await _repo.Context.ProcessSale_Section_ItemOptions.FirstOrDefaultAsync(x => x.Id == reply_section_value.PSaleSectionItemOptionId);
												if (pSection_ItemOption != null)
												{
													_optionLabel = pSection_ItemOption.OptionLabel;
												}
											}
											if (_itemType == FieldTypes.DropdownMaster && reply_section_value.Master_ListId.HasValue)
											{
												var master_Lists = await _repo.Context.Master_Lists.FirstOrDefaultAsync(x => x.Id == reply_section_value.Master_ListId);
												if (master_Lists == null) throw new ExceptionCustom("replyValue not match master_ListId.");
												_optionLabel = master_Lists.Name;

												if (!String.IsNullOrEmpty(reply_section_value.ReplyValue))
												{
													if (Guid.TryParse(reply_section_value.ReplyValue, out Guid master_id))
													{
														_replyName = await GetReplyName(master_id, master_Lists.Path);
													}
													else
													{
														throw new ExceptionCustom("Master not match replyValue.");
													}
												}
											}

											string? _fileUrl = string.Empty;
											if (reply_section_value.FileId.HasValue)
											{
												var fileUploads = await _repo.Context.FileUploads.FirstOrDefaultAsync(x => x.Id == reply_section_value.FileId);
												if (fileUploads == null) throw new ExceptionCustom("FileId not found.");
												_fileUrl = fileUploads.Url;
											}

											Sale_Reply_Section_ItemValue replySectionItemValue = new();
											replySectionItemValue.Status = StatusModel.Active;
											replySectionItemValue.SaleReplySectionItemId = replySectionItem.Id;
											replySectionItemValue.PSaleSectionItemOptionId = reply_section_value.PSaleSectionItemOptionId;
											replySectionItemValue.OptionLabel = _optionLabel;
											replySectionItemValue.ReplyValue = reply_section_value.ReplyValue;
											replySectionItemValue.ReplyName = _replyName;
											replySectionItemValue.ReplyDate = reply_section_value.ReplyDate;
											replySectionItemValue.ReplyTime = reply_section_value.ReplyTime;
											replySectionItemValue.FileId = reply_section_value.FileId;
											replySectionItemValue.FileUrl = _fileUrl;
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

				return _mapper.Map<Sale_ReplyCustom>(saleReply);
			}
		}

		public async Task<Sale_ReplyCustom> UpdateReply(Sale_ReplyCustom model)
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

				var saleReply = await _repo.Context.Sale_Replies.FirstOrDefaultAsync(x => x.Id == model.Id);
				if (saleReply == null) throw new ExceptionCustom("id not found.");

				saleReply.UpdateDate = _dateNow;
				saleReply.UpdateBy = model.CurrentUserId;
				saleReply.UpdateByName = _fullNameUser;
				saleReply.ProcessSaleName = _processSaleName;
				_db.Update(saleReply);
				await _db.SaveAsync();

				//Update Status To Delete All
				var sale_Reply_Sections_Remove = _repo.Context.Sale_Reply_Sections
					.Include(x => x.Sale_Reply_Section_Items.Where(x => x.Status == StatusModel.Active))
					.ThenInclude(x => x.Sale_Reply_Section_ItemValues.Where(x => x.Status == StatusModel.Active))
					.Where(x => x.SaleReplyId == model.Id).ToList();
				if (sale_Reply_Sections_Remove.Count > 0)
				{
					foreach (var reply_sec in sale_Reply_Sections_Remove)
					{
						reply_sec.Status = StatusModel.Delete;
						if (reply_sec.Sale_Reply_Section_Items.Count > 0)
						{
							foreach (var reply_sec_item in reply_sec.Sale_Reply_Section_Items)
							{
								reply_sec_item.Status = StatusModel.Delete;
								if (reply_sec_item.Sale_Reply_Section_ItemValues.Count > 0)
								{
									foreach (var reply_sec_item_val in reply_sec_item.Sale_Reply_Section_ItemValues)
									{
										reply_sec_item_val.Status = StatusModel.Delete;
									}
									await _db.SaveAsync();
								}
							}
							await _db.SaveAsync();
						}
					}
					await _db.SaveAsync();
				}

				if (model.Sale_Reply_Sections?.Count > 0)
				{
					foreach (var reply_section in model.Sale_Reply_Sections)
					{
						if (reply_section.IsSave && reply_section.Status == StatusModel.Active)
						{
							int CRUD = CRUDModel.Update;

							string? _replySectionName = string.Empty;
							var processSale_Section = await _repo.Context.ProcessSale_Sections.FirstOrDefaultAsync(x => x.Id == reply_section.PSaleSectionId);
							if (processSale_Section == null) throw new ExceptionCustom("PSaleSectionId id not found.");
							_replySectionName = processSale_Section.Name;

							var saleReplySection = await _repo.Context.Sale_Reply_Sections.FirstOrDefaultAsync(x => x.Id == reply_section.Id);
							if (saleReplySection == null)
							{
								CRUD = CRUDModel.Create;
								saleReplySection = new();
							}

							saleReplySection.Status = reply_section.Status;
							saleReplySection.SaleReplyId = saleReply.Id;
							saleReplySection.PSaleSectionId = reply_section.PSaleSectionId;
							saleReplySection.Name = _replySectionName;
							if (CRUD == CRUDModel.Create)
							{
								await _db.InsterAsync(saleReplySection);
							}
							else
							{
								_db.Update(saleReplySection);
							}
							await _db.SaveAsync();

							if (reply_section.Sale_Reply_Section_Items?.Count > 0)
							{
								foreach (var reply_section_item in reply_section.Sale_Reply_Section_Items)
								{
									if (reply_section_item.Status == StatusModel.Active)
									{
										CRUD = CRUDModel.Update;
										string? _itemLabel = string.Empty;
										string? _itemType = string.Empty;
										var processSaleItem = await _repo.Context.ProcessSale_Section_Items.FirstOrDefaultAsync(x => x.Id == reply_section_item.PSaleSectionItemId);
										if (processSaleItem != null)
										{
											_itemLabel = processSaleItem.ItemLabel;
											_itemType = processSaleItem.ItemType;
										}

										var replySectionItem = await _repo.Context.Sale_Reply_Section_Items.FirstOrDefaultAsync(x => x.Id == reply_section_item.Id);
										if (replySectionItem == null)
										{
											CRUD = CRUDModel.Create;
											replySectionItem = new();
										}

										replySectionItem.Status = reply_section_item.Status;
										replySectionItem.SaleReplySectionId = saleReplySection.Id;
										replySectionItem.PSaleSectionItemId = reply_section_item.PSaleSectionItemId;
										replySectionItem.ItemLabel = _itemLabel;
										replySectionItem.ItemType = _itemType;
										if (CRUD == CRUDModel.Create)
										{
											await _db.InsterAsync(replySectionItem);
										}
										else
										{
											_db.Update(replySectionItem);
										}
										await _db.SaveAsync();

										if (reply_section_item.Sale_Reply_Section_ItemValues?.Count > 0)
										{
											foreach (var reply_section_value in reply_section_item.Sale_Reply_Section_ItemValues)
											{
												CRUD = CRUDModel.Update;
												string? _optionLabel = reply_section_value.OptionLabel;
												string? _replyName = reply_section_value.ReplyName;

												if (_itemType == FieldTypes.Dropdown && reply_section_value.PSaleSectionItemOptionId != Guid.Empty)
												{
													var pSection_ItemOption = await _repo.Context.ProcessSale_Section_ItemOptions.FirstOrDefaultAsync(x => x.Id == reply_section_value.PSaleSectionItemOptionId);
													if (pSection_ItemOption != null)
													{
														_optionLabel = pSection_ItemOption.OptionLabel;
													}
												}
												if (_itemType == FieldTypes.DropdownMaster && reply_section_value.Master_ListId.HasValue)
												{
													var master_Lists = await _repo.Context.Master_Lists.FirstOrDefaultAsync(x => x.Id == reply_section_value.Master_ListId);
													if (master_Lists == null) throw new ExceptionCustom("replyValue not match master_ListId.");
													_optionLabel = master_Lists.Name;

													if (!String.IsNullOrEmpty(reply_section_value.ReplyValue))
													{
														if (Guid.TryParse(reply_section_value.ReplyValue, out Guid master_id))
														{
															_replyName = await GetReplyName(master_id, master_Lists.Path);
														}
														else
														{
															throw new ExceptionCustom("Master not match replyValue.");
														}
													}
												}

												string? _fileUrl = string.Empty;
												if (reply_section_value.FileId.HasValue)
												{
													var fileUploads = await _repo.Context.FileUploads.FirstOrDefaultAsync(x => x.Id == reply_section_value.FileId);
													if (fileUploads == null) throw new ExceptionCustom("FileId not found.");
													_fileUrl = fileUploads.Url;
												}

												var replySectionItemValue = await _repo.Context.Sale_Reply_Section_ItemValues.FirstOrDefaultAsync(x => x.Id == reply_section_value.Id);
												if (replySectionItemValue == null)
												{
													CRUD = CRUDModel.Create;
													replySectionItemValue = new();
												}

												replySectionItemValue.Status = reply_section_item.Status;
												replySectionItemValue.SaleReplySectionItemId = replySectionItem.Id;
												replySectionItemValue.PSaleSectionItemOptionId = reply_section_value.PSaleSectionItemOptionId;
												replySectionItemValue.OptionLabel = _optionLabel;
												replySectionItemValue.ReplyValue = reply_section_value.ReplyValue;
												replySectionItemValue.ReplyName = _replyName;
												replySectionItemValue.ReplyDate = reply_section_value.ReplyDate;
												replySectionItemValue.ReplyTime = reply_section_value.ReplyTime;
												replySectionItemValue.FileId = reply_section_value.FileId;
												replySectionItemValue.FileUrl = _fileUrl;
												if (CRUD == CRUDModel.Create)
												{
													await _db.InsterAsync(replySectionItemValue);
												}
												else
												{
													_db.Update(replySectionItemValue);
												}
												await _db.SaveAsync();
											}
										}

									}
								}
							}
						}
					}
				}

				_transaction.Commit();

				return _mapper.Map<Sale_ReplyCustom>(saleReply);
			}
		}

		public async Task<Sale_ReplyCustom> GetReplyById(Guid id)
		{
			var query = await _repo.Context.Sale_Replies
				.Include(x => x.Sale_Reply_Sections).ThenInclude(x => x.Sale_Reply_Section_Items).ThenInclude(x => x.Sale_Reply_Section_ItemValues)
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Sale_ReplyCustom>(query);
		}

		public async Task<PaginationView<List<Sale_ReplyCustom>>> GetListReply(allFilter model)
		{
			var query = _repo.Context.Sale_Replies
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

			return new PaginationView<List<Sale_ReplyCustom>>()
			{
				Items = _mapper.Map<List<Sale_ReplyCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<string?> GetReplyName(Guid master_id, string? path)
		{
			string? _replyName = null;
			if (path == "/v1/Master/GetYields")
			{
				var masterData = await _repo.MasterYield.GetById(master_id);
				if (masterData == null) throw new ExceptionCustom("MasterYield not match replyValue.");
				_replyName = masterData.Name;
			}
			else if (path == "/v1/Master/GetChains")
			{
				var masterData = await _repo.MasterChain.GetById(master_id);
				if (masterData == null) throw new ExceptionCustom("MasterChain not match replyValue.");
				_replyName = masterData.Name;
			}
			else if (path == "/v1/Master/GetBusinessType")
			{
				var masterData = await _repo.MasterBusinessType.GetById(master_id);
				if (masterData == null) throw new ExceptionCustom("BusinessType not match replyValue.");
				_replyName = masterData.Name;
			}
			else if (path == "/v1/Master/GetBusinessSize")
			{
				var masterData = await _repo.MasterBusinessSize.GetById(master_id);
				if (masterData == null) throw new ExceptionCustom("BusinessSize not match replyValue.");
				_replyName = masterData.Name;
			}
			return _replyName;
		}

		public async Task<Sale_ContactCustom> CreateContact(Sale_ContactCustom model)
		{
			var sale = await _repo.Sales.GetStatusById(model.SaleId);
			if (sale == null) throw new ExceptionCustom("saleid not found.");

			if (sale.StatusSaleId != StatusSaleModel.WaitContact
				&& sale.StatusSaleId != StatusSaleModel.Contact
				&& sale.StatusSaleId != StatusSaleModel.ContactFail)
			{
				throw new ExceptionCustom("statussale not match");
			}

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			int statusSaleId = StatusSaleModel.NotStatus;
			string? proceedName = "ติดต่อ";
			string? resultContactName = string.Empty;
			string? nextActionName = string.Empty;

			DateTime _dateNow = DateTime.Now;

			Sale_Contact sale_Contact = new();
			sale_Contact.Status = StatusModel.Active;
			sale_Contact.CreateDate = _dateNow;
			sale_Contact.SaleId = model.SaleId;
			sale_Contact.Name = model.Name;
			sale_Contact.Tel = model.Tel;
			sale_Contact.ContactDate = model.ContactDate;
			sale_Contact.ContactResult = model.ContactResult;
			sale_Contact.NextActionId = model.NextActionId;
			sale_Contact.AppointmentDate = model.AppointmentDate;
			sale_Contact.AppointmentTime = model.AppointmentTime;
			sale_Contact.Location = model.Location;
			sale_Contact.Note = model.Note;
			await _db.InsterAsync(sale_Contact);
			await _db.SaveAsync();

			if (model.ContactResult == 1 || model.ContactResult == 2)
			{
				resultContactName = model.ContactResult == 1 ? "รับสาย" : "ไม่รับสาย";
				statusSaleId = StatusSaleModel.Contact;
			}

			if (sale_Contact.NextActionId == 1)
			{
				proceedName = "รอเข้าพบ";
				statusSaleId = StatusSaleModel.WaitMeet;
				nextActionName = "ทำการนัดหมาย";
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				});
			}
			else
			{
				if (sale.StatusSaleId == StatusSaleModel.WaitContact)
				{
					await _repo.Sales.UpdateStatusOnly(new()
					{
						SaleId = model.SaleId,
						StatusId = statusSaleId,
						CreateBy = model.CurrentUserId,
						CreateByName = currentUserName,
					});
				}
			}

			await CreateContactHistory(new()
			{
				CurrentUserId = model.CurrentUserId,
				SaleId = model.SaleId,
				ProcessSaleCode = ProcessSaleCodeModel.Contact,
				StatusSaleId = statusSaleId,
				ProceedName = proceedName,
				ResultContactName = resultContactName,
				NextActionName = nextActionName,
				AppointmentDate = model.AppointmentDate,
				AppointmentTime = model.AppointmentTime,
				Location = model.Location,
				Note = model.Note,
			});

			return _mapper.Map<Sale_ContactCustom>(sale_Contact);
		}

		public async Task<Sale_MeetCustom> CreateMeet(Sale_MeetCustom model)
		{
			var sale = await _repo.Sales.GetStatusById(model.SaleId);
			if (sale == null) throw new ExceptionCustom("saleid not found.");

			if (sale.StatusSaleId != StatusSaleModel.WaitMeet
				&& sale.StatusSaleId != StatusSaleModel.Meet
				&& sale.StatusSaleId != StatusSaleModel.MeetFail)
			{
				throw new ExceptionCustom("statussale not match");
			}

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			int statusSaleId = StatusSaleModel.NotStatus;
			string? proceedName = "เข้าพบ";
			string? resultMeetName = string.Empty;
			string? nextActionName = string.Empty;

			DateTime _dateNow = DateTime.Now;

			Sale_Meet sale_Meet = new();
			sale_Meet.Status = StatusModel.Active;
			sale_Meet.CreateDate = _dateNow;
			sale_Meet.SaleId = model.SaleId;
			sale_Meet.Name = model.Name;
			sale_Meet.Tel = model.Tel;
			sale_Meet.MeetDate = model.MeetDate;
			sale_Meet.MeetId = model.MeetId;
			sale_Meet.Master_YieldId = model.Master_YieldId;
			sale_Meet.Master_ChainId = model.Master_ChainId;
			sale_Meet.LoanAmount = model.LoanAmount;
			sale_Meet.NextActionId = model.NextActionId;
			sale_Meet.AppointmentDate = model.AppointmentDate;
			sale_Meet.AppointmentTime = model.AppointmentTime;
			sale_Meet.Location = model.Location;
			sale_Meet.Note = model.Note;
			await _db.InsterAsync(sale_Meet);
			await _db.SaveAsync();

			statusSaleId = StatusSaleModel.Meet;

			if (model.MeetId == 1 || model.MeetId == 2)
			{
				resultMeetName = model.MeetId == 1 ? "เข้าพบสำเร็จ" : "เข้าพบไม่สำเร็จ";
			}

			if (sale_Meet.NextActionId == 1)
			{
				proceedName = "รอยื่นเอกสาร";
				statusSaleId = StatusSaleModel.WaitSubmitDocument;
				nextActionName = "นัดเก็บเอกสาร/ประสงค์กู้";
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				});
			}
			else
			{
				if (sale.StatusSaleId == StatusSaleModel.WaitMeet)
				{
					await _repo.Sales.UpdateStatusOnly(new()
					{
						SaleId = model.SaleId,
						StatusId = statusSaleId,
						CreateBy = model.CurrentUserId,
						CreateByName = currentUserName,
					});
				}
			}

			await CreateContactHistory(new()
			{
				CurrentUserId = model.CurrentUserId,
				SaleId = model.SaleId,
				ProcessSaleCode = ProcessSaleCodeModel.Meet,
				StatusSaleId = statusSaleId,
				ProceedName = proceedName,
				ResultMeetName = resultMeetName,
				NextActionName = nextActionName,
				AppointmentDate = model.AppointmentDate,
				AppointmentTime = model.AppointmentTime,
				Location = model.Location,
				Note = model.Note,
			});

			return _mapper.Map<Sale_MeetCustom>(sale_Meet);
		}

		public async Task<Sale_DocumentCustom> CreateDocument(Sale_DocumentCustom model)
		{
			var sale = await _repo.Sales.GetStatusById(model.SaleId);
			if (sale == null) throw new ExceptionCustom("saleid not found.");

			if (sale.StatusSaleId != StatusSaleModel.WaitSubmitDocument
				&& sale.StatusSaleId != StatusSaleModel.SubmitDocument
				&& sale.StatusSaleId != StatusSaleModel.SubmitDocumentFail)
			{
				throw new ExceptionCustom("statussale not match");
			}

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			int statusSaleId = StatusSaleModel.SubmitDocument;
			string? proceedName = "ยื่นเอกสาร";
			string? resultContactName = string.Empty;
			string? nextActionName = string.Empty;

			DateTime _dateNow = DateTime.Now;

			Sale_Document sale_Document = new();
			sale_Document.Status = StatusModel.Active;
			sale_Document.CreateDate = _dateNow;
			sale_Document.SaleId = model.SaleId;
			sale_Document.IDCardIMGPath = model.IDCardIMGPath;
			sale_Document.IDCardNumber = model.IDCardNumber;
			sale_Document.NameTh = model.NameTh;
			sale_Document.NameEn = model.NameEn;
			sale_Document.Birthday = model.Birthday;
			sale_Document.Religion = model.Religion;
			sale_Document.HouseNo = model.HouseNo;
			sale_Document.VillageNo = model.VillageNo;
			sale_Document.ProvinceId = model.ProvinceId;
			sale_Document.AmphurId = model.AmphurId;
			sale_Document.HouseRegistrationPath = model.HouseRegistrationPath;
			sale_Document.PathOtherDocument = model.PathOtherDocument;
			sale_Document.Master_BusinessTypeId = model.Master_BusinessTypeId;
			sale_Document.BusinessOperation = model.BusinessOperation;
			sale_Document.RegistrationDate = model.RegistrationDate;
			sale_Document.DateFirstContactBank = model.DateFirstContactBank;
			sale_Document.Master_TypeLoanRequestId = model.Master_TypeLoanRequestId;
			sale_Document.Master_TypeLoanRequestSpecify = model.Master_TypeLoanRequestSpecify;
			sale_Document.Master_ProductProgramBankId = model.Master_ProductProgramBankId;
			sale_Document.LoanLimitBusiness = model.LoanLimitBusiness;
			sale_Document.LoanLimitInvestmentCost = model.LoanLimitInvestmentCost;
			sale_Document.LoanLimitObjectiveOther = model.LoanLimitObjectiveOther;
			sale_Document.TotaLlimit = model.TotaLlimit;
			sale_Document.TotaLlimitCEQA = model.TotaLlimitCEQA;
			sale_Document.CommentEmployeeLoan = model.CommentEmployeeLoan;
			sale_Document.SignatureNamePath = model.SignatureNamePath;
			sale_Document.SignatureNameDate = model.SignatureNameDate;
			sale_Document.SignatureEmployeeLoanPath = model.SignatureEmployeeLoanPath;
			sale_Document.SignatureEmployeeLoanDate = model.SignatureEmployeeLoanDate;
			sale_Document.SignatureMCenterPath = model.SignatureMCenterPath;
			sale_Document.SignatureMCenterDate = model.SignatureMCenterDate;
			sale_Document.SubmitType = model.SubmitType;
			if (model.SubmitType == 1)
			{
				if (String.IsNullOrEmpty(model.IDCardIMGPath))
				{
					throw new ExceptionCustom("ระบุรูปบัตรประชาชน");
				}
				if (String.IsNullOrEmpty(model.SignatureNamePath) || String.IsNullOrEmpty(model.SignatureEmployeeLoanPath) || String.IsNullOrEmpty(model.SignatureMCenterPath))
				{
					throw new ExceptionCustom("ระบุลายเซ็นไม่ครบ");
				}
				sale_Document.SubmitDate = _dateNow;
			}

			await _db.InsterAsync(sale_Document);
			await _db.SaveAsync();

			if (sale_Document.SubmitType == 1)
			{
				proceedName = "รอลงนามอนุมัติเอกสาร";
				statusSaleId = StatusSaleModel.WaitApproveDocument;
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				});
			}
			else
			{
				if (sale.StatusSaleId == StatusSaleModel.WaitSubmitDocument)
				{
					await _repo.Sales.UpdateStatusOnly(new()
					{
						SaleId = model.SaleId,
						StatusId = statusSaleId,
						CreateBy = model.CurrentUserId,
						CreateByName = currentUserName,
					});
				}
			}

			await CreateContactHistory(new()
			{
				CurrentUserId = model.CurrentUserId,
				SaleId = model.SaleId,
				ProcessSaleCode = ProcessSaleCodeModel.Document,
				StatusSaleId = statusSaleId,
				ProceedName = proceedName,
				ResultContactName = resultContactName,
				NextActionName = nextActionName,
			});

			return _mapper.Map<Sale_DocumentCustom>(sale_Document);
		}

		public async Task<Sale_ResultCustom> CreateResult(Sale_ResultCustom model)
		{
			var sale = await _repo.Sales.GetStatusById(model.SaleId);
			if (sale == null) throw new ExceptionCustom("saleid not found.");

			if (sale.StatusSaleId != StatusSaleModel.WaitResults
				&& sale.StatusSaleId != StatusSaleModel.Results)
			{
				throw new ExceptionCustom("statussale not match");
			}

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			int statusSaleId = StatusSaleModel.NotStatus;
			string? proceedName = "ผลลัพธ์";
			string? resultMeetName = string.Empty;
			string? nextActionName = string.Empty;

			DateTime _dateNow = DateTime.Now;

			Sale_Result sale_Result = new();
			sale_Result.Status = StatusModel.Active;
			sale_Result.CreateDate = _dateNow;
			sale_Result.SaleId = model.SaleId;
			sale_Result.ProceedId = model.ProceedId;
			sale_Result.NextActionId = model.NextActionId;
			sale_Result.AppointmentDate = model.AppointmentDate;
			sale_Result.AppointmentTime = model.AppointmentTime;
			sale_Result.Location = model.Location;
			sale_Result.Note = model.Note;
			await _db.InsterAsync(sale_Result);
			await _db.SaveAsync();

			statusSaleId = StatusSaleModel.Results;

			resultMeetName = model.ResultMeetId == 1 ? "เข้าพบสำเร็จ" : model.ResultMeetId == 2 ? "เข้าพบไม่สำเร็จ" : "";

			//1=แจ้งข้อมูลเพิ่มเติม 2=ติดต่อขอเอกสาร 3=เข้าพบรับเอกสาร
			if (model.ProceedId == 1 || model.ProceedId == 2 || model.ProceedId == 3)
			{
				proceedName = model.ProceedId == 1 ? "แจ้งข้อมูลเพิ่มเติม" : model.ProceedId == 2 ? "ติดต่อขอเอกสาร" : model.ProceedId == 3 ? "เข้าพบรับเอกสาร" : string.Empty;
			}

			if (model.NextActionId == 1)
			{
				nextActionName = "ทำการนัดหมาย";
			}

			if (sale.StatusSaleId == StatusSaleModel.WaitResults)
			{
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				});
			}

			await CreateContactHistory(new()
			{
				CurrentUserId = model.CurrentUserId,
				SaleId = model.SaleId,
				ProcessSaleCode = ProcessSaleCodeModel.Result,
				StatusSaleId = statusSaleId,
				ProceedName = proceedName,
				ResultMeetName = resultMeetName,
				NextActionName = nextActionName,
				AppointmentDate = model.AppointmentDate,
				AppointmentTime = model.AppointmentTime,
				Location = model.Location,
				Note = model.Note,
			});

			return _mapper.Map<Sale_ResultCustom>(sale_Result);
		}

		public async Task<Sale_Close_SaleCustom> CreateCloseSale(Sale_Close_SaleCustom model)
		{
			var sale = await _repo.Sales.GetStatusById(model.SaleId);
			if (sale == null) throw new ExceptionCustom("saleid not found.");

			if (sale.StatusSaleId != StatusSaleModel.Results)
			{
				throw new ExceptionCustom("statussale not match");
			}

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			int statusSaleId = StatusSaleModel.NotStatus;
			string? proceedName = "เข้าพบ";
			string? resultContactName = string.Empty;

			DateTime _dateNow = DateTime.Now;

			Sale_Close_Sale sale_Close_Sale = new();
			sale_Close_Sale.Status = StatusModel.Active;
			sale_Close_Sale.CreateDate = _dateNow;
			sale_Close_Sale.SaleId = model.SaleId;
			sale_Close_Sale.Name = model.Name;
			sale_Close_Sale.Tel = model.Tel;
			sale_Close_Sale.ResultMeetId = model.ResultMeetId;
			sale_Close_Sale.DesireLoanId = model.DesireLoanId;
			sale_Close_Sale.ReasonId = model.ReasonId;
			sale_Close_Sale.Note = model.Note;
			await _db.InsterAsync(sale_Close_Sale);
			await _db.SaveAsync();

			resultContactName = model.ResultMeetId == 1 ? "เข้าพบสำเร็จ" : model.ResultMeetId == 2 ? "" : "";

			if (sale_Close_Sale.DesireLoanId == 1 || sale_Close_Sale.DesireLoanId == 2)
			{
				if (sale_Close_Sale.DesireLoanId == 1)
				{
					proceedName = "ประสงค์กู้";
					statusSaleId = StatusSaleModel.CloseSale;
				}
				else if (sale_Close_Sale.DesireLoanId == 2)
				{
					proceedName = "ไม่ประสงค์กู้";
					statusSaleId = StatusSaleModel.ResultsNotLoan;
				}
				else
				{
					throw new ExceptionCustom("desireLoanId not match");
				}

				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				});
			}

			await CreateContactHistory(new()
			{
				CurrentUserId = model.CurrentUserId,
				SaleId = model.SaleId,
				ProcessSaleCode = ProcessSaleCodeModel.CloseSale,
				StatusSaleId = statusSaleId,
				ProceedName = proceedName,
				ResultContactName = resultContactName,
				Note = model.Note,
			});

			return _mapper.Map<Sale_Close_SaleCustom>(sale_Close_Sale);
		}

		public async Task<Sale_Contact_HistoryCustom> CreateContactHistory(Sale_Contact_HistoryCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			Sale_Contact_History sale_Contact_History = new();
			sale_Contact_History.Status = StatusModel.Active;
			sale_Contact_History.CreateDate = _dateNow;
			sale_Contact_History.CreateBy = model.CurrentUserId;
			sale_Contact_History.CreateByName = currentUserName;
			sale_Contact_History.SaleId = model.SaleId;
			sale_Contact_History.StatusSaleId = model.StatusSaleId;

			sale_Contact_History.ProceedName = model.ProceedName;
			sale_Contact_History.ResultContactName = model.ResultContactName;
			sale_Contact_History.ResultMeetName = model.ResultMeetName;
			sale_Contact_History.NextActionName = model.NextActionName;
			sale_Contact_History.CreditLimit = model.CreditLimit;
			sale_Contact_History.Percent = model.Percent;

			sale_Contact_History.AppointmentDate = model.AppointmentDate;
			sale_Contact_History.AppointmentTime = model.AppointmentTime;
			sale_Contact_History.Location = model.Location;
			sale_Contact_History.Note = model.Note;
			await _db.InsterAsync(sale_Contact_History);
			await _db.SaveAsync();

			return _mapper.Map<Sale_Contact_HistoryCustom>(sale_Contact_History);
		}

		public async Task<PaginationView<List<Sale_Contact_HistoryCustom>>> GetListContactHistory(allFilter model)
		{
			var query = _repo.Context.Sale_Contact_Histories
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.CreateDate)
												 .AsQueryable();
			if (model.id != Guid.Empty)
			{
				query = query.Where(x => x.SaleId == model.id);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Sale_Contact_HistoryCustom>>()
			{
				Items = _mapper.Map<List<Sale_Contact_HistoryCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}
