
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cmp;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Shares;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalesPipeline.Utils
{
    public static class GeneralUtils
    {
        private const string DefaultCulture = "en-US";
        private const string DefaultDateFormat = "dd/MM/yyyy";
        private const string DateTimeWithSecondsFormat = "dd/MM/yyyy HH:mm:ss";
        private const int ThaiYearOffset = 543;
        private const int ThaiYearLowerBound = 2500;
        private const int ThaiYearUpperBound = 3000;
        private const string DefaultSeparator = "/";
        private const string DefaultSeparatorDash = "-";

        private static readonly ThaiBuddhistCalendar ThaiCalendar = new();

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

        public static string MapErrorModel(string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return string.Empty;

            var dataMap = JsonConvert.DeserializeObject<ErrorCustom>(content);
            return dataMap?.Message ?? string.Empty;
        }

        public static string DateToThString(DateTime? datetime, string separator = DefaultSeparator)
        {
            if (!datetime.HasValue || datetime.Value == DateTime.MinValue)
                return string.Empty;

            var date = datetime.Value;

            // แปลงปีเป็น พ.ศ.
            int year = date.Year + 543;

            string day = date.Day.ToString("D2");     // "D2" = เติม 0 อัตโนมัติ
            string month = date.Month.ToString("D2");

            return $"{day}{separator}{month}{separator}{year}";
        }

        public static DateTime? DateToEn(string? datetime, string format = DefaultDateFormat, string Culture = DefaultCulture)
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

        public static DateTime? DateToEnFormatMulti(string? datetime, string[] formats, string Culture = DefaultCulture)
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

        public static string? DateToEnStr(DateTime? datetime, string separator = DefaultSeparator, string? rNull = null)
        {
            if (!IsValidDateTime(datetime))
                return rNull;

            var date = datetime.Value;
            int year = ConvertThaiYearToEnglish(date.Year);

            // ✅ ใช้ PadLeft แทน string.Format
            string day = date.Day.ToString().PadLeft(2, '0');
            string month = date.Month.ToString().PadLeft(2, '0');

            return $"{day}{separator}{month}{separator}{year}";
        }

        public static int ConvertThaiYearToEnglish(int year)
        {
            return year > ThaiYearLowerBound ? year - ThaiYearOffset : year;
        }

        public static string? DateToStrParameter(DateTime? datetime, string separator = DefaultSeparatorDash)
        {
            if (!IsValidDateTime(datetime))
                return string.Empty;

            var dateInfo = ParseDateTime(datetime.Value);
            if (dateInfo == null)
                return string.Empty;

            return FormatDateParameter(dateInfo, separator);
        }

        private static ThaiDateInfo? ParseDateTime(DateTime datetime)
        {
            string dateString = datetime.ToString(DateTimeWithSecondsFormat);

            if (!DateTime.TryParseExact(
                dateString,
                DateTimeWithSecondsFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsedDate))
            {
                return null;
            }

            return new ThaiDateInfo
            {
                Year = parsedDate.Year,
                Month = ThaiCalendar.GetMonth(parsedDate),
                Day = ThaiCalendar.GetDayOfMonth(parsedDate)
            };
        }

        private static string FormatDateParameter(ThaiDateInfo dateInfo, string separator)
        {
            // ใช้ :D2 สำหรับ zero-padding (2 หลัก)
            return $"{dateInfo.Year}{separator}{dateInfo.Month:D2}{separator}{dateInfo.Day:D2}";
        }

        public static DateTime? DateNotNullToEn(string? datetime, string format = DefaultDateFormat, string Culture = DefaultCulture)
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

        public static DateOnly DateOnlyToEn(string datetime, string format = DefaultDateFormat, string Culture = DefaultCulture)
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
            string _datetime = datetime.ToString(DateTimeWithSecondsFormat);
            DateTime dateeng = DateTime.ParseExact(_datetime, DateTimeWithSecondsFormat, new CultureInfo(DefaultCulture));
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
                string _datetime = datetime.Value.ToString(DateTimeWithSecondsFormat);
                DateTime dateeng = DateTime.ParseExact(_datetime, DateTimeWithSecondsFormat, new CultureInfo(DefaultCulture));
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
                string _datetime = datetime.Value.ToString(DateTimeWithSecondsFormat);
                if (DateTime.TryParseExact(_datetime, DateTimeWithSecondsFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateeng))
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

        public static string GetFullThaiRange(DateTime? datetime, DateTime? datetime2)
        {
            if (!IsValidDateTime(datetime))
                return string.Empty;

            // ✅ Unwrap nullable ก่อนส่งเข้า method
            var thaiDate1 = ConvertToThaiDate(datetime.Value); // ใช้ .Value เพราะรู้แล้วว่าไม่ null
            if (thaiDate1 == null)
                return string.Empty;

            // กรณีมีแค่วันเดียว
            if (!IsValidDateTime(datetime2) || datetime == datetime2)
                return FormatSingleDate(thaiDate1);

            // ✅ Unwrap nullable ก่อนส่งเข้า method
            var thaiDate2 = ConvertToThaiDate(datetime2.Value);
            if (thaiDate2 == null)
                return string.Empty;

            return FormatDateRange(thaiDate1, thaiDate2);
        }

        private static bool IsValidDateTime(DateTime? datetime)
        {
            return datetime.HasValue && datetime.Value != DateTime.MinValue;
        }

        private static ThaiDateInfo? ConvertToThaiDate(DateTime datetime)
        {
            string dateString = datetime.ToString(DateTimeWithSecondsFormat);

            if (!DateTime.TryParseExact(
                dateString,
                DateTimeWithSecondsFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsedDate))
            {
                return null;
            }

            return new ThaiDateInfo
            {
                Day = ThaiCalendar.GetDayOfMonth(parsedDate),
                Month = ThaiCalendar.GetMonth(parsedDate),
                Year = NormalizeThaiYear(ThaiCalendar.GetYear(parsedDate))
            };
        }

        private static int NormalizeThaiYear(int year)
        {
            if (year < ThaiYearLowerBound)
                return year + ThaiYearOffset;

            if (year > ThaiYearUpperBound)
                return year - ThaiYearOffset;

            return year;
        }

        private static string FormatSingleDate(ThaiDateInfo date)
        {
            string day = FormatDay(date.Day);
            string month = GetFullMonth(date.Month);
            return $"{day} {month} {date.Year}";
        }

        private static string FormatDateRange(ThaiDateInfo date1, ThaiDateInfo date2)
        {
            string day1 = FormatDay(date1.Day);
            string month1 = GetFullMonth(date1.Month);
            string day2 = FormatDay(date2.Day);
            string month2 = GetFullMonth(date2.Month);

            // เดือนและปีเดียวกัน
            if (date1.Year == date2.Year && date1.Month == date2.Month)
                return $"{day1}-{day2} {month1} {date1.Year}";

            // คนละเดือนหรือปี
            return $"{day1} {month1} {date1.Year} - {day2} {month2} {date2.Year}";
        }

        private static string FormatDay(int day)
        {
            return day.ToString(); // ไม่จำเป็นต้อง pad เพราะ "{0}{1}" กับ "" ไม่มีผล
        }

        private class ThaiDateInfo
        {
            public int Day { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
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
                if (DateTime.TryParseExact(datetime.Value.ToString(DateTimeWithSecondsFormat), DateTimeWithSecondsFormat,
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
                if (DateTime.TryParseExact(datetime.Value.ToString(DateTimeWithSecondsFormat), DateTimeWithSecondsFormat,
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

        public static bool LOGExceptLimin(string? requestPath)
        {
            if (requestPath == null) return false;

            List<string> list = new List<string>();

            list.Add("/v1/ProcessSale/CreateReply");

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

            // ✅ 1. Validate และ Sanitize input
            string sanitizedFolder = SanitizePath(upload.Folder);
            string sanitizedId = SanitizePath(upload.Id);
            string sanitizedMimeType = SanitizeMimeType(upload.MimeType);

            // ✅ 2. สร้าง path อย่างปลอดภัย
            string basePath = Path.Combine(_appSet.ContentRootPath, "files");
            string relativePath = Path.Combine(sanitizedFolder, $"{sanitizedId}{sanitizedMimeType}");
            string fullPathSave = Path.Combine(basePath, relativePath);

            // ✅ 3. Normalize และตรวจสอบ Path Traversal
            string normalizedPath = Path.GetFullPath(fullPathSave);
            string normalizedBase = Path.GetFullPath(basePath);

            if (!normalizedPath.StartsWith(normalizedBase, StringComparison.OrdinalIgnoreCase))
            {
                throw new SecurityException("Invalid file path detected");
            }

            // ✅ 4. สร้าง directory - ใช้ normalizedPath แทน fullPathSave
            string? directory = Path.GetDirectoryName(normalizedPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // ✅ 5. บันทึกไฟล์ - ใช้ normalizedPath แทน fullPathSave
            using (var fileStream = new FileStream(normalizedPath, FileMode.Create, FileAccess.Write))
            {
                await upload.FileData.CopyToAsync(fileStream);
            }

            // ✅ 6. สร้าง URL response
            string pathForUrl = Path.Combine(sanitizedFolder, $"{sanitizedId}{sanitizedMimeType}");
            var parameter = Securitys.Base64StringEncode(pathForUrl);
            response.UploadUrl = $"{_appSet.baseUriApi}/v1/file/v?id={parameter}";

            return response;
        }

        // Helper Methods
        private static string SanitizePath(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // ลบ characters อันตราย
            char[] invalidChars = Path.GetInvalidFileNameChars()
                .Concat(Path.GetInvalidPathChars())
                .Concat(new[] { '.', '/' }).ToArray();

            return string.Join("", input.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        }

        private static string SanitizeMimeType(string mimeType)
        {
            if (string.IsNullOrWhiteSpace(mimeType))
                return string.Empty;

            // อนุญาตเฉพาะ extension ที่ปลอดภัย
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx" };

            string normalized = mimeType.ToLowerInvariant();
            if (!normalized.StartsWith("."))
                normalized = "." + normalized;

            return allowedExtensions.Contains(normalized) ? normalized : string.Empty;
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
            // ✅ Validate path ก่อนใช้งาน
            string normalizedPath = Path.GetFullPath(filefullpath);

            FileInfo fileInfo = new FileInfo(normalizedPath);

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

        /// <summary>
        /// สตริงมีเพียงตัวเลขเท่านั้น
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        public static T DeepCopyJson<T>(this T input)
        {
            //var serialized = JsonConvert.SerializeObject(input);
            //return JsonConvert.DeserializeObject<T>(serialized);

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var serialized = JsonConvert.SerializeObject(input, settings);

            // ตรวจสอบว่าค่า serialized เป็น null หรือไม่
            if (serialized == null)
            {
                return default(T); // คืนค่าเริ่มต้นของประเภท T หาก serialized เป็น null
            }

            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public static T DeepCopyStream<T>(T input)
        {
            using var stream = new MemoryStream();

            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream, input);
            stream.Position = 0;

            return (T)serializer.ReadObject(stream);
        }

        public static List<int?> ListStringToInt(List<string?> model)
        {
            List<int?> response = new();
            foreach (var item in model)
            {
                if (int.TryParse(item, out int id) && id > 0)
                {
                    response.Add(id);
                }
            }
            return response;
        }

        public static List<Guid?> ListStringToGuid(List<string?> model)
        {
            List<Guid?> response = new();
            foreach (var item in model)
            {
                if (Guid.TryParse(item, out Guid id) && id != Guid.Empty)
                {
                    response.Add(id);
                }
            }
            return response;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[new Random((int)DateTime.Now.Ticks).Next(s.Length)]).ToArray());
        }

        public static bool IsLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        public static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        public static bool IsSymbol(char c)
        {
            return c > 32 && c < 127 && !IsDigit(c) && !IsLetter(c);
        }

        public static bool IsValidPassword(string password)
        {
            //return
            //   password.Any(c => IsLetter(c)) &&
            //   password.Any(c => IsDigit(c)) &&
            //   password.Any(c => IsSymbol(c));
            return
               password.Any(c => IsLetter(c)) &&
               password.Any(c => IsDigit(c));
        }

        public static Regex PasswordValidation()
        {
            string pattern = "(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,15})$";

            return new Regex(pattern, RegexOptions.IgnoreCase);
        }

        public static void SendNotificationUtils(string message)
        {
            // Logic to send notification (e.g., email, SMS, push notification)
            Console.WriteLine($"Notification: {message}");
        }

        public static bool HasAtLeastOneCommonElement(List<int> A, List<int> B)
        {
            HashSet<int> setA = new HashSet<int>(A);
            foreach (int item in B)
            {
                if (setA.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        // ตรวจสอบว่ามีข้อความหน้าหรือไม่
        public static bool HasPrefix(string text, string[] prefixes)
        {
            if (string.IsNullOrEmpty(text) || prefixes == null || prefixes.Length == 0)
            {
                return false;
            }

            return prefixes.Any(prefix =>
                !string.IsNullOrEmpty(prefix) && text.StartsWith(prefix));
        }

        // ลบข้อความหน้าออก
        public static string RemovePrefixes(string text, string[] prefixes)
        {
            if (string.IsNullOrEmpty(text) || prefixes == null || prefixes.Length == 0)
            {
                return text;
            }

            foreach (string prefix in prefixes)
            {
                if (!string.IsNullOrEmpty(prefix) && text.StartsWith(prefix))
                {
                    return text.Substring(prefix.Length);
                }
            }

            return text;
        }

    }
}
