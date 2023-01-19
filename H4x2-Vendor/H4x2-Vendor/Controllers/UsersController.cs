namespace H4x2_Vendor.Controllers;

using Microsoft.AspNetCore.Mvc;
using H4x2_Vendor.Services;
using H4x2_Vendor.Entities;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    protected readonly IConfiguration _config;
    public UsersController(IUserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
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
    public async Task<IActionResult> Create([FromRoute] string uid, [FromForm] string secret)
    {  
        try
        {
            // check user exists in simulator first
            string Baseurl = _config.GetValue<string>("Endpoints:Simulator:Api");
            bool check = await _userService.GetEntryAsync(Baseurl + uid);
            User newUser = new User();
            newUser.UId =uid ;
            newUser.Secret = secret;
            _userService.Create(newUser);
            return Ok(new { message = "Entry created" });
        
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }  
    }   
}