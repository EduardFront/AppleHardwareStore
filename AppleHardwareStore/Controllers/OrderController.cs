using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppleHardwareStore.Data;
using AppleHardwareStore.Interfaces;
using AppleHardwareStore.Models;
using AutoMapper;
using FurnitureFactory.Initializers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleHardwareStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly AppleHardwareStoreDbContext _context;
        private readonly IAsyncRepository<Order> _repository;
        private readonly IAsyncRepository<Position> _positionRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public OrderController(AppleHardwareStoreDbContext context, IAsyncRepository<Order> repository,
            IMapper mapper, UserManager<User> userManager, IAsyncRepository<Position> positionRepository)
        {
            _positionRepository = positionRepository;
            _userManager = userManager;
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Test
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Order>>> GetList()
        {
            if (User.IsInRole(Rolse.Admin))
            {
                return _context.Orders.Include(u => u.User)
                    .Include(o => o.Positions)
                    .ThenInclude(o => o.Product)
                    .ToList();
            }

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            return _context.Orders.Include(u => u.User).Include(o => o.Positions)
                .ThenInclude(o => o.Product).Where(o => o.ClientId == user.Id).ToList();
        }


        // GET: api/Test/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpGet("history/{userId:int?}")]
        [Authorize(Roles = Rolse.Admin)]
        public async Task<List<Order>> HistoryForAdmin(int userId)
        {
            return await _context.Orders.Include(o => o.User)
                .Include(o => o.Positions)
                .ThenInclude(s => s.Product)
                .Where(o => o.ClientId == userId)
                .ToListAsync();
        }

        [HttpGet("history")]
        public async Task<List<Order>> History()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            return await _context.Orders.Include(o => o.User)
                .Include(o => o.Positions)
                .ThenInclude(s => s.Product)
                .Where(o => o.ClientId == user.Id)
                .ToListAsync();
        }

        // PUT: api/Test/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Test
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null)
            {
                return BadRequest();
            }

            await _repository.DeleteAsync(product);

            return Ok();
        }

        // DELETE: api/Test/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}