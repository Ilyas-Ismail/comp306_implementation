using _301168447_Ismail_Mehmood_COMP306_Implementation.Models;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _301168447_Ismail_Mehmood_COMP306_Implementation.Services
{
    public class BookRepository : IBookRepository
    {
        public async Task AddBookAsync(Book book)
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int epoch = (int)t.TotalSeconds;
            int uuid = epoch - 1669486372;

            //these properties should not be included in the book creation page
            book.BookId = uuid;
            book.CurrentOwner = "StoreAdmin";
            book.LoanDate = DateTime.Now.ToString();
            book.LoanDuration = 0;
            book.Returned = true;
            await DynamoClient.context.SaveAsync(book);
        }

        public async Task<bool> BookExistsAsync(int bookId)
        {
            if (await DynamoClient.context.LoadAsync<Book>(bookId) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        //delete the book once the user has purchased it
        public async void DeleteBookAsync(int bookId)
        {
            await DynamoClient.context.DeleteAsync<Book>(bookId);
        }

        public async Task ExtendLoanAsync(int bookId, int loanDuration)
        {
            var book = await GetBookByIdAsync(bookId);
            book.LoanDuration += loanDuration;
            await DynamoClient.context.SaveAsync(book);
        }

        public async Task BookReturnedAsync(int bookId)
        {
            var book = await GetBookByIdAsync(bookId);
            book.CurrentOwner = "StoreAdmin";
            book.LoanDate = DateTime.Now.ToString();
            book.LoanDuration = 0;
            book.Returned = true;
            await DynamoClient.context.SaveAsync(book);
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            List<ScanCondition> conditions = new List<ScanCondition>();
            ScanCondition condition = new ScanCondition("BookId", ScanOperator.Equal, bookId);
            conditions.Add(condition);

            var books = await DynamoClient.context.ScanAsync<Book>(conditions).GetRemainingAsync();

            var book = books[0];

            return book;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            List<ScanCondition> conditions = new();
            return await DynamoClient.context.ScanAsync<Book>(conditions).GetRemainingAsync();
        }

        //use this method for when a user wants to loan a book
        public async Task LoanBookAsync(int bookId, string currentOwner, int loanDuration)
        {
            var book = await GetBookByIdAsync(bookId);
            book.CurrentOwner = currentOwner;
            book.LoanDate = DateTime.Now.ToString();
            book.LoanDuration = loanDuration;
            book.Returned = false;
            await DynamoClient.context.SaveAsync(book);
        }

        public async Task UpdateBookAsync(int bookId, Book book)
        {
            book.BookId = bookId;
            await DynamoClient.context.SaveAsync(book);
        }
    }
}
