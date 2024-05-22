using Infra.DB;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infra.Repositories;

public interface ISubTaskRepository
{
    SubTask Create(SubTask subTask);
    List<SubTask> Get(int mainTaskId);
    SubTask Update(SubTask subTask, int subTaskId);
    void Delete(int subTaskId);
}


public class SubTaskRepository : ISubTaskRepository
{
    private readonly MyDBContext _myDBContext;

    public SubTaskRepository(MyDBContext myDbContext)
    {
        _myDBContext = myDbContext;
    }

    public SubTask Create(SubTask newSubTask)
    {
        _myDBContext.SubTasks.Add(newSubTask);
        _myDBContext.SaveChanges();
        return newSubTask;
    }

    public List<SubTask> Get(int mainTaskId)
    {
        return _myDBContext.SubTasks.Where(x => x.MainTaskId == mainTaskId).ToList();
    }

    public SubTask Update(SubTask subTaskUpdate, int subTaskId)
    {
        _myDBContext.SubTasks.Update(subTaskUpdate);
        _myDBContext.SaveChanges();
        return subTaskUpdate;
    }

    public void Delete(int subTaskId)
    {
        _myDBContext.SubTasks.Where(x => x.Id == subTaskId).ExecuteDelete();
    }

}