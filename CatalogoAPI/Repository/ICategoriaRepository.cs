using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repository;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IEnumerable<Categoria>> GetCategoriasProdutos();

    Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParameters);
}
