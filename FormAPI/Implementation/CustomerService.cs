using FormAPI.Data;
using FormAPI.Dto.Request;
using FormAPI.Dto.Response;
using FormAPI.Models;
using FormAPI.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormAPI.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly dbContext _context;
        public CustomerService(dbContext context)
        {
            _context = context;

        }
        Response response = new Response();

        public async Task<Response> Delete(int id)
        {
            try
            {
                var _customer = await _context.customers.FindAsync(id);
                if (_customer != null)
                {
                    _context.customers.Remove(_customer);
                    await _context.SaveChangesAsync();
                    response.Message = "user successfully deleted";
                    response.IsSuccess = true;

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "user not found";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public Task<Response> Login(LoginCustomer login)
        {
            throw new NotImplementedException();
        }


        public async Task<Response> Register(RegisterCustomer register)
        {

            try
            {
                bool checkIfCustomerExist = await _context.customers.AnyAsync(cus => cus.PhoneNo == register.PhoneNo);
                if (checkIfCustomerExist)
                {
                    response.Message = "Customer already exist";
                    return response;
                }

                Customer newCustomer = new Customer
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Address = register.Address,
                    Gender = register.Gender,
                    PhoneNo = register.PhoneNo,
                    Email = register.Email,
                    Password = PasswordHash.HashPassword(register.Password),
                    Username = register.Username

                };

                await _context.customers.AddAsync(newCustomer);
                await _context.SaveChangesAsync();
                response.IsSuccess = true;
                response.Message = "successfully register";
                response.Result = register.Email;



            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response> Update(RegisterCustomer data, int id)
        {
            try
            {
                var _customer = await _context.customers.FindAsync(id);
                if (_customer != null)
                {
                    _customer.FirstName = data.FirstName;
                    _customer.LastName = data.LastName;
                    _customer.Address = data.Address;
                    _customer.Gender = data.Gender;
                    _customer.Email = data.Email;
                    _customer.updatedTime = DateTime.Now;
                    await _context.SaveChangesAsync();

                    response.Message = "successfully updated";
                    response.Result = data.Email;
                    response.IsSuccess = true;

                }
                else
                {
                    response.Message = "user not found";
                    response.Result = data.Email;
                    response.IsSuccess = false;
                }

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<Response> GetAll()
        {
           //List<Customer> result = new List<Customer>();
            //var data = await _context.customers.ToListAsync();
            try
            {
                var data = await _context.customers.ToListAsync();
                List<Customer> result = new List<Customer>(data);
                if (data != null)
                {
                    response.Result = result;
                    response.Message = "successful";
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "no data found";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }


            return response;
        }

        public async Task<Response> GetSingle(int id)
        {
            try 
            {

                var _data = await _context.customers.SingleOrDefaultAsync(data => data.id == id);
                if (_data != null)
                {
                    response.Result = _data;
                    response.Message = "data found";
                    response.IsSuccess = true;
                }
                else 
                {
                    response.IsSuccess = false;
                    response.Message = $"data with id {id} not found";
                
                }
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message= ex.Message;
            }

            return response;
            
        }
    }
}
