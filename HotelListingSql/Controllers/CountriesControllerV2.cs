using AutoMapper;
using HotelListingSql.Core.Contracts;
using HotelListingSql.Core.DTOs;
using HotelListingSql.Core.DTOs.Country;
using HotelListingSql.Core.Exceptions;
using HotelListingSql.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelListingSql.Controllers;

[Route("api/v{version:apiVersion}/countries")]
[ApiController]
[ApiVersion("2.0", Deprecated = true)]
//[Authorize]  // For authorization on whole endpoint
public class CountriesControllerV2 : ControllerBase
{
    private readonly ICountriesRepository _countriesRepository;
    private readonly ILogger<CountriesControllerV2> _logger;
    private readonly IMapper _mapper;

    public CountriesControllerV2(ICountriesRepository countriesRepository, ILogger<CountriesControllerV2> logger, IMapper mapper)
    {
        _countriesRepository = countriesRepository;
        _mapper = mapper;
        _logger = logger;
        _logger.LogInformation("Countries Controller ...");
    }

    // GET: api/Countries/GetAll
    [HttpGet("GetAll")]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await _countriesRepository.GetAllAsync();
        var countriesList = _mapper.Map<List<GetCountryDto>>(countries);
        return Ok(countriesList);
    }

    // GET: api/Countries/?StartIndex=0&PageSize=4&PageNumber=1
    [HttpGet]
    public async Task<ActionResult<PagedResults<GetCountryDto>>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
    {
        // No mapped needed, because its mapped inside GetAllAsync !
        var pagedCountriesResult = await _countriesRepository.GetAllAsync<GetCountryDto>(queryParameters);
        return Ok(pagedCountriesResult);
    }


    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCountryDetailsDto>> GetCountry(int id)
    {
        // doing inner-join for hotels of updateCountryDto
        //var country = await _context.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);
        var country = await _countriesRepository.GetDetails(id);

        if (country == null)
        {
            throw new MyNotFoundException(nameof(GetCountry), id);
        }

        var countryDto = _mapper.Map<GetCountryDetailsDto>(country);

        // no Ok() needed ... more explicit in modern framework
        return countryDto;
    }

    // PUT: api/Countries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]  // Authorization specific request
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
        {
            return BadRequest("Invalid Record Id");
        }

        var country = await _countriesRepository.GetAsync(updateCountryDto.Id);
        if (country == null)
        {
            return NotFound();
        }

        _ = _mapper.Map(updateCountryDto, country);  // mapper does modification so its tracked as "Modified"

        try
        {
            await _countriesRepository.UpdateAsync(country);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CountryExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
    {
        var country = _mapper.Map<Country>(createCountryDto);
        await _countriesRepository.AddAsync(country);
        return CreatedAtAction("GetCountry", new { id = country.Id }, createCountryDto);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]  // Authorization request for specific ROLE
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _countriesRepository.GetAsync(id);

        if (country == null)
        {
            return NotFound();
        }

        await _countriesRepository.DeleteAsync(id);
        return NoContent();
    }

    private async Task<bool> CountryExists(int id)
    {
        return await _countriesRepository.Exists(id);
    }
}