namespace H4x2_Vendor.Services;


using H4x2_Vendor.Entities;
using H4x2_Vendor.Helpers;

public interface IUserService
{
    IEnumerable<User> GetAll();
    User GetById(string id);
    void Create(User user);

}

public class UserService : IUserService
{
    private DataContext _context;

    public UserService(DataContext context)
    {
        _context = context;

    }
    public IEnumerable<User> GetAll()
    {
        return _context.UserSecrets;
    }

    public User GetById(string id)
    {
        return getUserRecord(id);
    }
    public void Create(User user)
    {
        // validate
        if (_context.UserSecrets.Any(x => x.UId == user.UId))
            throw new Exception("Entry with the UId '" + user.UId + "' already exists");
        // save user secret
        _context.UserSecrets.Add(user);
        _context.SaveChanges();
    }

    private User getUserRecord(string id)
    {
        var user = _context.UserSecrets.Find(id);
        if (user == null) throw new KeyNotFoundException("Entry not found");
        return user;
    }

   
}