using Homework_Class03.Models.Domain;
using Homework_Class03.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Homework_Class03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet] //http://localhost:[port]/api/books
        public ActionResult<List<BookDto>> GetAll()
        {
            try
            {
                var booksDb = StaticDb.Books;
                if(booksDb == null)
                {
                    return Ok(new List<BookDto>());
                }
                //map
                var bookDto = booksDb.Select(x => new BookDto
                {
                    Author = x.Author,
                    Title = x.Title
                });

                return Ok(bookDto);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("queryString")] //http://localhost:[port]/api/books/querString?id=1
        public ActionResult<BookDto> GetByStringIndex(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("The id cannot be null");
                }
                if (id <= 0)
                {
                    return BadRequest("The id cannot be a negative number");
                }

                var bookDb = StaticDb.Books.FirstOrDefault(x => x.Id == id);

                if (bookDb == null)
                {
                    return NotFound($"The book with {id} id is not found");
                }

                var bookDto = new BookDto
                {
                    Author = bookDb.Author,
                    Title = bookDb.Title
                };

                return Ok(bookDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("filter")] //http://localhost:[port]/api/books/filter
                            //http://localhost:[port]/api/books/filter?author="Jane Austen"
                            //http://localhost:[port]/api/books/filter?title="Pride and Prejudice"
                            //http://localhost:[port]/api/books/filter?author="Jane Austen"&title="Pride and Prejudice"
                            //http://localhost:[port]/api/books/filter?title="Pride and Prejudice"&author="Jane Austen"
        public ActionResult<BookDto> FilterBooks(string? author, string? title)
        {
            try
            {
                if(string.IsNullOrEmpty(author) && string.IsNullOrEmpty(title))
                {
                    return BadRequest("You need to enter at least one parametar to filter!");
                }

                if (string.IsNullOrEmpty(title))
                {
                    Book filterBookByAuthor = StaticDb.Books.FirstOrDefault(x => x.Author.ToLower() == author.ToLower());

                    if(filterBookByAuthor == null)
                    {
                        return NotFound("There is not such author!");
                    }

                    var filterBookByAuthorDto = new BookDto
                    {
                        Author = filterBookByAuthor.Author,
                        Title = filterBookByAuthor.Title
                    };

                    return Ok(filterBookByAuthorDto);
                }

                if (string.IsNullOrEmpty(author))
                {
                    Book filterBookByTitle = StaticDb.Books.FirstOrDefault(x => x.Title.ToLower() == title.ToLower());
                    
                    if(filterBookByTitle == null)
                    {
                        return NotFound("There is not book with this title!");
                    }

                    var filterBookByTitleDto = new BookDto
                    {
                        Author = filterBookByTitle.Author,
                        Title = filterBookByTitle.Title
                    };

                    return Ok(filterBookByTitleDto);
                }

                Book bookDb = StaticDb.Books.FirstOrDefault(x => x.Author.ToLower() == author.ToLower() && x.Title.ToLower() == title.ToLower());

                if(bookDb == null)
                {
                    return NotFound("There is not such book!");
                }

                var filterBookDto = new BookDto
                {
                    Author = bookDb.Author,
                    Title =bookDb.Title
                };

                return Ok(filterBookDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] BookDto bookDto)
        {
            try
            {
                if(bookDto == null)
                {
                    return BadRequest("Book cannot be null!");
                }

                if (string.IsNullOrEmpty(bookDto.Author) || string.IsNullOrEmpty(bookDto.Title))
                {
                    return BadRequest("Each book must have author!");
                }

                if (StaticDb.Books.Any(x => x.Title.ToLower() == bookDto.Title.ToLower() && x.Author.ToLower() == bookDto.Author.ToLower()))
                {
                    return BadRequest("The book already exist");
                }


                Book book = new Book
                {
                    Author = bookDto.Author,
                    Title = bookDto.Title
                };

                StaticDb.Books.Add(book);
                return StatusCode(StatusCodes.Status201Created, "Book has been created!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("titles")]
        public ActionResult<List<string>> BookTitles([FromBody] List<BookDto> booksDto)
        {
            try 
            {
                if (booksDto == null)
                {
                    return BadRequest("Book cannot be null");
                }
                if (!booksDto.Any())
                {
                    return BadRequest("There isn't any books!");
                }
                if (booksDto.Any(x => string.IsNullOrEmpty(x.Title)))
                {
                    return BadRequest("This collection of books dosen't have titles");
                }
                return booksDto.Select(x => x.Title).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

    }
}
