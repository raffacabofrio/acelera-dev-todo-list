﻿using Domain.Mappers;
using Domain.Models;
using Infra.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Service;

public interface ISubTaskService
{
    SubTask Create(SubTaskRequest subTaskRequest);
    List<SubTask> List(int mainTaskId);
    SubTask Update(SubTaskUpdate subTaskUpdate, int subTaskId);
    void Delete(int subTaskId);
}
public class SubTaskService : ISubTaskService
{
    private readonly ISubTaskRepository _subTaskRepository;
    private readonly IMainTaskRepository _mainTaskRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SubTaskService(ISubTaskRepository subTaskRepository, IMainTaskRepository mainTaskRepository, IHttpContextAccessor httpContextAccessor)
    {
        _subTaskRepository = subTaskRepository;
        _mainTaskRepository = mainTaskRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public SubTask Create(SubTaskRequest subTaskRequest)
    {
        var newSubTask = SubTaskMapper.ToClass(subTaskRequest);
        return _subTaskRepository.Create(newSubTask);
    }

    public void Delete(int subTaskId)
    {
        var subTask = _subTaskRepository.Find(subTaskId);

        if (subTask is null)
            throw new Exception("subTask not found!");

        var mainTask = _mainTaskRepository.Find(subTask.MainTaskId);
        if (mainTask is null)
            throw new Exception("mainTask not found!");

        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null || mainTask.UserId.ToString() != userId)
        {
            throw new UnauthorizedAccessException("You don't have permission to delete this subtask.");
        }

        _subTaskRepository.Delete(subTaskId);
    }

    public List<SubTask> List(int mainTaskId)
    {
        return _subTaskRepository.Get(mainTaskId);
    }

    public SubTask Update(SubTaskUpdate updateSubTaskRequest, int subTaskId)
    {
        var subTask = _subTaskRepository.Find(subTaskId);

        if (subTask is null)
            throw new Exception("SubTask not found!");

        var mainTask = _mainTaskRepository.Find(subTask.MainTaskId);

        if (mainTask is null)
            throw new Exception("mainTask not found!");

        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null || mainTask.UserId.ToString() != userId)
        {
            throw new UnauthorizedAccessException("You don't have permission to update this subtask.");
        }

        subTask.Description = updateSubTaskRequest.Description;
        subTask.Finished = updateSubTaskRequest.Finished;
        return _subTaskRepository.Update(subTask);
    }
}