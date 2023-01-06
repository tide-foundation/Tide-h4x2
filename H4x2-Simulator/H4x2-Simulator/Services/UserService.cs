namespace H4x2_Simulator.Services;


using H4x2_Simulator.Entities;
using H4x2_Simulator.Helpers;
using AutoMapper;
using H4x2_Simulator.Models.Users;

public interface IUserService
{
    IEnumerable<User> GetAll();
    User GetById(string id);
    void Create(CreateRequest model);
    void Update(string id, UpdateRequest model);
    void Delete(string id);
}

public class UserService : IUserService
{
    private DataContext _context;

    private readonly IMapper _mapper;

    public UserService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users;
    }

    public User GetById(string id)
    {
        return getUser(id);
    }

    public void Create(CreateRequest model)
    {
        // validate
        if (_context.Users.Any(x => x.UserId == model.UserId))
            throw new Exception("User with the Id '" + model.UserId + "' already exists");
        var user = _mapper.Map<User>(model);
        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(string id, UpdateRequest model)
    {
        var user = getUser(id);

        _mapper.Map(model, user);
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    private User getUser(string id)
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    public void Delete(string id)
    {
        var user = getUser(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

}