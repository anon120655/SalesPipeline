using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Utils.Resources.ProcessSales
{
	public class ProcessSaleCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public DateTime UpdateDate { get; set; }

		public int UpdateBy { get; set; }

		public int SequenceNo { get; set; }

		public string? Name { get; set; }

		public string Code { get; set; } = null!;

		public virtual List<ProcessSale_SectionCustom>? ProcessSale_Sections { get; set; }
	}
}
