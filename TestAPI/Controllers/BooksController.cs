using _301168447_Ismail_Mehmood_COMP306_Implementation.Models;
using _301168447_Ismail_Mehmood_COMP306_Implementation.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _301168447_Ismail_Mehmood_COMP306_Implementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBookRepository _bookRepository;
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            var books = await _bookRepository.GetBooksAsync();
            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{bookId}")]
        public async Task<ActionResult<User>> GetBook(int bookId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            return Ok(book);
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{bookId}")]
        public async Task<IActionResult> PutBook(int bookId, Book book)
        {
            /*if (userId != user.UserId)
            {
                return BadRequest();
            }*/

            try
            {
                await _bookRepository.UpdateBookAsync(bookId, book);
            }
            catch (Exception)
            {
                if (await _bookRepository.BookExistsAsync(bookId) == false)
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

        [HttpPatch("{bookId}")]
        public async Task<IActionResult> LoanBook(int bookId, [FromForm] string currentOwner, [FromForm] int loanDuration)
        {
            await _bookRepository.LoanBookAsync(bookId, currentOwner, loanDuration);
            return NoContent();
        }

        [HttpPatch("/updateLoan/{bookId}")]
        public async Task<IActionResult> PatchLoan(int bookId, [FromForm] int loanDuration)
        {
            await _bookRepository.ExtendLoanAsync(bookId, loanDuration);
            return NoContent();
        }

        [HttpPatch("/returned/{bookId}")]
        public async Task<IActionResult> PatchReturn(int bookId)
        {
            await _bookRepository.BookReturnedAsync(bookId);
            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostBook(Book book)
        {
            await _bookRepository.AddBookAsync(book);

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            if (!await _bookRepository.BookExistsAsync(bookId))
            {
                return NotFound();
            }
            _bookRepository.DeleteBookAsync(bookId);

            return NoContent();
        }
    }
}
