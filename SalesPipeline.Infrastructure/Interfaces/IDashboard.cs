using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IDashboard
	{
		//Status_Total
		Task<Dash_Status_TotalCustom> GetStatus_TotalById(allFilter model);
		Task UpdateStatus_TotalById(allFilter model);
		//list เป้าหมายการขาย
		Task<PaginationView<List<User_Target_SaleCustom>>> GetListTarget_SaleById(allFilter model);
		Task UpdateTarget_SaleById(allFilter model);
		Task UpdateTarget_SaleAll(string year);
		//SalesPipeline
		Task<Dash_SalesPipelineModel> Get_SalesPipelineById(allFilter model);
		//มูลค่าเฉลี่ยต่อหนึ่งดีล ,ระยะเวลาเฉลี่ยที่ใช้ในการปิดการขาย ,ระยะเวลาเฉลี่ยที่ใช้ในการขายที่แพ้ให้กับคู่แข่ง
		Task<Dash_AvgTop_NumberCustom> GetAvgTop_NumberById(allFilter model);
		Task UpdateAvg_NumberById(allFilter model);
		//ดีลโดยเฉลี่ยต่อสาขา ,กิจกรรมการขายโดยเฉลี่ยต่อดีลที่ปิดการขาย ,ระยะเวลาเฉลี่ยในการส่งมอบ ,ดีลโดยเฉลี่ยต่อพนักงานสินเชื่อ
		Task<Dash_AvgBottom_NumberCustom> GetAvgBottom_NumberById(allFilter model);
		//list ดีลโดยเฉลี่ยต่อสาขา
		Task<PaginationView<List<GroupByModel>>> GetListDealBranchById(allFilter model);
		//จำนวนดีลของพนักงานสินเชื่อแต่ละคน
		Task<PaginationView<List<SaleGroupByModel>>> GetListDealRMById(allFilter model);
		//10 อันดับ ศูนย์ยอดขายสูงสุด ,10 อันดับ ศูนย์ยอดขายสูงสุด
		//Task<List<Dash_Map_ThailandCustom>> GetMap_ThailandById(allFilter model);
		//Task UpdateMap_ThailandById(allFilter model);
		Task<PaginationView<List<Dash_Map_ThailandCustom>>> GetTopSale(allFilter model);
		Task<PaginationView<List<Dash_Map_ThailandCustom>>> GetLostSale(allFilter model);
		//ระยะเวลาที่ใช้ในแต่ละสเตจ
		Task<Dash_Avg_NumberOnStage> GetAvgOnStage(allFilter model);
		//ลูกค้าที่ปิดการขาย ,เหตุผลไม่ประสงค์ขอสินเชื่อ
		Task<List<Dash_PieCustom>> GetPieCloseSaleReason(allFilter model);
		//เป้ายอดการขาย
		Task<User_Target_SaleCustom> GetSumTargetActual(allFilter model);
		//จำนวนลูกค้าตาม...
		Task<List<Dash_PieCustom>> GetPieNumberCustomer(allFilter model);
		Task<List<Dash_PieCustom>> GetListNumberCustomer(allFilter model);
		//มูลค่าสินเชื่อตาม...
		Task<List<Dash_PieCustom>> GetPieLoanValue(allFilter model);
		//ระยะเวลาเฉลี่ยที่ใช้ในการปิดการขาย ,ระยะเวลาเฉลี่ยที่ใช้ในการขายที่แพ้ให้กับคู่แข่ง
		Task<PaginationView<List<Sale_DurationCustom>>> GetDuration(allFilter model);
		Task UpdateDurationById(allFilter model);
		//กิจกรรมการขายโดยเฉลี่ยต่อดีลที่ปิดการขาย ,ระยะเวลาที่ใช้ในแต่ละสเตจ
		Task<PaginationView<List<Sale_ActivityCustom>>> GetActivity(allFilter model);
		Task UpdateActivityById(allFilter model);
		//เหตุผลไม่ประสงค์ขอสินเชื่อ
		Task<List<Dash_PieCustom>> GetGroupReasonNotLoan(allFilter model);
		//เหตุผลไม่ประสงค์ขอสินเชื่อ
		Task<PaginationView<List<GroupByModel>>> GetGroupDealBranch(allFilter model);
		//SalesPipeline
		Task<SalesFunnelModel> GetSalesFunnel(allFilter model);
		//รายได้จำแนกตามประเภทธุรกิจ ,สัดส่วนการปิดการขาย ,เหตุผลที่ปิดการขายไม่สำเร็จ
		Task<List<Dash_PieCustom>> GetPieRM(allFilter model);
		//ระยะเวลาที่ใช้ในแต่ละสเตจ moblie
		Task<Dash_Avg_NumberOnStage> GetAvgDuration(allFilter model);
		//มูลค่าเฉลี่ยต่อหนึ่งดีล ประเทศ,ภูมิภาคทั้งหมด,ศูนย์สาขาทั้งหมด,RM ทั้งหมด
		Task<List<GroupByModel>> GetAvgTopBar(allFilter model);
		//กิจการสาขาภาค
		Task<List<GroupByModel>> GetAvgRegionBar(allFilter model);
		//สาขา
		Task<List<GroupByModel>> GetAvgBranchBar(allFilter model);
		//RM
		Task<List<GroupByModel>> GetAvgRMBar(allFilter model);
		//RM
		Task<List<GroupByModel>> GetAvgRegionMonth12Bar(allFilter model);
		//เปรียบเทียบเดือนก่อนหน้า
		Task<List<GroupByModel>> GetAvgComparePreMonth(allFilter model);
		//ระยะเวลาในการส่งมอบ
		Task UpdateDeliverById(allFilter model);
		Task<PaginationView<List<Sale_DeliverCustom>>> GetDeliver(allFilter model);
	}
}
