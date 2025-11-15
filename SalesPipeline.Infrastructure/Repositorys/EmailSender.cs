using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Email;
using SalesPipeline.Utils.Resources.Shares;
using System.Net.Security;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class EmailSender : IEmailSender
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IMapper _mapper;
        private readonly IRepositoryBase _db;
        private readonly AppSettings _appSet;
        private readonly IHttpContextAccessor _accessor;
        private readonly bool isDevOrUat = false;

        public EmailSender(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper, IHttpContextAccessor accessor)
        {
            _db = db;
            _repo = repo;
            _mapper = mapper;
            _appSet = appSet.Value;
            _accessor = accessor;
            isDevOrUat = _appSet.ServerSite == ServerSites.DEV || _appSet.ServerSite == ServerSites.UAT;
        }

        public async Task<SendMail_TemplateCustom> GetTemplate(string code)
        {
            var query = await _repo.Context.SendMail_Templates
                .Where(x => x.Status == StatusModel.Active && x.Code == code).FirstOrDefaultAsync();
            return _mapper.Map<SendMail_TemplateCustom>(query);
        }

        public async Task SendEmail(SendMailModel indata)
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
                    resource.TemplateId = indata.TemplateId;
                    resource.Subject = indata.Subject ?? String.Empty;
                    resource.Email = indata.Email ?? String.Empty;
                    resource.Builder.HtmlBody = indata.Body ?? String.Empty;
                    resource.CcList = indata.CcList;
                    resource.CurrentUserId = indata.CurrentUserId;

                    var context = _accessor.HttpContext;

                    if (!String.IsNullOrEmpty(_setting.DefualtMail))
                    {
                        resource.Email = _setting.DefualtMail;
                    }

                    if (!String.IsNullOrEmpty(_setting.DefualtMailCc))
                    {
                        List<string> lists = _setting.DefualtMailCc.Split(',').ToList<string>();
                        if (lists.Count > 0)
                        {
                            resource.CcList = new();
                            foreach (var item in lists)
                            {
                                resource.CcList.Add(item);
                            }
                        }
                    }

                    if (context is not null && context.Request.Host.Value.Contains("localhost") && !resource.Email.Contains("ibusiness.co.th"))
                    {
                        resource.Email = "arnon.w@ibusiness.co.th";
                    }


                    var mimeMessage = new MimeKit.MimeMessage();

                    mimeMessage.Body = resource.Builder.ToMessageBody();

                    mimeMessage.From.Add(new MimeKit.MailboxAddress(resource.SenderName, resource.Sender));

                    mimeMessage.To.Add(new MailboxAddress(resource.SenderName, resource.Email));
                    mimeMessage.Subject = resource.Subject;

                    if (_appSet.EmailConfig.IsSentMailCc)
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

                    }

                    using (var client = new SmtpClient())
                    {
                        if (isDevOrUat)
                        {
                            client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                            {
                                // ยอมรับเฉพาะกรณีที่ไม่มี error
                                if (sslPolicyErrors == SslPolicyErrors.None)
                                    return true;

                                // ยอมรับเฉพาะ self-signed certificate error
                                // (ไม่ยอมรับ error อื่นๆ เช่น hostname mismatch, expired cert)
                                if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
                                {
                                    // อาจเพิ่มการตรวจสอบ certificate thumbprint ด้วย
                                    return true;
                                }

                                return false;
                            };
                        }

                        await client.ConnectAsync(resource.MailServer, resource.MailPort, SecureSocketOptions.Auto);

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
        }

        public async Task SendNewUser(int? id)
        {
            var user = await _repo.User.GetNewUserSendMail(id);
            if (user != null && user.Count > 0)
            {
                var template = await _repo.EmailSender.GetTemplate(EmailTemplateModel.NEWUSER);
                if (template == null) throw new ExceptionCustom("template not found.");

                foreach (var item in user)
                {
                    var userSendMail = await _repo.User.UpdateNewUserSendMail(item.Id);
                    if (userSendMail != null)
                    {
                        string messageBody = string.Format(template.Message,
                                                    item.FullName,
                                                    GeneralUtils.getFullThaiFullShot(item.CreateDate),
                                                    GeneralUtils.DateToTimeString(item.CreateDate),
                                                    userSendMail.DefaultPassword,
                                                    $"{_appSet.baseUriWeb}/changepassword");

                        await _repo.EmailSender.SendEmail(new()
                        {
                            CurrentUserId = item.CurrentUserId,
                            TemplateId = template.Id,
                            Email = item.Email,
                            Subject = $"{template.Subject} {item.FullName}",
                            Body = messageBody
                        });
                    }
                }
            }
        }

    }
}
