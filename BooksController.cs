using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _booksRepository;

        public BooksController(IBookRepository booksRepository)
        {
          _booksRepository = booksRepository;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _booksRepository.GetAllBooksAsync();
            return Ok(books);
        }

    }
}