using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListingSql.Core.Contracts;
using HotelListingSql.Core.DTOs;
using HotelListingSql.Core.Exceptions;
using HotelListingSql.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListingSql.Core.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;

    public GenericRepository(HotelListingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<T?> GetAsync(int? id)
    {
        var result = await _context.Set<T>().FindAsync(id);
        if (result is null)
        {
            throw new MyNotFoundException(typeof(T).Name, id.HasValue ? id : "No key provided!");
        }

        return result;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    // More generic version with mapping! Used in CountriesControllerV2.cs - GetPagedCountries()
    public async Task<PagedResults<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
    {
        var totalSize = await _context.Set<T>().CountAsync();

        var items = await _context.Set<T>()
            .Skip(queryParameters.StartIndex)
            .Take(queryParameters.PageSize)
            .ProjectTo<TResult>(_mapper.ConfigurationProvider)  // Mapper to DTO!!!
            .ToListAsync();

        return new PagedResults<TResult>
        {
            Items = items,
            PageNumber = queryParameters.PageNumber,
            RecordNumber = queryParameters.PageSize,
            TotalCount = totalSize
        };
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.AddAsync(entity);  // why not Set<T>.  ???
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Update(entity);  // also sets entity state to modified
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);

        if (entity == null)
        {
            throw new MyNotFoundException(typeof(T).Name, id);
        }

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(int id)
    {
        var entity = await GetAsync(id);
        return entity != null;
    }
}
