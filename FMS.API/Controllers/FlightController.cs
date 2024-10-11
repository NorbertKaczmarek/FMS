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
    private readonly ILogger<FlightController> _logger;

    public FlightController(IFlightService flightService, ILogger<FlightController> logger)
    {
        _flightService = flightService;
        _logger = logger;
        _logger.LogDebug(1, "NLog injected into FlightController");
    }

    /// <summary>
    /// Zwraca wszystkie utworzone loty.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<FlightDto>>> GetAll([FromQuery] PageQuery query)
    {
        var flights = await _flightService.GetAll(query);
        return Ok(flights);
    }

    /// <summary>
    /// Zwraca konkretny lot na podstawie jego id.
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<FlightDto>> GetById([FromRoute] Guid id)
    {
        var flight = await _flightService.GetById(id);
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
    public async Task<ActionResult> Create([FromBody] FlightCreateDto dto)
    {
        var id = await _flightService.Create(dto);
        return Created($"/flight/{id}", null);
    }

    /// <summary>
    /// Pozwala modyfikować utworzony wcześniej lot na podstawie jego id.
    /// </summary>
    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] FlightEditDto dto)
    {
        await _flightService.Update(id, dto);
        return Ok();
    }

    /// <summary>
    /// Pozwala usunąć lot na podstawie jego id.
    /// </summary>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        await _flightService.DeleteAsync(id);
        return NoContent();
    }
}

