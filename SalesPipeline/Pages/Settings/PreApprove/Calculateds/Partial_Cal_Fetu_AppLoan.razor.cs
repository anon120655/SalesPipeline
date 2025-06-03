using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
    public partial class Partial_Cal_Fetu_AppLoan
    {
        [Parameter]
        public Guid pre_CalId { get; set; }

        [Parameter]
        public bool IsShowTab { get; set; }

        string? _errorMessage = null;
        private bool isLoading = false;
        private Pre_Cal_Fetu_AppCustom formModel = new();
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
            var data = await _preCalAppViewModel.GetById(pre_CalId);
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
                formModel.Pre_Cal_Fetu_App_Items = new();
                await InsertItem();
            }

            await Task.Delay(1);
        }

        private async Task Seve()
        {
            ResultModel<Pre_Cal_Fetu_AppCustom> response;

            formModel.Pre_CalId = pre_CalId;
            formModel.CurrentUserId = UserInfo.Id;

            if (formModel.Id == Guid.Empty)
            {
                response = await _preCalAppViewModel.Create(formModel);
            }
            else
            {
                response = await _preCalAppViewModel.Update(formModel);
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
            if (formModel.Pre_Cal_Fetu_App_Items == null) formModel.Pre_Cal_Fetu_App_Items = new();

            var AppItemID = Guid.NewGuid();
            formModel.Pre_Cal_Fetu_App_Items.Add(new()
            {
                Id = AppItemID,
                Status = StatusModel.Active
            });
            await InsertScore(AppItemID);

            await Task.Delay(1);
            StateHasChanged();
        }

        protected async Task RemoveItem(Guid ID)
        {
            var itemToRemove = formModel.Pre_Cal_Fetu_App_Items?.FirstOrDefault(x => x.Id == ID);
            if (itemToRemove != null)
            {
                formModel.Pre_Cal_Fetu_App_Items?.Remove(itemToRemove);
            }

            await Task.Delay(1);
            StateHasChanged();
        }

        protected async Task InsertScore(Guid appItemID)
        {
            var app_Items = formModel.Pre_Cal_Fetu_App_Items?.FirstOrDefault(x => x.Id == appItemID);
            if (app_Items != null)
            {
                if (app_Items.Pre_Cal_Fetu_App_Item_Scores == null) app_Items.Pre_Cal_Fetu_App_Item_Scores = new();

                var app_Item_ScoreID = Guid.NewGuid();
                app_Items.Pre_Cal_Fetu_App_Item_Scores.Add(new()
                {
                    Id = app_Item_ScoreID,
                    Status = StatusModel.Active,
                    Pre_Cal_Fetu_App_ItemId = appItemID
                });
            }

            await Task.Delay(1);
            StateHasChanged();
        }

        protected async Task RemoveScore(Guid appItemID, Guid ID)
        {
            var app_Items = formModel.Pre_Cal_Fetu_App_Items?.FirstOrDefault(x => x.Id == appItemID);
            if (app_Items != null)
            {
                var itemToRemove = app_Items.Pre_Cal_Fetu_App_Item_Scores?.FirstOrDefault(r => r.Id == ID);
                if (itemToRemove != null)
                {
                    app_Items.Pre_Cal_Fetu_App_Item_Scores?.Remove(itemToRemove);
                }
            }

            await Task.Delay(1);
            StateHasChanged();
        }

    }
}