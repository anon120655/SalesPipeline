using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace SalesPipeline.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobsController : ControllerBase
    {
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
