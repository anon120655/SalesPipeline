namespace SalesPipeline.Shared
{
	public partial class NavLeftBackUp
	{
		bool collapseMenuUserLoan = false;
		bool collapseMenuSetting = false;

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