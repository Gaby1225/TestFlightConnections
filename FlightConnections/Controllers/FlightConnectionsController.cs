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

            var retorno = await _flightRoutesRepository.Get();
            return Ok(retorno);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightRoutes>> Get(int id)
        {
            if (id > 0)
            {
                var flight = await _flightRoutesRepository.Get(id);
                return Ok(flight);
            }
            return Ok();
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

                //IEnumerable<FlightRoutes> flightOrigin = await _flightRoutesRepository.Get(origin, "origin");


                BfsGraph bfs = new BfsGraph(_flightRoutesRepository);

                 var returnShipRoute = bfs.PathToReturn(await bfs.BfsGraphTest(origin, destiny));

                return Ok(returnShipRoute);

                //var routeLine = flightOrigin.ToList();
                //string destinyIteration = destiny;
                //string custo = "";
                //string passedDesitny = "";
                //StringBuilder sb = new StringBuilder(origin + " - ");

                //List<string> flightsDestiny = new List<string>();
                //List<FlightRoutes> passed = new List<FlightRoutes>();

                //custo += routeLine[0].Value;

                //if (routeLine[0].Destiny.ToString() == destinyIteration)
                //{
                //    sb.Append(destinyIteration + " - ao custo de $" + routeLine[0].Value);
                //    return Ok(sb.ToString());
                //}
                //else
                //{
                //    while (routeLine.Count > 0)
                //    {
                //        for (int n = 0; n < routeLine.Count; n++)
                //        {
                //            var destinyForIteration = routeLine[n].Destiny;
                //            if (destinyForIteration != destinyIteration)
                //            {
                //                bool containsDestiny = flightsDestiny.Contains(destinyForIteration);
                //                if (!containsDestiny || flightsDestiny.Count == 0)
                //                    flightsDestiny.Add(destinyForIteration);
                //            }
                //            else
                //            {
                //                passed.Add(routeLine[0]);
                //                routeLine.Remove(routeLine[0]);

                //                foreach (var item in passed)
                //                {

                //                    var itemDestiny = item.Destiny;
                //                    sb.Append(item.Origin + " - ");
                //                    if (passedDesitny != itemDestiny)
                //                        passedDesitny = itemDestiny;
                //                    sb.Append(itemDestiny + " - ");

                //                    custo += item.Value;
                //                }

                //                sb.Append("ao custo de $" + custo);

                //                return Ok(sb.ToString()); // talvez deva retornar só no final
                //            }
                //        }

                //        flightOrigin = await _flightRoutesRepository.Get(flightsDestiny[0], "origin");
                //        routeLine = flightOrigin.ToList();
                //        flightsDestiny.Remove(flightsDestiny[0]);
                //    }
                //}

                //return Ok(sb.ToString());

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
