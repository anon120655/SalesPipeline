using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using Microsoft.Extensions.Options;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SalesPipeline.Infrastructure.Repositorys;
using SalesPipeline.Utils.ValidationModel;
using System.Net.Http;

namespace SalesPipeline.API.Controllers
{
    [Authorizes]
    [ApiVersion(1.0)]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Route("v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        private readonly AppSettings _appSet;
        public readonly HttpClient _httpClient;

        public UserController(IRepositoryWrapper repo, IOptions<AppSettings> appSet, HttpClient httpClient)
        {
            _repo = repo;
            _appSet = appSet.Value;
            _httpClient = httpClient;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(UserCustom model)
        {
            try
            {
                var data = await _repo.User.Create(model);

                await Task.Factory.StartNew(async () =>
                {
                    string urlFull = $"{_appSet.baseUriApi}/v1/Mail/SendNewUser?id={data.Id}";
                    await _httpClient.GetAsync(urlFull);
                });

                if (data.RoleId == 7)
                {
                    await _repo.AssignmentCenter.UpdateCurrentNumber(data.Id);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserCustom model)
        {
            try
            {
                var data = await _repo.User.Update(model);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteById([FromQuery] UpdateModel model)
        {
            try
            {
                await _repo.User.DeleteById(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpPut("UpdateStatusById")]
        public async Task<IActionResult> UpdateStatusById(UpdateModel model)
        {
            try
            {
                await _repo.User.UpdateStatusById(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var data = await _repo.User.GetById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpGet("GetAreaByUserId")]
        public async Task<IActionResult> GetAreaByUserId([FromQuery] int id)
        {
            try
            {
                var data = await _repo.User.GetAreaByUserId(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] UserFilter model)
        {
            try
            {
                var response = await _repo.User.GetList(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(User_RoleCustom model)
        {
            try
            {
                var data = await _repo.User.CreateRole(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole(User_RoleCustom model)
        {
            try
            {
                var data = await _repo.User.UpdateRole(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpDelete("DeleteRoleById")]
        public async Task<IActionResult> DeleteRoleById([FromQuery] UpdateModel model)
        {
            try
            {
                await _repo.User.DeleteRoleById(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpPut("UpdateIsModifyRoleById")]
        public async Task<IActionResult> UpdateIsModifyRoleById(UpdateModel model)
        {
            try
            {
                await _repo.User.UpdateIsModifyRoleById(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetRoleById")]
        public async Task<IActionResult> GetRoleById([FromQuery] int id)
        {
            try
            {
                var data = await _repo.User.GetRoleById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpGet("GetListRole")]
        public async Task<IActionResult> GetListRole([FromQuery] allFilter model)
        {
            try
            {
                var response = await _repo.User.GetListRole(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpGet("GetUserByRole")]
        public async Task<IActionResult> GetUserByRole([FromQuery] int roleId)
        {
            try
            {
                var response = await _repo.User.GetUserByRole(roleId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpGet("GetListLevel")]
        public async Task<IActionResult> GetListLevel([FromQuery] allFilter model)
        {
            try
            {
                var response = await _repo.User.GetListLevel(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("ValidateUpload")]
        public async Task<IActionResult> ValidateUpload(List<UserCustom> model)
        {
            try
            {
                var data = await _repo.User.ValidateUpload(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("ImportUser")]
        public async Task<IActionResult> ImportUser(IFormFile files)
        {
            try
            {
                await Task.Delay(1);
                List<UserCustom> UserList = new List<UserCustom>();

                if (files == null)
                    throw new ExceptionCustom("Select File.");

                int fileLimit = 100; // MB
                int TenMegaBytes = fileLimit * 1024 * 1024;
                if (files.Length > TenMegaBytes)
                    throw new ExceptionCustom($"ขนาดไฟล์ไม่เกิน {fileLimit} MB");

                // 1. ตรวจสอบนามสกุลไฟล์อย่างปลอดภัย
                string fileExtension = Path.GetExtension(files.FileName)?.ToLowerInvariant();
                if (fileExtension != ".xlsx" && fileExtension != ".csv")
                    throw new ExceptionCustom("FileExtension Not Support. Only .xlsx and .csv are allowed.");

                if (fileExtension == ".xls")
                    throw new ExceptionCustom("not support Excel 97-2000 formats.");

                // 2. สร้างชื่อไฟล์ใหม่ที่ปลอดภัย (ใช้ Guid)
                string safeFileName = Guid.NewGuid().ToString("N") + fileExtension;

                // 3. โฟลเดอร์ปลอดภัย (ใช้ Path.Combine + Directory.CreateDirectory)
                string folderName = Path.Combine(_appSet.ContentRootPath, "import", "excel");
                Directory.CreateDirectory(folderName); // สร้างโฟลเดอร์ถ้ายังไม่มี

                string fullPath = Path.Combine(folderName, safeFileName);

                ISheet sheet;
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await files.CopyToAsync(stream);
                    stream.Position = 0;

                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                    sheet = hssfwb.GetSheetAt(0);
                    int firstRowNum = sheet.FirstRowNum;

                    for (int i = firstRowNum + 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        var EmployeeId = row.GetCell(0)?.ToString();
                        var FullName = row.GetCell(1)?.ToString();
                        var Email = row.GetCell(2)?.ToString();
                        var PositionId = row.GetCell(3)?.ToString();
                        var LevelId = row.GetCell(4)?.ToString();
                        var RoleId = row.GetCell(5)?.ToString();

                        int.TryParse(PositionId, out int _positionId);
                        int.TryParse(LevelId, out int _levelid);
                        int.TryParse(RoleId, out int _roleid);

                        UserList.Add(new UserCustom
                        {
                            CurrentUserId = 2,
                            EmployeeId = EmployeeId,
                            FullName = FullName,
                            Email = Email,
                            PositionId = _positionId > 0 ? _positionId : null,
                            LevelId = _levelid > 0 ? _levelid : null,
                            RoleId = _roleid > 0 ? _roleid : null
                        });
                    }
                }

                // (Optional) ลบไฟล์หลังใช้งานเสร็จ
                // File.Delete(fullPath);

                return Ok(UserList);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpPost("GetUserTargetList")]
        public async Task<IActionResult> GetUserTargetList(allFilter model)
        {
            try
            {
                var response = await _repo.User.GetUserTargetList(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpPut("UpdateUserTarget")]
        public async Task<IActionResult> UpdateUserTarget(User_Main model)
        {
            try
            {
                await _repo.User.UpdateUserTarget(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                await _repo.User.ChangePassword(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

    }
}
