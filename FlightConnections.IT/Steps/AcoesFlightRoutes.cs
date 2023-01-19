using FlightConnections;
using FlightConnections.Domain.Models;
using FlightConnections.IT.Steps;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Timesheet.IT.Steps
{
    [Binding]
    public class AcoesFlightRoutes : BaseStep
    {
        public AcoesFlightRoutes(WebApplicationFactory<Startup> factory) : base(factory)
        {
            httpClient = factory.CreateClient();
        }

        private List<FlightRoutes> listaRoutes = new List<FlightRoutes>();
        private List<FlightRoutes> listaId = new List<FlightRoutes>();
        private readonly HttpClient httpClient;
        private HttpResponseMessage retornoGetById;

        [Given(@"as seguintes informações para cadastro de rotas:")]

        public void DadoAsSeguintesInformacoesParaCadastroDeRotas(Table table)
        {
            DeleteAll();

            listaRoutes = table.CreateSet<FlightRoutes>().ToList();//testar com o if aqui
        }

        [When(@"cadastrar as rotas")]
        public async Task QuandoCadastrarAsRotas()
        {
            for (int n = 0; n < listaRoutes.Count(); n++)
            {
                var content = new StringContent(JsonSerializer.Serialize(listaRoutes[n]), Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync("api/V1/FlightConnections", content);
            }
        }

        [Then(@"devem existir as rotas cadastradas")]
        public async Task EntaoDevemExistirAsRotasCadastradas(Table table)
        {
            var result = await httpClient.GetAsync("api/V1/FlightConnections");
            var content = await result.Content.ReadAsStringAsync();
            var routesResult = JsonSerializer.Deserialize<IEnumerable<FlightRoutes>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            table.CreateSet<FlightRoutes>().Should().BeEquivalentTo(routesResult);
        }

        [Given(@"as seguintes informações para a consulta de todas as rotas:")]
        public void DadoAsSeguintesInformacoesParaAConsultaDeTodasAsRotas(Table table)
        {
            DeleteAll();

            listaRoutes = table.CreateSet<FlightRoutes>().ToList();
        }

        [When(@"consultar todas as rotas cadastradas")]
        public async Task QuandoConsultarTodasAsRotasCadastradas()
        {
            for (int n = 0; n < listaRoutes.Count(); n++)
            {
                var content = new StringContent(JsonSerializer.Serialize(listaRoutes[n]), Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync("api/V1/FlightConnections", content);
            }
        }

        [Then(@"devem existir as rotas consultadas")]
        public async Task EntaoDevemExistirAsRotasConsultadas(Table table)
        {
            var result = await httpClient.GetAsync("api/V1/FlightConnections");
            var content = await result.Content.ReadAsStringAsync();
            var categoriasResult = JsonSerializer.Deserialize<IEnumerable<FlightRoutes>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            table.CreateSet<FlightRoutes>().Should().BeEquivalentTo(categoriasResult);
        }

        [Given(@"as seguintes informações para exclusao por id:")]
        public async Task DadoAsSeguintesInformacoesParaExclusaoPorId(Table table)
        {
            DeleteAll();

            listaRoutes = table.CreateSet<FlightRoutes>().ToList();

            for (int n = 0; n < listaRoutes.Count(); n++)
            {
                var content = new StringContent(JsonSerializer.Serialize(listaRoutes[n]), Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync("api/V1/FlightConnections", content);
            }
        }

        [When(@"excluir a rota por id")]
        public async Task QuandoExcluirARotaPorId(Table table)
        {
            listaId = table.CreateSet<FlightRoutes>().ToList();
            var retornoId = await httpClient.DeleteAsync($"api/V1/FlightConnections/{listaId[0].Id}");
            listaRoutes.Remove(listaId[0]);
        }

        [Then(@"devem existir apenas os seguintes retornos")]
        public async Task EntaoDevemExistirApenasOsSeguintesRetornos(Table table)
        {

            var result = await httpClient.GetAsync("api/V1/FlightConnections");
            var content = await result.Content.ReadAsStringAsync();
            var categoriasResult = JsonSerializer.Deserialize<IEnumerable<FlightRoutes>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            table.CreateSet<FlightRoutes>().Should().BeEquivalentTo(categoriasResult);
        }

        [Given(@"as seguintes informações para edição por id:")]
        public async Task DadoAsSeguintesInformacoesParaEdicaoPorId(Table table)
        {
            DeleteAll();

            listaRoutes = table.CreateSet<FlightRoutes>().ToList();

            for (int n = 0; n < listaRoutes.Count(); n++)
            {
                var content = new StringContent(JsonSerializer.Serialize(listaRoutes[n]), Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync("api/V1/FlightConnections", content);
            }
        }

        [When(@"editar a rota por id")]
        public async Task QuandoEditarARotaPorId(Table table)
        {
            listaId = table.CreateSet<FlightRoutes>().ToList();

            var id = listaId[0].Id;

            //ver se existe para só depois alterar
            var retornoId = await httpClient.GetAsync($"api/V1/FlightConnections/{id}");
            var content = new StringContent(JsonSerializer.Serialize(listaId[0]), Encoding.UTF8, "application/json");
            var result = await httpClient.PutAsync($"api/V1/FlightConnections/{id}", content);

        }

        //editar na lista
        [Then(@"devem existir todas as rotas e a editada")]
        public async Task EntaoDevemExistirTodasAsRotasEAEditada(Table table)
        {
            var result = await httpClient.GetAsync("api/V1/FlightConnections");
            var content = await result.Content.ReadAsStringAsync();
            var categoriasResult = JsonSerializer.Deserialize<IEnumerable<FlightRoutes>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            table.CreateSet<FlightRoutes>().Should().BeEquivalentTo(categoriasResult);
        }

        [Given(@"as seguintes informações para a consulta de rota por id:")]
        public async Task DadoAsSeguintesInformacoesParaAConsultaDeRotasPorId(Table table)
        {
            DeleteAll();

            listaRoutes = table.CreateSet<FlightRoutes>().ToList();

            for (int n = 0; n < listaRoutes.Count(); n++)
            {
                var content = new StringContent(JsonSerializer.Serialize(listaRoutes[n]), Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync("api/V1/FlightConnections", content);
            }
        }

        [When(@"eu consultar rota cadastrada por id")]
        public async Task QuandoConsultarRotasCadastradaPorId(Table table)
        {
            listaId = table.CreateSet<FlightRoutes>().ToList();
            var retornoId = await httpClient.GetAsync($"api/V1/FlightConnections/{listaId[0].Id}");
            retornoGetById = retornoId;

        }

        [Then(@"deve existir a rota consultada por id")]
        public async Task EntaoDeveExistirARotaConsultadaPorId(Table table)
        {
            var content = await retornoGetById.Content.ReadAsStringAsync();
            var categoriasResult = JsonSerializer.Deserialize<FlightRoutes>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            table.CreateSet<FlightRoutes>().FirstOrDefault().Should().BeEquivalentTo(categoriasResult);
        }
    }
}
