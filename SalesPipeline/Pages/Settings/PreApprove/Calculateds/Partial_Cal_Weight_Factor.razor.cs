using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.PropertiesModel;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class Partial_Cal_Weight_Factor
	{
		[Parameter]
		public Guid pre_CalId { get; set; }

		[Parameter]
		public bool IsShowTab { get; set; }

		string? _errorMessage = null;
		private bool isDisableSave = false;
		private bool isLoading = false;
		private Pre_CalCustom formModel = new();
		private List<Pre_Cal_WeightFactorCustom> formWeightModel = new();
		public bool _internalIsShowTab { get; set; }

		protected override async Task OnInitializedAsync()
		{
			_internalIsShowTab = IsShowTab;
			await Task.Delay(1);
		}

		protected override async Task OnParametersSetAsync()
		{
			if (_internalIsShowTab != IsShowTab)
			{
				if (IsShowTab)
				{
					await SetModel();
				}
				_internalIsShowTab = IsShowTab;
			}
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await Task.Delay(1);
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetModel()
		{
			formWeightModel = new();

			List<Pre_Cal_WeightFactor_ItemCustom> factor_Item = new();

			var dataWeight = await _preCalWeightViewModel.GetAllPreCalById(pre_CalId);
			if (dataWeight != null && dataWeight.Status)
			{
				if (dataWeight.Data != null)
				{
					formWeightModel = dataWeight.Data;
				}
			}

			var dataInfo = await _preCalInfoViewModel.GetById(pre_CalId);
			if (dataInfo != null && dataInfo.Status)
			{
				if (dataInfo.Data != null)
				{
					factor_Item = new();

					var weightData = formWeightModel.Where(x => x.Type == PreCalType.Info).FirstOrDefault();
					if (weightData == null)
					{
						factor_Item = new() { new() { Name = "มูลค่าสินเชื่อที่ขอ", Percent = 0 } };
						formWeightModel.Add(new()
						{
							Pre_CalId = pre_CalId,
							Type = PreCalType.Info,
							TotalPercent = 0,
							Pre_Cal_WeightFactor_Items = factor_Item
						});
					}
					StateHasChanged();
				}
			}

			var dataStan = await _preCalStanViewModel.GetById(pre_CalId);
			if (dataStan != null && dataStan.Status)
			{
				if (dataStan.Data != null)
				{
					factor_Item = new();

					if (dataStan.Data.Pre_Cal_Fetu_Stan_Scores?.Count > 0)
					{
						var stanLookUp = dataStan.Data.Pre_Cal_Fetu_Stan_Scores.ToLookup(x => x.Type).OrderBy(x => x.Key);

						var weightData = formWeightModel.Where(x => x.Type == PreCalType.Stan).FirstOrDefault();
						if (weightData == null)
						{
							foreach (var item in stanLookUp)
							{
								factor_Item.Add(new()
								{
									StanScoreType = item.Key,
									Name = PropertiesMain.PerCalFetuStanName(item.Key.ToString() ?? string.Empty)?.Name,
									Percent = 0
								});
							}
							formWeightModel.Add(new()
							{
								Pre_CalId = pre_CalId,
								Type = PreCalType.Stan,
								TotalPercent = 0,
								Pre_Cal_WeightFactor_Items = factor_Item
							});
							StateHasChanged();
						}
					}
				}
			}

			var dataApp = await _preCalAppViewModel.GetById(pre_CalId);
			if (dataApp != null && dataApp.Status)
			{
				if (dataApp.Data != null)
				{
					factor_Item = new();

					if (dataApp.Data.Pre_Cal_Fetu_App_Items?.Count > 0)
					{
						var weightData = formWeightModel.Where(x => x.Type == PreCalType.AppLoan).FirstOrDefault();

						foreach (var item in dataApp.Data.Pre_Cal_Fetu_App_Items)
						{
							decimal _percent = 0;
							if (weightData != null && weightData.Pre_Cal_WeightFactor_Items?.Count > 0)
							{
								_percent = weightData.Pre_Cal_WeightFactor_Items.FirstOrDefault(x => x.RefItemId == item.Id)?.Percent ?? 0;
							}

							factor_Item.Add(new()
							{
								RefItemId = item.Id,
								Name = item.Name,
								Percent = _percent
							});
						}

						if (weightData == null)
						{
							formWeightModel.Add(new()
							{
								Pre_CalId = pre_CalId,
								Type = PreCalType.AppLoan,
								TotalPercent = 0,
								Pre_Cal_WeightFactor_Items = factor_Item
							});
						}
						else
						{
							weightData.Pre_Cal_WeightFactor_Items = factor_Item;
						}
						StateHasChanged();
					}

				}
			}


			var dataBus = await _preCalBusViewModel.GetById(pre_CalId);
			if (dataBus != null && dataBus.Status)
			{
				if (dataBus.Data != null)
				{
					factor_Item = new();

					if (dataBus.Data.Pre_Cal_Fetu_Bus_Items?.Count > 0)
					{
						var weightData = formWeightModel.Where(x => x.Type == PreCalType.BusType).FirstOrDefault();

						foreach (var item in dataBus.Data.Pre_Cal_Fetu_Bus_Items)
						{
							decimal _percent = 0;
							if (weightData != null && weightData.Pre_Cal_WeightFactor_Items?.Count > 0)
							{
								_percent = weightData.Pre_Cal_WeightFactor_Items.FirstOrDefault(x => x.RefItemId == item.Id)?.Percent ?? 0;
							}

							factor_Item.Add(new()
							{
								RefItemId = item.Id,
								Name = item.Name,
								Percent = _percent
							});
						}

						if (weightData == null)
						{
							formWeightModel.Add(new()
							{
								Pre_CalId = pre_CalId,
								Type = PreCalType.BusType,
								TotalPercent = 0,
								Pre_Cal_WeightFactor_Items = factor_Item
							});
						}
						else
						{
							weightData.Pre_Cal_WeightFactor_Items = factor_Item;
						}
						StateHasChanged();
					}

				}
			}

		}

		private async Task Seve()
		{
			var response = await _preCalWeightViewModel.Create(formWeightModel);

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				await SetModel();
			}
			else
			{
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		private void Cancel()
		{
			_Navs.NavigateTo("/setting/pre/calculated");
		}

	}
}