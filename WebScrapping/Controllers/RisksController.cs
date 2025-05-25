using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebScrapping.Application.Interfaces;

namespace WebScrapping.Controllers
{
    [EnableRateLimiting("FixedWindowPolicy")]
    [Route("api/[controller]")]
    [Authorize]
    public class RisksController : ControllerBase
    {
        private readonly IRisksApplication _risksApplication;

        public RisksController(IRisksApplication risksApplication)
        {
            _risksApplication = risksApplication;
        }

        [HttpGet("OffshoreLeaks")]
        public async Task<IActionResult> GetOffshoreLeaksData(string q)
        {
            try
            {
                var response = await _risksApplication.GetOffshoreLeaksData(q);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("WorldBankDebarredFirms")]
        public async Task<IActionResult> GetWorldBankDebarredFirmsData(string q)
        {
            try
            {
                var response = await _risksApplication.GetWorldBankDebarredFirmsData(q);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("OFACSanctions")]
        public async Task<IActionResult> GetOFACSanctionsData(string q)
        {
            try
            {
                var response = await _risksApplication.GetOFACSanctionsData(q);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("Screening")]
        public async Task<IActionResult> GetRisksScreening(string dbs, string q)
        {
            try
            {
                var response = await _risksApplication.GetRisksScreening(dbs, q);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

    }
}
