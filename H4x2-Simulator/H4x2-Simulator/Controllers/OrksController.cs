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
    public IActionResult Create(Ork ork)
    {
        _orkService.Create(ork);
        return Ok(new { message = "Ork created" });
    }
}

