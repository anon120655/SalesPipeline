using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Phoenixs;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System.Text.RegularExpressions;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class ProcessSales : IProcessSales
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;
		public string? NoteSystem = null;

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

				var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

				//มีการแก้ไขประวัติการติดต่อ
				//model.State = CRUDModel.Update;
				if (model.State == CRUDModel.Update)
				{
					NoteSystem = "Edit history";

					var sale = await _repo.Sales.GetStatusById(model.SaleId);
					if (sale == null) throw new ExceptionCustom("saleid not found.");

					//ถ้าเป็นสถานะดังนี้ จะถอยสถานะไปขั้นตอนก่อนหน้า (**จะเป็นสถานะที่มีการนัดหมาย) เพื่อให้เข้าเงื่อนไขปกติก่อน CreateReply
					//WaitMeet=รอการเข้าพบ 14 --> Contact=ติดต่อ 12
					//WaitSubmitDocument=รอยื่นเอกสาร 17--> Meet=เข้าพบ 15
					//WaitCloseSale=รอปิดการขาย 27--> Results=ผลลัพธ์ 25
					if (sale.StatusSaleId == StatusSaleModel.WaitMeet)
					{
						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = model.SaleId,
							StatusId = StatusSaleModel.Contact,
							CreateDate = _dateNow,
							CreateBy = model.CurrentUserId,
							CreateByName = currentUserName,
							Description = NoteSystem
						});
					}
					else if (sale.StatusSaleId == StatusSaleModel.WaitSubmitDocument)
					{
						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = model.SaleId,
							StatusId = StatusSaleModel.Meet,
							CreateDate = _dateNow,
							CreateBy = model.CurrentUserId,
							CreateByName = currentUserName,
							Description = NoteSystem
						});
					}
					else if (sale.StatusSaleId == StatusSaleModel.WaitCloseSale)
					{
						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = model.SaleId,
							StatusId = StatusSaleModel.Results,
							CreateDate = _dateNow,
							CreateBy = model.CurrentUserId,
							CreateByName = currentUserName,
							Description = NoteSystem
						});
					}
				}

				string? _processSaleName = string.Empty;
				var processSales = await _repo.Context.ProcessSales.FirstOrDefaultAsync(x => x.Id == model.ProcessSaleId);
				if (processSales == null) throw new ExceptionCustom("ProcessSaleId not found.");
				_processSaleName = processSales.Name;

				Sale_Reply saleReply = new();
				saleReply.Status = StatusModel.Active;
				saleReply.CreateDate = _dateNow;
				saleReply.CreateBy = model.CurrentUserId;
				saleReply.CreateByName = currentUserName;
				saleReply.UpdateDate = _dateNow;
				saleReply.UpdateBy = model.CurrentUserId;
				saleReply.UpdateByName = currentUserName;
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
											string? _fileName = string.Empty;
											if (reply_section_value.FileId.HasValue)
											{
												var fileUploads = await _repo.Context.FileUploads.FirstOrDefaultAsync(x => x.Id == reply_section_value.FileId);
												if (fileUploads == null) throw new ExceptionCustom("FileId not found.");
												_fileUrl = fileUploads.Url;
												_fileName = fileUploads.OriginalFileName;
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
											replySectionItemValue.FileName = _fileName;
											await _db.InsterAsync(replySectionItemValue);
											await _db.SaveAsync();
										}
									}

								}
							}
						}
					}
				}

				if (processSales.Code == ProcessSaleCodeModel.Contact)
				{
					if (model.Sale_Contact == null) throw new ExceptionCustom("Sale_Contact model not found.");

					model.Sale_Contact.SaleId = model.SaleId;
					model.Sale_Contact.SaleReplyId = saleReply.Id;
					model.Sale_Contact.CurrentUserId = model.CurrentUserId;
					await CreateContact(model.Sale_Contact);
				}
				else if (processSales.Code == ProcessSaleCodeModel.Meet)
				{
					if (model.Sale_Meet == null) throw new ExceptionCustom("Sale_Meet model not found.");

					model.Sale_Meet.SaleId = model.SaleId;
					model.Sale_Meet.SaleReplyId = saleReply.Id;
					model.Sale_Meet.CurrentUserId = model.CurrentUserId;
					await CreateMeet(model.Sale_Meet);
				}
				else if (processSales.Code == ProcessSaleCodeModel.Document)
				{
					if (model.Sale_Document == null) throw new ExceptionCustom("Sale_Document model not found.");

					model.Sale_Document.SaleId = model.SaleId;
					model.Sale_Document.SaleReplyId = saleReply.Id;
					model.Sale_Document.CurrentUserId = model.CurrentUserId;
					await CreateDocument(model.Sale_Document);
				}
				else if (processSales.Code == ProcessSaleCodeModel.Result)
				{
					if (model.Sale_Result == null) throw new ExceptionCustom("Sale_Result model not found.");

					model.Sale_Result.SaleId = model.SaleId;
					model.Sale_Result.SaleReplyId = saleReply.Id;
					model.Sale_Result.CurrentUserId = model.CurrentUserId;
					await CreateResult(model.Sale_Result);
				}
				else if (processSales.Code == ProcessSaleCodeModel.CloseSale && model.Sale_Close_Sale != null)
				{
					if (model.Sale_Close_Sale == null) throw new ExceptionCustom("Sale_Close_Sale model not found.");

					model.Sale_Close_Sale.SaleId = model.SaleId;
					model.Sale_Close_Sale.SaleReplyId = saleReply.Id;
					model.Sale_Close_Sale.CurrentUserId = model.CurrentUserId;
					await CreateCloseSale(model.Sale_Close_Sale);
				}

				//SetNoti
				await _repo.Notifys.SetScheduleNoti();

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
			var sale_Replay = await _repo.Context.Sale_Replies
				.Include(x => x.Sale_Reply_Sections).ThenInclude(x => x.Sale_Reply_Section_Items).ThenInclude(x => x.Sale_Reply_Section_ItemValues)
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			if (sale_Replay == null) throw new ExceptionCustom("data not found.");

			var response = _mapper.Map<Sale_ReplyCustom>(sale_Replay);

			var sale_Contacts = await _repo.Context.Sale_Contacts.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.SaleReplyId == id);
			if (sale_Contacts != null)
				response.Sale_Contact = _mapper.Map<Sale_ContactCustom>(sale_Contacts);

			var sale_Meets = await _repo.Context.Sale_Meets.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.SaleReplyId == id);
			if (sale_Meets != null)
				response.Sale_Meet = _mapper.Map<Sale_MeetCustom>(sale_Meets);

			var sale_Documents = await _repo.Context.Sale_Documents.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.SaleReplyId == id);
			if (sale_Documents != null)
				response.Sale_Document = _mapper.Map<Sale_DocumentCustom>(sale_Documents);

			var sale_Results = await _repo.Context.Sale_Results.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.SaleReplyId == id);
			if (sale_Results != null)
				response.Sale_Result = _mapper.Map<Sale_ResultCustom>(sale_Results);

			var sale_Close_Sales = await _repo.Context.Sale_Close_Sales.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.SaleReplyId == id);
			if (sale_Close_Sales != null)
				response.Sale_Close_Sale = _mapper.Map<Sale_Close_SaleCustom>(sale_Close_Sales);

			return response;
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

			if (sale.StatusSaleMainId != StatusSaleMainModel.Contact)
			{
				throw new ExceptionCustom("สถานะการขายไม่ถูกต้อง");
			}

			if (model.ContactDate.HasValue)
			{
				if (model.ContactDate.Value.Date > DateTime.Now.Date)
				{
					throw new ExceptionCustom("วันที่ติดต่อต้องไม่มากกว่าวันที่ปัจจุบัน");
				}
			}

			if (String.IsNullOrEmpty(model.Name)) throw new ExceptionCustom("ระบุชื่อผู้ติดต่อ");
			if (String.IsNullOrEmpty(model.Tel)) throw new ExceptionCustom("ระบุเบอร์ติดต่อ");
			if (!model.ContactDate.HasValue) throw new ExceptionCustom("ระบุวันที่ติดต่อ");
			if (!model.ContactResult.HasValue) throw new ExceptionCustom("ระบุผลการติดต่อ");
			if (!model.NextActionId.HasValue) throw new ExceptionCustom("ระบุ Next Action");
			if (model.ContactResult != 1 && model.ContactResult != 2) throw new ExceptionCustom("contactResult not match");
			if (model.NextActionId != 1 && model.NextActionId != 2) throw new ExceptionCustom("nextActionId not match");

			if (!model.AppointmentDate.HasValue) throw new ExceptionCustom("ระบุวันที่นัดหมาย");
			if (model.AppointmentDate.Value.Date < DateTime.Now.Date) throw new ExceptionCustom("วันที่นัดหมายต้องไม่น้อยกว่าวันที่ปัจจุบัน");
			if (!model.AppointmentTime.HasValue) throw new ExceptionCustom("ระบุเวลาที่นัดหมาย");
			if (String.IsNullOrEmpty(model.Location)) throw new ExceptionCustom("ระบุสถานที่");

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			int statusSaleId = StatusSaleModel.NotStatus;
			string? topicName = "ติดต่อ";
			string? resultContactName = string.Empty;
			string? nextActionName = string.Empty;

			DateTime _dateNow = DateTime.Now;

			Sale_Contact sale_Contact = new();
			sale_Contact.Status = StatusModel.Active;
			sale_Contact.CreateDate = _dateNow;
			sale_Contact.SaleId = model.SaleId;
			sale_Contact.SaleReplyId = model.SaleReplyId;
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

			DateTime createDate = DateTime.Now;
			if (!await _repo.Sales.CheckStatusById(model.SaleId, statusSaleId))
			{
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateDate = createDate,
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				}, new() { ContactStartDate = model.ContactDate });
			}

			if (sale_Contact.NextActionId == 1)
			{
				statusSaleId = StatusSaleModel.WaitMeet;
				nextActionName = "ทำการนัดหมาย";
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateDate = createDate.AddSeconds(1),
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				});
			}
			else
			{
				if (sale_Contact.NextActionId == 2)
				{
					nextActionName = "ติดต่ออีกครั้ง";
				}
			}

			await _repo.Sales.CreateInfo(new()
			{
				SaleId = model.SaleId,
				FullName = model.Name,
				Tel = model.Tel
			});

			await CreateContactHistory(new()
			{
				CurrentUserId = model.CurrentUserId,
				SaleId = model.SaleId,
				SaleReplyId = model.SaleReplyId,
				ProcessSaleCode = ProcessSaleCodeModel.Contact,
				StatusSaleId = statusSaleId,
				TopicName = topicName,
				ContactFullName = model.Name,
				ContactDate = model.ContactDate,
				ContactTel = model.Tel,
				ResultContactName = resultContactName,
				NextActionName = nextActionName,
				AppointmentDate = model.AppointmentDate,
				AppointmentTime = model.AppointmentTime,
				Location = model.Location,
				Note = model.Note,
				NoteSystem = NoteSystem
			});

			return _mapper.Map<Sale_ContactCustom>(sale_Contact);
		}

		public async Task<Sale_MeetCustom> CreateMeet(Sale_MeetCustom model)
		{
			var sale = await _repo.Sales.GetStatusById(model.SaleId);
			if (sale == null) throw new ExceptionCustom("saleid not found.");

			if (sale.StatusSaleMainId != StatusSaleMainModel.Meet)
			{
				throw new ExceptionCustom("สถานะการขายไม่ถูกต้อง");
			}

			if (model.MeetDate.HasValue)
			{
				if (model.MeetDate.Value.Date > DateTime.Now.Date)
				{
					throw new ExceptionCustom("วันที่เข้าพบต้องไม่มากกว่าวันที่ปัจจุบัน");
				}
			}

			if (String.IsNullOrEmpty(model.Name)) throw new ExceptionCustom("ระบุชื่อผู้เข้าพบ");
			if (String.IsNullOrEmpty(model.Tel)) throw new ExceptionCustom("ระบุเบอร์ติดต่อ");
			if (!model.MeetDate.HasValue) throw new ExceptionCustom("ระบุวันที่เข้าพบ");
			if (!model.MeetId.HasValue) throw new ExceptionCustom("ระบุผลการเข้าพบ");
			if (!model.NextActionId.HasValue) throw new ExceptionCustom("ระบุ Next Action");
			if (model.MeetId != 1 && model.MeetId != 2) throw new ExceptionCustom("meetId not match");
			if (model.NextActionId != 1 && model.NextActionId != 2) throw new ExceptionCustom("nextActionId not match");

			if (!model.LoanAmount.HasValue || model.LoanAmount <= 0) throw new ExceptionCustom("ระบุจำนวนการกู้");
			if (!model.AppointmentDate.HasValue) throw new ExceptionCustom("ระบุวันที่นัดหมาย");
			if (model.AppointmentDate.Value.Date < DateTime.Now.Date) throw new ExceptionCustom("วันที่นัดหมายต้องไม่น้อยกว่าวันที่ปัจจุบัน");
			if (!model.AppointmentTime.HasValue) throw new ExceptionCustom("ระบุเวลาที่นัดหมาย");
			if (String.IsNullOrEmpty(model.Location)) throw new ExceptionCustom("ระบุสถานที่");


			if (!model.Master_YieldId.HasValue) throw new ExceptionCustom("ระบุ ผลผลิตหลัก");
			if (!model.Master_ChainId.HasValue) throw new ExceptionCustom("ระบุ ห่วงโซ่คุณค่า");

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			int statusSaleId = StatusSaleModel.NotStatus;
			string? topicName = "เข้าพบ";
			string? resultMeetName = string.Empty;
			string? nextActionName = string.Empty;

			DateTime _dateNow = DateTime.Now;

			Sale_Meet sale_Meet = new();
			sale_Meet.Status = StatusModel.Active;
			sale_Meet.CreateDate = _dateNow;
			sale_Meet.SaleId = model.SaleId;
			sale_Meet.SaleReplyId = model.SaleReplyId;
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

			DateTime createDate = DateTime.Now;
			if (!await _repo.Sales.CheckStatusById(model.SaleId, statusSaleId))
			{
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateDate = createDate,
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				}, new() { LoanAmount = model.LoanAmount });
			}

			if (sale_Meet.NextActionId == 1)
			{
				statusSaleId = StatusSaleModel.WaitSubmitDocument;
				nextActionName = "นัดเก็บเอกสาร/ประสงค์กู้";
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateDate = createDate.AddSeconds(1),
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				});
			}
			else
			{
				if (sale_Meet.NextActionId == 2)
				{
					nextActionName = "เข้าพบอีกครั้ง";
				}
			}

			await _repo.Sales.CreateInfo(new()
			{
				SaleId = model.SaleId,
				FullName = model.Name,
				Tel = model.Tel
			});

			await CreateContactHistory(new()
			{
				CurrentUserId = model.CurrentUserId,
				SaleId = model.SaleId,
				SaleReplyId = model.SaleReplyId,
				ProcessSaleCode = ProcessSaleCodeModel.Meet,
				StatusSaleId = statusSaleId,
				TopicName = topicName,
				MeetFullName = model.Name,
				ResultMeetName = resultMeetName,
				NextActionName = nextActionName,
				AppointmentDate = model.AppointmentDate,
				AppointmentTime = model.AppointmentTime,
				Location = model.Location,
				Note = model.Note,
				NoteSystem = NoteSystem,
				CreditLimit = model.LoanAmount,
				PercentChanceLoanPass = sale.PercentChanceLoanPass
			});

			return _mapper.Map<Sale_MeetCustom>(sale_Meet);
		}

		public async Task<Sale_DocumentCustom> CreateDocument(Sale_DocumentCustom model)
		{
			var sale = await _repo.Sales.GetStatusById(model.SaleId);
			if (sale == null) throw new ExceptionCustom("saleid not found.");

			if (sale.StatusSaleMainId != StatusSaleMainModel.Document)
			{
				throw new ExceptionCustom("สถานะการขายไม่ถูกต้อง");
			}

			if (String.IsNullOrEmpty(model.NameTh)) throw new ExceptionCustom("ระบุชื่อภาษาไทย");
			if (!model.Birthday.HasValue) throw new ExceptionCustom("ระบุวันเกิด");
			if (String.IsNullOrEmpty(model.Religion)) throw new ExceptionCustom("ระบุศาสนา");
			if (String.IsNullOrEmpty(model.HouseNo)) throw new ExceptionCustom("ระบุบ้านเลขที่");
			if (String.IsNullOrEmpty(model.VillageNo)) throw new ExceptionCustom("ระบุหมู่ที่");
			if (!model.ProvinceId.HasValue) throw new ExceptionCustom("ระบุจังหวัด");
			if (!model.AmphurId.HasValue) throw new ExceptionCustom("ระบุอำเภอ");

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);
			var provinceName = await _repo.Thailand.GetProvinceNameByid(model.ProvinceId ?? 0);
			var amphurName = await _repo.Thailand.GetAmphurNameByid(model.AmphurId ?? 0);
			var master_BusinessTypeName = await _repo.MasterBusinessType.GetNameById(model.Master_BusinessTypeId ?? Guid.Empty);
			var master_TypeLoanRequestName = _repo.Context.Master_TypeLoanRequests.FirstOrDefault(x => x.Id == model.Master_TypeLoanRequestId)?.Name;
			var master_ProductProgramBankName = _repo.Context.Master_ProductProgramBanks.FirstOrDefault(x => x.Id == model.Master_ProductProgramBankId)?.Name;

			int statusSaleId = StatusSaleModel.SubmitDocument;
			string? topicName = "ยื่นเอกสาร";
			string? resultContactName = string.Empty;
			string? nextActionName = string.Empty;

			DateTime _dateNow = DateTime.Now;

			//if (_appSet.ServerSite != ServerSites.DEV)
			//{
			//	if (!String.IsNullOrEmpty(model.HouseRegistrationPath) && (!model.HouseRegistrationFileId.HasValue || model.HouseRegistrationFileId == Guid.Empty)) throw new ExceptionCustom("houseRegistrationFileId not found.");
			//	if (!String.IsNullOrEmpty(model.OtherDocumentPath) && (!model.OtherDocumentFileId.HasValue || model.OtherDocumentFileId == Guid.Empty)) throw new ExceptionCustom("otherDocumentFileId not found.");
			//	if (!String.IsNullOrEmpty(model.SignaturePath) && (!model.SignatureFileId.HasValue || model.SignatureFileId == Guid.Empty)) throw new ExceptionCustom("signatureFileId not found.");
			//	if (!String.IsNullOrEmpty(model.SignatureEmployeeLoanPath) && (!model.SignatureEmployeeFileId.HasValue || model.SignatureEmployeeFileId == Guid.Empty)) throw new ExceptionCustom("signatureEmployeeFileId not found.");
			//}

			Sale_Document sale_Document = new();
			sale_Document.Status = StatusModel.Active;
			sale_Document.CreateDate = _dateNow;
			sale_Document.SaleId = model.SaleId;
			sale_Document.SaleReplyId = model.SaleReplyId;
			sale_Document.IDCardIMGPath = model.IDCardIMGPath;
			sale_Document.IDCardNumber = model.IDCardNumber;
			sale_Document.NameTh = model.NameTh;
			sale_Document.NameEn = model.NameEn;
			sale_Document.Birthday = model.Birthday;
			sale_Document.Religion = model.Religion;
			sale_Document.HouseNo = model.HouseNo;
			sale_Document.VillageNo = model.VillageNo;
			sale_Document.ProvinceId = model.ProvinceId;
			sale_Document.ProvinceName = provinceName;
			sale_Document.AmphurId = model.AmphurId;
			sale_Document.AmphurName = amphurName;
			sale_Document.HouseRegistrationPath = model.HouseRegistrationPath;
			sale_Document.OtherDocumentPath = model.OtherDocumentPath;
			sale_Document.Master_BusinessTypeId = model.Master_BusinessTypeId;
			sale_Document.Master_BusinessTypeName = master_BusinessTypeName;
			sale_Document.BusinessOperation = model.BusinessOperation;
			sale_Document.RegistrationDate = model.RegistrationDate;
			sale_Document.DateFirstContactBank = model.DateFirstContactBank;
			sale_Document.Master_TypeLoanRequestId = model.Master_TypeLoanRequestId;
			sale_Document.Master_TypeLoanRequesName = master_TypeLoanRequestName;
			sale_Document.Master_TypeLoanRequestSpecify = model.Master_TypeLoanRequestSpecify;
			sale_Document.Master_ProductProgramBankId = model.Master_ProductProgramBankId;
			sale_Document.Master_ProductProgramBankName = master_ProductProgramBankName;
			sale_Document.LoanLimitBusiness = model.LoanLimitBusiness;
			sale_Document.LoanLimitInvestmentCost = model.LoanLimitInvestmentCost;
			sale_Document.LoanLimitObjectiveOther = model.LoanLimitObjectiveOther;
			sale_Document.TotaLlimit = model.TotaLlimit;
			sale_Document.TotaLlimitCEQA = model.TotaLlimitCEQA;
			sale_Document.CommentEmployeeLoan = model.CommentEmployeeLoan;
			sale_Document.SignaturePath = model.SignaturePath;
			sale_Document.SignatureDate = model.SignatureDate;
			sale_Document.SignatureEmployeeLoanPath = model.SignatureEmployeeLoanPath;
			sale_Document.SignatureEmployeeLoanDate = model.SignatureEmployeeLoanDate;
			sale_Document.SignatureMCenterPath = model.SignatureMCenterPath;
			sale_Document.SignatureMCenterDate = model.SignatureMCenterDate;
			sale_Document.SubmitType = model.SubmitType;
			if (model.SubmitType == 1)
			{
				//if (String.IsNullOrEmpty(model.IDCardIMGPath))
				//{
				//	throw new ExceptionCustom("ระบุรูปบัตรประชาชน");
				//}

				if (String.IsNullOrEmpty(model.SignaturePath))
				{
					throw new ExceptionCustom("ระบุรูปลายเซ็นผู้กู้ยืม");
				}
				else if (String.IsNullOrEmpty(model.SignatureEmployeeLoanPath))
				{
					throw new ExceptionCustom("ระบุรูปลายเซ็นพนักงานสินเชื่อ");
				}
				sale_Document.SubmitDate = _dateNow;
			}

			await _db.InsterAsync(sale_Document);
			await _db.SaveAsync();

			DateTime createDate = DateTime.Now;
			if (!await _repo.Sales.CheckStatusById(model.SaleId, statusSaleId))
			{
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateDate = createDate,
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				});
			}

			if (sale_Document.SubmitType == 1)
			{
				nextActionName = "รอลงนามอนุมัติเอกสาร";
				NoteSystem = "รอผู้จัดการศูนย์ลงนามอนุมัติเอกสาร";
				statusSaleId = StatusSaleModel.WaitApproveLoanRequest;
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateDate = createDate.AddSeconds(1),
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				});
			}

			await CreateContactHistory(new()
			{
				CurrentUserId = model.CurrentUserId,
				SaleId = model.SaleId,
				SaleReplyId = model.SaleReplyId,
				ProcessSaleCode = ProcessSaleCodeModel.Document,
				StatusSaleId = statusSaleId,
				TopicName = topicName,
				ResultContactName = resultContactName,
				NextActionName = nextActionName,
				NoteSystem = NoteSystem
			});

			return _mapper.Map<Sale_DocumentCustom>(sale_Document);
		}

		public async Task<Sale_ResultCustom> CreateResult(Sale_ResultCustom model)
		{
			var sale = await _repo.Sales.GetStatusById(model.SaleId);
			if (sale == null) throw new ExceptionCustom("saleid not found.");

			if (sale.StatusSaleMainId != StatusSaleMainModel.Result)
			{
				throw new ExceptionCustom("สถานะการขายไม่ถูกต้อง");
			}

			if (sale.StatusSaleId == StatusSaleModel.ResultsNotConsidered)
			{
				throw new ExceptionCustom("ลูกค้าท่านนี้ไม่ผ่านการพิจารณา");
			}

			//1=แจ้งข้อมูลเพิ่มเติม 2=ติดต่อขอเอกสาร 3=เข้าพบรับเอกสาร 4=ไม่ผ่านการพิจารณา
			if (!model.ProceedId.HasValue) throw new ExceptionCustom("ระบุการดำเนินการ");
			if (model.ProceedId != 1 && model.ProceedId != 2 && model.ProceedId != 3 && model.ProceedId != 4 && model.ProceedId != 5) throw new ExceptionCustom("proceedId not match");
			if (model.ProceedId == 1 || model.ProceedId == 4 || model.ProceedId == 5)
			{
				if (String.IsNullOrEmpty(model.Note)) throw new ExceptionCustom("ระบุหมายเหตุ");
			}
			else if (model.ProceedId == 2)
			{
				if (!model.DateContact.HasValue) throw new ExceptionCustom("ระบุวันที่ติดต่อ");
				if (!model.Master_ContactChannelId.HasValue) throw new ExceptionCustom("ระบุช่องทางการติดต่อ");
				if (String.IsNullOrEmpty(model.MeetName)) throw new ExceptionCustom("ระบุผู้ติดต่อ");
				if (String.IsNullOrEmpty(model.Tel)) throw new ExceptionCustom("ระบุเบอร์โทร");
				if (!model.ResultMeetId.HasValue) throw new ExceptionCustom("ระบุผลการเข้าพบ");
				if (model.ResultMeetId != 1 && model.ResultMeetId != 2) throw new ExceptionCustom("ระบุผลการเข้าพบไม่ถูกต้อง");
				if (!model.NextActionId.HasValue) throw new ExceptionCustom("ระบุ Next Action");
				if (!model.AppointmentDate.HasValue) throw new ExceptionCustom("ระบุวันที่นัดหมาย");
				if (model.AppointmentDate.Value.Date < DateTime.Now.Date) throw new ExceptionCustom("วันที่นัดหมายต้องไม่น้อยกว่าวันที่ปัจจุบัน");
				if (!model.AppointmentTime.HasValue) throw new ExceptionCustom("ระบุเวลาที่นัดหมาย");
				if (String.IsNullOrEmpty(model.Location)) throw new ExceptionCustom("ระบุสถานที่");
			}
			else if (model.ProceedId == 3)
			{
				if (!model.ResultMeetId.HasValue) throw new ExceptionCustom("ระบุผลการเข้าพบ");
				if (model.ResultMeetId != 1 && model.ResultMeetId != 2) throw new ExceptionCustom("ระบุผลการเข้าพบไม่ถูกต้อง");
				if (!model.NextActionId.HasValue) throw new ExceptionCustom("ระบุ Next Action");
				if (!model.AppointmentDate.HasValue) throw new ExceptionCustom("ระบุวันที่นัดหมาย");
				if (model.AppointmentDate.Value.Date < DateTime.Now.Date) throw new ExceptionCustom("วันที่นัดหมายต้องไม่น้อยกว่าวันที่ปัจจุบัน");
				if (!model.AppointmentTime.HasValue) throw new ExceptionCustom("ระบุเวลาที่นัดหมาย");
				if (String.IsNullOrEmpty(model.Location)) throw new ExceptionCustom("ระบุสถานที่");
			}

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			int statusSaleId = StatusSaleModel.NotStatus;
			string? topicName = "ผลลัพธ์";
			string? proceedName = null;
			string? resultMeetName = string.Empty;
			string? nextActionName = string.Empty;

			DateTime _dateNow = DateTime.Now;

			Sale_Result sale_Result = new();
			sale_Result.Status = StatusModel.Active;
			sale_Result.CreateDate = _dateNow;
			sale_Result.SaleId = model.SaleId;
			sale_Result.SaleReplyId = model.SaleReplyId;
			sale_Result.ProceedId = model.ProceedId;
			sale_Result.NextActionId = model.NextActionId;
			sale_Result.DateContact = model.DateContact;
			sale_Result.Master_ContactChannelId = model.Master_ContactChannelId;
			sale_Result.MeetName = model.MeetName;
			sale_Result.Tel = model.Tel;
			sale_Result.AppointmentDate = model.AppointmentDate;
			sale_Result.AppointmentTime = model.AppointmentTime;
			sale_Result.Location = model.Location;
			sale_Result.Note = model.Note;
			await _db.InsterAsync(sale_Result);
			await _db.SaveAsync();

			statusSaleId = StatusSaleModel.Results;

			resultMeetName = model.ResultMeetId == 1 ? "เข้าพบสำเร็จ" : model.ResultMeetId == 2 ? "เข้าพบไม่สำเร็จ" : "";

			//1=แจ้งข้อมูลเพิ่มเติม 2=ติดต่อขอเอกสาร 3=เข้าพบรับเอกสาร 4=ไม่ผ่านการพิจารณา
			if (model.ProceedId == 1 || model.ProceedId == 2 || model.ProceedId == 3)
			{
				proceedName = model.ProceedId == 1 ? "แจ้งข้อมูลเพิ่มเติม"
					: model.ProceedId == 2 ? "ติดต่อขอเอกสาร"
					: model.ProceedId == 3 ? "เข้าพบรับเอกสาร"
					: model.ProceedId == 4 ? "ไม่ผ่านการพิจารณา"
					: model.ProceedId == 5 ? "รอปิดการขาย"
					: string.Empty;
			}

			DateTime createDate = DateTime.Now;
			if (!await _repo.Sales.CheckStatusById(model.SaleId, statusSaleId))
			{
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateDate = createDate,
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				});
			}

			if (model.ProceedId == 4)
			{
				statusSaleId = StatusSaleModel.ResultsNotConsidered;
				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = statusSaleId,
					CreateDate = createDate,
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
				});
			}
			else
			{
				if (model.NextActionId == 1)
				{
					nextActionName = "ทำการนัดหมาย";
				}
				else if (model.NextActionId == 2 || model.ProceedId == 5)
				{
					statusSaleId = StatusSaleModel.WaitCloseSale;
					nextActionName = "รอปิดการขาย";
					await _repo.Sales.UpdateStatusOnly(new()
					{
						SaleId = model.SaleId,
						StatusId = statusSaleId,
						CreateDate = createDate.AddSeconds(1),
						CreateBy = model.CurrentUserId,
						CreateByName = currentUserName,
					});
				}
			}

			await CreateContactHistory(new()
			{
				CurrentUserId = model.CurrentUserId,
				SaleId = model.SaleId,
				SaleReplyId = model.SaleReplyId,
				ProcessSaleCode = ProcessSaleCodeModel.Result,
				StatusSaleId = statusSaleId,
				TopicName = topicName,
				ProceedName = proceedName,
				MeetFullName = model.MeetName,
				ResultMeetName = resultMeetName,
				NextActionName = nextActionName,
				AttachmentPath = model.AttachmentPath,
				AppointmentDate = model.AppointmentDate,
				AppointmentTime = model.AppointmentTime,
				Location = model.Location,
				Note = model.Note,
				NoteSystem = NoteSystem
			});

			return _mapper.Map<Sale_ResultCustom>(sale_Result);
		}

		public async Task<Sale_Close_SaleCustom> CreateCloseSale(Sale_Close_SaleCustom model)
		{
			var sale = await _repo.Sales.GetStatusById(model.SaleId);
			if (sale == null) throw new ExceptionCustom("saleid not found.");

			if (sale.StatusSaleMainId != StatusSaleMainModel.CloseSale)
			{
				throw new ExceptionCustom("สถานะการขายไม่ถูกต้อง");
			}
			if (sale.StatusSaleId == StatusSaleModel.CloseSale)
			{
				throw new ExceptionCustom("ปิดการขายแล้ว");
			}

			//if (!model.ContactDate.HasValue) throw new ExceptionCustom("ระบุวันที่ติดต่อ");
			if (String.IsNullOrEmpty(model.Name)) throw new ExceptionCustom("ระบุชื่อผู้ติดต่อ");
			if (String.IsNullOrEmpty(model.Tel)) throw new ExceptionCustom("ระบุเบอร์ติดต่อ");

			if (model.ResultMeetId != 1 && model.ResultMeetId != 2) throw new ExceptionCustom("ระบุผลการติดต่อไม่ถูกต้อง");
			if (model.NextActionId != 1 && model.NextActionId != 2) throw new ExceptionCustom("ระบุ Next Action ไม่ถูกต้อง");

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			int statusSaleId = StatusSaleModel.WaitCloseSale;
			string? topicName = "รอปิดการขาย";
			string? desireLoanName = null;
			string? reason = null;
			string? resultContactName = string.Empty;
			string? descriptionStatus = null;
			string? nextActionName = string.Empty;

			DateTime _dateNow = DateTime.Now;

			Sale_Close_Sale sale_Close_Sale = new();
			sale_Close_Sale.Status = StatusModel.Active;
			sale_Close_Sale.CreateDate = _dateNow;
			sale_Close_Sale.SaleId = model.SaleId;
			sale_Close_Sale.SaleReplyId = model.SaleReplyId;
			sale_Close_Sale.Name = model.Name;
			sale_Close_Sale.Tel = model.Tel;
			sale_Close_Sale.ContactDate = model.ContactDate;
			sale_Close_Sale.ResultMeetId = model.ResultMeetId;
			sale_Close_Sale.NextActionId = model.NextActionId;
			sale_Close_Sale.AppointmentDate = model.AppointmentDate;
			sale_Close_Sale.AppointmentTime = model.AppointmentTime;
			sale_Close_Sale.Location = model.Location;
			sale_Close_Sale.Note = model.Note;
			sale_Close_Sale.DesireLoanId = model.DesireLoanId;
			sale_Close_Sale.Master_Reason_CloseSaleId = model.Master_Reason_CloseSaleId;
			await _db.InsterAsync(sale_Close_Sale);
			await _db.SaveAsync();

			resultContactName = model.ResultMeetId == 1 ? "รับสาย" : model.ResultMeetId == 2 ? "ไม่รับสาย" : "";

			//if (model.ResultMeetId == 2)
			//{
			//	if (String.IsNullOrEmpty(model.Note)) throw new ExceptionCustom("ระบุหมายเหตุ");
			//}

			if (model.ResultMeetId == 2 && model.NextActionId == 1)
			{
				throw new ExceptionCustom("ไม่สามารถเลือกปิดการขาย เนื่องจากลูกค้าไม่รับสาย");
			}

			if (model.NextActionId == 2)
			{
				if (!model.AppointmentDate.HasValue) throw new ExceptionCustom("ระบุวันที่นัดหมาย");
				if (model.AppointmentDate.Value.Date < DateTime.Now.Date) throw new ExceptionCustom("วันที่นัดหมายต้องไม่น้อยกว่าวันที่ปัจจุบัน");
				if (!model.AppointmentTime.HasValue) throw new ExceptionCustom("ระบุเวลาที่นัดหมาย");
				//if (String.IsNullOrEmpty(model.Location)) throw new ExceptionCustom("ระบุสถานที่");

				nextActionName = "ติดต่ออีกครั้ง";
			}
			else
			{
				nextActionName = "ปิดการขาย";

				if (sale_Close_Sale.DesireLoanId == 1 || sale_Close_Sale.DesireLoanId == 2)
				{
					if (sale_Close_Sale.DesireLoanId == 1)
					{
						topicName = "ปิดการขาย";
						desireLoanName = "สำเร็จ";
						statusSaleId = StatusSaleModel.CloseSale;
						nextActionName = null;
						model.AppointmentDate = null;
						model.AppointmentTime = null;
						model.Location = null;
					}
					else if (sale_Close_Sale.DesireLoanId == 2)
					{
						topicName = "ปิดการขาย";
						desireLoanName = "ไม่สำเร็จ";
						statusSaleId = StatusSaleModel.CloseSaleNotLoan;
						//if (!model.Master_Reason_CloseSaleId.HasValue) throw new ExceptionCustom("master_Reason_CloseSaleId not found.");
					}

					if (model.Master_Reason_CloseSaleId.HasValue)
					{
						descriptionStatus = await _repo.MasterReasonCloseSale.GetNameById(model.Master_Reason_CloseSaleId.Value);
						reason = descriptionStatus;
					}

					await _repo.Sales.UpdateStatusOnly(new()
					{
						SaleId = model.SaleId,
						StatusId = statusSaleId,
						CreateBy = model.CurrentUserId,
						CreateByName = currentUserName,
						Description = descriptionStatus,
						Master_Reason_CloseSaleId = model.Master_Reason_CloseSaleId
					});

					await _repo.Dashboard.UpdateDurationById(new() { saleid = model.SaleId });
					await _repo.Dashboard.UpdateActivityById(new() { saleid = model.SaleId });
					await _repo.Dashboard.UpdateDeliverById(new() { saleid = model.SaleId });
					await _repo.Dashboard.UpdateTarget_SaleById(new() { userid = sale.AssUserId, year = sale.CreateDate.Year.ToString() });
				}
				else
				{
					throw new ExceptionCustom("ระบุผลคำขอสินเชื่อไม่ถูกต้อง");
				}
			}

			await _repo.Sales.CreateInfo(new()
			{
				SaleId = model.SaleId,
				FullName = model.Name,
				Tel = model.Tel
			});

			await CreateContactHistory(new()
			{
				CurrentUserId = model.CurrentUserId,
				SaleId = model.SaleId,
				SaleReplyId = model.SaleReplyId,
				ProcessSaleCode = ProcessSaleCodeModel.CloseSale,
				StatusSaleId = statusSaleId,
				TopicName = topicName,
				ContactFullName = model.Name,
				ContactDate = model.ContactDate,
				ContactTel = model.Tel,
				ResultContactName = resultContactName,
				DesireLoanName = desireLoanName,
				Reason = reason,
				NextActionName = nextActionName,
				AppointmentDate = model.AppointmentDate,
				AppointmentTime = model.AppointmentTime,
				Location = model.Location,
				Note = model.Note,
				NoteSystem = NoteSystem
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
			sale_Contact_History.ProcessSaleCode = model.ProcessSaleCode;
			sale_Contact_History.SaleId = model.SaleId;
			sale_Contact_History.SaleReplyId = model.SaleReplyId;
			sale_Contact_History.StatusSaleId = model.StatusSaleId;

			sale_Contact_History.TopicName = model.TopicName;
			sale_Contact_History.ContactFullName = model.ContactFullName;
			sale_Contact_History.ContactDate = model.ContactDate;
			sale_Contact_History.ContactTel = model.ContactTel;
			sale_Contact_History.ProceedName = model.ProceedName;
			sale_Contact_History.ResultContactName = model.ResultContactName;
			sale_Contact_History.MeetFullName = model.MeetFullName;
			sale_Contact_History.ResultMeetName = model.ResultMeetName;
			sale_Contact_History.NextActionName = model.NextActionName;
			sale_Contact_History.CreditLimit = model.CreditLimit;
			sale_Contact_History.Percent = model.Percent;
			sale_Contact_History.PercentChanceLoanPass = model.PercentChanceLoanPass;

			sale_Contact_History.AttachmentPath = model.AttachmentPath;
			sale_Contact_History.AppointmentDate = model.AppointmentDate;
			sale_Contact_History.AppointmentTime = model.AppointmentTime;
			sale_Contact_History.Location = model.Location;
			sale_Contact_History.DesireLoanName = model.DesireLoanName;
			sale_Contact_History.Reason = model.Reason;
			sale_Contact_History.Note = model.Note;
			sale_Contact_History.NoteSystem = model.NoteSystem;
			await _db.InsterAsync(sale_Contact_History);
			await _db.SaveAsync();

			return _mapper.Map<Sale_Contact_HistoryCustom>(sale_Contact_History);
		}

		public async Task<List<Sale_DocumentCustom>> GetListDocument(allFilter model)
		{
			var query = await _repo.Context.Sale_Documents.Where(x => x.Status != StatusModel.Delete && x.SaleId == model.id).OrderByDescending(x => x.CreateDate).Skip(0).Take(1).ToListAsync();

			return _mapper.Map<List<Sale_DocumentCustom>>(query);
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

		public async Task<List<Sale_Contact_HistoryCustom>> GetListCalendar(allFilter model)
		{
			var query = _repo.Context.Sale_Contact_Histories
												 .Include(x => x.Sale.Sale_Contact_Histories)
												 .Where(x => x.Status != StatusModel.Delete && x.AppointmentDate.HasValue)
												 .OrderBy(x => x.AppointmentDate)
												 .AsQueryable();

			//query = query.Where(x => x.AppointmentDate.HasValue);

			if (model.userid.HasValue)
			{
				query = query.Where(x => x.Sale.AssUserId == model.userid);
			}

			if (model.statussaleid.HasValue)
			{
				//รอยื่นเอกสาร
				if (model.statussaleid == StatusSaleModel.WaitSubmitDocument && model.psalecode == ProcessSaleCodeModel.Document)
				{
					query = query.Where(x => x.StatusSaleId == StatusSaleModel.WaitSubmitDocument);
				}
				else
				{
					query = query.Where(x => x.StatusSaleId == model.statussaleid.Value);
				}
			}
			else
			{
				if (!String.IsNullOrEmpty(model.psalecode))
				{
					query = query.Where(x => x.ProcessSaleCode == model.psalecode);
				}
			}

			if (model.startdate.HasValue && !model.enddate.HasValue)
			{
				query = query.Where(x => x.AppointmentDate.HasValue && x.AppointmentDate.Value.Date >= model.startdate.Value.Date).OrderBy(x => x.AppointmentDate);
			}
			if (!model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.AppointmentDate.HasValue && x.AppointmentDate.Value.Date <= model.enddate.Value.Date).OrderBy(x => x.AppointmentDate);
			}
			if (model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.AppointmentDate.HasValue && x.AppointmentDate.Value.Date >= model.startdate.Value.Date && x.AppointmentDate.Value.Date <= model.enddate.Value.Date).OrderBy(x => x.AppointmentDate);
			}

			//สร้าง Scheduled Hangfire แล้ว
			if (model.isScheduledJob.HasValue)
			{
				//1=กำหนดเวลาแล้ว
				if (model.isScheduledJob.Value == 1)
				{
					query = query.Where(x => x.IsScheduledJob == model.isScheduledJob.Value);
				}
				else
				{
					query = query.Where(x => x.IsScheduledJob == null || x.IsScheduledJob == 0);
				}
			}

			var response = await query.Select(x => new Sale_Contact_HistoryCustom()
			{
				Id = x.Id,
				SaleId = x.SaleId,
				StatusSaleId = x.StatusSaleId,
				TopicName = x.TopicName,
				ContactFullName = x.ContactFullName,
				ProceedName = x.ProceedName,
				ContactDate = x.ContactDate,
				ContactTel = x.ContactTel,
				ResultContactName = x.ResultContactName,
				MeetFullName = x.MeetFullName,
				ResultMeetName = x.ResultMeetName,
				ProcessSaleCode = x.ProcessSaleCode,
				NextActionName = x.NextActionName,
				AppointmentDate = x.AppointmentDate,
				AppointmentTime = x.AppointmentTime,
				Location = x.Location,
				Note = x.Note,
				AssUserId = x.Sale.AssUserId
			}).ToListAsync();

			return response;
			//return _mapper.Map<List<Sale_Contact_HistoryCustom>>(await query.ToListAsync());
		}

		public async Task UpdateScheduledJob(Guid id)
		{
			var sale_Contact_Histories = await _repo.Context.Sale_Contact_Histories.FirstOrDefaultAsync(x => x.Id == id);
			if (sale_Contact_Histories == null) throw new ExceptionCustom("id not found.");

			sale_Contact_Histories.IsScheduledJob = 1;
			_db.Update(sale_Contact_Histories);
			await _db.SaveAsync();
		}

		public async Task UpdateScheduledJobSucceed(Guid id)
		{
			var sale_Contact_Histories = await _repo.Context.Sale_Contact_Histories.FirstOrDefaultAsync(x => x.Id == id);
			if (sale_Contact_Histories == null) throw new ExceptionCustom("id not found.");

			sale_Contact_Histories.IsScheduledJobSucceed = 1;
			_db.Update(sale_Contact_Histories);
			await _db.SaveAsync();
		}

		public async Task<Sale_ContactCustom> CreateContactDiscard(Sale_ContactCustom model)
		{
			//20240826 คุณเจนขอปรับ โดยตัดบางส่วนออก เลย fix ส่วนนี้
			model.NextActionId = 3;
			model.DesireLoanId = 2;

			var sale = await _repo.Sales.GetStatusById(model.SaleId);
			if (sale == null) throw new ExceptionCustom("saleid not found.");

			if (sale.StatusSaleMainId == StatusSaleMainModel.CloseSale || sale.StatusSaleId == StatusSaleModel.CloseSale)
			{
				throw new ExceptionCustom("statussale not match");
			}

			if (String.IsNullOrEmpty(model.Name)) throw new ExceptionCustom("ระบุชื่อผู้ติดต่อ");
			if (String.IsNullOrEmpty(model.Tel)) throw new ExceptionCustom("ระบุเบอร์ติดต่อ");
			if (!model.ContactDate.HasValue) throw new ExceptionCustom("ระบุวันที่ติดต่อ");
			//if (!model.ContactResult.HasValue) throw new ExceptionCustom("ระบุผลการติดต่อ");
			if (!model.NextActionId.HasValue) throw new ExceptionCustom("ระบุ Next Action");
			if (!model.DesireLoanId.HasValue) throw new ExceptionCustom("ระบุความประสงค์กู้");
			//if (model.ContactResult != 1 && model.ContactResult != 2) throw new ExceptionCustom("contactResult not match");
			if (model.NextActionId != 3) throw new ExceptionCustom("nextActionId not match");
			if (model.DesireLoanId != 2) throw new ExceptionCustom("desireLoanId not match");

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			int statusSaleId = StatusSaleModel.NotStatus;
			string? topicName = "ติดต่อ";
			string? resultContactName = string.Empty;
			string? nextActionName = string.Empty;
			string? desireLoanName = string.Empty;
			string? descriptionStatus = null;

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
			sale_Contact.Note = model.Note;
			sale_Contact.DesireLoanId = model.DesireLoanId;
			sale_Contact.Master_Reason_CloseSaleId = model.Master_Reason_CloseSaleId;
			await _db.InsterAsync(sale_Contact);
			await _db.SaveAsync();

			//if (model.ContactResult == 1 || model.ContactResult == 2)
			//{
			//	resultContactName = model.ContactResult == 1 ? "รับสาย" : "ไม่รับสาย";
			//}
			if (model.NextActionId == 3)
			{
				nextActionName = "ส่งกลับรายการ";
			}

			if (sale_Contact.DesireLoanId == 2)
			{
				topicName = "ไม่ประสงค์กู้";
				desireLoanName = "ไม่ประสงค์กู้";
				statusSaleId = StatusSaleModel.CloseSaleNotLoan;
				if (!model.Master_Reason_CloseSaleId.HasValue) throw new ExceptionCustom("master_Reason_CloseSaleId not found.");
			}

			if (model.Master_Reason_CloseSaleId.HasValue)
			{
				descriptionStatus = await _repo.MasterReasonCloseSale.GetNameById(model.Master_Reason_CloseSaleId.Value);
			}

			DateTime createDate = DateTime.Now;

			if (model.DesireLoanId == 2)
			{
				statusSaleId = StatusSaleModel.CloseSaleNotLoan;
			}
			await _repo.Sales.UpdateStatusOnly(new()
			{
				SaleId = model.SaleId,
				StatusId = statusSaleId,
				CreateDate = createDate,
				CreateBy = model.CurrentUserId,
				CreateByName = currentUserName,
				Description = descriptionStatus,
				Master_Reason_CloseSaleId = model.Master_Reason_CloseSaleId
			});

			await CreateContactHistory(new()
			{
				CurrentUserId = model.CurrentUserId,
				SaleId = model.SaleId,
				ProcessSaleCode = ProcessSaleCodeModel.CloseSale,
				StatusSaleId = statusSaleId,
				TopicName = topicName,
				ContactFullName = model.Name,
				ContactDate = model.ContactDate,
				ContactTel = model.Tel,
				ResultContactName = resultContactName,
				NextActionName = nextActionName,
				DesireLoanName = desireLoanName,
				Reason = descriptionStatus,
				Note = model.Note
			});

			return _mapper.Map<Sale_ContactCustom>(sale_Contact);
		}

		public async Task<List<Sale_PhoenixCustom>?> GetPhoenixBySaleId(Guid id)
		{
			var query = await _repo.Context.Sale_Phoenixes.Where(x => x.Status != StatusModel.Delete && x.SaleId == id).OrderBy(x => x.CreateDate).ToListAsync();

			return _mapper.Map<List<Sale_PhoenixCustom>>(query);
		}

		public async Task UpdatePhoenix(PhoenixModel model, List<Sale_PhoenixCustom>? phoenix)
		{
			if (string.IsNullOrEmpty(model.CIF))
				throw new ExceptionCustom("ระบุ CIF");

			if (!Regex.IsMatch(model.CIF, @"^\d{1,12}$"))
			{
				throw new ExceptionCustom("ระบุตัวเลขเท่านั้นความยาวไม่เกิน 12 หลัก");
			}

			if (model.SaleId == Guid.Empty)
				throw new ExceptionCustom("ระบุ SaleId");

			var sales = await _repo.Context.Sales.Where(x => x.Id == model.SaleId).FirstOrDefaultAsync();
			if (sales != null)
			{
				if (sales.StatusSaleMainId != StatusSaleMainModel.Document || sales.StatusSaleId != StatusSaleModel.WaitAPIPHOENIX)
				{
					throw new ExceptionCustom("สถานะการขายไม่ถูกต้อง");
				}

				sales.CIF = model.CIF;
				_db.Update(sales);
				await _db.SaveAsync();

				var customers = await _repo.Context.Customers.Where(x => x.Id == sales.CustomerId).FirstOrDefaultAsync();
				if (customers != null)
				{
					customers.CIF = model.CIF;
					_db.Update(customers);
					await _db.SaveAsync();
				}

				if (phoenix != null && phoenix.Count > 0)
				{
					await SyncPhoenixBySaleId(model.SaleId, phoenix);
				}

				await _repo.Sales.UpdateStatusOnly(new()
				{
					SaleId = model.SaleId,
					StatusId = StatusSaleModel.WaitResults,
					CreateBy = model.CurrentUserId
				});
			}
			else
			{
				throw new ExceptionCustom("ไม่พบ SaleId");
			}
		}

		public async Task SyncPhoenixBySaleId(Guid id, List<Sale_PhoenixCustom>? phoenix)
		{
			var sales = await _repo.Context.Sales.Where(x => x.Id == id).FirstOrDefaultAsync();
			if (sales == null) throw new ExceptionCustom("ไม่พบข้อมูล");

			DateTime _dateNow = DateTime.Now;

			if (phoenix != null && phoenix.Count > 0)
			{
				_repo.Context.Sale_Phoenixes.RemoveRange(_repo.Context.Sale_Phoenixes.Where(x => x.SaleId == id));
				await _db.SaveAsync();

				foreach (var item in phoenix)
				{
					Sale_Phoenix sale_Phoenix = new();
					sale_Phoenix.Status = StatusModel.Active;
					sale_Phoenix.CreateDate = _dateNow;
					sale_Phoenix.SaleId = id;
					sale_Phoenix.workflow_id = item.workflow_id;
					sale_Phoenix.app_no = item.app_no;
					sale_Phoenix.ana_no = item.ana_no;
					sale_Phoenix.fin_type = item.fin_type;
					sale_Phoenix.cif_no = item.cif_no;
					sale_Phoenix.cif_name = item.cif_name;
					sale_Phoenix.branch_customer = item.branch_customer;
					sale_Phoenix.branch_user = item.branch_user;
					sale_Phoenix.approve_level = item.approve_level;
					sale_Phoenix.status_type = item.status_type;
					sale_Phoenix.status_code = item.status_code;
					sale_Phoenix.create_by = item.create_by;
					sale_Phoenix.created_date = item.created_date;
					sale_Phoenix.update_by = item.update_by;
					sale_Phoenix.update_date = item.update_date;
					sale_Phoenix.approve_by = item.approve_by;
					sale_Phoenix.approve_date = item.approve_date;
					await _db.InsterAsync(sale_Phoenix);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<Sale_Document_UploadCustom> CreateDocumentFile(Sale_Document_UploadCustom model)
		{
			if (model.Files == null || model.Files.FileData == null) throw new ExceptionCustom("files not found");

			if (model.Type != 6)
			{
				var UploaR = await _repo.Context.Sale_Document_Uploads
					.Where(x => x.Status == StatusModel.Active && x.SaleId == model.SaleId && x.Type == model.Type).ToListAsync();
				if (UploaR.Count > 0)
				{
					foreach (var item in UploaR)
					{
						item.Status = StatusModel.Delete;
						_db.Update(item);
						await _db.SaveAsync();
					}
				}
			}

			var originalFileName = model.OriginalFileName ?? model.Files.FileData.FileName;
			//if (model.Type == DocumentFileType.IDCard) originalFileName = "บัตรประชาชน";

			var extension = Path.GetExtension(model.Files.FileData.FileName);

			Sale_Document_Upload sale_Document_Upload = new();
			sale_Document_Upload.Status = StatusModel.Active;
			sale_Document_Upload.CreateDate = DateTime.Now;
			sale_Document_Upload.CreateBy = model.CurrentUserId;
			sale_Document_Upload.SaleId = model.SaleId;
			sale_Document_Upload.Type = model.Type;
			sale_Document_Upload.OriginalFileName = originalFileName;
			sale_Document_Upload.MimeType = extension;
			await _db.InsterAsync(sale_Document_Upload);
			await _db.SaveAsync();

			var _upload = await GeneralUtils.UploadFormFile(new FileModel()
			{
				appSet = _appSet,
				FileData = model.Files.FileData,
				Folder = $@"sale_document\{GeneralUtils.GetYearEn(DateTime.Now.Year)}",
				Id = sale_Document_Upload.Id.ToString(),
				MimeType = extension
			});

			if (_upload == null) throw new ExceptionCustom("upload file error");

			sale_Document_Upload.FileSize = model.Files.FileData.Length;
			sale_Document_Upload.FileName = $"{sale_Document_Upload.Id.ToString()}{extension}";
			sale_Document_Upload.Url = _upload.UploadUrl ?? string.Empty;
			_db.Update(sale_Document_Upload);
			await _db.SaveAsync();

			return _mapper.Map<Sale_Document_UploadCustom>(sale_Document_Upload);
		}

		public async Task<Sale_Document_UploadCustom> UpdateDocumentFile(Sale_Document_UploadCustom model)
		{
			var sale_Document_File = await _repo.Context.Sale_Document_Uploads.FirstOrDefaultAsync(x => x.Id == model.Id);
			if (sale_Document_File == null) throw new ExceptionCustom("id not found.");

			sale_Document_File.Url = model.Url;
			sale_Document_File.FileName = model.FileName;
			_db.Update(sale_Document_File);
			await _db.SaveAsync();
			return _mapper.Map<Sale_Document_UploadCustom>(sale_Document_File);
		}

		public async Task<Sale_Document_UploadCustom> GetDocumentFileById(Guid id)
		{
			var query = await _repo.Context.Sale_Document_Uploads
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Sale_Document_UploadCustom>(query);
		}

		public async Task<Sale_Document_UploadCustom> GetDocumentFileSaleType(Guid saleid, short type)
		{
			var query = await _repo.Context.Sale_Document_Uploads
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.SaleId == saleid && x.Type == type);

			return _mapper.Map<Sale_Document_UploadCustom>(query);
		}

		public async Task DocumentFileById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Sale_Document_Uploads.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
			if (query != null)
			{
				query.Status = StatusModel.Delete;
				_db.Update(query);
				await _db.SaveAsync();
			}
		}

		public async Task<List<Sale_Document_UploadCustom>> GetListDocumentFile(allFilter model)
		{
			var query = await _repo.Context.Sale_Document_Uploads
				.Where(x => x.Status != StatusModel.Delete && x.SaleId == model.saleid)
				.OrderBy(x => x.Type)
				.ToListAsync();

			return _mapper.Map<List<Sale_Document_UploadCustom>>(query);
		}

		public async Task DocumentFileDeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Sale_Document_Uploads.Where(x => x.Id == id).FirstOrDefaultAsync();
			if (query != null)
			{
				query.Status = StatusModel.Delete;
				_db.Update(query);
				await _db.SaveAsync();
			}
		}
	}
}
