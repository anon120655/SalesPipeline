using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.iAuthen;
using System.Linq;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class Authorizes : IAuthorizes
    {
        private IRepositoryWrapper _repo;
        private readonly IRepositoryBase _db;
        private readonly AppSettings _appSet;
        //private readonly IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public Authorizes(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet/*, IJwtUtils jwtUtils*/, IMapper mapper)
        {
            _db = db;
            _repo = repo;
            _appSet = appSet.Value;
            //_jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            int? expires_in = 1; //days

            var user = _repo.Context.Users.Include(x => x.Role).SingleOrDefault(x => x.UserName == model.Username);

            // return null if user not found
            if (user == null) throw new ExceptionCustom($"อีเมลหรือรหัสผ่านของท่านไม่ถูกต้อง");

            if (user.PasswordHash == null) throw new ExceptionCustom("อีเมลหรือรหัสผ่านของท่านไม่ถูกต้อง");

            try
            {
                if (user.Status == StatusModel.Delete)
                    throw new ExceptionCustom($"อีเมลหรือรหัสผ่านของท่านไม่ถูกต้อง!");

                if (user.Status == StatusModel.InActive)
                    throw new ExceptionCustom($"ท่านถูกปิดการใช้งาน กรุณาติดต่อผู้ดูแลระบบ");

                short maxLoginFail = 5;
                var config = await _repo.System.GetConfigByCode(ConfigCode.LOGIN_FAIL);
                if (config != null && short.TryParse(config.Value, out short _maxLoginFail))
                {
                    maxLoginFail = _maxLoginFail;
                }

                if (user.LoginFail >= maxLoginFail)
                    throw new ExceptionCustom($"ท่านถูกระงับการใช้งาน กรุณาติดต่อผู้ดูแลระบบ");

                //string passwordHashGen = BCrypt.Net.BCrypt.EnhancedHashPassword("password", hashType: HashType.SHA384);
                bool verified = BCrypt.Net.BCrypt.EnhancedVerify(model.Password, user.PasswordHash, hashType: HashType.SHA384);
                if (!verified)
                {
                    user.LoginFail = user.LoginFail.HasValue ? (short?)(user.LoginFail + 1) : (short?)1;

                    _db.Update(user);
                    await _db.SaveAsync();

                    throw new ExceptionCustom($"อีเมลหรือรหัสผ่านของท่านไม่ถูกต้อง ท่านกรอกผิดอีก {maxLoginFail - user.LoginFail} ครั้ง จะถูกระงับการใช้งาน");
                }
                else
                {
                    user.LoginFail = null;
                    _db.Update(user);
                    await _db.SaveAsync();

                    if (user.Role != null && user.Role.Status != StatusModel.Active) throw new ExceptionCustom("ไม่พบบทบาทการใช้งาน");

                    //นำออกชั่วคราวเพื่อทดสอบ JMeter
                    //if (user.Role != null && user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
                    //{
                    //    if (!await _repo.AssignmentRM.CheckAssignmentByUserId(user.Id))
                    //    {
                    //        var assignment = await _repo.AssignmentRM.Create(new()
                    //        {
                    //            Status = StatusModel.Active,
                    //            UserId = user.Id,
                    //            EmployeeId = user.EmployeeId,
                    //            EmployeeName = user.FullName,
                    //        });
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionCustom(ex.Message);
            }

            var userMap = _mapper.Map<UserCustom>(user);

            string txt_exp_res = $"{expires_in}d";
            int? expires_in_fcc = null; //days
            if (_appSet.SystemType == SystemTypeModel.FCC)
            {
                //expires_in = null;
                //expires_in_fcc = 1;
                //txt_exp_res = $"{expires_in_fcc}m";
            }
            // authentication successful so generate jwt token
            var generate_response = await _repo.jwtUtils.GenerateJwtToken(userMap, expires_in, expires_in_fcc);

            return new AuthenticateResponse(userMap, generate_response.AccessToken, txt_exp_res, generate_response.RefreshToken);
        }

        public async Task<AuthenticateResponse?> AuthenticateBAAC(AuthenticateRequest model, iAuthenResponse.ResponseData modeliAuth)
        {
            int expires_in = 10; //days

            int? roleId = 0;
            int? levelId = null;

            if (modeliAuth.job_id != null)
            {
                var user_Roles = await _repo.Context.User_Roles
                    .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete
                    && x.iAuthenRoleCode != null
                    && x.iAuthenRoleCode.Contains(modeliAuth.job_id));
                if (user_Roles != null)
                {
                    roleId = user_Roles.Id;
                    if (int.TryParse(modeliAuth.employee_position_level, out int _levelid))
                    {
                        levelId = _levelid;
                    }
                }
            }

            UserCustom userCustom = new()
            {
                Status = StatusModel.Active,
                Create_Type = 1,
                UpdateChannel = 1,
                CurrentUserId = 0,
                UserName = modeliAuth.Username,
                EmployeeId = modeliAuth.employee_id,
                TitleName = modeliAuth.title_th,
                FirstName = modeliAuth.first_name_th,
                LastName = modeliAuth.last_name_th,
                FullName = $"{modeliAuth.first_name_th} {modeliAuth.last_name_th}",
                Email = modeliAuth.email,
                Tel = modeliAuth.mobile_no,
                RoleId = roleId,
                LevelId = levelId,
                authen_fail_time = modeliAuth.authen_fail_time,
                branch_code = modeliAuth.branch_code,
                branch_name = modeliAuth.branch_name,
                cbs_id = modeliAuth.cbs_id,
                change_password_url = modeliAuth.change_password_url,
                create_password_url = modeliAuth.create_password_url,
                email_baac = modeliAuth.email,
                employee_id = modeliAuth.employee_id,
                employee_position_id = modeliAuth.employee_position_id,
                employee_position_level = modeliAuth.employee_position_level,
                employee_position_name = modeliAuth.employee_position_name,
                employee_status = modeliAuth.employee_status,
                first_name_th = modeliAuth.first_name_th,
                image_existing = modeliAuth.image_existing,
                job_field_id = modeliAuth.job_field_id,
                job_field_name = modeliAuth.job_field_name,
                job_id = modeliAuth.job_id,
                job_name = modeliAuth.job_name,
                last_name_th = modeliAuth.last_name_th,
                lastauthen_timestamp = modeliAuth.lastauthen_timestamp,
                mobile_no = modeliAuth.mobile_no,
                name_en = modeliAuth.name_en,
                org_id = modeliAuth.org_id,
                org_name = modeliAuth.org_name,
                organization_48 = modeliAuth.organization_48,
                organization_abbreviation = modeliAuth.organization_abbreviation,
                organization_upper_id = modeliAuth.organization_upper_id,
                organization_upper_id2 = modeliAuth.organization_upper_id2,
                organization_upper_id3 = modeliAuth.organization_upper_id3,
                organization_upper_name = modeliAuth.organization_upper_name,
                organization_upper_name2 = modeliAuth.organization_upper_name2,
                organization_upper_name3 = modeliAuth.organization_upper_name3,
                password_unexpire = modeliAuth.password_unexpire,
                requester_active = modeliAuth.requester_active,
                requester_existing = modeliAuth.requester_existing,
                timeresive = modeliAuth.timeresive,
                timesend = modeliAuth.timesend,
                title_th = modeliAuth.title_th,
                title_th_2 = modeliAuth.title_th_2,
                user_class = modeliAuth.user_class,
                username_active = modeliAuth.username_active,
                username_existing = modeliAuth.username_existing,
                working_status = modeliAuth.working_status
            };

            var user = _repo.Context.Users.SingleOrDefault(x => x.UserName == model.Username);
            if (user == null)
            {
                await _repo.User.Create(userCustom);
            }
            else
            {
                userCustom.Id = user.Id;
                await _repo.User.Update(userCustom);
            }

            user = _repo.Context.Users.Include(x => x.Role).SingleOrDefault(x => x.UserName == model.Username);

            var userMap = _mapper.Map<UserCustom>(user);

            // authentication successful so generate jwt token
            var generate_response = await _repo.jwtUtils.GenerateJwtToken(userMap, expires_in);

            return new AuthenticateResponse(userMap, generate_response.AccessToken, expires_in + "d", generate_response.RefreshToken);
        }

        public UserAuth? GetById(int id)
        {
            var user = _repo.Context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return null;

            UserAuth userAuth = new UserAuth()
            {
                Id = user.Id,
                FirstName = user.FirstName ?? String.Empty,
                LastName = user.LastName ?? String.Empty
            };
            return userAuth;
        }

        public async Task CreateRefreshJwtToken(User_RefreshTokenCustom model)
        {
            var user_RefreshToken = new Data.Entity.User_RefreshToken();
            user_RefreshToken.Status = StatusModel.Active;
            user_RefreshToken.CreateDate = DateTime.Now;
            user_RefreshToken.UserId = model.UserId;
            user_RefreshToken.TokenValue = model.TokenValue;
            user_RefreshToken.ExpiryDate = model.ExpiryDate;
            await _db.InsterAsync(user_RefreshToken);
            await _db.SaveAsync();
        }

        public async Task<User_RefreshTokenCustom?> GetRefreshJwtToken(string refreshToken)
        {
            var user_RefreshToken = await _repo.Context.User_RefreshTokens.FirstOrDefaultAsync(x => x.TokenValue == refreshToken);
            if (user_RefreshToken == null) return null;

            var user_RefreshTokenMap = _mapper.Map<User_RefreshTokenCustom>(user_RefreshToken);
            return user_RefreshTokenMap;
        }

        public async Task RemoveRefreshJwtToken(string refreshToken)
        {
            var user_Login_TokenNotis = await _repo.Context.User_RefreshTokens.FirstOrDefaultAsync(x => x.TokenValue == refreshToken);
            if (user_Login_TokenNotis != null)
            {
                _db.Delete(user_Login_TokenNotis);
                await _db.SaveAsync();
            }
        }

        public async Task<RefreshTokenResponse> RefreshJwtToken(string refreshToken)
        {
            var response = await _repo.jwtUtils.RefreshJwtToken(refreshToken);

            return new RefreshTokenResponse() { access_token = response.AccessToken, refresh_token = response.RefreshToken };
        }

        public bool ExpireToken(string? token)
        {
            var valToken = _repo.jwtUtils.ValidateJwtToken(token);
            if (valToken == null) return false;

            return true;
        }

        public async Task RemoveNotiToken(User_Login_LogCustom model)
        {
            var user_Login_TokenNotis = await _repo.Context.User_Login_TokenNotis.FirstOrDefaultAsync(x => x.UserId == model.UserId && x.DeviceId == model.DeviceId);
            if (user_Login_TokenNotis != null)
            {
                _db.Delete(user_Login_TokenNotis);
                await _db.SaveAsync();
            }
        }

    }
}
