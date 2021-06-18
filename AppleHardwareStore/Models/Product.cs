using System.Collections.Generic;

namespace AppleHardwareStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }


        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }

        public ICollection<Position> Positions { get; set; }
    }
}