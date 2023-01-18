namespace H4x2_Simulator.Helpers;

using AutoMapper;
using H4x2_Simulator.Entities;
using H4x2_Simulator.Models.Users;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        
        //UpdateRequest->User
        CreateMap<UpdateRequest, User>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;


                    return true;
                }
            ));
    }
}