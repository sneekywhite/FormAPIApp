﻿using AutoMapper;
using FormAPI.Data;
using FormAPI.Dto.Request;
using FormAPI.Dto.Response;
using FormAPI.Models;
using FormAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormAPI.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly dbContext _context;
        private readonly Response response;
        private readonly IMapper _mapper;
        
        public CustomerService(dbContext context, IMapper mapper)
        {
            _context = context;
            response = new Response();
            _mapper = mapper;
           
        }
       // Response response = new Response();

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

                Customer newCustomer = _mapper.Map<Customer>(register);
                await _context.customers.AddAsync(newCustomer);
                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = "successfully register";
                response.Result = register.FirstName;
                

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response> Update(UpdateCustomer _data, int id)
        {
            try
            {
                var _customer = await _context.customers.FindAsync(id);
                if (_customer != null)
                {
                    _customer.FirstName = _data.FirstName;
                    _customer.LastName = _data.LastName;
                    _customer.Address = _data.Address;
                    _customer.Gender = _data.Gender;
                    _customer.Email = _data.Email;
                    _customer.Username = _data.Username;
                    _customer.Password = PasswordHash.HashPassword(_data.Password);
                    _customer.updatedTime = DateTime.Now;
                    await _context.SaveChangesAsync();

                    response.Message = "successfully updated";
                    response.Result = _customer.Email;
                    response.IsSuccess = true;

                }
                else
                {
                    response.Message = $"user with id {id} not found";
                    response.Result = null;
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
                IEnumerable<Customer> data = await _context.customers.ToListAsync(); //Select(x=>new Modal() { FirstName=x.FirstName,LastName=x.LastName,Gender=x.Gender,Address=x.Address,PhoneNo=x.PhoneNo,Email=x.Email,Username=x.Username});
                if (data != null)
                {
                    response.Result = _mapper.Map<IEnumerable<Modal>>(data);
                    response.Message = "successful";
                    response.IsSuccess = true;
                    
                }
                else
                {
                    response.Result = null;
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
                    response.Result = _mapper.Map<Modal>(_data);
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
