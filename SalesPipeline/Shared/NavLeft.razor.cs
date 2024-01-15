using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;

namespace SalesPipeline.Shared
{
	public partial class NavLeft
	{
		string currentPath = String.Empty;

		bool collapseMenuUserLoan = false;
		bool collapseMenuSetting = false;

		protected override void OnInitialized()
		{
			currentPath = $"/{_Navs.ToBaseRelativePath(_Navs.Uri)}";
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
			{
				if (_Navs.ToAbsoluteUri(_Navs.Uri).LocalPath.Contains("/loans/"))
				{
					collapseMenuUserLoan = true;
				}
				else if (_Navs.ToAbsoluteUri(_Navs.Uri).LocalPath.Contains("/setting/"))
				{
					collapseMenuSetting = true;
				}

				await Task.Delay(1);
                StateHasChanged();
                firstRender = false;
            }
        }
    }
}