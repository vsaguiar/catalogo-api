using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext contexto) : base(contexto)
    {
    }

    public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
    {
        return Get()
            .OrderBy(on => on.Nome)
            .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
            .Take(produtosParameters.PageSize)
            .ToList();
    }

    public IEnumerable<Produto> GetProdutosPorPrecos()
    {
        return Get().OrderBy(p => p.Preco).ToList();
    }
}
