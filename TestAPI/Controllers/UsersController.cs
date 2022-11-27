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

namespace _301168447_Ismail_Mehmood_COMP306_Implementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        string bucketName = "ismail_mehmood_implementation";
        public UsersController()
        {
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            List<ScanCondition> conditions = new();
            return await DynamoClient.context.ScanAsync<User>(conditions).GetRemainingAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            List<ScanCondition> conditions = new List<ScanCondition>();
            ScanCondition condition = new ScanCondition("UserId", ScanOperator.Equal, id);
            conditions.Add(condition);

            var users = await DynamoClient.context.ScanAsync<User>(conditions).GetRemainingAsync();

            if (users == null)
            {
                return NotFound();
            }

            var user = users[0];

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user, [FromForm] IFormFile file)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            try
            {
                if (!String.IsNullOrEmpty(user.FileName))
                {
                    var request = new PutObjectRequest()
                    {
                        BucketName = bucketName,
                        Key = file.FileName,
                        InputStream = file.OpenReadStream(),
                        ContentType = file.ContentType
                    };
                }
                await DynamoClient.context.SaveAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await DynamoClient.context.LoadAsync<User>(id) == null)
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
            if(new FileInfo(file.FileName).Exists)
            {
                var request = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType
                };
            }

            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int epoch = (int)t.TotalSeconds;
            int uuid = epoch - 1669486372;

            user.UserId = uuid;
            user.Approved = false;
            await DynamoClient.context.SaveAsync(user);

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await DynamoClient.context.DeleteAsync<User>(id);

            return NoContent();
        }
    }
}
