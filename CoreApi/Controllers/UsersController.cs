using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreApi.Data;
using CoreApi.Models;
using System.Text;
using System.Security.Cryptography;
using CoreApi.Helpers;

namespace CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CoreApiContext _context;

        public UsersController(CoreApiContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.ID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(NewUser newUser)
        {
            if(newUser.userName == null || newUser.password == null)
                return BadRequest(new { message = "Null error" });
            var user = await _context.User.SingleOrDefaultAsync(x => x.userName == newUser.userName && x.password == MD5Hash(newUser.password));
             
            if (user == null)
                return BadRequest(new { message = "Kullanici veya sifre hatalı!" });
            return Ok(user);
        }


        [HttpGet("ReqList")]
        public async Task<IActionResult> GetRequests()
        {
            var reqs = await _context.Requests.ToDictionaryAsync( x=> x.ReqTypes, x => x.Url);
             
            //ModelSerializer.Model2String<List<Requests>>(reqs)
            return Ok(ModelSerializer.Model2String<Dictionary<string,string>>(reqs).Trim());
        }


        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> AddNewUser(NewUser newUser)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.userName == newUser.userName || x.email == newUser.email);
            if(user == null)
            {
                _context.User.Add(new Models.User
                { 
                    userName = newUser.userName,
                    password = MD5Hash(newUser.password),
                    email = newUser.email
                });
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            else
            {
                return BadRequest(new { message = "Existing user" });
            }
           
        }


        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User newUser)
        //{
        //    var user = await _context.User.SingleOrDefaultAsync(x => x.userName == newUser.userName && x.email == newUser.email);
        //    //if ()
        //        //User user = new User();
        //        //user.userName = userName;
        //        //user.password = MD5Hash(password);

        //        //_context.User.Add(user);

        //        await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.ID }, user);
        //}

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }

        public static byte[] MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                /*var result =*/ return md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                //return Encoding.ASCII.GetString(result);
            }
        }

        public class NewUser
        {
            public string userName { get; set; }
            public string email { get; set; }
            public string password { get; set; }
        }
    }
}
