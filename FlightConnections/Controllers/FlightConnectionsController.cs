using AutoMapper;
using FlightConnections.Domain.Interfaces;
using FlightConnections.Domain.Logic;
using FlightConnections.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace FlightConnections.Controllers
{
    [ApiController]
    [Produces("application/json")]
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<FlightRoutes>>> Get()
        {
            try
            {
                var retorno = await _flightRoutesRepository.Get();
                return Ok(retorno);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<FlightRoutes>> Get(int id)
        {
            try
            {
                if (id > 0)
                {
                    var flight = await _flightRoutesRepository.Get(id);
                    return Ok(flight);
                }
                return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet("{origin}/{destiny}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<FlightRoutes>>> Get(string origin, string destiny)
        {
            try
            {
                origin = origin.ToUpper();
                destiny = destiny.ToUpper();

                BfsGraph bfs = new BfsGraph(_flightRoutesRepository);

                var returnShipRoute = bfs.PathToReturn(await bfs.BfsGraphTest(origin, destiny));

                return Ok(returnShipRoute);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([Bind("Origin, Destiny, Value")][FromBody] FlightRoutes model)
        {
            model.Origin = model.Origin.ToString().ToUpper();
            model.Destiny = model.Destiny.ToString().ToUpper();
            model.Value = Decimal.Parse(model.Value.ToString());

            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                if (model.Origin != "" && model.Destiny != "" && model.Value != null)
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
                if (m.Origin != "" && m.Destiny != "" && m.Value != null)
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var flightRoutes = await _flightRoutesRepository.Delete(id);
                return Ok(flightRoutes);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
