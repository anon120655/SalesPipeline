using NPOI.SS.Formula.Functions;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils
{
	public static class LoanCalculator
	{
		public static double CalculateMonthlyPayment(double principal, double monthlyInterestRate, int numberOfPayments)
		{
			double ratePow = Math.Pow(1 + monthlyInterestRate, numberOfPayments);
			return principal * (monthlyInterestRate * ratePow) / (ratePow - 1);
		}

		public static double CalculateInterestRate(double principal, int numberOfPayments, double targetPayment)
		{
			double low = 0.0;
			double high = 1.0;
			double mid = 0.0;
			double tolerance = 0.0000001;

			while (high - low > tolerance)
			{
				mid = (low + high) / 2;
				double ratePow = Math.Pow(1 + mid, numberOfPayments);
				double calculatedPayment = principal * (mid * ratePow) / (ratePow - 1);

				if (calculatedPayment > targetPayment)
				{
					high = mid;
				}
				else
				{
					low = mid;
				}
			}

			return mid;
		}

		public static XLookUpModel? XLookupList(double lookupValue, List<XLookUpModel> lookUpModel, int searchMode = 1)
		{
			// ค้นหาค่าที่ระบุ ถ้าไม่พบจะคืนค่าลำดับที่น้อยกว่า

			if (lookUpModel == null || lookUpModel.Count == 0)
			{
				return null;
			}

			if (searchMode == 1)
			{
				// ค้นหาจากแรกไปหลัง
				for (int i = 0; i < lookUpModel.Count; i++)
				{
					var checkValue = lookUpModel[i].CheckValue;
					if (lookupValue >= checkValue)
					{
						return lookUpModel[i];
					}
				}

				// ถ้าไม่พบค่าที่ตรงเงื่อนไขจะคืนค่าสุดท้าย
				return lookUpModel[lookUpModel.Count - 1];
			}
			else
			{
				// ค้นหาจากหลังไปแรก (default)
				for (int i = lookUpModel.Count - 1; i >= 0; i--)
				{
					var checkValue = lookUpModel[i].CheckValue;
					if (lookupValue >= checkValue)
					{
						return lookUpModel[i];
					}
				}

				// ถ้าไม่พบค่าที่ตรงเงื่อนไขจะคืนค่าแรก
				return lookUpModel[0];
			}
		}

		// [match_mode] คือ กำหนดค่าจะให้เรียงลำดับในกรณีมีค่าเท่ากันอย่างไร
		//     0  คือ การค้นหาแบบพอดีเป๊ะ(Exact match) โดยหากไม่พบจะคืนค่า #N/A. (หากไม่ระบุ จะเป็นค่าตั้งต้น)
		//     -1 คือ การค้นหาแบบพอดีเป๊ะ(Exact match) โดยหากไม่พบจะคืนค่าเป็นลำดับที่ค่าน้อยกว่าลำดับถัดไป
		// [search_mode] คือ กำหนดค่าโหมดของการค้นหา
		//     1 คือ ทำการค้นหาตั้งแต่รายการแรกเป็นต้นไป(หากไม่ระบุ จะเป็นค่าตั้งต้น)
		//    -1 คือ ทำการค้นหารายการสุดท้ายเป็นต้นไป
		public static XLookUpModel? XLookupLists2(double lookupValue, List<XLookUpModel> lookUpModel, int match_mode = 1, int search_mode = 1)
		{
			if (lookUpModel == null || lookUpModel.Count == 0)
			{
				return null;
			}

			if (match_mode == 0 || match_mode == -1)
			{
				// Exact match search or find the last value less than lookupValue
				XLookUpModel? lessThanMatch = null;

				foreach (var model in lookUpModel)
				{
					if (lookupValue == model.CheckValue)
					{
						return model; // Exact match found
					}
					if (lookupValue > model.CheckValue)
					{
						lessThanMatch = model;
					}
					else
					{
						break; // No need to continue as the list is assumed to be sorted
					}
				}

				// If exact match not found
				return match_mode == 0 ? null : lessThanMatch;
			}

			// Original search logic for match_mode = 1 (default)
			if (search_mode == 1)
			{
				// ค้นหาจากแรกไปหลัง
				foreach (var model in lookUpModel)
				{
					if (lookupValue >= model.CheckValue)
					{
						return model;
					}
				}
				// ถ้าไม่พบค่าที่ตรงเงื่อนไขจะคืนค่าสุดท้าย
				return lookUpModel[lookUpModel.Count - 1];
			}
			else
			{
				// ค้นหาจากหลังไปแรก
				for (int i = lookUpModel.Count - 1; i >= 0; i--)
				{
					if (lookupValue >= lookUpModel[i].CheckValue)
					{
						return lookUpModel[i];
					}
				}
				// ถ้าไม่พบค่าที่ตรงเงื่อนไขจะคืนค่าแรก
				return lookUpModel[0];
			}
		}

		public static XLookUpModel? XLookupLists(double lookupValue, List<XLookUpModel> lookUpModel, int match_mode = 1, int search_mode = 1)
		{
			if (lookUpModel == null || lookUpModel.Count == 0)
			{
				return null;
			}

			// Ensure the list is sorted
			lookUpModel = lookUpModel.OrderBy(m => m.CheckValue).ToList();

			if (match_mode == 0 || match_mode == -1)
			{
				XLookUpModel? lessThanMatch = null;

				if (search_mode == 1) // Forward search
				{
					foreach (var model in lookUpModel)
					{
						if (lookupValue == model.CheckValue)
						{
							return model;
						}
						if (lookupValue > model.CheckValue)
						{
							lessThanMatch = model;
						}
						else
						{
							break;
						}
					}
				}
				else // Backward search
				{
					for (int i = lookUpModel.Count - 1; i >= 0; i--)
					{
						if (lookupValue == lookUpModel[i].CheckValue)
						{
							return lookUpModel[i];
						}
						if (lookupValue > lookUpModel[i].CheckValue)
						{
							lessThanMatch = lookUpModel[i];
							break;
						}
					}
				}

				return match_mode == 0 ? null : lessThanMatch;
			}

			if (search_mode == 1)
			{
				foreach (var model in lookUpModel)
				{
					if (lookupValue >= model.CheckValue)
					{
						return model;
					}
				}

				return lookUpModel[lookUpModel.Count - 1];
			}
			else
			{
				for (int i = lookUpModel.Count - 1; i >= 0; i--)
				{
					if (lookupValue >= lookUpModel[i].CheckValue)
					{
						return lookUpModel[i];
					}
				}

				return lookUpModel[0];
			}
		}

	}
}
