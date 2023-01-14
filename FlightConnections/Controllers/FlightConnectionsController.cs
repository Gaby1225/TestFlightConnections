using FlightConnections.Domain.Interfaces;
using FlightConnections.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightConnections.Controllers
{
    [ApiController]
    [Route("/api/V1/flight")]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightRoutes>> Get(int id)
        {
            if (id > 0)
            {
                var flightRoutes = await _flightRoutesRepository.Get(id);
                return Ok(flightRoutes);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post(FlightRoutes model)
        {
            if (model.RouteStart != "" && model.RouteFinish != "" && model.Value != 0)
            {
                //var flightRoutes = await _flightRoutesRepository.Create(model.RouteStart , model.RouteFinish, model.Value);
                var flightRoutes = await _flightRoutesRepository.Create(model);
                return Ok(flightRoutes);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(FlightRoutes m, int id)
        {
            m.Id = id;
            if (m.RouteStart != "" && m.RouteFinish != "" && m.Value != 0)
            {
                //var flightRoutes = await _flightRoutesRepository.Update(m.RouteStart, m.RouteFinish, m.Value);
                var flightRoutes = await _flightRoutesRepository.Update(m);
                return Ok(flightRoutes);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var flightRoutes = await _flightRoutesRepository.Delete(id);
            return Ok(flightRoutes);
        }
    }
}
