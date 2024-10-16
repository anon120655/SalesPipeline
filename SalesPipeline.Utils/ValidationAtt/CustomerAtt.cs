using SalesPipeline.Utils.Resources.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ValidationAtt
{
	public class CustomerAtt : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			// ดึงค่า IsValidate จาก Object ที่กำลัง Validate
			var model = (CustomerCustom)validationContext.ObjectInstance;

			// ถ้า IsValidate เป็น true และ HouseNo เป็นค่าว่าง ให้แสดงข้อผิดพลาด
			if (model.IsFileUpload != true && string.IsNullOrEmpty(value?.ToString()))
			{
				return new ValidationResult("กรุณาระบุข้อมูล");
			}

			// ถ้า IsValidate != true เป็นค่าว่างได้
			return ValidationResult.Success;
		}
	}
}
