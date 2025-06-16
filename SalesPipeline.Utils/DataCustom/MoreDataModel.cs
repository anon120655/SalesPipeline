using SalesPipeline.Utils.Resources.Phoenixs;
using SalesPipeline.Utils.Resources.PreApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.DataCustom
{
	public class MoreDataModel
	{
		public static List<CreditScoreModel>? CreditScore()
		{
			return new() {
				new() {Level = "" ,CreditScore = "0" ,Grade = "D",LimitMultiplier = "Reject",RateMultiplier = "Reject" ,CreditScoreColor="#f8696b" },
				new() {Level = "" ,CreditScore = "53" ,Grade = "C",LimitMultiplier = "Reject",RateMultiplier = "Reject",CreditScoreColor="#fdd17f" },
				new() {Level = "0" ,CreditScore = "59" ,Grade = "CC",LimitMultiplier = "1.000",RateMultiplier = "1.000",CreditScoreColor="#fedd81" },
				new() {Level = "1" ,CreditScore = "61" ,Grade = "CCC",LimitMultiplier = "0.857",RateMultiplier = "0.857" ,CreditScoreColor="#fee182"},
				new() {Level = "2" ,CreditScore = "64" ,Grade = "B",LimitMultiplier = "0.714",RateMultiplier = "0.714",CreditScoreColor="#fee783" },
				new() {Level = "3" ,CreditScore = "66" ,Grade = "BB",LimitMultiplier = "0.571",RateMultiplier = "0.571",CreditScoreColor="#ffeb84" },
				new() {Level = "4" ,CreditScore = "69" ,Grade = "BBB",LimitMultiplier = "0.429",RateMultiplier = "0.429",CreditScoreColor="#f2e884" },
				new() {Level = "5" ,CreditScore = "75" ,Grade = "A",LimitMultiplier = "0.286",RateMultiplier = "0.286",CreditScoreColor="#d7e082" },
				new() {Level = "6" ,CreditScore = "80" ,Grade = "AA",LimitMultiplier = "0.143",RateMultiplier = "0.143",CreditScoreColor="#c1da81" },
				new() {Level = "7" ,CreditScore = "86" ,Grade = "AAA",LimitMultiplier = "0.000",RateMultiplier = "0.000",CreditScoreColor="#a6d27f" },
				new() {Level = "" ,CreditScore = "101" ,Grade = "Error",LimitMultiplier = "Error",RateMultiplier = "Error",CreditScoreColor="#63be7b" }
			};
		}

		public static List<ChancePassModel>? ChancePass()
		{
			return new(){
				new() { Z = "-6" ,CreditScore = "100" ,Prob = "0.002472623"},
				new() {Z = "-5.9" ,CreditScore = "99" ,Prob = "0.002731961"},
				new() {Z = "-5.8" ,CreditScore = "98" ,Prob = "0.003018416"},
				new() {Z = "-5.7" ,CreditScore = "97" ,Prob = "0.003334807"},
				new() {Z = "-5.6" ,CreditScore = "96" ,Prob = "0.00368424"},
				new() {Z = "-5.5" ,CreditScore = "95" ,Prob = "0.004070138"},
				new() {Z = "-5.4" ,CreditScore = "94" ,Prob = "0.004496273"},
				new() {Z = "-5.3" ,CreditScore = "93" ,Prob = "0.004966802"},
				new() {Z = "-5.2" ,CreditScore = "92" ,Prob = "0.005486299"},
				new() {Z = "-5.1" ,CreditScore = "91" ,Prob = "0.006059801"},
				new() {Z = "5" ,CreditScore = "90" ,Prob = "0.006692851"}
			};
		}

		public static List<Sale_PhoenixCustom>? Phoenixs()
		{
			List<Sale_PhoenixCustom> phoenixModel = new List<Sale_PhoenixCustom>();

			for (int i = 1; i <= 9; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					phoenixModel.Add(new Sale_PhoenixCustom
					{
						cif_no = $"900000{i}",
						cif_name = $"บริษัท 00{i}_{j}",
						status_type = $"วิเคราะห์โครงการ00{i}_{j}",
						status_code = $"100 - RM วิเคราะห์โครงการ{i}_{j}",
						ana_no = $"0458CA67000{i}{j}"
					});
				}
			}

			return phoenixModel;
		}
	}
}
