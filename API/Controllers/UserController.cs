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
using API.Services;

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
    private readonly HttpClient _httpClient;
    public UserController(UserManager<User> userManager,
      SignInManager<User> signInManager, TokenService tokenService,
    IConfiguration config)
    {
      _config = config;
      _tokenService = tokenService;
      _userManager = userManager;
      _signInManager = signInManager;
      _httpClient = new HttpClient{
        BaseAddress = new System.Uri(""),
      
      };
    }

  [HttpPost("login")]
  public async Task<ActionResult<UserDTO>> Login(LoginDTO LoginDTO)
  {

    var user = await _userManager.FindByEmailAsync(LoginDTO.Email);

    if (user == null) return Unauthorized();

    var result = await _signInManager.CheckPasswordSignInAsync(user, LoginDTO.Password, false);

    if (result.Succeeded)
    {
      return new UserDTO
      {
        DisplayName = user.DisplayName,
        //   Token = _tokenService.CreateToken(LoginDTO.Email),
        Token = "Replace this with CreateToken",
        Username = user.UserName
      };
    }
    return Unauthorized();

  }

}
}