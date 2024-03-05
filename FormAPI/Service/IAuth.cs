using FormAPI.Dto.Request;
using FormAPI.Dto.Response;

namespace FormAPI.Service
{
    public interface IAuth
    {
        public Task<Response> Token(LoginCustomer cred);
    }
}
