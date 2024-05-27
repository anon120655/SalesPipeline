using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.PropertiesModel
{
	public class PropertiesMain
	{
		public static PropertiesOptions PerActive(string code)
		{
			var Properties = new List<PropertiesOptions>()
			{
				new PropertiesOptions()
				{
					Code = "-1",
					Name = "ลบ",
					NameEN = "Delete",
					ClassTxt = "text-danger"
				},
				new PropertiesOptions()
				{
					Code = "0",
					Name = "ไม่ใช้งาน",
					NameEN = "Inactive",
					ClassTxt = "text-danger"
				},
				new PropertiesOptions()
				{
					Code = "1",
					Name = "ใช้งาน",
					NameEN = "Active",
					ClassTxt = "text-success"
				},
			};

			var data = Properties.Where(x => x.Code == code).Select(x => x).FirstOrDefault();
			if (data != null)
			{
				return data;
			}

			return new PropertiesOptions();
		}

		public static PropertiesOptions PerFieldType(string? code)
		{
			var Properties = new List<PropertiesOptions>()
			{
				new PropertiesOptions()
				{
					Code = "ShortAnswer",
					Name = "Short answer"
				},
				new PropertiesOptions()
				{
					Code = "Number",
					Name = "Number"
				},
				new PropertiesOptions()
				{
					Code = "TextArea",
					Name = "Text area"
				},
				new PropertiesOptions()
				{
					Code = "Multiplechoice",
					Name = "Multiple choice"
				},
				new PropertiesOptions()
				{
					Code = "Dropdown",
					Name = "Drop down"
				},
				new PropertiesOptions()
				{
					Code = "DropdownMaster",
					Name = "Drop down master"
				},
				new PropertiesOptions()
				{
					Code = "DropdownSection",
					Name = "Drop down section"
				},
				new PropertiesOptions()
				{
					Code = "Fileupload",
					Name = "File upload"
				},
				new PropertiesOptions()
				{
					Code = "Date",
					Name = "Date"
				},
				new PropertiesOptions()
				{
					Code = "Time",
					Name = "Time"
				}
			};

			var data = Properties.Where(x => x.Code == code).Select(x => x).FirstOrDefault();
			if (data != null)
			{
				return data;
			}

			return new PropertiesOptions();
		}

	}
}
