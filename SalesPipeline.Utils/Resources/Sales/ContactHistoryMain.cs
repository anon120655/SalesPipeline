
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class ContactHistoryMain
	{
        public short ISCloseSale { get; set; }
        public short ISPhoenix { get; set; }
		public PaginationView<List<Sale_Contact_HistoryCustom>>? History { get; set; }
    }
}
