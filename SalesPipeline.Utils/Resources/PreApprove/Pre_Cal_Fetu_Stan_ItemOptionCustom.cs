using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class Pre_Cal_Fetu_Stan_ItemOptionCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid Pre_Cal_Fetu_StanId { get; set; }

        /// <summary>
        /// 1=ประเภทหลักประกัน 2=ประวัติการชำระหนี้
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// ลำดับ
        /// </summary>
        public int SequenceNo { get; set; }

        public string? Name { get; set; }

	}
}
