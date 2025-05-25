using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebScrapping.Application.Interfaces;
using WebScrapping.Dto.Users;
using WebScrapping.Utils;

namespace WebScrapping.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IHomeApplication _homeApplication;

        public HomeController(IHomeApplication homeApplication)
        {
            _homeApplication = homeApplication;
        }

        [HttpPost("api/Login")]
        public IActionResult Login([FromBody] UserDto user)
        {
            bool success = _homeApplication.Login(user);

            if(success)
            {
                return new JsonResult(new { jwt = _homeApplication.CreateToken(user) });
            }

            return Unauthorized(new { error = Constants.Messages.ERROR_LOGIN });
        }

        [HttpGet("api/Check")]
        [Authorize]
        public IActionResult Check()
        {
            return new JsonResult(new { authorized = true });
        }

    }
}
