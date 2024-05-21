﻿using Infra.DB;
using Domain.Mappers;
using Domain.Request;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TarefaController : ControllerBase
{
    private readonly MyDBContext _context;

    public TarefaController(MyDBContext context)
    {
        _context = context;
    }

    [HttpGet("Lista")]
    public IActionResult Get()
    {
        return Ok(_context.MainTasks);
    }

    [HttpGet("IdUsuario")]
    public IActionResult Get(int userId)
    {
        var task = _context.MainTasks.Where(x => x.UserId == userId).ToList();
        return task is null ? NotFound() : Ok(task);
    }

    [HttpPost("Criacao")]
    public IActionResult Post(MainTaskRequest taskRequest)
    {
        var newTask = MainTaskMapper.ToClass(taskRequest);
        _context.MainTasks.Add(newTask);
        _context.SaveChanges();
        return Ok(newTask);
    }

    [HttpPut("Update/{Id}")]
    public IActionResult Put(int Id, [FromBody] MainTaskRequest updateDescription)
    {
        var task = _context.MainTasks.Find(Id);
        if (task is null)
            return NotFound($"Tarefa não encontrada.");

        task.Description = updateDescription.Description;

        _context.Update(task);
        _context.SaveChanges();

        return Ok(task);
    }

    [HttpDelete("Delete")]
    public IActionResult Delete(int Id)
    {
        var task = _context.MainTasks.Find(Id);
        if (task is null)
            return NotFound($"Tarefa não encontrada.");

        _context.Remove(task);
        _context.SaveChanges();

        return NoContent();
    }
}
