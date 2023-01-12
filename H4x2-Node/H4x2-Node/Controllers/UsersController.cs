namespace H4x2_Node.Controllers;

using Microsoft.AspNetCore.Mvc;
using H4x2_Node.Services;
using H4x2_Node.Entities;

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

    [HttpPost]
    public IActionResult Create(User user)
    {
        _userService.Create(user);
        return Ok(new { message = "User created" });
    }

   
}