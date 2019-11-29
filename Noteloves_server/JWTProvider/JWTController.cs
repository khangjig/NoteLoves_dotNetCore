using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Noteloves_server.Helpers;
using Noteloves_server.JWTProvider.Services;
using Noteloves_server.Messages.Requests.JWT;
using Noteloves_server.Messages.Responses;
using Noteloves_server.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.JWTProvider
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly AppSettings _appSettings;
        private IJWTService _jWTService;
        private IUserService _userService;

        public JWTController(ILogger<JWTController> logger, IJWTService jWTService, IOptions<AppSettings> appSettings,IUserService userService)
        {
            _logger = logger;
            _jWTService = jWTService;
            _appSettings = appSettings.Value;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/auth/token")]
        public IActionResult Authenticate([FromBody] LoginForm login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_jWTService.CheckAccount(new LoginForm(login.email, _userService.EncodePassword(login.password))))
            {
                return NotFound(new Response("404", "Email or password is not correct!"));
            }

            var id = _userService.GetIdByEmail(login.email);
            var AccessToken = _jWTService.GenerateToken(id, login.email);
            var RefreshToken = _jWTService.GenerateRefreshToken();

            _userService.UpdateRefreshToken(_userService.GetIdByEmail(login.email), RefreshToken);

            return Ok(new LoginRespone(AccessToken, RefreshToken));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/auth/refreshtoken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var email = _jWTService.GetEmailByToken(refreshTokenRequest.AccessToken);
            var id = _jWTService.GetIdByToken(refreshTokenRequest.AccessToken);

            var savedRefreshToken = _userService.GetRefreshToken(email);
            if (savedRefreshToken != refreshTokenRequest.RefreshToken)
                return BadRequest(new Response("400", "Invalid refresh token"));

            var newAccessToken = _jWTService.GenerateToken(id, email);
            var newRefreshToken = _jWTService.GenerateRefreshToken();
            _userService.UpdateRefreshToken(_userService.GetIdByEmail(email), newRefreshToken);

            return Ok(new LoginRespone(newAccessToken, newRefreshToken));
        }
    }
}
