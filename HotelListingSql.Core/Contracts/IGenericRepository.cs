using HotelListingSql.Core.DTOs;

namespace HotelListingSql.Core.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetAsync(int? id);
    Task<List<T>> GetAllAsync();
    Task<PagedResults<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> Exists(int id);
}