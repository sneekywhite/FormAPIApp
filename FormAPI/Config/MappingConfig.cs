using AutoMapper;
using FormAPI.Dto.Request;
using FormAPI.Dto.Response;
using FormAPI.Models;
using FormAPI.Service;

namespace FormAPI.Config
{
    public class MappingConfig
    {
        RegisterCustomer register = new RegisterCustomer();
        Customer customer = new Customer();
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMap<Modal, Customer>();
                config.CreateMap<Customer, Modal>();
                config.CreateMap<RegisterCustomer, Customer>().AfterMap((register, customer) =>
                {
                    customer.Password = PasswordHash.HashPassword(register.Password);
                  
                });
                

            });
            return mappingConfiguration;
        }
    }
}
