using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AppleHardwareStore.Models
{
    public class User:IdentityUser<int>
    {
        public string Name { get; set; }
        public string ClientPhone { get; set; }
        public string ClientAddress { get; set; }
        public string ClientCardNumber { get; set; }

        public List<Order> Orders { get; set; }

    }
}
