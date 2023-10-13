using AutoMapper;
using CatalogoAPI.DTOs;
using CatalogoAPI.Filters;
using CatalogoAPI.Models;
using CatalogoAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{

    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;
    public ProdutosController(IUnitOfWork context, IMapper mapper)
    {
        _uof = context;
        _mapper = mapper;
    }


    [HttpGet("menorpreco")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
    {
        var produtos = _uof.ProdutoRepository.GetProdutosPorPrecos().ToList();
        var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
        return produtosDTO;
    }


    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        var produtos = _uof.ProdutoRepository.Get().ToList();

        if (produtos is null)
        {
            return NotFound("Produtos não encontrados...");
        }

        var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

        return produtosDTO;
    }


    [HttpGet("{id:int:min(1)}", Name ="ObterProduto")] // Restrição de rota com o valor mínimo para evitar consulta desnecessária no banco
    public ActionResult<ProdutoDTO> Get(int id)
    {
        var produtos = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produtos is null)
        {
            return NotFound("Produto não encontrado.");
        }

        var produtosDTO = _mapper.Map<ProdutoDTO>(produtos);

        return produtosDTO;
    }


    [HttpPost]
    public ActionResult Post([FromBody] ProdutoDTO produtoDTO)
    {
        var produto = _mapper.Map<Produto>(produtoDTO);
        if (produto is null)
        {
            return BadRequest();
        }
        _uof.ProdutoRepository.Add(produto);
        _uof.Commit();

        var produtoDto = _mapper.Map<ProdutoDTO>(produtoDTO);

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDto);
    }


    [HttpPut("{id:int}")]
    public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDTO)
    {
        if (id != produtoDTO.ProdutoId)
        {
            return BadRequest();
        }

        var produto = _mapper.Map<Produto>(produtoDTO);

        _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        return Ok(produto);
    }


    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não localizado.");
        }

        _uof.ProdutoRepository.Delete(produto);
        _uof.Commit();

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

        return produtoDTO;
    }

}
