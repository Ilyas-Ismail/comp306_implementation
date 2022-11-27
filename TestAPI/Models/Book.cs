using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _301168447_Ismail_Mehmood_COMP306_Implementation.Models
{
    [DynamoDBTable("Implementation_Books")]
    public class Book
    {
        [DynamoDBHashKey]
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentOwner { get; set; }
        public string LoanDate { get; set; }
        public int LoanDuration { get; set; }
        public bool Returned { get; set; }

        public Book()
        {

        }
    }
}
