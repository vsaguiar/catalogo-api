﻿using CatalogoAPI.Context;

namespace CatalogoAPI.Repository;

public class UnitOfWork : IUnitOfWork
{
    private ProdutoRepository _produtoRepo;
    private CategoriaRepository _categoriaRepo;
    public AppDbContext _context;

    public UnitOfWork(AppDbContext contexto)
    {
        _context = contexto;
    }


    public IProdutoRepository ProdutoRepository
    {
        get
        {
            return _produtoRepo ??= new ProdutoRepository(_context);
        }
    }

    public ICategoriaRepository CategoriaRepository
    {
        get
        {
            return _categoriaRepo ??= new CategoriaRepository(_context);
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

}
