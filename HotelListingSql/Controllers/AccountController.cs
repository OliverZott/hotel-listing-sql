using HotelListingSql.Contracts;
using HotelListingSql.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace HotelListingSql.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthenticationRepository authenticationRepository, ILogger<AccountController> logger)
    {
        _authenticationRepository = authenticationRepository;
        _logger = logger;
    }

    // POST: api/Account/register
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
    {
        _logger.LogInformation($"Registration attempt for {apiUserDto.Email}");

        try
        {
            throw new NotImplementedException();  // testing purpose
            var errors = await _authenticationRepository.Register(apiUserDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Something went wrong in {nameof(Register)} - User registration attempt for {apiUserDto.Email}");
            return Problem($"Something went wrong in {nameof(Register)}, please contact support.", statusCode: 500);
        }

    }

    // POST: api/Account/login
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        var authResponseDto = await _authenticationRepository.Login(loginDto);

        if (authResponseDto is null)
        {
            return Unauthorized();
        }

        return Ok(authResponseDto);
    }


    // POST: api/Account/refreshtoken
    [HttpPost]
    [Route("refreshtoken")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
    {
        var authResponseDto = await _authenticationRepository.VerifyRefreshToken(request);

        if (authResponseDto is null)
        {
            return Unauthorized();
        }

        return Ok(authResponseDto);
    }

}
