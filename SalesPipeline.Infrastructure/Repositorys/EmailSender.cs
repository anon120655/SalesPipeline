using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Email;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class EmailSender : IEmailSender
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;
		private readonly IHttpContextAccessor _accessor;

		public EmailSender(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper, IHttpContextAccessor accessor)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
			_accessor = accessor;
		}

		public async Task<SendMail_TemplateCustom> GetTemplate(string code)
		{
			var query = await _repo.Context.SendMail_Templates
				.Where(x => x.Status == StatusModel.Active && x.Code == code).FirstOrDefaultAsync();
			return _mapper.Map<SendMail_TemplateCustom>(query);
		}

		public async Task SendEmail(SendMailModel data)
		{
			ResourceEmail resource = new ResourceEmail();

			try
			{
				if (_appSet.EmailConfig != null && _appSet.EmailConfig.IsSentMail)
				{
					var _setting = _appSet.EmailConfig;
					resource.SenderName = _setting.SenderName;
					resource.Sender = _setting.Sender;
					resource.Password = _setting.Password;
					resource.MailServer = _setting.MailServer;
					resource.MailPort = _setting.MailPort;

					//indata
					resource.TemplateId = data.TemplateId;
					resource.Subject = data.Subject ?? String.Empty;
					resource.Email = data.Email ?? String.Empty;
					resource.Builder.HtmlBody = data.Body ?? String.Empty;
					resource.CcList = data.CcList;
					resource.CurrentUserId = data.CurrentUserId;

					var context = _accessor.HttpContext;

					if (!String.IsNullOrEmpty(_setting.DefualtMail))
					{
						resource.Email = _setting.DefualtMail;
					}

					if (context is not null)
					{
						if (context.Request.Host.Value.Contains("localhost"))
						{
							resource.Email = "arnon.w@ibusiness.co.th";
						}
					}

					var mimeMessage = new MimeKit.MimeMessage();

					mimeMessage.Body = resource.Builder.ToMessageBody();

					mimeMessage.From.Add(new MimeKit.MailboxAddress(resource.SenderName, resource.Sender));
					//mimeMessage.To.Add(new MailboxAddress("Mrs. Chanandler Bong", "chandler@friends.com")); EX.
					mimeMessage.To.Add(new MailboxAddress(resource.SenderName, resource.Email));
					mimeMessage.Subject = resource.Subject;

					if (_appSet.ServerSite.ToUpper() == ServerSites.UAT || _appSet.ServerSite.ToUpper() == ServerSites.PRO)
					{
						if (resource.CcList != null && resource.CcList.Count > 0)
						{
							foreach (string CCEmail in resource.CcList)
							{
								if (!String.IsNullOrEmpty(CCEmail) && CCEmail != resource.Email)
								{
									mimeMessage.Cc.Add(new MailboxAddress(resource.SenderName, CCEmail));
								}
							}
						}

						//mimeMessage.Cc.Add(new MailboxAddress(resource.SenderName, "arnon.w@ibusiness.co.th"));
					}

					using (var client = new SmtpClient())
					{
						// For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
						client.ServerCertificateValidationCallback = (s, c, h, e) => true;

						// The third parameter is useSSL (true if the client should make an SSL-wrapped
						// connection to the server; otherwise, false).
						await client.ConnectAsync(resource.MailServer, resource.MailPort, false);

						// Note: only needed if the SMTP server requires authentication
						await client.AuthenticateAsync(resource.Sender, resource.Password);

						await Task.Delay(200);
						await client.SendAsync(mimeMessage);

						await client.DisconnectAsync(true);

						resource.IsCompleted = true;
						resource.StatusMessage = "OK";
						await this.LogSendEmail(resource);
					}
				}
			}
			catch (Exception ex)
			{
				resource.IsCompleted = false;
				resource.StatusMessage = GeneralUtils.GetExMessage(ex);
				await this.LogSendEmail(resource);
			}
		}

		public async Task LogSendEmail(ResourceEmail data)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				string? emailCc = null;
				if (data.CcList != null && data.CcList.Count > 0)
				{
					string EmailCC = string.Join(",", data.CcList);
					emailCc = $"{emailCc} CC {EmailCC}";
				}

				SendMail_Log SendMaillog = new SendMail_Log()
				{
					CreateById = data.CurrentUserId,
					CreateDate = DateTime.Now,
					SendMail_TemplateId = data.TemplateId,
					EmailTo = data.Email,
					EmailToCc = emailCc,
					Subject = data.Subject ?? string.Empty,
					Message = data.Builder.HtmlBody,
					IsCompleted = data.IsCompleted,
					StatusMessage = data.StatusMessage,
				};
				await _db.InsterAsync(SendMaillog);
				await _db.SaveAsync();

				await _transaction.CommitAsync();
			}
		}

	}
}
