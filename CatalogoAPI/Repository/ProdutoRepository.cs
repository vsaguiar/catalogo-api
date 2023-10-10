using CatalogoAPI.Context;
using CatalogoAPI.Models;

namespace CatalogoAPI.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext contexto) : base(contexto)
    {
    }

    public IEnumerable<Produto> GetProdutosPorPrecos()
    {
        return Get().OrderBy(p => p.Preco).ToList();
    }
}
