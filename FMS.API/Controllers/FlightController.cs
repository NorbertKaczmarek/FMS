using FMS.API.Entities;
using FMS.API.Models;
using FMS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FMS.API.Controllers;


[Route("api/flight")]
[ApiController]
[Authorize]
public class FlightController : ControllerBase
{
    private readonly IFlightService _flightService;

    public FlightController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    /// <summary>
    /// Zwraca wszystkie utworzone loty.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Flight>>> GetAll([FromQuery] PageQuery query)
    {
        var flights = await _flightService.GetAll(query);
        return Ok(flights);
    }

    /// <summary>
    /// Zwraca konkretny lot na podstawie jego id.
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public ActionResult<Flight> GetById([FromRoute] Guid id)
    {
        var flight = _flightService.GetById(id);
        return Ok(flight);
    }

    /// <summary>
    /// Pozwala utworzyć nowy lot na podstawie numeru lotu, daty, miejsca wylotu, miejsca przylotu oraz typu samolotu.
    /// </summary>
    /// <remarks>
    /// Wartości numerLotu, dataWylotu miejsceWylotu, miejscePrzylotu oraz typSamolotu nie mogą być puste.
    /// 
    /// Wartość typSamolotu musi należeć do tego zbioru: Embraer, Boeing, Airbus. Przyjmuje też wartości: 0, 1, 2.
    ///  
    /// Przykładowe zapytanie:
    ///
    ///     POST /flight
    ///     {
    ///         "numerLotu": 5,
    ///         "dataWylotu": "2024-05-10T06:30:00",
    ///         "miejsceWylotu": "Warszawa",
    ///         "miejscePrzylotu": "Poznań",
    ///         "typSamolotu": "Embraer"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    public ActionResult Create([FromBody] FlightCreateDto dto)
    {
        var id = _flightService.Create(dto);
        return Created($"/flight/{id}", null);
    }

    /// <summary>
    /// Pozwala modyfikować utworzony wcześniej lot na podstawie jego id.
    /// </summary>
    [Authorize]
    [HttpPost("{id}")]
    public ActionResult Update([FromRoute] Guid id, [FromBody] FlightEditDto dto)
    {
        _flightService.Update(id, dto);
        return Ok();
    }

    /// <summary>
    /// Pozwala usunąć lot na podstawie jego id.
    /// </summary>
    [Authorize]
    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] Guid id)
    {
        _flightService.Delete(id);
        return NoContent();
    }
}

