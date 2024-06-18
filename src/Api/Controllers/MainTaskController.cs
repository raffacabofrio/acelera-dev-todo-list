﻿using Domain.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
[EnableCors("AllowAllHeaders")]
public class MainTaskController : ControllerBase
{
    private readonly IMainTaskService _mainTaskService;

    public MainTaskController(IMainTaskService mainTaskService)
    {
        _mainTaskService = mainTaskService;
    }

    [HttpGet("{userId}")]
    public IActionResult Get([FromRoute] int userId)
    {
        var mainTasks = _mainTaskService.Get(userId);
        return mainTasks is null ? NotFound() : Ok(mainTasks);
    }

    [HttpGet("search")]
    public IActionResult Get([FromQuery] int? MainTaskId,
                             [FromQuery] string? UserName,
                             [FromQuery] string? MainTaskDescription)
    {
        if (MainTaskId == null && string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(MainTaskDescription))
            return BadRequest("At least one parameter is required (MainTaskId, UserName, MainTaskDescription)");

        var mainTasks = _mainTaskService.SearchByParams(MainTaskId, UserName, MainTaskDescription);
        return mainTasks is null ? NotFound() : Ok(mainTasks);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] MainTaskRequest mainTaskRequest)
    {
        var newMainTask = await _mainTaskService.Create(mainTaskRequest);
        return Ok(newMainTask);
    }

    [HttpPut("{mainTaskId}")]
    public IActionResult Put(int mainTaskId, [FromBody] MainTaskUpdate updateMainTask)
    {
        var mainTask = _mainTaskService.Update(updateMainTask, mainTaskId);
        return Ok(mainTask);
    }

    [HttpDelete("{mainTaskId}")]
    public IActionResult Delete(int mainTaskId)
    {
        _mainTaskService.Delete(mainTaskId);
        return NoContent();
    }
}
