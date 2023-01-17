using System;
using H4x2_Simulator.Services;
using H4x2_Simulator.Entities;
using Microsoft.AspNetCore.Mvc;

namespace H4x2_Simulator.Controllers;

[ApiController]
[Route("[controller]")]
public class OrksController : ControllerBase
{

    private IOrkService _orkService;

    public OrksController(IOrkService orkService)
    {
        _orkService = orkService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var orks = _orkService.GetAll();
        return Ok(orks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var ork = _orkService.GetById(id);
        return Ok(ork);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] string orkUrl, [FromForm] string signedOrkUrl)
    {
        try
        {
            Ork ork = await _orkService.ValidateOrk(orkUrl, signedOrkUrl);
            _orkService.Create(ork);
            return Ok(new { message = "Ork created" });
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("TideOrk")]
    public  IActionResult GetTideOrkUrl(){
        var tideOrkUrl = _orkService.GetTideOrk();
        return  Ok(tideOrkUrl);
    }
}

