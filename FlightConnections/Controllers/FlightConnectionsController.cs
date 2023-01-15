using AutoMapper;
using FlightConnections.Domain.Interfaces;
using FlightConnections.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlightConnections.Controllers
{
    [ApiController]
    //[Produces("application/json")]
    [Route("/api/V1/FlightConnections")]
    public class FlightConnectionsController : ControllerBase
    {
        private readonly IGenerateMethodsCrud<FlightRoutes> _flightRoutesRepository;
        private readonly ILogger<FlightConnectionsController> _logger;

        public FlightConnectionsController(ILogger<FlightConnectionsController> logger, IGenerateMethodsCrud<FlightRoutes> flightRoutesRepository)
        {
            _logger = logger;
            _flightRoutesRepository = flightRoutesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightRoutes>>> Get()
        {

            var retorno = await _flightRoutesRepository.Get();
            return Ok(retorno);
        }

        [HttpGet("{start}/{finish}")]
        public async Task<ActionResult<FlightRoutes>> Get(string start, string finish) // trazer específico a rota marba
        {
            var flightRoutes = await _flightRoutesRepository.Get();

            for (int n = 0; n < flightRoutes.Count(); n++)
            {
                var routes = flightRoutes.ToList();

                if (routes[n].Origin == start && routes[n].Destiny == finish)
                {
                    return Ok(routes[n]);
                }
                else
                {
                    CalculateBestRoute(routes[n].Origin, routes[n].Destiny);
                }
            }

            return Ok(flightRoutes);

        }

        public string CalculateBestRoute(string origin, string destiny)
        {

        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([Bind("Origin, Destiny, Value")][FromBody] FlightRoutes model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                if (model.Origin != "" && model.Destiny != "" && model.Value != "")
                {
                    var flightRoutes = await _flightRoutesRepository.Create(model);
                    return Ok(flightRoutes);
                }
                return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put(FlightRoutes m, int id)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                m.Id = id;
                if (m.Origin != "" && m.Destiny != "" && m.Value != "")
                {
                    var flightRoutes = await _flightRoutesRepository.Update(m);
                    return Ok(flightRoutes);
                }
                return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var flightRoutes = await _flightRoutesRepository.Delete(id);
            return Ok(flightRoutes);
        }
    }
}
