using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppleHardwareStore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppleHardwareStore.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AppleHardwareStore.Interfaces;
using AutoMapper;
using FurnitureFactory.Initializers;
using FurnitureFactory;
using AppleHardwareStore.DTO.Product;

namespace AppleHardwareStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ControllerBase
    {
        private readonly AppleHardwareStoreDbContext _context;
        private readonly IAsyncRepository<Product> _repository;
        private readonly IMapper _mapper;

        public ProductController(AppleHardwareStoreDbContext context, IAsyncRepository<Product> repository,
            IMapper mapper)
        {
            _repository = repository;
            _context = context;
           _mapper = mapper;
        }

        
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _context.Products.ToListAsync();
        }

        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(product);
        }

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostPerson(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public ActionResult<List<Product>> GetList() => _context.Products.ToList();

        [HttpPost("")]
        [Authorize(Roles = Rolse.Admin)]
        public async Task<ActionResult> Post([FromForm] CreateProductDTO createProductDTO)
        {
            var fileName = createProductDTO.Image.GetHashFileName();
            await FileHelper.CreateFile(new[] { "upload", "module" }, fileName, createProductDTO.Image);

            var product = new Product
            {
                Name = createProductDTO.Name,
                Price = createProductDTO.Price,
                Image = fileName,
                Description = createProductDTO.Description
            };

            try
            {
                await _repository.AddAsync(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Rolse.Admin)]
        public async Task<ActionResult> Put([FromForm] EditProductDTO editProductDTO, int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null)
            {
                return BadRequest();
            }

            if (editProductDTO.Image is not null)
            {
                var fileName = editProductDTO.Image.GetHashFileName();
                if (product.Image is not null)
                    await FileHelper.DeleteFile(new[] { "upload", "product" }, product.Image);
                await FileHelper.CreateFile(new[] { "upload", "product" }, fileName, editProductDTO.Image);
                product.Image = fileName;
            }

            product.Name = editProductDTO.Name;
            product.Price = editProductDTO.Price;
            product.Description = editProductDTO.Description;
            await _repository.UpdateAsync(product);
            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var module = await _repository.GetByIdAsync(id);
            if (module is null)
            {
                return NotFound();
            }

            module.Image = $"{Request.Scheme}://{Request.Host}/img/{module.Image}";
            return Ok(module);
        }

        [Authorize(Roles = Rolse.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product is null)
            {
                return BadRequest();
            }
            await FileHelper.DeleteFile(new[] { "upload", "product" }, product.Image);


            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}