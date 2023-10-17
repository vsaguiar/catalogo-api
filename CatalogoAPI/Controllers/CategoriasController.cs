﻿using AutoMapper;
using CatalogoAPI.DTOs;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatalogoAPI.Controllers
{
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
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriaProdutos()
        {
            var categorias = _context.CategoriaRepository.GetCategoriasProdutos().ToList();
            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDTO;
        }


        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = _context.CategoriaRepository.GetCategorias(categoriasParameters);

            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious

            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDTO;
        }


        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _context.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("Categoria não localizada.");
            }

            var categoriasDTO = _mapper.Map<CategoriaDTO>(categoria);

            return categoriasDTO;
        }


        [HttpPost]
        public ActionResult Post([FromBody] CategoriaDTO categoriaDTO)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            if (categoria is null)
            {
                return BadRequest("Dados inválidos!");
            }

            _context.CategoriaRepository.Add(categoria);
            _context.Commit();

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDto);
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                return BadRequest("Dados inválidos!");
            }

            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            _context.CategoriaRepository.Update(categoria);
            _context.Commit();

            return Ok(categoria);
        }


        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _context.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não localizada.");
            }

            _context.CategoriaRepository.Delete(categoria);
            _context.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return categoriaDTO;
        }

    }
}
