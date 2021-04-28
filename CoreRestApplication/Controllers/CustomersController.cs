﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreRestApplication.Core.Data;
using CoreRestApplication.Core.Data.Model;
using CoreRestApplication.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CoreRestApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository CustomerRepository;
        
        public CustomersController(ICustomerRepository customerRepository)
        {
            CustomerRepository = customerRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                IEnumerable<CustomerDto> customers = CustomerRepository.GetCustomers();
                if (customers.Any())
                    return Ok(customers);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                var customer = CustomerRepository.GetById(id);
                if (customer != null)
                    return Ok(customer);
                    
                return NotFound("The id provided is not associated with any registered user");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerModel>> Post(CustomerDto newCustomer)
        {
            try
            {
                CustomerDto registeredCustomer = CustomerRepository.RegisterNewCustomer(newCustomer);
                if (registeredCustomer == null)
                    return Conflict("Id already associated with a registered user");

                return Created($"api/Customers/{registeredCustomer.Id}", registeredCustomer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }
        
        [HttpPut]
        public async Task<ActionResult<CustomerModel>> Put(CustomerDto customerDto)
        {
            try
            {
                var updatedCustomer = CustomerRepository.UpdateCustomerData(customerDto);
                if (updatedCustomer == null)
                    return NotFound("User not found");
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerModel>> Delete(int id)
        {
            try
            {
                var deletedCustomer = CustomerRepository.DeleteCustomer(id);
                if (deletedCustomer == null) 
                    return NotFound();
                
                return Ok(deletedCustomer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }
    }
}
