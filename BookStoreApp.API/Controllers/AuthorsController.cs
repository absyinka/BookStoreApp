using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.Author;
using AutoMapper;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthorsController : ControllerBase
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper mapper;
    private readonly ILogger logger;

    public AuthorsController(
        BookStoreDbContext context,
        IMapper mapper,
        ILogger<AuthorsController> logger)
    {
        _context = context;
        this.mapper = mapper;
        this.logger = logger;
    }

    // GET: api/Authors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
    {
        try
        {
            var authors = mapper.Map<IEnumerable<AuthorReadOnlyDto>>(await _context.Authors.ToListAsync());
            return Ok(authors);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{Messages.GeneralErrorMessage} {nameof(GetAuthors)}");
            return StatusCode(500, Messages.Error500Message);
        }
    }

    // GET: api/Authors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
    {
        try
        {
            var author = mapper.Map<AuthorReadOnlyDto>(await _context.Authors.FindAsync(id));

            if (author == null)
            {
                string message = $"{Messages.NotFoundMessage}: {nameof(GetAuthor)} - ID: {id}";
                logger.LogWarning(message);
                return NotFound();
            }

            return Ok(author);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{Messages.GeneralErrorMessage} {nameof(GetAuthor)}");
            return StatusCode(500, Messages.Error500Message);
        }
    }

    // PUT: api/Authors/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> PutAuthor(int id, UpdateAuthorDto request)
    {
        if (id != request.Id)
        {
            logger.LogWarning($"{Messages.InvalidIdMessage}: {nameof(PutAuthor)} - ID: {id}");
            return BadRequest();
        }

        var author = await _context.Authors.FindAsync(id);

        if (author == null)
        {
            logger.LogWarning($"{Messages.NotFoundMessage}: {nameof(PutAuthor)} - ID: {id}");
            return NotFound();
        }

        mapper.Map(request, author);

        _context.Entry(author).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException dbce)
        {
            if (!await AuthorExists(id))
            {
                logger.LogWarning($"{Messages.NotFoundMessage}: {nameof(PutAuthor)} - ID: {id}");
                return NotFound();
            }
            else
            {
                logger.LogError(dbce, $"{Messages.GeneralErrorMessage}: {nameof(PutAuthor)} - ID: {id}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        return NoContent();
    }

    // POST: api/Authors
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<CreateAuthorDto>> PostAuthor(CreateAuthorDto request)
    {
        try
        {
            var author = mapper.Map<Author>(request);

            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, $"{Messages.GeneralErrorMessage}: {nameof(PutAuthor)}");
            return StatusCode(500, Messages.Error500Message);
        }
    }

    // DELETE: api/Authors/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        try
        {
            if (_context.Authors == null)
            {
                logger.LogWarning($"{Messages.NotFoundMessage}: {nameof(DeleteAuthor)} - ID: {id}");
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                logger.LogWarning($"{Messages.NotFoundMessage}: {nameof(DeleteAuthor)} - ID: {id}");
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch(Exception ex)
        {
            logger.LogError(ex, $"{Messages.GeneralErrorMessage}: {nameof(DeleteAuthor)}");
            return StatusCode(500, Messages.Error500Message);
        }
    }

    private async Task<bool> AuthorExists(int id)
    {
        return await _context.Authors.AnyAsync(e => e.Id == id);
    }
}
