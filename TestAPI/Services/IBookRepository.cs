using _301168447_Ismail_Mehmood_COMP306_Implementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _301168447_Ismail_Mehmood_COMP306_Implementation.Services
{
    public interface IBookRepository
    {
        Task<bool> BookExistsAsync(int bookId);
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookId);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(int bookId, Book book);
        Task LoanBookAsync(int bookId, string currentOwner, int loanDuration);
        Task ExtendLoanAsync(int bookId, int loanDuration);
        Task BookReturnedAsync(int bookId);
        void DeleteBookAsync(int bookId);
    }
}
