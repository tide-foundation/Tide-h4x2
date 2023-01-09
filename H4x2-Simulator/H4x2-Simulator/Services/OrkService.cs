using System;
using H4x2_Simulator.Entities;
using H4x2_Simulator.Helpers;
using Microsoft.EntityFrameworkCore;

namespace H4x2_Simulator.Services;

public interface IOrkService
{
    IEnumerable<Ork> GetAll();
    Ork GetById(string id);
    void Create(Ork ork);
}

public class OrkService : IOrkService
{
    private DataContext _context;

    public OrkService(DataContext context)
	{
        _context = context;
    }

    public IEnumerable<Ork> GetAll()
    {
        return _context.Orks;
    }

    public Ork GetById(string id)
    {
        return getOrk(id);
    }

    public void Create(Ork ork)
    {
        // validate
        if (_context.Orks.Any(x => x.OrkId == ork.OrkId))
            throw new Exception("Ork with the Id '" + ork.OrkId + "' already exists");
      
        // save ork
        _context.Orks.Add(ork);
        _context.SaveChanges();
    }

    private Ork getOrk(string id)
    {
        var ork = _context.Orks.Find(id);
        if (ork == null) throw new KeyNotFoundException("Ork not found");
        return ork;
    }

}

