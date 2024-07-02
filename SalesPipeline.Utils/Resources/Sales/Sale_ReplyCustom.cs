using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
    public class Sale_ReplyCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public string? CreateByName { get; set; }

		public DateTime UpdateDate { get; set; }

		public int UpdateBy { get; set; }

		public string? UpdateByName { get; set; }

		public Guid SaleId { get; set; }

		public Guid ProcessSaleId { get; set; }

		public string? ProcessSaleName { get; set; }

		public virtual List<Sale_Reply_SectionCustom>? Sale_Reply_Sections { get; set; }

        //Custom
		/// <summary>
		/// ติดต่อ
		/// </summary>
        public Sale_ContactCustom? Sale_Contact { get; set; }
		/// <summary>
		/// เข้าพบ
		/// </summary>
		public Sale_MeetCustom? Sale_Meet { get; set; }
		/// <summary>
		/// ยื่นเอกสาร
		/// </summary>
		public Sale_DocumentCustom? Sale_Document { get; set; }
		/// <summary>
		/// ผลลัพธ์
		/// </summary>
		public Sale_ResultCustom? Sale_Result { get; set; }
		/// <summary>
		/// ปิดกาขาย
		/// </summary>
		public Sale_Close_SaleCustom? Sale_Close_Sale { get; set; }


		public int? Nex { get; set; }
		public int? ProceedId { get; set; }
		public DateTime? AppointmentDate { get; set; }
		public TimeOnly? AppointmentTime { get; set; }
		public string? Location { get; set; }
		public string? Note { get; set; }

		//Custom
		public int State { get; set; } = CRUDModel.Create;

	}
}
