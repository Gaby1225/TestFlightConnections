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

        public async Task<FlightRoutes> Create(FlightRoutes flight)
        {
            flight.Id = 0;

            _appDbContext.Connections.Add(flight);
            await _appDbContext.SaveChangesAsync();

            return flight;
        }

        public async Task<IEnumerable<FlightRoutes>> Get()
        {
            return await _appDbContext.Connections.ToListAsync();
        }

        public async Task<IEnumerable<FlightRoutes>> Get(string consult, string identification) //ajustar para o certo
        {
            if (identification == "destiny")
            {
                return await _appDbContext.Connections.Where(x => x.Destiny == consult).ToListAsync();
            }
            else
            {
                try
                {
                    var a = await _appDbContext.Connections.Where(x => x.Origin == consult).ToListAsync();
                    return a;
                }
                catch (Exception e)
                {
                    return null;
                }
                
            }
        }

        public async Task<FlightRoutes> Get(int id)
        {
            return await _appDbContext.Connections.FindAsync(id);
        }

        public async Task<FlightRoutes> Update(FlightRoutes flight)
        {
            _appDbContext.Connections.Update(flight);
            await _appDbContext.SaveChangesAsync();
            return flight;
        }

        
        public async Task<FlightRoutes> Delete(int id)
        {
            var flight = await _appDbContext.Connections.FindAsync(id);
            _appDbContext.Connections.Remove(flight);
            await _appDbContext.SaveChangesAsync();
            return flight;
        }


    }
}
