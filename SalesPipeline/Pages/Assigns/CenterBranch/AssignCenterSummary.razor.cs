using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using System.Text.Json;

namespace SalesPipeline.Pages.Assigns.CenterBranch
{
	public partial class AssignCenterSummary
	{
		[Parameter]
		public int userid { get; set; }

		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private Assignment_CenterCustom? Items;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.AssignLoan) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				try
				{
					var storedValue = await _jsRuntimes.InvokeAsync<string?>("localStorage.getItem", $"assignCenterData_{UserInfo.Id}");
					if (!string.IsNullOrEmpty(storedValue))
					{
						// ใช้ค่าที่ดึงมาจาก localStorage
					}

					var jsonData = await _jsRuntimes.InvokeAsync<string>("localStorage.getItem", $"assignCenterData_{UserInfo.Id}");
					if (jsonData != null)
					{
						var centerItems = JsonSerializer.Deserialize<List<Assignment_CenterCustom>>(jsonData);
						if (centerItems?.Count > 0)
						{
							Items = centerItems.FirstOrDefault(x => x.UserId == userid);
						}

						// ลบข้อมูลจาก Local Storage หลังจากดึงมาใช้แล้ว
						await _jsRuntimes.InvokeVoidAsync("localStorage.removeItem", $"AssignCenterData_{UserInfo.Id}");
					}
					else
					{
						Cancel();
					}
				}
				catch (Exception ex)
				{

				}
				StateHasChanged();
				firstRender = false;
			}
		}

		protected void Cancel()
		{
			_Navs.NavigateTo("/assign/center");
		}

	}
}