using FMS.API.Entities;
using FMS.API.Middleware;
using FMS.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FMS.API.Services;

public interface IFlightService
{
    Task<PageResult<Flight>> GetAll(PageQuery query);
    Flight GetById(int id);
    int Create(FlightCreateDto dto);
    void Update(int id, FlightEditDto dto);
    void Delete(int id);
}

public class FlightService : IFlightService
{
    private readonly FMSDbContext _context;
    public FlightService(FMSDbContext context)
    {
        _context = context;
    }

    public async Task<PageResult<Flight>> GetAll(PageQuery query)
    {
        //await Console.Out.WriteLineAsync("GetAll");
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

        var totalItemsCount = baseQuery.Count();
        return new PageResult<Flight>(result, totalItemsCount, query.PageSize, query.PageNumber);
    }

    public Flight GetById(int id)
    {
        var flight = _context
            .Flights
            .FirstOrDefault(f => f.Id == id);

        if (flight is null) throw new NotFoundException("Flight not found");

        return flight;
    }

    public int Create(FlightCreateDto dto)
    {
        var newFlight = new Flight
        {
            NumerLotu = dto.NumerLotu,
            DataWylotu = dto.DataWylotu,
            MiejsceWylotu = dto.MiejsceWylotu,
            MiejscePrzylotu = dto.MiejscePrzylotu,
            TypSamolotu = dto.TypSamolotu,
        };

        _context
            .Flights
            .Add(newFlight);

        _context.SaveChanges();

        return newFlight.Id;
    }

    public void Update(int id, FlightEditDto dto)
    {
        var flight = GetById(id);

        var NumerLotuInUse = _context.Flights.FirstOrDefault(u => u.NumerLotu == dto.NumerLotu);

        if (!(NumerLotuInUse == null) && !(NumerLotuInUse.Id == flight.Id)) throw new BadRequestException("NumerLotu already exists.");

        flight.NumerLotu = dto.NumerLotu;
        flight.DataWylotu = dto.DataWylotu;
        flight.MiejsceWylotu = dto.MiejsceWylotu;
        flight.MiejscePrzylotu = dto.MiejscePrzylotu;
        flight.TypSamolotu = dto.TypSamolotu;

        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var flight = GetById(id);

        _context.Flights.Remove(flight);
        _context.SaveChanges();
    }
}
