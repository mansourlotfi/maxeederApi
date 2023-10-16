using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using ecommerceApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace ecommerceApi.Controllers
{
    public class OtpController:BaseApiController
    {

        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly StoreContext _context;
        public OtpController(UserManager<User> userManager, TokenService tokenService, StoreContext context)
        {
            _context = context;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterOtpDto registerDto)
        {
            Random random = new Random();
            string randomNumber = random.Next(1000000).ToString("D6");

            var user = await _userManager.FindByNameAsync(registerDto.PhoneNumber);

            if (user == null )
            {
            user = new User { PhoneNumber = registerDto.PhoneNumber,UserName=registerDto.PhoneNumber,Email= registerDto.PhoneNumber+"@gmail.com" };

            var result = await _userManager.CreateAsync(user, randomNumber);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return ValidationProblem();
                }
                await _userManager.AddToRoleAsync(user, "Member");
            }
            
            return StatusCode(201);

        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(RegisterOtpDto registerDto)
        {
            Random random = new Random();
            string randomNumber = random.Next(1000000).ToString("D6");

            var user = await _userManager.FindByNameAsync(registerDto.PhoneNumber);

            if (user == null)
            {
                return Unauthorized();
            }
            else
            {
                await _userManager.AddPasswordAsync(user, randomNumber);

            }


            var options = new RestClientOptions("https://api.kavenegar.com/v1/7547556F3864357863693944736F362B6172746F43565A432B344A35485A797A674E37676B6154776658673D/verify/lookup.json")
            {
                ThrowOnAnyError = true,
            };
            var client = new RestClient(options);

            var request = new RestRequest
            {
                Timeout = -1
            };

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("receptor", registerDto.PhoneNumber);
            request.AddParameter("token", randomNumber.ToString());
            request.AddParameter("template", "Verify");


            var response = await client.PostAsync(request);


            if (response != null)
            {
                return StatusCode(201);
            };



            return BadRequest("Problem Login User with otp");

        }


        [HttpPost("Verify")]
        public async Task<ActionResult<UserDtoOtb>> Verify(LoginOtpDto loginDto)
        {

            var user = await _userManager.FindByNameAsync(loginDto.PhoneNumber);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Code))
                return Unauthorized();

       
            return new UserDtoOtb
            {
                PhoneNumber = user.PhoneNumber,
                Token = await _tokenService.GenerateToken(user),
            };
        }

   
    }
}
