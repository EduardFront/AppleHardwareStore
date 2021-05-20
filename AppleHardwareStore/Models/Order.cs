using System.Collections.Generic;

namespace AppleHardwareStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string ClientAddress { get; set; }
        public string ClientCardNumber { get; set; }

        public double TotalCost { get; set; }
        
        public int OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }

        public ICollection<Position> Positions { get; set; }
    }
}