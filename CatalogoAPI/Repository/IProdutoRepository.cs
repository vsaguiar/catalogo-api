using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorPrecos();

    // Paginação
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
}
