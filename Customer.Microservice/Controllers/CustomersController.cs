using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Customer.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private static List<Customer> _customers = new List<Customer>
        {
            new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
            new Customer { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" },
            new Customer { Id = 3, FirstName = "Michael", LastName = "Johnson", Email = "michael@example.com" }
        };

        [HttpGet]
        public IActionResult GetCustomers()
        {
            return Ok(_customers);
        }
    }
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
