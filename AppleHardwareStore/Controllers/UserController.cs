using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppleHardwareStore.DTO;
using AppleHardwareStore.Data;
using AppleHardwareStore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FurnitureFactory.Initializers;

namespace FurnitureFactory.Controllers
{
    [Route("api/users")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly AppleHardwareStoreDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserController(AppleHardwareStoreDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var result = new List<UserDto>();
            foreach (var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                result.Add(new UserDto(user, role.First()));
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return NotFound();
            if (_userManager.GetRolesAsync(user).GetAwaiter().GetResult().Contains(Rolse.Admin)) return BadRequest("Нельзя удалить админа");
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
