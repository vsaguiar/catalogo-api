using AutoMapper;
using CatalogoAPI.DTOs;
using CatalogoAPI.Filters;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatalogoAPI.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
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
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
    {
        var produtos = await _uof.ProdutoRepository.GetProdutosPorPrecos();
        var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
        return produtosDTO;
    }


    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameters);

        if (produtos is null)
        {
            return NotFound("Produtos não encontrados...");
        }

        var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious

        };

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

        var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }


    [HttpGet("{id:int:min(1)}", Name ="ObterProduto")] // Restrição de rota com o valor mínimo para evitar consulta desnecessária no banco
    public async Task<ActionResult<ProdutoDTO>> Get(int id)
    {
        var produtos = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produtos is null)
        {
            return NotFound("Produto não encontrado.");
        }

        var produtosDTO = _mapper.Map<ProdutoDTO>(produtos);

        return produtosDTO;
    }


    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ProdutoDTO produtoDTO)
    {
        var produto = _mapper.Map<Produto>(produtoDTO);
        if (produto is null)
        {
            return BadRequest();
        }
        _uof.ProdutoRepository.Add(produto);
        await _uof.Commit();

        var produtoDto = _mapper.Map<ProdutoDTO>(produto);

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDto);
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDTO)
    {
        if (id != produtoDTO.ProdutoId)
        {
            return BadRequest();
        }

        var produto = _mapper.Map<Produto>(produtoDTO);

        _uof.ProdutoRepository.Update(produto);
        await _uof.Commit();

        return Ok(produto);
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> Delete(int id)
    {
        var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não localizado.");
        }

        _uof.ProdutoRepository.Delete(produto);
        await _uof.Commit();

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

        return produtoDTO;
    }

}
