using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pract.Requests;
using Pract.Services;

namespace Pract.Controllers
{
    [Authorize]
    [Route("/user")]
    public class UserController: Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            try
            {
                var user = await _userService.GetUser(id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetUsers();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] UserRequest userRequest)
        //{
        //    try
        //    {
        //        var user = await _userService.CreateUser(userRequest);

        //        return Ok(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPatch("{id:long}")]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] UserRequest userRequest)
        {
            try
            {
                var user = await _userService.UpdateUser(id, userRequest);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        //[HttpDelete("{id:long}")]
        //public async Task<IActionResult> Delete([FromRoute] long id)
        //{
        //    try
        //    {
        //        var user = await _userService.DeleteUser(id);

        //        return Ok(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
