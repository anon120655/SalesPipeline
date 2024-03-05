using global::Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.ProcessSales
{
	public partial class FormGenerator
	{
		[Parameter]
		public Guid id { get; set; }
		[Parameter]
		public Guid id_reply { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private LookUpResource LookUp = new();
		private Sale_ReplyCustom formModel = new();

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				if (id_reply != Guid.Empty)
				{
					await SetModel();
				}
				else
				{
					await SetDefaultModel();
				}

				StateHasChanged();

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				firstRender = false;
			}
		}

		protected async Task SetModel()
		{
			var data = await _processSaleViewModel.GetReplyById(id_reply);
			if (data != null && data.Status)
			{
				if (data.Data != null)
				{
					formModel = data.Data;
					if (formModel != null)
					{
						var masterLists = await _masterViewModel.MasterLists(new() { status = StatusModel.Active });
						if (masterLists != null && masterLists.Status)
						{
							LookUp.MasterList = masterLists.Data;
						}

						foreach (var reply_sec in formModel.Sale_Reply_Sections ?? new())
						{
							foreach (var reply_sec_item in reply_sec.Sale_Reply_Section_Items ?? new())
							{
								foreach (var reply_sec_item_val in reply_sec_item.Sale_Reply_Section_ItemValues ?? new())
								{
									if (reply_sec_item_val.Master_ListId.HasValue)
									{
										reply_sec_item_val.Path = await SetMasterModel(reply_sec_item_val.Master_ListId.Value);
									}
								}
							}
						}
					}
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task SetDefaultModel()
		{
			var data = await _processSaleViewModel.GetById(id);
			if (data != null && data.Status)
			{
				if (data.Data != null)
				{
					var masterLists = await _masterViewModel.MasterLists(new() { status = StatusModel.Active });
					if (masterLists != null && masterLists.Status)
					{
						LookUp.MasterList = masterLists.Data;
					}

					var saleSections = data.Data.ProcessSale_Sections?.Where(x => x.Status == StatusModel.Active).ToList() ?? new();

					if (saleSections?.Count > 0)
					{
						formModel.Status = data.Data.Status;
						formModel.ProcessSaleId = data.Data.Id;
						formModel.ProcessSaleName = data.Data.Name;
						formModel.Sale_Reply_Sections = new();

						foreach (var section in saleSections)
						{
							var replySection = new Sale_Reply_SectionCustom();
							replySection.Status = section.Status;
							replySection.PSaleSectionId = section.Id;
							replySection.Name = section.Name;
							replySection.Sale_Reply_Section_Items = new();

							var sectionItems = section.ProcessSale_Section_Items?.Where(x => x.Status == StatusModel.Active).ToList() ?? new();

							if (sectionItems?.Count > 0)
							{
								foreach (var item in sectionItems)
								{
									var replyItem = new Sale_Reply_Section_ItemCustom();
									replyItem.Status = item.Status;
									replyItem.PSaleSectionItemId = item.Id;
									replyItem.ItemLabel = item.ItemLabel;
									replyItem.ItemType = item.ItemType;
									replyItem.ShowType = item.ShowType;

									var itemOptions = item.ProcessSale_Section_ItemOptions?.Where(x => x.Status == StatusModel.Active).ToList() ?? new();

									if (itemOptions?.Count > 0)
									{
										replyItem.Sale_Reply_Section_ItemValues = new();

										string? _Path = null;

										foreach (var item_option in itemOptions)
										{
											if (item_option.Master_ListId.HasValue)
											{
												_Path = await SetMasterModel(item_option.Master_ListId.Value);
											}

											replyItem.Sale_Reply_Section_ItemValues.Add(new()
											{
												Status = item_option.Status,
												SaleReplySectionItemId = replyItem.Id,
												PSaleSectionItemOptionId = item_option.Id,
												OptionLabel = item_option.OptionLabel,
												ReplyValue = item_option.DefaultValue,
												Master_ListId = item_option.Master_ListId,
												Path = _Path
											});

										}
									}

									replySection.Sale_Reply_Section_Items.Add(replyItem);
								}
							}

							formModel.Sale_Reply_Sections.Add(replySection);
						}
					}

				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task<string?> SetMasterModel(Guid master_ListId)
		{
			string? _Path = null;

			if (LookUp.MasterList != null)
			{
				var masterData = LookUp.MasterList.FirstOrDefault(x => x.Id == master_ListId);
				if (masterData != null)
				{
					_Path = masterData.Path;
					if (masterData.Path == "/v1/Master/GetYields" && LookUp.Yield == null)
					{
						var dataModel = await _masterViewModel.GetYields(new allFilter() { status = StatusModel.Active });
						if (dataModel != null && dataModel.Status)
						{
							LookUp.Yield = dataModel.Data?.Items;
						}
					}
					else if (masterData.Path == "/v1/Master/GetChains" && LookUp.Chain == null)
					{
						var dataModel = await _masterViewModel.GetChains(new allFilter() { status = StatusModel.Active });
						if (dataModel != null && dataModel.Status)
						{
							LookUp.Chain = dataModel.Data?.Items;
						}
					}
					else if (masterData.Path == "/v1/Master/GetBusinessType" && LookUp.BusinessType == null)
					{
						var dataModel = await _masterViewModel.GetBusinessType(new allFilter() { status = StatusModel.Active });
						if (dataModel != null && dataModel.Status)
						{
							LookUp.BusinessType = dataModel.Data?.Items;
						}
					}
					else if (masterData.Path == "/v1/Master/GetBusinessSize" && LookUp.BusinessSize == null)
					{
						var dataModel = await _masterViewModel.GetBusinessSize(new allFilter() { status = StatusModel.Active });
						if (dataModel != null && dataModel.Status)
						{
							LookUp.BusinessSize = dataModel.Data?.Items;
						}
					}
				}
			}

			return _Path;
		}

		protected void OnCheckBox(ChangeEventArgs e, Guid sectionId, Guid saleItemId, Guid itemOptionId)
		{
			var section = formModel.Sale_Reply_Sections?.FirstOrDefault(r => r.PSaleSectionId == sectionId);
			if (section != null)
			{
				var answerItem = section.Sale_Reply_Section_Items?.FirstOrDefault(r => r.PSaleSectionItemId == saleItemId);
				if (answerItem != null && answerItem.Sale_Reply_Section_ItemValues != null)
				{
					var itemOption = answerItem.Sale_Reply_Section_ItemValues.FirstOrDefault(r => r.PSaleSectionItemOptionId == itemOptionId);
					if (itemOption != null)
					{
						if (bool.TryParse(e.Value?.ToString(), out bool value))
						{
							itemOption.ReplyValue = value.ToString();
						}
					}
				}
			}
		}

		protected void OnDropdown(ChangeEventArgs e, Guid sectionId, Guid saleItemId)
		{
			var section = formModel.Sale_Reply_Sections?.FirstOrDefault(r => r.PSaleSectionId == sectionId);
			if (section != null)
			{
				var answerItem = section.Sale_Reply_Section_Items?.FirstOrDefault(r => r.PSaleSectionItemId == saleItemId);
				if (answerItem != null && answerItem.Sale_Reply_Section_ItemValues != null)
				{
					if (Guid.TryParse(e.Value?.ToString(), out Guid itemOptionId))
					{
						foreach (var item in answerItem.Sale_Reply_Section_ItemValues)
						{
							item.ReplyValue = item.PSaleSectionItemOptionId == itemOptionId ? true.ToString() : null;
						}
					}
				}
			}
		}

		protected void OnDropdownMaster(ChangeEventArgs e, Guid sectionId, Guid saleItemId)
		{
			var section = formModel.Sale_Reply_Sections?.FirstOrDefault(r => r.PSaleSectionId == sectionId);
			if (section != null)
			{
				var answerItem = section.Sale_Reply_Section_Items?.FirstOrDefault(r => r.PSaleSectionItemId == saleItemId);
				if (answerItem != null && answerItem.Sale_Reply_Section_ItemValues != null)
				{
					if (Guid.TryParse(e.Value?.ToString(), out Guid itemOptionId))
					{
						foreach (var item in answerItem.Sale_Reply_Section_ItemValues)
						{
							item.ReplyValue = itemOptionId.ToString();
							item.Master_ListId = item.Master_ListId;
						}
					}
				}
			}
		}

		private async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			formModel.CurrentUserId = UserInfo.Id;

			ResultModel<Sale_ReplyCustom> response;

			if (id_reply != Guid.Empty)
			{
				response = await _processSaleViewModel.UpdateReply(formModel);
			}
			else
			{
				response = await _processSaleViewModel.CreateReply(formModel);
			}

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				HideLoading();
				Cancel();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		public void Cancel()
		{
			_Navs.NavigateTo("/setting/processsales");
		}

		private async Task OnFileuploadChanged(InputFileChangeEventArgs inputFileChangeEvent, Sale_Reply_Section_ItemValueCustom itemValue)
		{
			_errorMessage = null;
			StateHasChanged();
			var file = inputFileChangeEvent.File;
			int _SizeLimit = 50; //MB
			var _Size = 1024000 * _SizeLimit;

			if (file.Size > _Size)
			{
				itemValue.errorMessage = $"Limited Max. {_SizeLimit} MB per file.";
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", itemValue.errorMessage);
			}
			else
			{
				using (var stream = file.OpenReadStream(_Size))
				{
					isLoading = true;
					MemoryStream ms = new MemoryStream();
					await stream.CopyToAsync(ms);

					var fileByte = ms.ToArray();
					var extension = Path.GetExtension(file.Name);
					var fileUpload = await _fileViewModel.Upload(new Utils.Resources.Shares.FileModel()
					{
						FileName = file.Name,
						FileByte = fileByte,
						Folder = FolderTypes.Forms,
						MimeType = extension
					});
					if (fileUpload != null && fileUpload.Status)
					{
						if (fileUpload.Data != null)
						{
							itemValue.FileId = fileUpload.Data.FileId;
							itemValue.FileUrl = fileUpload.Data.FileUrl;
						}
					}
					else
					{
						ClearInputFileMedia(itemValue);
						_errorMessage = fileUpload?.errorMessage;
						_utilsViewModel.AlertWarning(_errorMessage);
					}

					isLoading = false;
					StateHasChanged();
				}
			}
			itemValue._inputFileId = Guid.NewGuid().ToString();
		}

		private void ClearInputFileMedia(Sale_Reply_Section_ItemValueCustom itemValue)
		{
			_errorMessage = null;
			StateHasChanged();
			itemValue.bClearInput = true;
			StateHasChanged();
			itemValue.bClearInput = false;
			StateHasChanged();
		}

		private void ShowLoading()
		{
			isLoading = true;
			StateHasChanged();
		}

		private void HideLoading()
		{
			isLoading = false;
			StateHasChanged();
		}

	}


}