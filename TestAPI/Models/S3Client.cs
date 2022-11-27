using Amazon.S3;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace _301168447_Ismail_Mehmood_COMP306_Implementation.Models
{
    public class S3Client
    {
        public static readonly IAmazonS3 s3Client;

        static S3Client()
        {
            s3Client = GetS3Client();
        }

        private static IAmazonS3 GetS3Client()
        {
            string accessKey = ConfigurationManager.AppSettings["accessId"];
            string secretKey = ConfigurationManager.AppSettings["secretKey"];
            return new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.CACentral1);
        }
    }
}
