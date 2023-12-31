﻿using AutoMapper;
using MediBuyApi.Data;
using MediBuyApi.Models.DTO;
using MediBuyApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediBuyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly MediBuyDbContext dbContext;
        private readonly IMapper mapper;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, MediBuyDbContext dbContext, IMapper mapper)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        //POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDTO.Password);

            if (identityResult.Succeeded)
            {
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);

                    if (identityResult.Succeeded)
                    {
                        // Return a 200 OK status code for a successful registration
                        return Ok("User Registered Successfully");
                    }
                }

                // If adding roles failed, return a 500 Internal Server Error
                return StatusCode(500, "Failed to add roles to the user.");
            }

            // If user creation failed, return a 400 Bad Request with error details
            return BadRequest(identityResult.Errors);
        }


        //POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if(roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());
                        var response = new LoginResponseDTO
                        {

                            UserId = user.Id,
                            Roles = roles.ToList(),
                            JwtToken = jwtToken,
                        };

                        return Ok(response);
                    }                    
                }
            }
                return BadRequest("Error logging in.");
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = dbContext.Users.ToList();
            var usersDTO = mapper.Map<List<UserDTO>>(users);
            return Ok(usersDTO);
        }
    }
}
