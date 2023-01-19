using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightConnections.Domain.Interfaces
{
    public interface IGenerateMethodsCrud<T>
    {
        Task<IEnumerable<T>> Get();
        Task<IEnumerable<T>> Get(string consult, string identification);
        Task<T> Get(int id);
        Task<T> Create(T t);
        Task<T> Update(T t);
        Task<T> Delete(int id);
    }
}
