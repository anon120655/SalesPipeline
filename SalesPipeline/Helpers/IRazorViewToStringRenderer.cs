namespace SalesPipeline.Helpers
{
	public interface IRazorViewToStringRenderer
	{
		Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
	}
}
