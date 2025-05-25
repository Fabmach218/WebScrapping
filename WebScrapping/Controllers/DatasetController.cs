using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebScrapping.Application.Interfaces;
using WebScrapping.Dto.Users;
using WebScrapping.Utils;

namespace WebScrapping.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class DatasetController : ControllerBase
    {
        private readonly IDatasetApplication _datasetApplication;

        public DatasetController(IDatasetApplication datasetApplication)
        {
            _datasetApplication = datasetApplication;
        }

        [HttpGet]
        public IActionResult GetDatasets()
        {
            return new JsonResult(_datasetApplication.GetDatasets());
        }

    }
}
