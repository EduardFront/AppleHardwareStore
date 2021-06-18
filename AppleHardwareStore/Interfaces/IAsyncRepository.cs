using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppleHardwareStore.Interfaces
{
    public interface IAsyncRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task<List<T>> AddAsyncRange(List<T> entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> CountAllAsync();
    }
}
