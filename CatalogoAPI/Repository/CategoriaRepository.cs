using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Repository;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext contexto) : base(contexto) { }

    public async Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParameters)
    {
        return await PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.CategoriaId), categoriasParameters.PageNumber, categoriasParameters.PageSize);
    }

    public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        return await Get().Include(c => c.Produtos).ToListAsync();
    }
}
