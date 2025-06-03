using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.ProcessSales
{
    public partial class SettingProcessSalesForm_Backup
    {
        [Parameter]
        public Guid id { get; set; }

        string? _errorMessage = null;
        private bool isLoading = false;
        private User_PermissionCustom _permission = new();
        private ProcessSaleCustom formModel = new();

        protected override async Task OnInitializedAsync()
        {
            _permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetProcessSales) ?? new User_PermissionCustom();
            StateHasChanged();

            await SetModel();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
                StateHasChanged();
                firstRender = false;
            }
        }

        protected async Task SetModel()
        {
            var data = await _processSaleViewModel.GetById(id);
            if (data != null && data.Status && data.Data != null)
            {
                formModel = data.Data;

                //if (formModel.ProcessSaleItems == null || formModel.ProcessSaleItems.Count == 0)
                //{
                //	await InsertItem();
                //}
            }
            else
            {
                _errorMessage = data?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }
        }

        //protected async Task OnItemTypes(ChangeEventArgs e, Guid saleItemId)
        //{
        //	var item = formModel.ProcessSaleItems?.FirstOrDefault(r => r.Id == saleItemId);
        //	if (e.Value != null && item != null)
        //	{
        //		item.ItemType = e.Value.ToString();
        //		item.ProcessSaleItemOptions = new List<ProcessSaleItemOptionCustom>();
        //		//if (item.ItemType == FieldTypes.Multiplechoice || item.ItemType == FieldTypes.Dropdown)
        //		//{
        //		await InsertItemOption(saleItemId);
        //		//}

        //		await Task.Delay(1);
        //		StateHasChanged();
        //	}
        //}

        //protected async Task InsertItem()
        //{
        //	if (formModel.ProcessSaleItems == null) formModel.ProcessSaleItems = new();

        //	int sequenceNo = 1;
        //	var saleItems = formModel.ProcessSaleItems.Where(x => x.Status == StatusModel.Active).ToList();
        //	if (saleItems.Count() > 0)
        //	{
        //		sequenceNo = saleItems.Max(x => x.SequenceNo) + 1;
        //	}

        //	Guid _idGen = Guid.NewGuid();
        //	formModel.ProcessSaleItems.Add(new()
        //	{
        //		Id = _idGen,
        //		Status = StatusModel.Active,
        //		ProcessSaleId = id,
        //		SequenceNo = sequenceNo,
        //		ItemType = FieldTypes.ShortAnswer
        //	});
        //	await InsertItemOption(_idGen);

        //	await Task.Delay(1);
        //	StateHasChanged();
        //}

        //protected async Task RemoveItem(Guid saleItemId)
        //{
        //	var item = formModel.ProcessSaleItems?.FirstOrDefault(r => r.Id == saleItemId);
        //	if (item != null)
        //	{
        //		item.Status = StatusModel.Delete;
        //		//formModel.ProcessSaleItems?.Remove(item);

        //		SetSequenceItem();
        //	}

        //	await Task.Delay(1);
        //	StateHasChanged();
        //}

        //protected void SetSequenceItem()
        //{
        //	var saleItems = formModel.ProcessSaleItems?.Where(x => x.Status == StatusModel.Active).ToList() ?? new();

        //	if (saleItems.Count() > 0)
        //	{
        //		int sequenceNo = 1;
        //		foreach (var item in saleItems.OrderBy(x => x.SequenceNo))
        //		{
        //			item.SequenceNo = sequenceNo;
        //			sequenceNo++;
        //		}
        //	}
        //	StateHasChanged();
        //}

        //protected async Task InsertItemOption(Guid saleItemId)
        //{
        //	var item = formModel.ProcessSaleItems?.FirstOrDefault(r => r.Id == saleItemId);
        //	if (item != null)
        //	{
        //		if (item.ProcessSaleItemOptions == null) item.ProcessSaleItemOptions = new();

        //		int sequenceNo = 1;

        //		var itemOptions = item.ProcessSaleItemOptions.Where(x => x.Status == StatusModel.Active).ToList();

        //		if (itemOptions.Count() > 0)
        //		{
        //			sequenceNo = itemOptions.Max(x => x.SequenceNo) + 1;
        //		}

        //		item.ProcessSaleItemOptions.Add(new()
        //		{
        //			Id = Guid.NewGuid(),
        //			Status = StatusModel.Active,
        //			ProcessSaleItemId = saleItemId,
        //			SequenceNo = sequenceNo
        //		});
        //	}

        //	await Task.Delay(1);
        //	StateHasChanged();
        //}

        //protected async Task RemoveItemOption(Guid saleItemId, Guid saleItemOptionId)
        //{
        //	var item = formModel.ProcessSaleItems?.FirstOrDefault(r => r.Id == saleItemId);
        //	if (item != null && item.ProcessSaleItemOptions?.Count > 0)
        //	{
        //		var itemOption = item.ProcessSaleItemOptions.FirstOrDefault(r => r.Id == saleItemOptionId);
        //		if (itemOption != null)
        //		{
        //			itemOption.Status = StatusModel.Delete;
        //			//item.ProcessSaleItemOptions.Remove(itemOption);

        //			int sequenceNo = 1;
        //			foreach (var item_option in item.ProcessSaleItemOptions.Where(x => x.Status == StatusModel.Active).OrderBy(x => x.SequenceNo))
        //			{
        //				item_option.SequenceNo = sequenceNo;
        //				sequenceNo++;
        //			}
        //		}

        //	}
        //	await Task.Delay(1);
        //	StateHasChanged();
        //}

        //private async Task Save()
        //{
        //	_errorMessage = null;
        //	ShowLoading();

        //	formModel.CurrentUserId = UserInfo.Id;

        //	var response = await _processSaleViewModel.Update(formModel);

        //	if (response.Status)
        //	{
        //		await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
        //		HideLoading();
        //		Cancel();
        //	}
        //	else
        //	{
        //		HideLoading();
        //		_errorMessage = response.errorMessage;
        //		await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
        //	}
        //}

        //public void Cancel()
        //{
        //	_Navs.NavigateTo("/setting/processsales");
        //}

        //private void ShowLoading()
        //{
        //	isLoading = true;
        //	StateHasChanged();
        //}

        //private void HideLoading()
        //{
        //	isLoading = false;
        //	StateHasChanged();
        //}

    }
}