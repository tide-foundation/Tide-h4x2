using System;
using System.Numerics;
using H4x2_Simulator.Entities;
using H4x2_Simulator.Helpers;
using Microsoft.EntityFrameworkCore;
using H4x2_TinySDK.Ed25519;

namespace H4x2_Simulator.Services;

public interface IOrkService
{
    IEnumerable<Ork> GetAll();
    Ork GetById(string id);
    void Create(Ork ork);
    Task<Ork> ValidateOrk(string OrkUrl, string SignedOrkUrl);
}

public class OrkService : IOrkService
{
    private DataContext _context;
    static readonly HttpClient _client = new HttpClient();

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

    
    public async Task<Ork> ValidateOrk(string orkUrl, string signedOrkUrl)
    {
        // Query ORK public
        string orkPub = await _client.GetStringAsync(orkUrl + "/public");

        // Verify signature

        //  Generate ID
        BigInteger orkId = Ork.GenerateID(orkPub);

        return new Ork{
            OrkId = orkId.ToString(),
            OrkPub = orkPub,
            OrkUrl = orkUrl,
            SignedOrkUrl = signedOrkUrl
        };
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

