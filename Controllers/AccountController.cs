using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pract.Requests;
using Pract.Services;
using System.Security.Claims;

namespace Pract.Controllers
{
    [Route("/auth")]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountRequest accountRequest)
        {
            try
            {
                var user = await _accountService.Authenticate(accountRequest);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRequest accountRequest)
        {
            try
            {
                var user = await _accountService.CreateAccount(accountRequest);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // получаем идентификатор пользователя из Claims
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);

            // удаляем информацию аутентификации из базы данных или хранилища
            _accountService.RemoveRefreshTokens(userId);

            return Ok();
        }

        //[Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            // получаем идентификатор пользователя из Claims
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);

            // удаляем информацию аутентификации из базы данных или хранилища
            var account = await _accountService.GetAccount(userId);

            return Ok(account);
        }
    }
}
