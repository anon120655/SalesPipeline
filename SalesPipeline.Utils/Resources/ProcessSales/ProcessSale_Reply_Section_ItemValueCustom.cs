using Newtonsoft.Json;

namespace SalesPipeline.Utils.Resources.ProcessSales
{
	public class ProcessSale_Reply_Section_ItemValueCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public Guid PSaleReplySectionItemId { get; set; }

		public Guid PSaleSectionItemOptionId { get; set; }

		public string? OptionLabel { get; set; }

		public string? ReplyValue { get; set; }

		public DateTime? ReplyDate { get; set; }

		public TimeOnly? ReplyTime { get; set; }

		public Guid? FileId { get; set; }

		public string? FileUrl { get; set; }

		public virtual ProcessSale_Reply_Section_ItemCustom? PSaleReplySectionItem { get; set; }

		public virtual ProcessSale_Section_ItemOptionCustom? PSaleSectionItemOption { get; set; }

		//Custom
		[JsonIgnore]
		public string? errorMessage { get; set; }
		[JsonIgnore]
		public bool bClearInput { get; set; }
		[JsonIgnore]
		public string _inputFileId { get; set; } = Guid.NewGuid().ToString();
	}
}
