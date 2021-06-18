using System.Linq;
using System.Threading.Tasks;
using AppleHardwareStore.Data;
using AppleHardwareStore.Models;
using Microsoft.EntityFrameworkCore;

namespace AppleHardwareStore.Initializers
{
    public class MigrationInitializer
    {

        private readonly AppleHardwareStoreDbContext _сontext;

        public MigrationInitializer(AppleHardwareStoreDbContext сontext)
        {
            _сontext = сontext;
        }
        public async Task Run()
        {
            if (!_сontext.ProductTypes.Any())
            {
                _сontext.ProductTypes.Add(new ProductType()
                {
                    Name = "Phone"
                });
                _сontext.ProductTypes.Add(new ProductType()
                {
                    Name = "Laptop"
                });
                await _сontext.SaveChangesAsync();
            }
            if (!_сontext.Products.Any())
            {
                _сontext.Products.Add(new Product()
                {
                    Name = "Iphone 4S",
                    Price = 15.500,
                    Description = "Used!"
                });
                _сontext.Products.Add(new Product()
                {
                    Name = "Iphone 8",
                    Description = "Brand new!",
                    Price = 30.000
                });
                await _сontext.SaveChangesAsync();
            }
            await _сontext.Database.MigrateAsync();
        }
    }
}