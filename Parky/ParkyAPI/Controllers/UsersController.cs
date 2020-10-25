using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Authorize] 
    [Route("api/v{version:apiversion}/Users")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate ([FromBody] AuthenticationModel model)
        {
            var user = _userRepo.Authenticate(model.UserName, model.Password);

            if(user == null)
            {
                return BadRequest(new { message = "UserName or password is incorrect" });
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthenticationModel model)
        {
            bool ifUserUnique = _userRepo.isUniqueUser(model.UserName);

            if (!ifUserUnique)
            {
                return BadRequest(new { message = "UserName already exist" });
            }

            var user = _userRepo.Register(model.UserName, model.Password);

            if(user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }

            return Ok(user);
        }
    }
}
