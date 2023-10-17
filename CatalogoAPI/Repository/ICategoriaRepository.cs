using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repository;

public interface ICategoriaRepository : IRepository<Categoria>
{
    IEnumerable<Categoria> GetCategoriasProdutos();

    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
}
