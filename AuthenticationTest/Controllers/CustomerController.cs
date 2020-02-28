using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace RayhanASPRestTest.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly OnlineStoreContext _context;

        public CustomerController(OnlineStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<object> allData = new List<object>();
            var data = _context.Customers;
            foreach (var x in data)
            {
                allData.Add(new { x.Id, x.Full_name, x.Username, x.Email, x.Phone_number });
            }
            return Ok(new { Message = "Success retreiving data", Status = true, Data = allData });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var data = _context.Customers.Find(id);

            if (data == null)
            {
                return NotFound(new { Message = "Customer not found", Status = false });
            }

            return Ok(new { Message = "Success retreiving data", Status = true, Data = data });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Customers.FindAsync(id);

            if (data == null)
            {
                return NotFound(new { Message = "Customer not found", Status = false });
            }

            _context.Customers.Remove(data);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        [HttpPost]
        public IActionResult Post(Customer data)
        {
            _context.Customers.Add(data);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Customer data)
        {
            var query = _context.Customers.Find(id);
            query.Full_name = data.Full_name;
            query.Username = data.Username;
            query.Email = data.Email;
            query.Phone_number = data.Phone_number;
            query.Updated_at = DateTime.Now;
            _context.SaveChanges();
            return NoContent();
        }
    }
}