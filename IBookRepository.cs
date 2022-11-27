using Books.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.API.Repository
{
    public interface IBookRepository
    {
       Task<List<BooksModel>> GetAllBooksAsync() 
    }
}
