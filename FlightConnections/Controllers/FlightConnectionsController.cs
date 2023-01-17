﻿using AutoMapper;
using FlightConnections.Domain.Interfaces;
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
        public Dictionary<FlightRoutes, HashSet<FlightRoutes>> AdjacencyList { get; } = new Dictionary<FlightRoutes, HashSet<FlightRoutes>>();

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

        /// <summary>
        /// Get all informations from database
        /// </summary>
        /// <remarks>
        /// Funciona com no máximo 3 caminhos: SCL - ORL - CDG :'C
        /// </remarks>
        /// <param name="destiny">string com 3 letras</param>
        /// <param name="origin">string com 3 letras</param>
        [HttpGet("{origin}/{destiny}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<FlightRoutes>>> Get(string origin, string destiny)
        {
            try
            {
                origin = origin.ToString().ToUpper();
                destiny = destiny.ToString().ToUpper();                

                IEnumerable<FlightRoutes> flightOrigin = await _flightRoutesRepository.Get(origin, "origin");

                var routeLine = flightOrigin.ToList();
                string destinyIteration = destiny;
                string custo = "";
                string passedDesitny = "";
                StringBuilder sb = new StringBuilder(origin + " - ");
                
                List<string> flightsDestiny = new List<string>();
                List<FlightRoutes> passed = new List<FlightRoutes>();

                custo += routeLine[0].Value; //mudar pra double  > não é necesariamente na posição 0 

                if (routeLine[0].Destiny.ToString() == destinyIteration)
                {
                    sb.Append(destinyIteration + " - ao custo de $" + routeLine[0].Value); //converter pra double
                    return Ok(sb);
                }
                else
                {
                    while (routeLine.Count > 0)
                    {
                        for (int n = 0; n < routeLine.Count; n++)
                        {
                            var destinyForIteration = routeLine[n].Destiny;
                            if (destinyForIteration != destinyIteration)
                            {
                                bool containsDestiny = flightsDestiny.Contains(destinyForIteration);
                                if (!containsDestiny || flightsDestiny.Count == 0)
                                    flightsDestiny.Add(destinyForIteration);
                            }
                            else
                            {
                                passed.Add(routeLine[0]);
                                routeLine.Remove(routeLine[0]);

                                foreach (var item in passed)
                                {

                                    var itemDestiny = item.Destiny;
                                    sb.Append(item.Origin + " - ");
                                    if (passedDesitny != itemDestiny)
                                        passedDesitny = itemDestiny;
                                    sb.Append(itemDestiny + " - ");

                                    custo += item.Value;
                                }

                                sb.Append("ao custo de $" + custo);

                                // return Ok(sb.ToString()); // talvez deva retornar só no final
                            }
                        }

                        flightOrigin = await _flightRoutesRepository.Get(flightsDestiny[0], "origin");
                        routeLine = flightOrigin.ToList();
                        flightsDestiny.Remove(flightsDestiny[0]);
                    }
                }

                return Ok(sb.ToString());

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
            model.Value = Double.Parse(model.Value.ToString());

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
