﻿using AceleraDevTodoListApi.DB;
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

    [HttpPost("Criacao")]
    public IActionResult Post(RequisicaoTarefa requisicaoTarefa)
    {
        try
        {
            var novaTarefa = MapeadorTarefa.ParaClasse(requisicaoTarefa);
            _context.Tarefas.Add(novaTarefa);
            _context.SaveChanges();
            return Ok(novaTarefa);
        }
        catch (Exception ex)
        {
            return BadRequest($"Falha na criação da tarefa. {ex.Message}");
        }
    }
    [HttpGet("Lista")]
    public IActionResult Get()
    {
        try
        {
            return Ok(_context.Tarefas);
        }
        catch (Exception ex)
        {
            return BadRequest($"Falha ao mostrar as Tarefas. {ex.Message}");
        }
    }
    [HttpGet("{id}")]
    public IActionResult Get(int idUsuario)
    {
        try
        {
            var tarefa = _context.Tarefas.Where(x => x.IdUsuario == idUsuario).FirstOrDefault();
            return tarefa is null ? NotFound() : Ok(tarefa);
        }
          catch (Exception ex)
        {
            return BadRequest($"Falha ao mostrar a Tarefa. {ex.Message}");   
        }
        
    }
}
