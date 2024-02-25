using FormAPI.Dto.Request;
using FormAPI.Dto.Response;
using FormAPI.Models;

namespace FormAPI.Service
{
    public interface ICustomerService
    {
        public Task<Response> Register(RegisterCustomer register);
        public Task<Response> Login(LoginCustomer login);
        public Task<Response> Update(RegisterCustomer register, int id);
        public Task<Response> Delete(int id);
        public Task<Response> GetAll();
        public Task<Response> GetSingle(int id);




    }
}
