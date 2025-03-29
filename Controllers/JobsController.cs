using Microsoft.AspNetCore.Mvc;
using GetJobsBackend.Data;
using GetJobsBackend.DTO;
using GetJobsBackend.Models;

namespace GetJobsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetJobs([FromQuery] string domain)
        {
            var jobs = string.IsNullOrEmpty(domain) 
                ? _context.Jobs.ToList() 
                : _context.Jobs.Where(j => j.Domain.ToLower() == domain.ToLower()).ToList();

            return Ok(jobs);
        }

        [HttpPost("apply")]
        public IActionResult Apply(JobApplicationDTO application)
        {
            return Ok($"Application received for Job ID {application.JobId} from {application.FullName}");
        }
    }
}
