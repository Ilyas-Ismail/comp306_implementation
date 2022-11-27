using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace _301168447_Ismail_Mehmood_COMP306_Implementation.Models
{
    public class DynamoClient
    {
        public static readonly DynamoDBContext context;

        static DynamoClient()
        {
            context = GetDynamoClient();
        }

        private static DynamoDBContext GetDynamoClient()
        {
            string accessKey = ConfigurationManager.AppSettings["accessId"];
            string secretKey = ConfigurationManager.AppSettings["secretKey"];
            Amazon.Runtime.BasicAWSCredentials credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.CACentral1);
            return new DynamoDBContext(client);
        }
    }
}
