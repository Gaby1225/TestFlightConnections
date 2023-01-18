using FlightConnections.Domain.Interfaces;
using FlightConnections.Domain.Models;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightConnections.Domain.Logic
{
    public class BfsGraph
    {
        private readonly IGenerateMethodsCrud<FlightRoutes> _flightRoutesRepository; // = new List<FlightRoutes>();

        public BfsGraph(IGenerateMethodsCrud<FlightRoutes> flightRoutesRepository)
        {
            _flightRoutesRepository = flightRoutesRepository;
        }

        public Graph getRoutesBfs(string origem)
        {
            IEnumerable<FlightRoutes> originGet = (IEnumerable<FlightRoutes>)_flightRoutesRepository.Get(origem, "origin");
            var routeList = originGet.ToList();
            var result = new Graph(origem);

            foreach (var item in routeList)
                result.Neigborhood.Enqueue(new Graph(item.Destiny) { Parent = (result, item.Value) });

            return result;

        }
        public Graph BfsGraphTest(string origin, string destiny)
        {
            var originGraph = getRoutesBfs(origin); //não pode passar os dados da lista
            var neigborhoodToTest = new Queue<Graph>(originGraph.Neigborhood);
            var visiteds = new HashSet<Graph>();

            while (neigborhoodToTest.Count > 0)
            {
                var actualNeigborhood = neigborhoodToTest.Dequeue();
                if (visiteds.Contains(actualNeigborhood)) continue;
                if (actualNeigborhood.Nome == destiny) return actualNeigborhood;
                visiteds.Add(actualNeigborhood);
                var nextNode = getRoutesBfs(actualNeigborhood.Nome);
                nextNode.Parent = actualNeigborhood.Parent;
                foreach (var item in nextNode.Neigborhood) neigborhoodToTest.Enqueue(item);
            }

            return null;
        }

        public string PathToReturn(Graph graph)
        {
            var pilha = new Stack<(Graph, double)>();
            pilha.Push((graph, graph.Parent.Item2));
            var currentParent = graph.Parent;

            while (true)
            {
                if (currentParent.Item1 is null) return string.Join(" - ", pilha.Select(x => x.Item1.Nome)) +
                        Environment.NewLine + $"Distância total: {pilha.Sum(x => x.Item2)}";
                pilha.Push((currentParent.Item1, currentParent.Item1.Parent.Item2));

                currentParent = currentParent.Item1.Parent;
            }
        }
    }

}
