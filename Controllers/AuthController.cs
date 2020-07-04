using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using taskmanager_api.Models;

namespace taskmanager_api.Controllers {
    [ApiController]
    [Route ("/api/v1.0/[controller]")]
    public class AuthController : ControllerBase {

        private readonly ILogger<AuthController> _logger;
        private readonly TaskdbContext _context;
        public IConfiguration Configuration { get; }
        public AuthController (ILogger<AuthController> logger, TaskdbContext context, IConfiguration configuration) {
            _logger = logger;
            this._context = context;
            Configuration = configuration;
        }

        [HttpPost ("login")]
        public ActionResult Login ([FromBody] Login login) {
            Users user = this._context.Users.FirstOrDefault (element => element.Email == login.UserName && element.Password == login.Password);
            if (user == null) {
                return NotFound ();
            } else {
                return Ok (new { token = GenerarTokenJWT (user) });
            }
        }
        // GENERAMOS EL TOKEN CON LA INFORMACIÓN DEL USUARIO
        private string GenerarTokenJWT (Users user) {
            // CREAMOS EL HEADER //

            this._logger.LogWarning ("id {0} not found", Configuration["JWT:ClaveSecreta"]);

            var _symmetricSecurityKey = new SymmetricSecurityKey (
                Encoding.UTF8.GetBytes (Configuration["JWT:ClaveSecreta"])
            );
            var _signingCredentials = new SigningCredentials (
                _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
            );
            var _Header = new JwtHeader (_signingCredentials);

            // CREAMOS LOS CLAIMS //
            var _Claims = new [] {
                new Claim ("email", user.Email),
                new Claim (JwtRegisteredClaimNames.NameId, user.Id.ToString ()),
                new Claim ("role", user.Role)
            };

            // CREAMOS EL PAYLOAD //
            var _Payload = new JwtPayload (
                issuer: Configuration["JWT:Issuer"],
                audience : Configuration["JWT:Audience"],
                claims : _Claims,
                notBefore : DateTime.UtcNow,
                // Exipra a la 24 horas.
                expires : DateTime.UtcNow.AddHours (24)
            );

            // GENERAMOS EL TOKEN //
            var _Token = new JwtSecurityToken (
                _Header,
                _Payload
            );

            return new JwtSecurityTokenHandler ().WriteToken (_Token);
        }
    }
}