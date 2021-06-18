using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AppleHardwareStore.DTO;
using AppleHardwareStore.DTO.Product;
using AppleHardwareStore.Models;
using System.Reflection;

namespace AppleHardwareStore
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
