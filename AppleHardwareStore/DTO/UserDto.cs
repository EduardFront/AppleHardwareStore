using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppleHardwareStore.Models;

namespace AppleHardwareStore.DTO
{
    public class UserDto
    {
        public UserDto(User user, string role)
        {
            Id = user.Id;
            ClientName = user.UserName;
            Email = user.Email;
            Role = role;
        }

        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
