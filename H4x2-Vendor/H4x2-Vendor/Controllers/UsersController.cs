namespace H4x2_Vendor.Controllers;

using Microsoft.AspNetCore.Mvc;
using H4x2_Vendor.Services;
using H4x2_Vendor.Entities;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var user = _userService.GetById(id);
        return Ok(user);
    }

    [HttpPost("{uid}")]
    public IActionResult Create([FromRoute] string uid, [FromForm] string secret)
    {
         User newUser = new User();
         newUser.UId =uid ;
         newUser.Secret = secret;
        _userService.Create(newUser);
        return Ok(new { message = "Entry created" });
    }


   
}