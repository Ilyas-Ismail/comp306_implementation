using _301168447_Ismail_Mehmood_COMP306_Implementation.Models;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _301168447_Ismail_Mehmood_COMP306_Implementation.Services
{
    public class UserRepository : IUserRepository
    {
        readonly string bucketName = "ismail_mehmood_implementation";
        public async Task AddUserAsync(User user, IFormFile file)
        {
            if (!String.IsNullOrEmpty(user.FileName))
            {
                var putRequest = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType
                };

                await S3Client.s3Client.PutObjectAsync(putRequest);
            }

            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int epoch = (int)t.TotalSeconds;
            int uuid = epoch - 1669486372;

            user.UserId = uuid;
            user.Approved = false;
            await DynamoClient.context.SaveAsync(user);
        }

        public async void DeleteUserAsync(int userId)
        {
            await DynamoClient.context.DeleteAsync<User>(userId);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            List<ScanCondition> conditions = new List<ScanCondition>();
            ScanCondition condition = new ScanCondition("UserId", ScanOperator.Equal, userId);
            conditions.Add(condition);

            var users = await DynamoClient.context.ScanAsync<User>(conditions).GetRemainingAsync();

            var user = users[0];

            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            List<ScanCondition> conditions = new();
            return await DynamoClient.context.ScanAsync<User>(conditions).GetRemainingAsync();
        }

        public async Task UpdateUserAsync(int userId, User user, IFormFile file)
        {
            var checkUser = await GetUserByIdAsync(userId);
            user.UserId = userId;

            if (!String.IsNullOrEmpty(user.FileName))
            {
                if(user.FileName == checkUser.FileName)
                {
                    var delRequest = new DeleteObjectRequest
                    {
                        BucketName = bucketName,
                        Key = user.FileName
                    };

                    await S3Client.s3Client.DeleteObjectAsync(delRequest);
                }

                var putRequest = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType
                };

                await S3Client.s3Client.PutObjectAsync(putRequest);
            }
            await DynamoClient.context.SaveAsync(user);
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            if (await DynamoClient.context.LoadAsync<User>(userId) == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
