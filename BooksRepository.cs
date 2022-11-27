using BooksStore.API.Data;
using Books.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Books.API.Repository
{
    public class BooksRepository : IBookRepository
    {
        private readonly BookStoreContext _context;

        public BooksRepository(BookStoreContext context)
        {
            _context = context;
        }
            
        public async Task<List<BooksModel>> GetAllBooksAsync()
        {
            var records = await _context.Books.Select(x=> new BooksModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description

                }).ToListAsync();

                return records;

        }

        
    }
}