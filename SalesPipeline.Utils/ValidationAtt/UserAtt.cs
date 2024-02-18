using SalesPipeline.Utils.Resources.Authorizes.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ValidationAtt
{
	public class UserAtt : ValidationAttribute
	{
		public string? FieldName { get; set; }

		protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
		{
			if (validationContext != null && validationContext.MemberName != null)
			{
				var model = (UserCustom)validationContext.ObjectInstance;

				var resultValidate = new ValidationResult("กรุณาระบุข้อมูล", new[] { validationContext.MemberName });

				//ต้องการใบเสร็จ / ใบกำกับภาษี
				if (model.RoleId.HasValue)
				{
					if (model.RoleId == 5 || model.RoleId == 6 || model.RoleId == 7 || model.RoleId == 8)
					{
						//ฝ่ายกิจการสาขาภาค
						if (FieldName == "Master_Department_BranchId" && model.RoleId != 7 && value == null)
						{
							return resultValidate;
						}
						//ศูนย์ธุรกิจสินเชื่อ
						if (FieldName == "Master_Department_CenterId" && model.RoleId == 7 && value == null)
						{
							return resultValidate;
						}
						if ((model.RoleId == 5 || model.RoleId == 6) && FieldName == "LevelId" && value == null)
						{
							return resultValidate;
						}
						if (FieldName == "ProvinceId" || FieldName == "AmphurId")
						{
							if (model.RoleId == 8 && value == null)
							{
								return resultValidate;
							}
						}
					}
				}
			}

			return ValidationResult.Success;
		}

	}
}
