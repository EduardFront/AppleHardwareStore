namespace AppleHardwareStore.Models
{
    public class Position
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Product count
        /// </summary>
        public int Count { get; set; }

        public double Cost { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
