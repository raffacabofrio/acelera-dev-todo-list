using Infra.DB;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infra.Repositories;

public interface IUserRepository
{
    User Create(User user);
    User? Get(int userId);
    List<User> GetAll();
    User Update(User userUpdate);
    void Delete(int userId);
    Task<User?> FindByUsernameAsync(string username);
    List<User> GetByName(string name);
}

public class UserRepository : IUserRepository
{
    private readonly MyDBContext _myDBContext;

    public UserRepository(MyDBContext myDBContext)
    {
        _myDBContext = myDBContext;
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        return await _myDBContext.Users.FirstOrDefaultAsync(u => u.Email == username);
    }

    public User Create(User newUser)
    {
        _myDBContext.Users.Add(newUser);
        _myDBContext.SaveChanges();
        return newUser;
    }

    public User? Get(int userId)
    {
        return _myDBContext.Users.Find(userId);
    }

    public List<User> GetAll()
    {
        return _myDBContext.Users.ToList();
    }

    public User Update(User userUpdate)
    {
        var updatedUser = _myDBContext.Users.FirstOrDefault(x => x.Id == userUpdate.Id);
        if (updatedUser is null)
        {
            throw new Exception("User not found!");
        }
        else
        {
            updatedUser.Name = userUpdate.Name;
            updatedUser.Password = userUpdate.Password;
            updatedUser.Email = userUpdate.Email;

            _myDBContext.SaveChanges();
            return userUpdate;
        }
    }

    public void Delete(int userId)
    {
        var user = Get(userId);

        if (user is null)
            throw new Exception("User not found!");

        _myDBContext.Users.Where(x => x.Id == userId).ExecuteDelete();
        _myDBContext.SaveChanges();
    }

    public List<User> GetByName(string name)
    {
        return _myDBContext.Users.Where(x => x.Name!.Contains(name)).ToList();
    }
}