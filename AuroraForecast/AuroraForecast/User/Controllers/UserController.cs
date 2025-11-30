using AuroraForecast.User.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuroraForecast.User.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public Models.User Get(int id)
        {
            return _userService.GetUserById(id);
        }
    }
}
