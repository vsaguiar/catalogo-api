using AutoMapper;
using CatalogoAPI.Context;
using CatalogoAPI.Controllers;
using CatalogoAPI.DTOs;
using CatalogoAPI.DTOs.Mappings;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CatalogoApixUnitTests;

public class CategoriasUnitTestController
{
    private IMapper mapper;
    private IUnitOfWork repository;

    public static DbContextOptions<AppDbContext> dbContextOptions {  get; }


    #region String de Conexão
    public static string connectionString = "Server=localhost;DataBase=CatalogoDB;Uid=root;Pwd=root";

    // O construtor estático é chamado automaticamente antes que a primeira instância seja criada.
    static CategoriasUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion
            .AutoDetect(connectionString))
            .Options;
    }
    #endregion


    public CategoriasUnitTestController()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        mapper = config.CreateMapper();

        var context = new AppDbContext(dbContextOptions);

        repository = new UnitOfWork(context);
    }



    //======================================= TESTES UNITÁRIOS =======================================

    #region Testar o método GET
    [Fact]
    public async void GetCategorias_Return_OkResult()
    {
        // Arrange
        var controller = new CategoriasController(repository, mapper);

        CategoriasParameters parameters = new CategoriasParameters()
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act 
        var data = await controller.Get(parameters);

        // Assert
        Assert.IsType<List<CategoriaDTO>>(data.Value);
    }
    #endregion


    #region Testar o método GET - BadRequest
    [Fact]
    public async void GetCategorias_Return_BadRequestResult()
    {
        // Arrange
        var controller = new CategoriasController(repository, mapper);

        CategoriasParameters parameters = new CategoriasParameters()
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act 
        var data = await controller.Get(parameters);

        // Assert
        Assert.IsType<BadRequestResult>(data.Result);
    }
    #endregion


    #region GET - retornar uma lista de objetos categoria
    [Fact]
    public async void GetCategorias_MatchResult()
    {
        // Arrange
        var controller = new CategoriasController(repository, mapper);

        CategoriasParameters parameters = new CategoriasParameters()
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act 
        var data = await controller.Get(parameters);

        // Assert
        Assert.IsType<List<CategoriaDTO>>(data.Value);
        var cat = data.Value.Should().BeAssignableTo<List<CategoriaDTO>>().Subject;

        Assert.Equal("Bebidas", cat[0].Nome);
        Assert.Equal("bebidas.jpg", cat[0].ImagemUrl);

        Assert.Equal("Sobremesas", cat[2].Nome);
        Assert.Equal("sobremesas.jpg", cat[2].ImagemUrl);
    }
    #endregion


    #region GET - retorna um objeto CategoriaDTO por id
    [Fact]
    public async void GetCategoriasById_Return_OkResult()
    {
        // Arrange
        var controller = new CategoriasController(repository, mapper);
        var catId = 2;

        // Act 
        var data = await controller.Get(catId);

        // Assert
        Assert.IsType<CategoriaDTO>(data.Value);
    }
    #endregion


    #region GET por id - NotFound
    [Fact]
    public async void GetCategoriasById_Return_NotFoundResult()
    {
        // Arrange
        var controller = new CategoriasController(repository, mapper);
        var catId = 20;

        // Act 
        var data = await controller.Get(catId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(data.Result);
    }
    #endregion


    #region POST - CreateResult
    [Fact]
    public async void Post_Categoria_AddValidData_Return_CreatedResult()
    {
        // Arrange
        var controller = new CategoriasController(repository, mapper);

        var cat = new CategoriaDTO()
        {
            Nome = "Teste Unitário Inclusão",
            ImagemUrl = "testecat.jpg"
        };

        // Act 
        var data = await controller.Post(cat);

        // Assert
        Assert.IsType<CreatedAtRouteResult>(data);
    }
    #endregion


    #region PUT - Alterar o objeto categoria
    [Fact]
    public async void Put_Categoria_Update_ValidData_Return_OkResult()
    {
        // Arrange
        var controller = new CategoriasController(repository, mapper);
        var catId = 7;

        // Act 
        var existingPost = await controller.Get(catId);
        var result = existingPost.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;

        var catDto = new CategoriaDTO();
        catDto.CategoriaId = catId;
        catDto.Nome = "Testando Update (PUT)";
        catDto.ImagemUrl = result.ImagemUrl;

        var updateData = await controller.Put(catId, catDto);

        // Assert
        Assert.IsType<OkObjectResult>(updateData);
    }
    #endregion


    #region DELETE - Alterar o objeto categoria por id
    [Fact]
    public async void Delete_Categoria_Return_OkResult()
    {
        // Arrange
        var controller = new CategoriasController(repository, mapper);
        var catId = 7;

        // Act 
        var data = await controller.Delete(catId);

        // Assert
        Assert.IsType<CategoriaDTO>(data.Value);
    }
    #endregion
}
