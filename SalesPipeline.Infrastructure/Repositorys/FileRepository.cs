using AutoMapper;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Cmp;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class FileRepository : IFileRepository
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public FileRepository(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<FormFileResponse?> UploadFormFile(FormFileUpload model)
		{
			var response = new FormFileResponse();

			var Id = Guid.NewGuid();

			if (model.FileData != null)
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					var extension = Path.GetExtension(model.FileData.FileName);
					response.FileId = Id;
					var _upload = await GeneralUtils.UploadFormFile(new FileModel()
					{
						appSet = _appSet,
						FileData = model.FileData,
						Folder = model.Folder,
						Id = Id.ToString(),
						MimeType = extension
					});

					if (_upload == null) throw new ExceptionCustom("upload file error");

					response.FileUrl = _upload.UploadUrl;


					DateTime _dateNow = DateTime.Now;

					var fileUpload = new Data.Entity.FileUpload()
					{
						Id = Id,
						Status = StatusModel.Active,
						CreateDate = _dateNow,
						CreateBy = 0,
						Url = response.FileUrl,
						OriginalFileName = model.FileData.FileName,
						FileName = Id.ToString(),
						FileSize = (int)model.FileData.Length,
						MimeType = extension
					};
					await _db.InsterAsync(fileUpload);
					await _db.SaveAsync();

					response.OriginalFileName = fileUpload.OriginalFileName;

					_transaction.Commit();


				}
			}

			return response;

		}

	}
}
