// 
// Tide Protocol - Infrastructure for a TRUE Zero-Trust paradigm
// Copyright (C) 2022 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Code License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Code License for more details.
// You should have received a copy of the Tide Community Open 
// Code License along with this program.
// If not, see https://tide.org/licenses_tcoc2-0-0-en
//

namespace H4x2_Simulator.Services;


using H4x2_Simulator.Entities;
using H4x2_Simulator.Helpers;
using H4x2_TinySDK.Ed25519;
using H4x2_TinySDK.Math;
using System.Text;

public interface IUserService
{
    IEnumerable<User> GetAll();
    User GetById(string id);
    void Create(User user);
    void Delete(string id);
    Task<User> ValidateUser(string uid, string[] orkUrls, string[] signedEntries);
}

public class UserService : IUserService
{
    private DataContext _context;
    static readonly HttpClient _client = new HttpClient();

    public UserService(DataContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users;
    }

    public User GetById(string id)
    {
        return getUser(id);
    }

    public void Create(User user)
    {
        // validate for user existence
        if (_context.Users.Any(x => x.UserId == user.UserId))
            throw new Exception("User with the Id '" + user.UserId + "' already exists");

        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public async Task<User> ValidateUser(string uid, string[] orkUrls, string[] signedEntries)
    {    
        List<string> orkPubList = new List<string>();
        // Query ORK public
        foreach(string orkUrl in orkUrls)
            orkPubList.Add(await _client.GetStringAsync(orkUrl + "/public"));
        
        String[] orksPubs = orkPubList.ToArray();

        // Verify signature
        for(int i = 0 ; i < orkUrls.Length ; i++){
            var edPoint = Point.FromBase64(orksPubs[i]);
            Console.WriteLine(orksPubs[i]);
            if(!EdDSA.Verify(orkUrls[i], signedEntries[i], edPoint))
                throw new Exception("Invalid signed entry for ork url '" + orkUrls[i] + "' !");
        }

        return new User{
            UserId = uid,
            OrkUrls = orkUrls,
            SignedEntries = signedEntries
        };
        
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