using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _301168447_Ismail_Mehmood_COMP306_Implementation.Models;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3.Model;
using System.IO;
using _301168447_Ismail_Mehmood_COMP306_Implementation.Services;

namespace _301168447_Ismail_Mehmood_COMP306_Implementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutUser(int userId, User user, [FromForm] IFormFile file)
        {
            /*if (userId != user.UserId)
            {
                return BadRequest();
            }*/

            try
            {
                await _userRepository.UpdateUserAsync(userId, user, file);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _userRepository.UserExistsAsync(userId) == false)
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user, [FromForm] IFormFile file)
        {
            await _userRepository.AddUserAsync(user, file);

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
                _userRepository.DeleteUserAsync(userId);

            return NoContent();
        }
    }
}
