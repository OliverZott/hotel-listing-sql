using AutoMapper;
using HotelListingSql.Data;
using HotelListingSql.DTOs.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListingSql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly HotelListingDbContext _context;
        private readonly ILogger<CountriesController> _logger;
        private readonly IMapper _mapper;

        public CountriesController(HotelListingDbContext context, ILogger<CountriesController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }
            var countries = await _context.Countries.ToListAsync();
            var countriesList = _mapper.Map<List<GetCountryDto>>(countries);
            return Ok(countriesList);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCountryDetailsDto>> GetCountry(int id)
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }

            // doing inner-join for hotels of updateCountryDto
            var country = await _context.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);

            if (country == null)
            {
                return NotFound();
            }

            var countryDto = _mapper.Map<GetCountryDetailsDto>(country);

            // no Ok() needed ... more explicit in modern framework
            return countryDto;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest("Invalid Record Id");
            }

            if (_context.Countries == null)
            {
                return NotFound();
            }

            //_context.Entry(updateCountryDto).State = EntityState.Modified;

            var country = await _context.Countries.FindAsync(updateCountryDto.Id);
            if (country == null)
            {
                return NotFound();
            }
            _ = _mapper.Map(updateCountryDto, country);  // mapper does modification so its tracked as "Modified"


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;  // not wanted, until you really want to kill runtime of program
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
        {
            // Without AutoMapper:
            //var updateCountryDto = new Country()
            //{
            //    Name = createCountryDto.Name,
            //    ShortName = createCountryDto.ShortName,
            //};

            var country = _mapper.Map<Country>(createCountryDto);

            if (_context.Countries == null)
            {
                return Problem("Entity set 'HotelListingDbContext.Countries'  is null.");
            }

            try
            {
                _context.Countries.Add(country);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message);
            }

            return CreatedAtAction("GetCountry", new { id = country.Id }, createCountryDto);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
