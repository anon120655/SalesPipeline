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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResultCustom))]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResultCustom))]
        public IActionResult FireAndForget()
        {
            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget job executed"));
                return Ok("Fire-and-forget job enqueued");
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpGet("delayed")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResultCustom))]
        public IActionResult Delayed()
        {
            try
            {
                BackgroundJob.Schedule(() => Console.WriteLine("Delayed job executed"), TimeSpan.FromMinutes(1));
                return Ok("Delayed job enqueued");
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpGet("recurring")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResultCustom))]
        public IActionResult Recurring()
        {
            try
            {
                RecurringJob.AddOrUpdate(
                    recurringJobId: "RecurringJobExecuted",
                    methodCall: () => Console.WriteLine("Recurring job executed"),
                    cronExpression: Cron.Daily,
                    options: new RecurringJobOptions()
                );
                return Ok("Recurring job enqueued");
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpGet("continuation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResultCustom))]
        public IActionResult Continuation()
        {
            try
            {
                var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Initial job executed"));
                BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Continuation job executed"));
                return Ok("Continuation job enqueued");
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

    }
}
