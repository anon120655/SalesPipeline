using Microsoft.AspNetCore.Components;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.PropertiesModel;
using SalesPipeline.Utils.Resources.PreApprove;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class Partial_Cal_Weight_Factor
	{
		[Parameter]
		public Guid pre_CalId { get; set; }

		[Parameter]
		public bool IsShowTab { get; set; }

		string? _errorMessage = null;
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
			var data = await _preCalViewModel.GetIncludeAllById(pre_CalId);
			if (data != null && data.Status)
			{
				if (data.Data != null)
				{
					formModel = data.Data;

					if (formModel.Pre_Cal_Infos != null)
					{
						formWeightModel.Add(new()
						{
							Type = PreCalType.Info,
							TotalPercent = 0,
							Pre_Cal_WeightFactor_Items = new() { new() { Name = "มูลค่าสินเชื่อที่ขอ", Percent = 5 } }
						});
					}

					if (formModel.Pre_Cal_Fetu_Stans != null)
					{
						var fetuStan = formModel.Pre_Cal_Fetu_Stans.FirstOrDefault();
						if (fetuStan != null && fetuStan.Pre_Cal_Fetu_Stan_Scores?.Count > 0)
						{
							List<Pre_Cal_WeightFactor_ItemCustom> factor_Item = new();

							var stanLookUp = fetuStan.Pre_Cal_Fetu_Stan_Scores.ToLookup(x => x.Type).OrderBy(x => x.Key);

							foreach (var item in stanLookUp)
							{
								factor_Item.Add(new()
								{
									StanScoreType = item.Key,
									Percent = item.Key
								});
							}
							formWeightModel.Add(new()
							{
								Type = PreCalType.Stan,
								TotalPercent = 0,
								Pre_Cal_WeightFactor_Items = factor_Item
							});
						}
					}

					StateHasChanged();
					await Task.Delay(1);
				}
			}

		}

	}
}