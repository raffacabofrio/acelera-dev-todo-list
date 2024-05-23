using Domain.Models;
using Infra.DB;

namespace Infra.Repositories;

public interface IUserRepository
{
    User Create(User user);
    User? Get(int userId);
    List<User> GetAll();
    User Update(User userUpdate);
    void Delete(int userId);
}

public class UserRepository : IUserRepository
{
    private readonly MyDBContext _myDBContext;

    public UserRepository(MyDBContext myDBContext)
    {
        _myDBContext = myDBContext;
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

    public User Update(User updatedUser)
    {
        var existingUser = Get(updatedUser.Id);

        if (existingUser is null)
            throw new Exception("User not found!");

        var updatedUser = _myDBContext.Users.FirstOrDefault(x => x.Id == userUpdate.Id);
        updatedUser.Name = userUpdate.Name;
        updatedUser.Password = userUpdate.Password;
        updatedUser.Email = userUpdate.Email;
        _context.SaveChanges();
        return updateUser;
    }

    public void Delete(int userId)
    {
        var user = Get(userId);

        if (user is null)
            throw new Exception("User not found!");

        _myDBContext.Users.Where(x => x.Id == userId).ExecuteDelete();
        _myDBContext.SaveChanges();
    }
}
