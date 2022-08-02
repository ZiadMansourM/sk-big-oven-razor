﻿using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
// ConfigureServices
builder.Services.AddCors(options =>
{
    options.AddPolicy("AnyOrigin", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("AnyOrigin");
app.UseSwagger(
    options =>
    {
        options.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            swagger.Servers = new List<Microsoft.OpenApi.Models.OpenApiServer>
            {
                // You can add as many OpenApiServer instances as you want by creating them like below
                new Microsoft.OpenApi.Models.OpenApiServer
                {
                    // You can set the Url from the default http request data or by hard coding it
                    // Url = $"{httpReq.Scheme}://{httpReq.Host.Value}",
                    Url = "http://localhost/api",
                    Description = "Backend server"
                }
            };
        }); 
    }
);

app.UseSwaggerUI();

Main.Router(app);
Recipe.Router(app);
Category.Router(app);

app.Run();

public partial class Program
{
    public static readonly IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
}

public static class Main
{
    private static IResult Index() => Results.Json(new { message = "Home Page ^^" });
    private static IResult About() => Results.Json(new { message = "About page!" });

    public static void Router(IEndpointRouteBuilder router)
    {
        router.MapGet("/", Index);
        router.MapGet("/about", About);
    }
}

public static class Recipe
{
    private static readonly Backend.Services.JsonService _service = new(
        Program.config["RecipesPath"],
        Program.config["CategoriesPath"]
    );

    public static void Router(IEndpointRouteBuilder router)
    {
        router.MapGet("/recipes", ListRecipes);
        router.MapPost("/recipes", CreateRecipe);
        router.MapGet("/recipes/{id:guid}", GetRecipe);
        router.MapPut("/recipes/{id:guid}", UpdateRecipe);
        router.MapDelete("/recipes/{id:guid}", DeleteRecipe);
    }

    private static IResult ListRecipes()
    {
        return Results.Json(_service.ListRecipes(), statusCode: 200);
    }

    private static IResult CreateRecipe([FromBody] Backend.Models.Recipe recipe)
    {
        Backend.Models.RecipeValidator validator = new();
        ValidationResult results = validator.Validate(
            new Backend.Models.Recipe(
                recipe.Name,
                recipe.Ingredients,
                recipe.Instructions,
                recipe.CategoriesIds
            )
        );
        if (results.IsValid)
        {
            return Results.Json(
                _service.CreateRecipe(
                    recipe.Name,
                    recipe.Ingredients,
                    recipe.Instructions,
                    recipe.CategoriesIds
                ),
                statusCode: 200
            );
        }
        else
        {
            List<string> msgs = new();
            foreach (var failure in results.Errors)
                msgs.Add(
                    $"Property {failure.PropertyName}: {failure.ErrorMessage}"
                );
            return Results.Json(
                msgs,
                statusCode: 400
            );
        }
    }

    private static IResult GetRecipe(Guid id)
    {
        return Results.Json(_service.GetRecipe(id), statusCode: 200);
    }

    private static IResult UpdateRecipe(Guid id, [FromBody] Backend.Models.Recipe recipe)
    {
        Backend.Models.RecipeValidator validator = new();
        ValidationResult results = validator.Validate(
            new Backend.Models.Recipe(
                recipe.Name,
                recipe.Ingredients,
                recipe.Instructions,
                recipe.CategoriesIds
            )
        );
        if (results.IsValid)
        {
            return Results.Json(
                _service.UpdateRecipe(
                    id,
                    recipe.Name,
                    recipe.Ingredients,
                    recipe.Instructions,
                    recipe.CategoriesIds
                ),
                statusCode: 200
            );
        }
        else
        {
            List<string> msgs = new();
            foreach (var failure in results.Errors)
                msgs.Add(
                    $"Property {failure.PropertyName}: {failure.ErrorMessage}"
                );
            return Results.Json(
                msgs,
                statusCode: 400
            );
        }
    }

    private static IResult DeleteRecipe(Guid id)
    {
        _service.DeleteRecipe(id);
        return Results.Json(new { message = "Deleted Successfully" }, statusCode: 200);
    }
}

public static class Category
{
    private static readonly Backend.Services.JsonService _service = new(
        Program.config["RecipesPath"],
        Program.config["CategoriesPath"]
    );

    public static void Router(IEndpointRouteBuilder router)
    {
        router.MapGet("/categories", ListCategories);
        router.MapPost("/categories", CreateCategory);
        router.MapGet("/categories/{id:guid}", GetCategory);
        router.MapPut("/categories/{id:guid}", UpdateCategory);
        router.MapDelete("/categories/{id:guid}", DeleteCategory);
    }

    private static IResult ListCategories()
    {
        return Results.Json(_service.ListCategories(), statusCode: 200);
    }

    private static IResult CreateCategory([FromBody] Backend.Models.Category category)
    {
        Backend.Models.CategoryValidator validator = new();
        ValidationResult results = validator.Validate(
            new Backend.Models.Category(category.Name)
        );
        if (results.IsValid)
        {
            return Results.Json(
                _service.CreateCategory(category.Name),
                statusCode: 200
            );
        }
        else
        {
            List<string> msgs = new();
            foreach (var failure in results.Errors)
                msgs.Add(
                    $"Property {failure.PropertyName}: {failure.ErrorMessage}"
                );
            return Results.Json(
                msgs,
                statusCode: 400
            );
        }
    }

    private static IResult GetCategory(Guid id)
    {
        return Results.Json(_service.GetCategory(id), statusCode: 200);
    }

    private static IResult UpdateCategory(Guid id, [FromBody] Backend.Models.Category category)
    {
        Backend.Models.CategoryValidator validator = new();
        ValidationResult results = validator.Validate(
            new Backend.Models.Category(category.Name)
        );
        if (results.IsValid)
        {
            return Results.Json(
                _service.UpdateCategory(
                    id,
                    category.Name
                ),
                statusCode: 200
            );
        }
        else
        {
            List<string> msgs = new();
            foreach (var failure in results.Errors)
                msgs.Add(
                    $"Property {failure.PropertyName}: {failure.ErrorMessage}"
                );
            return Results.Json(
                msgs,
                statusCode: 400
            );
        }
    }

    private static IResult DeleteCategory(Guid id)
    {
        _service.DeleteCategory(id);
        return Results.Json(
            new { message = "Deleted Successfully!" },
            statusCode: 200
        );
    }
}
