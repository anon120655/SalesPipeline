
using SalesPipeline.Utils.Resources.ProcessSales;
using System.Text.Json.Serialization;

namespace SalesPipeline.Utils.Resources.Sales
{
    public class Sale_Reply_Section_ItemValueCustom
    {
        public Guid Id { get; set; }

        /// <summary>
        /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
        /// </summary>
        public short Status { get; set; }

        public Guid SaleReplySectionItemId { get; set; }

        public Guid PSaleSectionItemOptionId { get; set; }

        public string? OptionLabel { get; set; }

        public string? ReplyValue { get; set; }

		public string? ReplyName { get; set; }

		public DateTime? ReplyDate { get; set; }

        public TimeOnly? ReplyTime { get; set; }

        public Guid? FileId { get; set; }

        public string? FileUrl { get; set; }

		public string? FileName { get; set; }

		public Guid? Master_ListId { get; set; }

		public virtual ProcessSale_Section_ItemOptionCustom? PSaleSectionItemOption { get; set; }

		public virtual Sale_Reply_Section_ItemCustom? SaleReplySectionItem { get; set; }


		//Custom
		public string? Path { get; set; }
		[JsonIgnore]
        public string? errorMessage { get; set; }
        [JsonIgnore]
        public bool bClearInput { get; set; }
        [JsonIgnore]
        public string _inputFileId { get; set; } = Guid.NewGuid().ToString();

    }
}
