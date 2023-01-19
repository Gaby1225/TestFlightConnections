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
        private readonly IGenerateMethodsCrud<FlightRoutes> _flightRoutesRepository;

        public BfsGraph(IGenerateMethodsCrud<FlightRoutes> flightRoutesRepository)
        {
            _flightRoutesRepository = flightRoutesRepository;
        }

        public async Task<Graph> getRoutesBfs(string origem)
        {
            IEnumerable<FlightRoutes> originGet = await _flightRoutesRepository.Get(origem, "origin");
            var routeList = originGet.ToList();
            var result = new Graph(origem);

            foreach (var item in routeList)
                result.Neigborhood.Enqueue(new Graph(item.Destiny) { Parent = (result, ((double)item.Value)) });

            return result;

        }
        public async Task<Graph> BfsGraphTest(string origin, string destiny)
        {
            var originGraph = await getRoutesBfs(origin);
            var neigborhoodToTest = new Queue<Graph>(originGraph.Neigborhood);
            var visiteds = new HashSet<Graph>();

            while (neigborhoodToTest.Count > 0)
            {
                var actualNeigborhood = neigborhoodToTest.Dequeue();
                if (visiteds.Contains(actualNeigborhood)) continue;
                if (actualNeigborhood.Origin == destiny) return actualNeigborhood;
                visiteds.Add(actualNeigborhood);
                var nextNode = await getRoutesBfs(actualNeigborhood.Origin);
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
                if (currentParent.Item1 is null)
                {
                    var x = string.Join(" - ", pilha.Select(x => x.Item1.Origin))
                        + $" ao custo de $ {pilha.Sum(x => x.Item2)}";
                } //retornar só se for o melhor valor - salvar em uma lista de string?

                pilha.Push((currentParent.Item1, currentParent.Item1.Parent.Item2));

                currentParent = currentParent.Item1.Parent;
            } //talvez salvar em uma lista todos que forem achados

            //fazer if pra vir o item2 mais barato - testar se fica tudo salvo na lista "pilha"
        }
    }

}
