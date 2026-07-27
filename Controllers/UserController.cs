using Microsoft.AspNetCore.Mvc;
using PoseDatabaseWebApi.Service;
using PoseDatabaseWebApi.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using PoseDatabaseWebApi.Data.Dto.Identity.Users;

namespace PoseDatabaseWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(UserManager<AppUserModel> _userManager) : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestDto  userData)
        {
            var response = await _userManager.CreateAsync(new AppUserModel()
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Email = userData.Email,
                UserName = userData.Email

            }, userData.Password);

            if (!response.Succeeded)
            {
                return BadRequest(new {response.Errors });
            }

            //return Ok();
            return StatusCode(StatusCodes.Status201Created, new { email = userData.Email });
        }

        [Authorize]
        [HttpGet]
        [Route("current_user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Pass specific, safe identity info to front-end
            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.UserName
            });
        }
    }
}
