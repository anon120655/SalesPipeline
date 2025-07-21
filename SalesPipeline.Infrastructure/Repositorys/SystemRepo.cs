using AutoMapper;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.ManageSystems;
using Microsoft.EntityFrameworkCore;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Customers;
using NetTopologySuite.Index.HPRtree;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class SystemRepo : ISystemRepo
    {
        private IRepositoryWrapper _repo;
        private readonly IMapper _mapper;
        private readonly IRepositoryBase _db;
        private readonly AppSettings _appSet;

        public SystemRepo(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
        {
            _db = db;
            _repo = repo;
            _mapper = mapper;
            _appSet = appSet.Value;
        }

        public async Task<System_SignatureCustom> CreateSignature(System_SignatureCustom model)
        {
            using (var _transaction = _repo.BeginTransaction())
            {
                DateTime _dateNow = DateTime.Now;

                var systemSignature = new Data.Entity.System_Signature()
                {
                    Status = StatusModel.Active,
                    CreateDate = _dateNow,
                    CreateBy = model.CurrentUserId,
                };
                await _db.InsterAsync(systemSignature);
                await _db.SaveAsync();

                if (model.Files != null && model.Files.ImgBase64Only != null)
                {
                    if (GeneralUtils.IsBase64String(model.Files.ImgBase64Only))
                    {
                        model.Files.appSet = _appSet;
                        model.Files.Id = systemSignature.Id.ToString();
                        model.Files.Folder = "systems\\signature";
                        var uploadModel = GeneralUtils.UploadImage(model.Files);

                        systemSignature.Name = model.Files.FileName;
                        systemSignature.ImgUrl = uploadModel.ImageUrl;
                        systemSignature.ImgThumbnailUrl = uploadModel.ImageThumbnailUrl;
                        _db.Update(systemSignature);
                        await _db.SaveAsync();


                        var users = await _repo.Context.Users.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == model.CurrentUserId);
                        if (users != null)
                        {
                            users.UrlSignature = systemSignature.ImgUrl;
                            _db.Update(users);
                            await _db.SaveAsync();
                        }
                    }
                }

                _transaction.Commit();

                return _mapper.Map<System_SignatureCustom>(systemSignature);
            }
        }

        public async Task<System_SignatureCustom> GetSignatureLast(int userid)
        {
            var query = await _repo.Context.System_Signatures
                .OrderByDescending(o => o.CreateDate)
                .FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.CreateBy == userid);

            return _mapper.Map<System_SignatureCustom>(query);
        }

        public async Task<System_SLACustom> CreateSLA(System_SLACustom model)
        {
            using (var _transaction = _repo.BeginTransaction())
            {
                DateTime _dateNow = DateTime.Now;

                var systemSla = new Data.Entity.System_SLA()
                {
                    Status = StatusModel.Active,
                    CreateDate = _dateNow,
                    CreateBy = model.CurrentUserId,
                    UpdateDate = _dateNow,
                    UpdateBy = model.CurrentUserId,
                    Code = model.Code,
                    Name = model.Name,
                    Number = model.Number,
                };
                await _db.InsterAsync(systemSla);
                await _db.SaveAsync();

                _transaction.Commit();

                return _mapper.Map<System_SLACustom>(systemSla);
            }
        }

        public async Task<System_SLACustom> UpdateSLA(System_SLACustom model)
        {
            using (var _transaction = _repo.BeginTransaction())
            {
                var _dateNow = DateTime.Now;

                var systemSlas = await _repo.Context.System_SLAs.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (systemSlas != null)
                {
                    systemSlas.UpdateDate = _dateNow;
                    systemSlas.UpdateBy = model.CurrentUserId;
                    systemSlas.Number = model.Number;
                    _db.Update(systemSlas);
                    await _db.SaveAsync();

                    _transaction.Commit();
                }

                return _mapper.Map<System_SLACustom>(systemSlas);
            }
        }

        public async Task DeleteSLAById(UpdateModel model)
        {
            Guid id = Guid.Parse(model.id);
            var query = await _repo.Context.System_SLAs.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
            if (query != null)
            {
                query.UpdateDate = DateTime.Now;
                query.UpdateBy = model.userid;
                query.Status = StatusModel.Delete;
                _db.Update(query);
                await _db.SaveAsync();
            }
        }

        public async Task<System_SLACustom> GetSLAById(Guid id)
        {
            var query = await _repo.Context.System_SLAs
                .OrderByDescending(o => o.CreateDate)
                .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

            return _mapper.Map<System_SLACustom>(query);
        }

        public async Task<PaginationView<List<System_SLACustom>>> GetListSLA(allFilter model)
        {
            var query = _repo.Context.System_SLAs.Where(x => x.Status != StatusModel.Delete)
                                                 .OrderBy(x => x.CreateDate)
                                                 .AsQueryable();
            if (model.status.HasValue)
            {
                query = query.Where(x => x.Status == model.status);
            }

            var pager = new Pager(query.Count(), model.page, model.pagesize, null);

            var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            return new PaginationView<List<System_SLACustom>>()
            {
                Items = _mapper.Map<List<System_SLACustom>>(await items.ToListAsync()),
                Pager = pager
            };
        }

        public async Task<List<System_ConfigCustom>> GetConfig()
        {
            var query = _repo.Context.System_Configs.Where(x => x.Status != StatusModel.Delete)
                                                 .AsQueryable();
            return _mapper.Map<List<System_ConfigCustom>>(await query.ToListAsync());
        }

        public async Task<System_ConfigCustom?> GetConfigByCode(string code)
        {
            var system_Config = await _repo.Context.System_Configs.Where(x => x.Status == StatusModel.Active && x.Code == code).FirstOrDefaultAsync();
            if (system_Config != null)
            {
                return _mapper.Map<System_ConfigCustom>(system_Config);
            }
            return null;
        }

        public async Task UpdateConfig(List<System_ConfigCustom> model)
        {
            using (var _transaction = _repo.BeginTransaction())
            {
                foreach (var item in model)
                {
                    var system_Config = await _repo.Context.System_Configs.Where(x => x.Status == StatusModel.Active && x.Code == item.Code).FirstOrDefaultAsync();
                    if (system_Config != null)
                    {
                        system_Config.Value = item.Value;
                        _db.Update(system_Config);
                        await _db.SaveAsync();
                    }
                }

                _transaction.Commit();
            }
        }

        public async Task ClearDatabase(string code)
        {
            if (code != "Ibusiness02")
            {
                throw new Exception("code not match.");
            }

            var context = _repo.Context;

            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // ใส่ ExecuteSqlRawAsync ทั้งหมดที่นี่...
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Pre_Factor_Info");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Pre_Factor_Bus");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Pre_Factor_Stan");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Pre_Factor_App");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Pre_Result_Item");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Pre_Result");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Pre_Factor");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Notification");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Dash_Avg_Number");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Dash_Map_Thailand");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Dash_Pie");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Dash_Status_Total");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Assignment_RM_Sale");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Assignment_RM");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Assignment_Center");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Assignment_BranchReg");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Reply_Section_ItemValue");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Reply_Section_Item");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Reply_Section");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Reply");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Contact_History");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Contact_Info");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Close_Sale");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Duration");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Status_Total");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Activity");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Partner");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Deliver");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Result");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Document");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Document_Upload");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Meet");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Contact");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Return");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Phoenix");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale_Status");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Sale");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Customer_Shareholder");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Customer_Committee");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Customer");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM FileUpload");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM User_Target_Sale");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM User_Login_TokenNoti");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM User_Login_Log");

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


    }
}
