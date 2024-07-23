using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Services;
using Models.Services.Interface;

namespace ParametrizacionesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _service;
        private readonly IConfiguration _configuration;
        private readonly ConfigurationSectionWebApi _config;
        public TokenController(IConfiguration configuration, IOptions<ConfigurationSectionWebApi> config)
        {
            this._config = config.Value;
            _configuration = configuration;
            _service = new TokenService(_config);
        }

        /// <summary>
        /// Token authentication by credentials.
        /// </summary>
        /// <param name="filters">Filters to apply.</param>
        /// <returns></returns>
        /// <response code="200">Succes, retrieve the token.</response>
        /// <response code="404">No Content</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Authentication")]
        public IActionResult Authentication([FromForm] string User,[FromForm] string Password)
        {
            if (ValidateUser(User, Password))
            {
                var token = GenerateToken();
                return Ok(new { token = token });
            }

            return NotFound();
        }

        private bool ValidateUser(string User, string Password)
        {
            bool response = _service.Authentication(User, Password);
            return response;
        }

        private string GenerateToken()
        {
            // Header
            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "Andres Betancur"),
                new Claim(ClaimTypes.Email, "Andres@Andres.com"),
                new Claim(ClaimTypes.Role, "Administrador"),
            };

            // Payload 
            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(5) 
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
