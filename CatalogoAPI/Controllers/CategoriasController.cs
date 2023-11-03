using AutoMapper;
using CatalogoAPI.DTOs;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatalogoAPI.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiConventionType(typeof(DefaultApiConventions))] // o conjunto padrão de convenções (tipos de retorno. ex: 200, 404 etc) é aplicado a todas as Actions
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriaProdutos()
        {
            var categorias = await _context.CategoriaRepository.GetCategoriasProdutos();
            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDTO;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                var categorias = await _context.CategoriaRepository.GetCategorias(categoriasParameters);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious

                };

                Response?.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

                if (categorias is null)
                    return NotFound("Categoria não encontrada.");

                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDTO;
            }
            catch (Exception)
            {

                return BadRequest();
            }
            
        }


        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _context.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não localizada.");
            }

            var categoriasDTO = _mapper.Map<CategoriaDTO>(categoria);

            return categoriasDTO;
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDTO)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            if (categoria is null)
            {
                return BadRequest("Dados inválidos!");
            }

            _context.CategoriaRepository.Add(categoria);
            await _context.Commit();

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDto);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                return BadRequest("Dados inválidos!");
            }

            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            _context.CategoriaRepository.Update(categoria);
            await _context.Commit();

            return Ok(categoria);
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _context.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não localizada.");
            }

            _context.CategoriaRepository.Delete(categoria);
            await _context.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return categoriaDTO;
        }

    }
}
