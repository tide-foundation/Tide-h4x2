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
using AutoMapper;
using H4x2_Simulator.Models.Users;
using H4x2_TinySDK.Ed25519;

public interface IUserService
{
    IEnumerable<User> GetAll();
    User GetById(string id);
    void Create(User user);
    void Update(string id, UpdateRequest model);
    void Delete(string id);
    User ValidateUser(string uid, string pubKey, string signedUID);
}

public class UserService : IUserService
{
    private DataContext _context;

    private readonly IMapper _mapper;

    private IOrkService _orkService;

    public UserService(DataContext context, IMapper mapper, IOrkService orkService)
    {
        _context = context;
        _mapper = mapper;
        _orkService = orkService;

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

    public User ValidateUser(string uid, string pubKey, string signedUID)
    {
        // Verify signature
        var EdKey = Key.ParsePublic(pubKey);
        if(!EdKey.EdDSAVerifyStr(uid, signedUID))
            throw new Exception("Invalid signed UId !");

        // Gel all the ork urls from Ork table
        var orks =  _orkService.GetAll();
        List<string> list = new List<string>();
        foreach(Ork e in orks){
            list.Add(e.OrkUrl);
        }
        String[] orksUrl = list.ToArray();

        return new User{
            UserId = uid,
            PubKey = pubKey,
            OrkUrls = orksUrl,
            SignedUID = signedUID
        };
        
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