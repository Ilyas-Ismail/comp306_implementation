using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _301168447_Ismail_Mehmood_COMP306_Implementation.Models
{
    [DynamoDBTable("Implementation_Users")]
    public class User
    {
        [DynamoDBHashKey]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FileName { get; set; }
        public bool Approved { get; set; }

        public User()
        {

        }
    }
}
