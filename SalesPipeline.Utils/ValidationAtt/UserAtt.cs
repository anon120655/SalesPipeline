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

				if (model.RoleId.HasValue)
				{
					if (model.RoleId == 3 || model.RoleId == 4)
					{
						if (FieldName == "LevelId" && value == null)
						{
							return resultValidate;
						}
					}
					if (model.RoleId == 5 || model.RoleId == 6 || model.RoleId == 7 || model.RoleId == 8)
					{
						if (FieldName == "Master_Branch_RegionId" && value == null)
						{
							return resultValidate;
						}
						if (FieldName == "ProvinceId" && value == null)
						{
							return resultValidate;
						}
						if (FieldName == "BranchId" && value == null)
						{
							return resultValidate;
						}
						if (model.RoleId != 7) //ผจศ. ไม่มีระดับ
						{
							if (FieldName == "LevelId" && value == null)
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
