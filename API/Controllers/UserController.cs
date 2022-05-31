using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using API.DTOs;
using Microsoft.AspNetCore.Identity;
using Domain;
using Application.Services;
using System.Security.Claims;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly TokenService _tokenService;
    private readonly IConfiguration _config;
    private readonly FirebaseService _firebaseService;
    public UserController(UserManager<User> userManager,
      SignInManager<User> signInManager, TokenService tokenService,
      IConfiguration config, FirebaseService firebaseService)
    {
      _firebaseService = firebaseService;
      _config = config;
      _tokenService = tokenService;
      _userManager = userManager;
      _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<ActionResult<UserDTO>> GetCurrentUser()
    {
      var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

      return CreateUserObject(user);
    }

    [HttpPost("google-login")]
    public async Task<ActionResult<UserDTO>> GoogleLogin([FromQuery] string token)
    {
      var result = await _firebaseService.VerifyIdToken(token);
      if (result == null)
      {
        return Unauthorized();
      }

      var ec = result.Claims.FirstOrDefault(c => c.Key == "email").Value.ToString();
      var user = await _userManager.FindByEmailAsync(ec);

      if (result != null)
      {
        return CreateUserObject(user);
      }
      return Unauthorized();

    }

    private UserDTO CreateUserObject(User user)
    {
      return new UserDTO
      {
        DisplayName = user.DisplayName,
        Token = _tokenService.CreateToken(user.Email),
        Email = user.Email
      };
    }
  }
}