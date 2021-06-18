using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppleHardwareStore.DTO;

namespace AppleHardwareStore.Models.Input
{
    public class UserInput : LoginDto
    {
        public string ClientName { get; set; }
    }
}
