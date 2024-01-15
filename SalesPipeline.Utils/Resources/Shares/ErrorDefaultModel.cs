namespace SalesPipeline.Utils.Resources.Shares
{
	public class ErrorDefaultModel
	{
		public string? type { get; set; }
		public string? title { get; set; }
		public int? status { get; set; }
		public string? traceId { get; set; }
		public Errors? errors { get; set; }

		public class Errors
		{
			public List<string?>? Code { get; set; }
			public List<string?>? Value { get; set; }
		}

	}
}
