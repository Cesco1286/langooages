using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using langooages.Models;
using System.Linq;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserContext _context;
        public UserController(UserContext context)
        {
            _context = context;

            if (_context.Users.Count() == 0)
            {
                _context.Users.Add(new User { Username = "admin", Password ="password" });
                _context.SaveChanges();
            }
        }       
        
        [HttpGet]
        public IEnumerable<User> GetAll(){
            return _context.Users.ToList();
        }

        [HttpGet("id", Name = "GetUser")]
        public IActionResult GetById(long id){
            System.Diagnostics.Debug.WriteLine("Entered inside 'GetById'");
            var item = _context.Users.FirstOrDefault(t => t.Id == id);
            
            if(item == null){
                return NotFound();
            }
            return new ObjectResult(item);
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] User user){
            System.Diagnostics.Debug.WriteLine("Entered inside 'Create'");
            if(user == null){
                return BadRequest();
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtRoute("GetUser", new {id = user.Id,user});

        }
    }
}