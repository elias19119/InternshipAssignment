namespace ImageSourcesStorage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    [ApiController]
    public class CredentialsController : ControllerBase
    {
        private readonly ICredentials credentails;
        private readonly IConfiguration configuration;

        public CredentialsController(ICredentials credentails , IConfiguration configuration)
        {
            this.credentails = credentails;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var response = new Dictionary<string, string>();

            var result = this.credentails.IsCredentailsValid(request.Username, request.Password);

            if (!result.Result)
            {
                response.Add("Error", "Invalid username or password");
                return this.BadRequest(response);
            }

            var roles = new string[] { "Role1", "Role2" };
            var token = this.GenerateJwtToken(request.Username, roles.ToList());

            return this.Ok(new LoginResponse()
            {
                Access_Token = token,
                UserName = request.Username,
            });
        }

        private string GenerateJwtToken(string username, List<string> roles)
        {
            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, username),
            };

            roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(this.configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                this.configuration["JwtIssuer"],
                this.configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
