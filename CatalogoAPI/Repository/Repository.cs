﻿using CatalogoAPI.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogoAPI.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    // Injetando uma instância do meu contexto
    protected AppDbContext _context;
    public Repository(AppDbContext contexto)
    {
        _context = contexto;
    }

    public IQueryable<T> Get()
    {
        return _context.Set<T>().AsNoTracking();
    }

    public async Task<T> GetById(Expression<Func<T,bool>> predicate)
    {
        return await _context.Set<T>().SingleOrDefaultAsync(predicate);
    }
    
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified; // informando ao contexto (_context) que a entidade foi alterada
        _context.Set<T>().Update(entity);
    }

}
