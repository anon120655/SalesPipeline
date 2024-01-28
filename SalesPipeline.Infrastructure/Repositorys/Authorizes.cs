using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.Authorizes.Users;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class Authorizes : IAuthorizes
	{
		private IRepositoryWrapper _repo;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;
		private readonly IJwtUtils _jwtUtils;
		private readonly IMapper _mapper;

		public Authorizes(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IJwtUtils jwtUtils, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_appSet = appSet.Value;
			_jwtUtils = jwtUtils;
			_mapper = mapper;
		}

		public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
		{
			int expires_in = 1; //days

			//var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
			var user = _repo.Context.Users.SingleOrDefault(x => x.Email == model.Username);

			// return null if user not found
			if (user == null) throw new ExceptionCustom($"อีเมล์หรือรหัสผ่านของท่านไม่ถูกต้อง");

			if (user.PasswordHash == null) throw new ExceptionCustom("อีเมล์หรือรหัสผ่านของท่านไม่ถูกต้อง");

			try
			{
				const short maxLoginFail = 5;

				if (user.LoginFail >= maxLoginFail)				
					throw new ExceptionCustom($"ท่านถูกระงับการใช้งาน กรุณาติดต่อผู้ดูแลระบบ");				

				//string passwordHashGen = BCrypt.Net.BCrypt.EnhancedHashPassword("password", hashType: HashType.SHA384);
				bool verified = BCrypt.Net.BCrypt.EnhancedVerify(model.Password, user.PasswordHash, hashType: HashType.SHA384);
				if (!verified)
				{
					user.LoginFail = user.LoginFail.HasValue ? user.LoginFail += 1 : 1;
					_db.Update(user);
					await _db.SaveAsync();

					throw new ExceptionCustom($"อีเมล์หรือรหัสผ่านของท่านไม่ถูกต้อง ท่านกรอกผิดอีก {maxLoginFail - user.LoginFail} ครั้ง จะถูกระงับการใช้งาน");
				}
				else
				{
					user.LoginFail = null;
					_db.Update(user);
					await _db.SaveAsync();

					var userRole = await _repo.User.GetRoleByUserId(user.Id);
					if (userRole == null) throw new ExceptionCustom("user not role.");

					//
					if (userRole.Code.ToUpper().StartsWith(RoleCodes.RM))
					{
						if (!await _repo.Assignment.CheckAssignmentByUserId(user.Id))
						{
							var assignment = await _repo.Assignment.Create(new()
							{
								UserId = user.Id,
								EmployeeId = user.EmployeeId,
								EmployeeName = user.FullName,
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new ExceptionCustom(ex.Message);
			}

			var userMap = _mapper.Map<UserCustom>(user);

			// authentication successful so generate jwt token
			var token = _jwtUtils.GenerateJwtToken(userMap, expires_in);

			return new AuthenticateResponse(userMap, token, expires_in + "d");
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

		public bool ExpireToken(string? token)
		{
			var valToken = _jwtUtils.ValidateJwtToken(token);
			if (valToken == null) return false;

			return true;
		}

	}
}
