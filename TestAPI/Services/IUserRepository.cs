using _301168447_Ismail_Mehmood_COMP306_Implementation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _301168447_Ismail_Mehmood_COMP306_Implementation.Services
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(int userId);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task AddUserAsync(User user, IFormFile file);
        Task UpdateUserAsync(int userId, User user, IFormFile file);
        Task PatchUserAsync(int userId, bool approved);
        void DeleteUserAsync(int userId);
    }
}
