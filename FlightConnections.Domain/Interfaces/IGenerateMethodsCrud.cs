﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightConnections.Domain.Interfaces
{
    public interface IGenerateMethodsCrud<T>
    {
        Task<IEnumerable<T>> Get();
        Task<T> Get(string start, string finish);
        Task<T> Create(T t);
        Task<T> Update(T t);
        Task<T> Delete(int id);
    }
}
