using CatalogoAPI.Context;
using CatalogoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Repository;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext contexto) : base(contexto) { }

    public IEnumerable<Categoria> GetCategoriasProdutos()
    {
        return Get().Include(c => c.Produtos);
    }
}
