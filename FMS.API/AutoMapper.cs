using AutoMapper;
using FMS.API.Entities;
using FMS.API.Models;

namespace FMS.API;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<User, UserDto>();
    }
}
