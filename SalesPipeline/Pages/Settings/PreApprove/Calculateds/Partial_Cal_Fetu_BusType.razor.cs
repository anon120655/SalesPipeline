using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
    public partial class Partial_Cal_Fetu_BusType
    {
        [Parameter]
        public Guid pre_CalId { get; set; }

        [Parameter]
        public bool IsShowTab { get; set; }

        string? _errorMessage = null;
        private bool isLoading = false;
        private Pre_Cal_Fetu_BuCustom formModel = new();
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
            var data = await _preCalBusViewModel.GetById(pre_CalId);
            if (data != null && data.Status)
            {
                if (data.Data != null)
                {
                    formModel = data.Data;
                    StateHasChanged();
                }
            }

            if (formModel.Id == Guid.Empty)
            {
                formModel.Pre_Cal_Fetu_Bus_Items = new();
                await InsertItem();
            }

            await Task.Delay(1);
        }

        private async Task Seve()
        {
            ResultModel<Pre_Cal_Fetu_BuCustom> response;

            formModel.Pre_CalId = pre_CalId;
            formModel.CurrentUserId = UserInfo.Id;

            if (formModel.Id == Guid.Empty)
            {
                response = await _preCalBusViewModel.Create(formModel);
            }
            else
            {
                response = await _preCalBusViewModel.Update(formModel);
            }

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

        protected async Task InsertItem()
        {
            if (formModel.Pre_Cal_Fetu_Bus_Items == null) formModel.Pre_Cal_Fetu_Bus_Items = new();

            var BusItemID = Guid.NewGuid();
            formModel.Pre_Cal_Fetu_Bus_Items.Add(new()
            {
                Id = BusItemID,
                Status = StatusModel.Active
            });
            await InsertScore(BusItemID);

            await Task.Delay(1);
            StateHasChanged();
        }

        protected async Task RemoveItem(Guid ID)
        {
            var itemToRemove = formModel.Pre_Cal_Fetu_Bus_Items?.FirstOrDefault(x => x.Id == ID);
            if (itemToRemove != null)
            {
                formModel.Pre_Cal_Fetu_Bus_Items?.Remove(itemToRemove);
            }

            await Task.Delay(1);
            StateHasChanged();
        }

        protected async Task InsertScore(Guid busItemID)
        {
            var bus_Items = formModel.Pre_Cal_Fetu_Bus_Items?.FirstOrDefault(x => x.Id == busItemID);
            if (bus_Items != null)
            {
                if (bus_Items.Pre_Cal_Fetu_Bus_Item_Scores == null) bus_Items.Pre_Cal_Fetu_Bus_Item_Scores = new();

                var bus_Item_ScoreID = Guid.NewGuid();
                bus_Items.Pre_Cal_Fetu_Bus_Item_Scores.Add(new()
                {
                    Id = bus_Item_ScoreID,
                    Status = StatusModel.Active,
                    Pre_Cal_Fetu_Bus_ItemId = busItemID
                });
            }

            await Task.Delay(1);
            StateHasChanged();
        }

        protected async Task RemoveScore(Guid busItemID, Guid ID)
        {
            var bus_Items = formModel.Pre_Cal_Fetu_Bus_Items?.FirstOrDefault(x => x.Id == busItemID);
            if (bus_Items != null)
            {
                var itemToRemove = bus_Items.Pre_Cal_Fetu_Bus_Item_Scores?.FirstOrDefault(r => r.Id == ID);
                if (itemToRemove != null)
                {
                    bus_Items.Pre_Cal_Fetu_Bus_Item_Scores?.Remove(itemToRemove);
                }
            }

            await Task.Delay(1);
            StateHasChanged();
        }


    }
}