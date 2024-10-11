using AutoMapper;
using FMS.API.Entities;
using FMS.API.Middleware;
using FMS.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FMS.API.Services;

public interface IFlightService
{
    Task<PageResult<FlightDto>> GetAll(PageQuery query);
    Task<FlightDto> GetById(Guid id);
    Task<Guid> Create(FlightCreateDto dto);
    Task Update(Guid id, FlightEditDto dto);
    Task DeleteAsync(Guid id);
}

public class FlightService : IFlightService
{
    private readonly FMSDbContext _context;
    private readonly IMapper _mapper;

    public FlightService(FMSDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PageResult<FlightDto>> GetAll(PageQuery query)
    {
        var flights = await _context
            .Flights
            .ToListAsync();

        var baseQuery = _context
            .Flights
            .Where(
                f => query.SearchPhrase == null ||
                (
                f.NumerLotu.ToString().Equals(query.SearchPhrase) ||
                f.DataWylotu.ToString().ToLower().Contains(query.SearchPhrase.ToLower()) ||
                f.MiejsceWylotu.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                f.MiejscePrzylotu.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                ((string)(object)f.TypSamolotu).ToLower().Contains(query.SearchPhrase.ToLower())
                )
            );

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Flight, object>>>
                {
                    {nameof(Flight.Id), f => f.Id },
                    {nameof(Flight.NumerLotu), f => f.NumerLotu },
                    {nameof(Flight.DataWylotu), f => f.DataWylotu},
                    {nameof(Flight.MiejsceWylotu), f => f.MiejsceWylotu},
                    {nameof(Flight.MiejscePrzylotu), f => f.MiejscePrzylotu},
                    {nameof(Flight.TypSamolotu), f => f.TypSamolotu}
                };

            var selectedColumn = columnsSelector[query.SortBy];

            baseQuery = query.SortDirection == SortDirection.ASC
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }
        var result = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        var resultMapped = _mapper.Map<List<FlightDto>>(flights);

        var totalItemsCount = baseQuery.Count();
        return new PageResult<FlightDto>(resultMapped, totalItemsCount, query.PageSize, query.PageNumber);
    }

    public async Task<FlightDto> GetById(Guid id)
    {
        var flight = await getByIdAsync(id);

        var result = _mapper.Map<FlightDto>(flight);

        return result;
    }

    private Flight getById(Guid id)
    {
        var flight = _context
            .Flights
            .FirstOrDefault(f => f.Id == id);

        if (flight is null) throw new NotFoundException("Flight not found");

        return flight;
    }

    private async Task<Flight> getByIdAsync(Guid id)
    {
        var flight = await _context
            .Flights
            .FirstOrDefaultAsync(f => f.Id == id);

        if (flight is null) throw new NotFoundException("Flight not found");

        return flight;
    }

    public async Task<Guid> Create(FlightCreateDto dto)
    {
        var newFlight = _mapper.Map<Flight>(dto);

        await _context
            .Flights
            .AddAsync(newFlight);

        await _context.SaveChangesAsync();

        return newFlight.Id;
    }

    public async Task Update(Guid id, FlightEditDto dto)
    {
        var flight = await getByIdAsync(id);

        var NumerLotuInUse = await _context.Flights.FirstOrDefaultAsync(u => u.NumerLotu == dto.NumerLotu);

        if (!(NumerLotuInUse == null) && !(NumerLotuInUse.Id == flight.Id)) throw new BadRequestException("NumerLotu already exists.");

        _mapper.Map(dto, flight);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var flight = await getByIdAsync(id);

        _context.Flights.Remove(flight);
        await _context.SaveChangesAsync();
    }
}
