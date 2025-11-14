using Asp.Versioning;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;
using System.Net.Http;

namespace SalesPipeline.API.Controllers
{
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class JobsController : ControllerBase
	{
		private readonly DatabaseBackupService _databaseBackup;

		public JobsController(DatabaseBackupService databaseBackup)
		{
			_databaseBackup = databaseBackup;
		}

		[HttpGet("BackupDatabase")]
		public IActionResult BackupDatabase()
		{
			try
			{
				_databaseBackup.BackupDatabase();

				return Ok("Backup completed successfully.");
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("fire-and-forget")]
        public IActionResult FireAndForget()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget job executed"));
            return Ok("Fire-and-forget job enqueued");
        }

        [HttpGet("delayed")]
        public IActionResult Delayed()
        {
            BackgroundJob.Schedule(() => Console.WriteLine("Delayed job executed"), TimeSpan.FromMinutes(1));
            return Ok("Delayed job enqueued");
        }

        [HttpGet("recurring")]
        public IActionResult Recurring()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring job executed"), Cron.Daily);
            return Ok("Recurring job enqueued");
        }

        [HttpGet("continuation")]
        public IActionResult Continuation()
        {
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Initial job executed"));
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Continuation job executed"));
            return Ok("Continuation job enqueued");
        }
    }
}
