using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.Book;
using AutoMapper;
using BookStoreApp.API.Static;
using BookStoreApp.API.DTOs.Author;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<BooksController> logger;

        public BooksController(
            BookStoreDbContext context,
            IMapper mapper,
            ILogger<BooksController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            try
            {
                var books = mapper.Map<IEnumerable<BookReadOnlyDto>>(await _context.Books.ToListAsync());
                return Ok(books);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{Messages.GeneralErrorMessage} {nameof(GetBooks)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookReadOnlyDto>> GetBook(int id)
        {
            try
            {
                var book = mapper.Map<BookReadOnlyDto>(await _context.Books.FindAsync(id));

                if (book == null)
                {
                    logger.LogWarning($"{Messages.NotFoundMessage}: {nameof(GetBook)} - ID: {id}");
                    return NotFound();
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{Messages.GeneralErrorMessage} {nameof(GetBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto request)
        {
            if (id != request.Id)
            {
                logger.LogWarning($"{Messages.InvalidIdMessage}: {nameof(PutBook)} - ID: {id}");
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                logger.LogWarning($"{Messages.NotFoundMessage}: {nameof(PutBook)} - ID: {id}");
                return NotFound();
            }

            mapper.Map(request, book);

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbce)
            {
                if (!await BookExists(id))
                {
                    logger.LogWarning($"{Messages.NotFoundMessage}: {nameof(PutBook)} - ID: {id}");
                    return NotFound();
                }
                else
                {
                    logger.LogError(dbce, $"{Messages.GeneralErrorMessage}: {nameof(PutBook)} - ID: {id}");
                    return StatusCode(500, Messages.Error500Message);
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto request)
        {
            try
            {
                var book = mapper.Map<Book>(request);

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{Messages.GeneralErrorMessage}: {nameof(PostBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                if (_context.Books == null)
                {
                    logger.LogWarning($"{Messages.NotFoundMessage}: {nameof(DeleteBook)} - ID: {id}");
                    return NotFound();
                }

                var book = await _context.Books.FindAsync(id);

                if (book == null)
                {
                    logger.LogWarning($"{Messages.NotFoundMessage}: {nameof(DeleteBook)} - ID: {id}");
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"{Messages.GeneralErrorMessage}: {nameof(DeleteBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        private async Task<bool> BookExists(int id)
        {
            return await _context.Books.AnyAsync(e => e.Id == id);
        }
    }
}
