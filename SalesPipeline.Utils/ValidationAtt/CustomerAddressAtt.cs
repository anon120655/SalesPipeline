using SalesPipeline.Utils.Resources.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ValidationAtt
{
	public class CustomerAddressAtt : ValidationAttribute
	{
		public string? FieldName { get; set; }

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var resultValidate = new ValidationResult("กรุณาระบุข้อมูล", new[] { validationContext.MemberName });

			// ดึงค่า IsValidate จาก Object ที่กำลัง Validate
			var model = (CustomerCustom)validationContext.ObjectInstance;

			if (model.IsExceptValidAddress != true && string.IsNullOrEmpty(value?.ToString()))
			{
				return resultValidate;
			}

			// ถ้า IsValidate != true เป็นค่าว่างได้
			return ValidationResult.Success;
		}
	}
}
