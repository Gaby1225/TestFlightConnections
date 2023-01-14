using FlightConnections.Data.Context;
using FlightConnections.Domain.Interfaces;
using FlightConnections.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightConnections.Data.FlightRoutesToData
{
    public class FlightRoutesData : IGenerateMethodsCrud<FlightRoutes>
    {
        private readonly AppDbContext _appDbContext;

        public FlightRoutesData(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<FlightRoutes> Create(FlightRoutes flight) //testar para cadastrar vários de uma vez
        {
            flight.Id = 0;
            flight.RouteStart = "";
            flight.RouteFinish = "";
            flight.Value = 0;

            _appDbContext.FlightRoute.Add(flight);
            await _appDbContext.SaveChangesAsync();

            return flight;
        }

        public async Task<IEnumerable<FlightRoutes>> Get()
        {
            return await _appDbContext.FlightRoute.ToListAsync();
        }

        public async Task<FlightRoutes> Get(int id)
        {
            return await _appDbContext.FlightRoute.FindAsync(id);
        }

        public async Task<FlightRoutes> Update(FlightRoutes flight)
        {
            _appDbContext.FlightRoute.Update(flight);
            await _appDbContext.SaveChangesAsync();
            return flight;
        }


        public async Task<FlightRoutes> Delete(int id)
        {
            var cat = await _appDbContext.FlightRoute.FindAsync(id);
            _appDbContext.FlightRoute.Remove(cat);
            await _appDbContext.SaveChangesAsync();
            return cat;
        }


    }
}
