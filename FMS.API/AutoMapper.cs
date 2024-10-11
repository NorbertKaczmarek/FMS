using AutoMapper;
using FMS.API.Entities;
using FMS.API.Models;

namespace FMS.API;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<User, UserDto>();
        CreateMap<Flight, FlightDto>();

        CreateMap<FlightCreateDto, Flight>();
        CreateMap<FlightEditDto, Flight>();
    }
}
