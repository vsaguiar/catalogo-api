using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repository;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IEnumerable<Categoria>> GetCategoriasProdutos();

    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
}
