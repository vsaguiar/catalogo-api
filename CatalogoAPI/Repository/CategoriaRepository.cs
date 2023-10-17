using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Repository;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext contexto) : base(contexto) { }

    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
    {
        return PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.CategoriaId), categoriasParameters.PageNumber, categoriasParameters.PageSize);
    }

    public IEnumerable<Categoria> GetCategoriasProdutos()
    {
        return Get().Include(c => c.Produtos);
    }
}
