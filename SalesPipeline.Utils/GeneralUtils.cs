
using SalesPipeline.Utils.Resources.Shares;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Cmp;
using Newtonsoft.Json;

namespace SalesPipeline.Utils
{
	public static class GeneralUtils
	{
		public static string GetExMessage(Exception ex)
		{
			String messageError = ex.Message;
			if (ex.InnerException != null && ex.InnerException.Message != null)
			{
				messageError = ex.InnerException.Message;
			}

			messageError = messageError.Replace(GeneralTxt.ErrorTxt, string.Empty);

			return messageError;
		}

		public static string? MapErrorModel(string? content)
		{
			if (content != null)
			{
				var dataMap = JsonConvert.DeserializeObject<ErrorCustom>(content);
				if (dataMap != null)
				{
					return dataMap?.Message;
				}
			}
			return string.Empty;
		}

		public static string DateToThString(DateTime? datetime, string separator = "/")
		{
			if (datetime.HasValue && datetime != DateTime.MinValue)
			{
				string _datetime = datetime.Value.ToString("dd/MM/yyyy HH:mm:ss");
				//DateTime dateeng = DateTime.TryParseExact(_datetime, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
				DateTime dateeng;
				if (DateTime.TryParseExact(_datetime, "dd/MM/yyyy HH:mm:ss",
						   System.Globalization.CultureInfo.InvariantCulture,
						   System.Globalization.DateTimeStyles.None, out dateeng))
				{
					int thaiYear = new ThaiBuddhistCalendar().GetYear(dateeng);
					int thaiMonth = new ThaiBuddhistCalendar().GetMonth(dateeng);
					int thaiDay = new ThaiBuddhistCalendar().GetDayOfMonth(dateeng);

					if (thaiYear < 2500)
					{
						thaiYear = thaiYear + 543;
					}
					if (thaiYear > 3000)
					{
						thaiYear = thaiYear - 543;
					}

					string _thaiDay = thaiDay < 10 ? string.Format("{0}{1}", "0", thaiDay) : thaiDay.ToString();
					string _thaiMonth = thaiMonth < 10 ? string.Format("{0}{1}", "0", thaiMonth) : thaiMonth.ToString();

					return $"{_thaiDay}{separator}{_thaiMonth}{separator}{thaiYear}";
				}

				return String.Empty;
			}
			else
			{
				return String.Empty;
			}
		}

		public static DateTime? DateToEn(string? datetime, string format = "dd/MM/yyyy", string Culture = "en-US")
		{
			if (DateTime.TryParseExact(datetime, format, new CultureInfo(Culture), DateTimeStyles.None, out DateTime date))
			{
				if (date.Year > 2500)
				{
					date = date.AddYears(-543);
				}
				return date;
			}
			return null;
		}

		public static DateTime? DateToEnFormatMulti(string? datetime, string[] formats, string Culture = "en-US")
		{
			if (datetime != null && datetime.Length > 19)
			{
				datetime = datetime.Substring(0, 19);
			}

			if (DateTime.TryParseExact(datetime, formats, new CultureInfo(Culture), DateTimeStyles.None, out DateTime date))
			{
				if (date.Year > 2500)
				{
					date = date.AddYears(-543);
				}
				return date;
			}
			return null;
		}

		public static string? DateToEnStr(DateTime? datetime, string separator = "/", string? rNull = null)
		{
			if (datetime.HasValue && datetime.Value != DateTime.MinValue)
			{
				int Year = datetime.Value.Year;
				int Month = datetime.Value.Month;
				int Day = datetime.Value.Day;

				if (Year > 2500)
				{
					Year = Year - 543;
				}

				string _Day = Day < 10 ? string.Format("{0}{1}", "0", Day) : Day.ToString();
				string _Month = Month < 10 ? string.Format("{0}{1}", "0", Month) : Month.ToString();

				return $"{_Day}{separator}{_Month}{separator}{Year}";
			}

			return rNull;
		}

		public static string? DateToStrParameter(DateTime? datetime, string separator = "-")
		{
			if (datetime.HasValue && datetime != DateTime.MinValue)
			{
				string _datetime = datetime.Value.ToString("dd/MM/yyyy HH:mm:ss");
				DateTime dateeng;
				if (DateTime.TryParseExact(_datetime, "dd/MM/yyyy HH:mm:ss",
						   System.Globalization.CultureInfo.InvariantCulture,
						   System.Globalization.DateTimeStyles.None, out dateeng))
				{
					int enYear = dateeng.Year;
					int thaiMonth = new ThaiBuddhistCalendar().GetMonth(dateeng);
					int thaiDay = new ThaiBuddhistCalendar().GetDayOfMonth(dateeng);

					string _thaiDay = thaiDay < 10 ? string.Format("{0}{1}", "0", thaiDay) : thaiDay.ToString();
					string _thaiMonth = thaiMonth < 10 ? string.Format("{0}{1}", "0", thaiMonth) : thaiMonth.ToString();

					return $"{enYear}{separator}{_thaiMonth}{separator}{_thaiDay}";
				}

				return String.Empty;
			}
			else
			{
				return String.Empty;
			}
		}

		public static DateTime? DateNotNullToEn(string? datetime, string format = "dd/MM/yyyy", string Culture = "en-US")
		{
			try
			{
				if (datetime == null) return null;

				var date = DateTime.ParseExact(datetime, format, new CultureInfo(Culture), DateTimeStyles.None);
				if (date.Year > 2500)
				{
					date = date.AddYears(-543);
				}
				return date;
			}
			catch
			{
				return null;
			}
		}

		public static DateOnly DateOnlyToEn(string datetime, string format = "dd/MM/yyyy", string Culture = "en-US")
		{
			return DateOnly.ParseExact(datetime, format, new CultureInfo(Culture), DateTimeStyles.None);
		}

		public static int GetYearEn(int year)
		{
			if (year > 2500)
			{
				year = year - 543;
			}
			return year;
		}

		public static int GetYearTH(int year)
		{
			if (year < 2500)
			{
				year = year + 543;
			}
			return year;
		}

		public static string getDayNameThai(DateTime datetime)
		{
			String[] DayNameArray = { "วันอาทิตย์", "วันจันทร์", "วันอังคาร", "วันพุธ", "วันพฤหัสบดี", "วันศุกร์", "วันเสาร์" };

			var dayName = DayNameArray[(int)datetime.DayOfWeek];

			return dayName;
		}

		public static string getThaiMonth(int m, string format = "MMMM")
		{
			if (m < 1 || m > 12) return string.Empty;
			var dt = new DateTime(2000, m, 1);
			return dt.ToString(format, new CultureInfo("th-TH"));
		}

		public static string getThaiYear(DateTime datetime)
		{
			string _datetime = datetime.ToString("dd/MM/yyyy HH:mm:ss");
			DateTime dateeng = DateTime.ParseExact(_datetime, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
			int thaiYear = new ThaiBuddhistCalendar().GetYear(dateeng);
			if (thaiYear > 3000)
			{
				thaiYear = thaiYear - 543;
			}

			return thaiYear.ToString();
		}

		public static string getFullThaiShot(DateTime? datetime, bool Day0 = true, int digitYear = 4)
		{
			if (datetime.HasValue && datetime != DateTime.MinValue)
			{
				string _datetime = datetime.Value.ToString("dd/MM/yyyy HH:mm:ss");
				DateTime dateeng = DateTime.ParseExact(_datetime, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
				int thaiYear = new ThaiBuddhistCalendar().GetYear(dateeng);
				if (thaiYear < 2500)
				{
					thaiYear = thaiYear + 543;
				}
				if (thaiYear > 3000)
				{
					thaiYear = thaiYear - 543;
				}

				int thaiMonth = new ThaiBuddhistCalendar().GetMonth(dateeng);
				int thaiDay = new ThaiBuddhistCalendar().GetDayOfMonth(dateeng);

				string _thaiDay = thaiDay.ToString();
				if (Day0)
				{
					_thaiDay = thaiDay < 10 ? string.Format("{0}{1}", "0", thaiDay) : thaiDay.ToString();
				}

				string MonthCurrentName = string.Empty;
				if (thaiMonth == 1) MonthCurrentName = "ม.ค.";
				if (thaiMonth == 2) MonthCurrentName = "ก.พ.";
				if (thaiMonth == 3) MonthCurrentName = "มี.ค.";
				if (thaiMonth == 4) MonthCurrentName = "เม.ย.";
				if (thaiMonth == 5) MonthCurrentName = "พ.ค.";
				if (thaiMonth == 6) MonthCurrentName = "มิ.ย.";
				if (thaiMonth == 7) MonthCurrentName = "ก.ค.";
				if (thaiMonth == 8) MonthCurrentName = "ส.ค.";
				if (thaiMonth == 9) MonthCurrentName = "ก.ย.";
				if (thaiMonth == 10) MonthCurrentName = "ต.ค.";
				if (thaiMonth == 11) MonthCurrentName = "พ.ย.";
				if (thaiMonth == 12) MonthCurrentName = "ธ.ค.";

				String _thaiYear = thaiYear.ToString();
				if (digitYear != 4)
				{
					_thaiYear = _thaiYear.Substring(digitYear);
				}

				return string.Format("{0} {1} {2}", _thaiDay, MonthCurrentName, _thaiYear);
			}
			else
			{
				return String.Empty;
			}
		}

		public static string getFullThaiFullShot(DateTime? datetime)
		{
			if (datetime.HasValue && datetime != DateTime.MinValue)
			{
				string _datetime = datetime.Value.ToString("dd/MM/yyyy HH:mm:ss");
				if (DateTime.TryParseExact(_datetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateeng))
				{
					int thaiYear = new ThaiBuddhistCalendar().GetYear(dateeng);
					if (thaiYear < 2500)
					{
						thaiYear = thaiYear + 543;
					}
					if (thaiYear > 3000)
					{
						thaiYear = thaiYear - 543;
					}

					int thaiMonth = new ThaiBuddhistCalendar().GetMonth(dateeng);
					int thaiDay = new ThaiBuddhistCalendar().GetDayOfMonth(dateeng);

					string _thaiDay = thaiDay < 10 ? string.Format("{0}{1}", "", thaiDay) : thaiDay.ToString();
					string _thaiMonth = thaiMonth < 10 ? string.Format("{0}{1}", "0", thaiMonth) : thaiMonth.ToString();

					string MonthCurrentName = GetFullMonth(thaiMonth);

					return string.Format("{0} {1} {2}", _thaiDay, MonthCurrentName, thaiYear);
				}

				return String.Empty;
			}
			else
			{
				return String.Empty;
			}
		}

		public static string getFullThaiRange(DateTime? datetime, DateTime? datetime2)
		{
			if (datetime.HasValue && datetime != DateTime.MinValue)
			{
				string _datetime = datetime.Value.ToString("dd/MM/yyyy HH:mm:ss");
				if (DateTime.TryParseExact(_datetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateeng))
				{
					int thaiYear = new ThaiBuddhistCalendar().GetYear(dateeng);
					if (thaiYear < 2500)
					{
						thaiYear = thaiYear + 543;
					}
					if (thaiYear > 3000)
					{
						thaiYear = thaiYear - 543;
					}

					int thaiMonth = new ThaiBuddhistCalendar().GetMonth(dateeng);
					int thaiDay = new ThaiBuddhistCalendar().GetDayOfMonth(dateeng);

					string _thaiDay = thaiDay < 10 ? string.Format("{0}{1}", "", thaiDay) : thaiDay.ToString();

					string MonthCurrentName = GetFullMonth(thaiMonth);

					if (datetime2 == null || (datetime == datetime2))
					{
						return $"{_thaiDay} {MonthCurrentName} {thaiYear}";
					}
					else
					{
						string _datetime2 = datetime2.Value.ToString("dd/MM/yyyy HH:mm:ss");
						if (DateTime.TryParseExact(_datetime2, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateeng2))
						{
							int thaiYear2 = new ThaiBuddhistCalendar().GetYear(dateeng2);
							if (thaiYear2 < 2500)
							{
								thaiYear2 = thaiYear2 + 543;
							}
							if (thaiYear2 > 3000)
							{
								thaiYear2 = thaiYear2 - 543;
							}

							int thaiMonth2 = new ThaiBuddhistCalendar().GetMonth(dateeng2);
							int thaiDay2 = new ThaiBuddhistCalendar().GetDayOfMonth(dateeng2);

							string _thaiDay2 = thaiDay < 10 ? string.Format("{0}{1}", "", thaiDay2) : thaiDay2.ToString();

							string MonthCurrentName2 = GetFullMonth(thaiMonth2);


							if (thaiYear == thaiYear2 && thaiMonth == thaiMonth2)
							{
								return $"{_thaiDay}-{thaiDay2}  {MonthCurrentName} {thaiYear}";
							}
							else
							{
								return $"{_thaiDay} {MonthCurrentName} {thaiYear} - {_thaiDay2} {MonthCurrentName2} {thaiYear2}";
							}

						}
					}

				}

				return String.Empty;
			}
			else
			{
				return String.Empty;
			}
		}

		public static string GetFullMonth(int Month)
		{
			string MonthCurrentName = string.Empty;
			if (Month == 1) MonthCurrentName = "มกราคม";
			if (Month == 2) MonthCurrentName = "กุมภาพันธ์";
			if (Month == 3) MonthCurrentName = "มีนาคม";
			if (Month == 4) MonthCurrentName = "เมษายน";
			if (Month == 5) MonthCurrentName = "พฤษภาคม";
			if (Month == 6) MonthCurrentName = "มิถุนายน";
			if (Month == 7) MonthCurrentName = "กรกฎาคม";
			if (Month == 8) MonthCurrentName = "สิงหาคม";
			if (Month == 9) MonthCurrentName = "กันยายน";
			if (Month == 10) MonthCurrentName = "ตุลาคม";
			if (Month == 11) MonthCurrentName = "พฤศจิกายน";
			if (Month == 12) MonthCurrentName = "ธันวาคม";

			return MonthCurrentName;
		}

		public static string DateToTimeString(DateTime? datetime, string? minute_end = "น.", bool? isSecond = false)
		{
			if (datetime.HasValue && datetime != DateTime.MinValue)
			{
				if (DateTime.TryParseExact(datetime.Value.ToString("dd/MM/yyyy HH:mm:ss"), "dd/MM/yyyy HH:mm:ss",
						   System.Globalization.CultureInfo.InvariantCulture,
						   System.Globalization.DateTimeStyles.None, out DateTime dateeng))
				{
					int hour = new ThaiBuddhistCalendar().GetHour(dateeng);
					int minute = new ThaiBuddhistCalendar().GetMinute(dateeng);
					int second = new ThaiBuddhistCalendar().GetSecond(dateeng);
					string _hour = hour < 10 ? "0" + hour : hour.ToString();
					string _minute = minute < 10 ? "0" + minute : minute.ToString();
					string _second = second < 10 ? ":0" + second : ":" + second.ToString();

					if (isSecond != true) _second = String.Empty;

					return $"{_hour}:{_minute}{_second} {minute_end}";
				}
				return String.Empty;
			}
			else
			{
				return String.Empty;
			}
		}

		public static string DateToHourMinuteString(DateTime? datetime, string? minute_end = "นาที")
		{
			if (datetime.HasValue && datetime != DateTime.MinValue)
			{
				if (DateTime.TryParseExact(datetime.Value.ToString("dd/MM/yyyy HH:mm:ss"), "dd/MM/yyyy HH:mm:ss",
						   System.Globalization.CultureInfo.InvariantCulture,
						   System.Globalization.DateTimeStyles.None, out DateTime dateeng))
				{
					int hour = new ThaiBuddhistCalendar().GetHour(dateeng);
					int minute = new ThaiBuddhistCalendar().GetMinute(dateeng);
					string _hour = hour < 10 ? "0" + hour : hour.ToString();
					string _minute = minute < 10 ? "0" + minute : minute.ToString();
					return $"{_hour} ชั่วโมง {_minute} {minute_end}";
				}
				return String.Empty;
			}
			else
			{
				return String.Empty;
			}
		}

		public static string? LimitTo(string? data, int length)
		{
			try
			{
				if (data == null) return null;

				return data.Length < length ? data : $"{data.Substring(0, length)}...";
			}
			catch
			{
				return data;
			}
		}

		public static string EmptyTo(object? val, string mark = "-")
		{
			if (val is int)
			{
				return val.ToString() ?? mark;
			}
			if (val is string)
			{
				if (!String.IsNullOrEmpty(val.ToString()))
				{
					return val.ToString() ?? mark;
				}
			}

			return mark;
		}

		public static string? UrlEncode(string? url)
		{
			if (url == null) return url;

			return System.Net.WebUtility.UrlEncode(url);
		}

		public static string? UrlDecode(string? url)
		{
			if (url == null) return url;

			return System.Net.WebUtility.UrlDecode(url);
		}

		public static bool IsEngOnly(string str)
		{
			return Regex.Matches(str, @"[a-zA-Z]").Count > 0;
		}

		public static bool LOGExcept(string? requestPath)
		{
			if (requestPath == null) return false;

			List<string> list = new List<string>();

			list.Add("/api/v1/Master/");
			list.Add("/api/v1/Authorize/ExpireToken");
			list.Add("/api/v1/User/GetLevels");

			foreach (var item in list)
			{
				if (requestPath.Contains(item))
				{
					return true;
				}
			}

			return false;
		}

		public static bool LineTxtAlert(string? txt)
		{
			if (txt == null) return false;

			List<string> list = new List<string>();

			list.Add("traceId");

			foreach (var item in list)
			{
				if (txt.Contains(item))
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsBase64String(string base64)
		{
			if (base64 == null) return false;
			if (base64.StartsWith("data:image")) return true;

			base64 = base64.Trim();
			return (base64.Length % 4 == 0) && Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
		}

		public static FileResponseModel UploadImage(FileModel upload, bool? isThumbnail = true)
		{
			AppSettings _mySet = upload.appSet ?? new AppSettings();

			var response = new FileResponseModel();

			string pathImage = $@"{upload.Folder}\{upload.Id}{upload.MimeType}";
			string fullPathSave = $@"{_mySet.ContentRootPath}\{pathImage}";
			SaveBase64ToImage(upload.ImgBase64Only, fullPathSave, 950);
			response.ImageUrl = $"{_mySet.baseUriWeb}/{pathImage.Replace("\\", "/")}";

			if (isThumbnail == true)
			{
				string pathImageThumbnail = $@"{upload.Folder}\{upload.Id}_thumbnail{upload.MimeType}";
				string fullPathSaveThumbnail = $@"{_mySet.ContentRootPath}\{pathImageThumbnail}";
				SaveBase64ToImage(upload.ImgBase64Only, fullPathSaveThumbnail, 90);
				response.ImageThumbnailUrl = $"{_mySet.baseUriWeb}/{pathImageThumbnail.Replace("\\", "/")}";
			}

			return response;
		}

		public static FileResponseModel UploadFileByte(FileModel upload, string folder)
		{
			AppSettings _mySet = upload.appSet ?? new AppSettings();

			var response = new FileResponseModel();

			string path = $@"{upload.Folder}\{upload.Id}{upload.MimeType}";
			string fullPathSave = $@"{_mySet.ContentRootPath}\{path}";
			SaveByteToFolder(upload, fullPathSave);
			response.UploadUrl = $"{_mySet.baseUriWeb}/{path.Replace("\\", "/")}";

			return response;
		}

		public static async Task<FileResponseModel?> UploadFormFile(FileModel upload)
		{
			if (upload.FileData == null || upload.FileData.Length <= 0) return null;

			var response = new FileResponseModel();

			AppSettings _appSet = upload.appSet ?? new AppSettings();
			string path = $@"{upload.Folder}\{upload.Id}{upload.MimeType}";
			string fullPathSave = $@"{_appSet.ContentRootPath}\files\{path}";

			GeneralUtils.CreateDirectory(fullPathSave);

			using (var fileStream = new FileStream(fullPathSave, FileMode.Create, FileAccess.Write))
			{
				await upload.FileData.CopyToAsync(fileStream);
			}
			var parametere = Securitys.Base64StringEncode(path);
			response.UploadUrl = $"{_appSet.baseUriApi}/v1/file/v?id={parametere}";

			return response;
		}

		public static void SaveBase64ToImage(string? base64Only, string fullPathSave, int thumbnailSize)
		{
			if (base64Only != null)
			{
				byte[] imageBytes = Convert.FromBase64String(base64Only);

				using var image = SixLabors.ImageSharp.Image.Load(imageBytes);

				int newWidth = image.Width;
				int newHeight = image.Height;

				if (image.Width > thumbnailSize)
				{
					if (image.Width > image.Height)
					{
						newWidth = thumbnailSize;
						newHeight = image.Height * thumbnailSize / image.Width;
					}
					else
					{
						newWidth = image.Width * thumbnailSize / image.Height;
						newHeight = thumbnailSize;
					}
				}

				//แก้บน server linux error
				fullPathSave = fullPathSave.Replace(@"\", "/");

				GeneralUtils.CreateDirectory(fullPathSave);

				image.Mutate(x => x.Resize(newWidth, newHeight));
				image.Save(fullPathSave);
			}
		}

		public static void SaveByteToFolder(FileModel upload, string fullPathSave)
		{
			if (upload.FileByte != null)
			{
				if (upload.FileByte.Length <= 0) return;

				//แก้บน server linux error
				fullPathSave = fullPathSave.Replace(@"\", "/");

				GeneralUtils.CreateDirectory(fullPathSave);

				File.WriteAllBytes(fullPathSave, upload.FileByte);
			}
		}

		public static void CreateDirectory(string filefullpath)
		{
			FileInfo fileInfo = new FileInfo(filefullpath);

			if (!fileInfo.Exists && fileInfo.Directory != null && !fileInfo.Directory.Exists)
				Directory.CreateDirectory(fileInfo.Directory.FullName);
		}

		public static decimal ToDecimal(object? value)
		{
			try
			{
				var val = Decimal.Parse(value.ToString());
				return val;
			}
			catch
			{
				return 0;
			}
		}

		public static bool IsDecimal(decimal value)
		{
			try
			{
				if ((value % 1) == 0)
				{
					return false;
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool IsDigit(string? value)
		{
			try
			{
				if (value == null) return false;

				return value.All(Char.IsDigit);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// อยู่ระหว่างดำเนินการ
		/// </summary>
		/// <param name="statusSaleId"></param>
		/// <returns></returns>
		public static bool CheckInProcessSale(int statusSaleId)
		{
			if (statusSaleId >= StatusSaleModel.WaitContact && statusSaleId <= StatusSaleModel.CloseSale)
			{
				return true;
			}
			return false;
		}

		public static List<T>[] PartitionList<T>(List<T> list, int totalPartitions)
		{
			if (list == null)
				throw new ArgumentNullException("list");

			if (totalPartitions < 1)
				throw new ArgumentOutOfRangeException("totalPartitions");

			List<T>[] partitions = new List<T>[totalPartitions];

			int maxSize = (int)Math.Ceiling(list.Count / (double)totalPartitions);
			int k = 0;

			for (int i = 0; i < partitions.Length; i++)
			{
				partitions[i] = new List<T>();
				for (int j = k; j < k + maxSize; j++)
				{
					if (j >= list.Count)
						break;
					partitions[i].Add(list[j]);
				}
				k += maxSize;
			}

			return partitions;
		}

	}
}
