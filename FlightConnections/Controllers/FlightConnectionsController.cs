using AutoMapper;
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
        public async Task<ActionResult<IEnumerable<FlightRoutes>>> Get()
        {

            var retorno = await _flightRoutesRepository.Get();
            return Ok(retorno);
        }

        [HttpGet("{origin}/{destiny}")]
        public async Task<ActionResult<IEnumerable<FlightRoutes>>> Get(string origin, string destiny) // trazer específico a rota marba
        {
            origin = origin.ToString().ToUpper();
            destiny = destiny.ToString().ToUpper(); //metodo genérico?

            var flightOrigin = await _flightRoutesRepository.Get(origin, "origin");

            var result = CalculateBestRoute(flightOrigin, origin, destiny);

            return Ok(result); //chama método pra fazer a lógica do mais barato //CalculateBestRoute(flightOrigin);

        }

        [NonAction]
        public string CalculateBestRoute(IEnumerable<FlightRoutes> routes, string origin, string destiny) //trazer os 2?
        {
            int n = 0;

            List<FlightRoutes> results = new List<FlightRoutes>();
            Dictionary<int, FlightRoutes> graph = new Dictionary<int, FlightRoutes>();
            List<FlightRoutes> queue = new List<FlightRoutes>();
            IEnumerable<FlightRoutes> nextRout;

            var routeLine = routes.ToList();
            var routeNext = nextRout.ToList();

            foreach (var route in routeLine)
            {
                queue.Add(route);
            }

            if (routeLine[0].Destiny == destiny)
            {
                //return Ok(); //retorna a fila e o valor
            }

            

            int vlr = queue.Count;
            string newOrigin;
            int i = 0;
            int vlrNext = 0;
            StringBuilder stringBuilder = new StringBuilder();
            double preco;
            var resultFind;

            while (vlr != 0)
            {
                foreach (var route in routeLine)
                {
                    string c = route.Destiny.ToString();
                    results.Add(c);
                }                             

                //nextRout = routes;
                if (vlrNext != 0)
                {
                    foreach (var route in routeNext)
                    {
                        queue.Add(route);
                    }

                    resultFind = routeNext.Find(x => x.Destiny == destiny);
                }
                else
                {
                    resultFind = queue.Find(x => x.Destiny == destiny);
                }

                if (resultFind != null)
                {
                    
                    graph.Add(i, queue);
                    vlr = 0;
                    stringBuilder += "Caminho: ";
                    for (int b = 1; b < graph.Count; b++)
                    {
                        preco += Convert.ToDouble(graph.Values);
                        stringBuilder += graph[n].Origin.ToString() + " - ";    
                    }
                    //return Ok(); //retorna a fila e o valor
                    stringBuilder += destiny.ToString() + " - R$" + preco.ToString();
                    
                }
                else
                {
                    //graph.Add(i, queue);
                    newOrigin = results[0].Destiny;
                    results.Remove(results[0]);
                    IEnumerable<FlightRoutes> nextRout = _flightRoutesRepository.Get(newOrigin, "origin");
                    
                    vlrNext = routeNext.Count;
                    vlr = vlrNext;
                }

            }

            return stringBuilder.ToString();

            //    //bfs old
            //    while (queue.Count != 0)
            //{
            //    List<FlightRoutes> path = new List<FlightRoutes>(queue.Remove(0));

            //    List<FlightRoutes> node = new List<FlightRoutes>(path[-1]);

            //    List<FlightRoutes> neighbours = new List<FlightRoutes>(graph.Enqueue(node));

            //    foreach(var neigbour in neighbours)
            //    {
            //        if (path.Contains(neigbour)) 
            //        {
            //            continue;
            //        }

            //        List<FlightRoutes> newPath = new List<FlightRoutes>(path);

            //        newPath.Append<FlightRoutes>(path);
            //        newPath.Append<FlightRoutes>(neigbour);

            //        queue.Append<FlightRoutes>(newPath);

            //        if(neigbour == destiny)
            //        {
            //            results.Append<FlightRoutes>(newPath);
            //            n = n + 1;
            //        }
            //        else
            //        {

            //        }
            //    }
            //}

            //return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([Bind("Origin, Destiny, Value")][FromBody] FlightRoutes model)
        {
            model.Origin = model.Origin.ToString().ToUpper();
            model.Destiny = model.Destiny.ToString().ToUpper();
            //fazer de valor 

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
