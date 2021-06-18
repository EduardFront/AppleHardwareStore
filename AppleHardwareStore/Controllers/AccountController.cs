using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AppleHardwareStore.Data;
using AppleHardwareStore.DTO;
using FurnitureFactory.Initializers;
using AppleHardwareStore.Models;
using AppleHardwareStore.Models.Input;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AppleHardwareStore.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppleHardwareStoreDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(AppleHardwareStoreDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register(UserInput model)
        {
            User user = new User()
            {
                Email = model.Email,
                UserName = model.ClientName
            };
            // добавляем пользователя
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Rolse.User);
                    var token = await Token(model);
                    return token;
                }

                return BadRequest(result.Errors);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Token([FromBody] LoginDto request)
        {
            var identity = await GetIdentity(request.Email, request.Password);
            if (identity == null)
            {
                return BadRequest(new
                {
                    errorText = "Invalid username or password."
                });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: Auth.ISSUER,
                audience: Auth.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(Auth.LIFETIME)),
                signingCredentials: new SigningCredentials(Auth.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                accessToken = encodedJwt,
                role = identity.Claims.First(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).Value
            };
            return new JsonResult(response);
        }

        private async Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            // добавляем пользователя
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (user != null && result)
            {
                var role = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role.First()),
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims,
                        "Token",
                        ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}