using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class RecipesModel : PageModel
{
    public string Message { get; set; } = "Error";
    public List<Models.Recipe> Recipes { get; set; } = new();
    public List<Models.Category> Categories { get; set; } = new();
    public List<List<string>> CategoriesNames { get; set; } = new();

    public async Task OnGet()
    {
        Recipes = await Requests.ListRecipes();
        // Map recipe.CategoriesIds to Names
        Categories = await Requests.ListCategories();
        CategoriesNames = GetCategoriesNames(Recipes, Categories);
        Message = "List handler fired";
    }

    public async Task OnPostCreate()
    {
        // Start
        string name = Request.Form["name"];
        String[] spearator = {"- ", "\n"};
        string ingredients = Request.Form["ingredients"];
        List<string> ingredientslist = ingredients.Split(spearator,
           StringSplitOptions.RemoveEmptyEntries).ToList();
        string instructions = Request.Form["instructions"];
        List<string> instructionslist = instructions.Split(spearator,
           StringSplitOptions.RemoveEmptyEntries).ToList();
        List<Guid> guidIds = new();
        foreach (var guid in Request.Form["categoriesIds"])
            guidIds.Add(new Guid(guid));
        await Requests.CreateRecipe(name, ingredientslist, instructionslist, guidIds);
        Message = "Create handler fired";
        Recipes = await Requests.ListRecipes();
        // Map recipe.CategoriesIds to Names
        Categories = await Requests.ListCategories();
        CategoriesNames = GetCategoriesNames(Recipes, Categories);
    }

    public void OnPostDetail(int id)
    {
        Message = $"Get handler fired {id}";
    }

    public async Task OnPostUpdate(Guid id)
    {
        Recipes = await Requests.ListRecipes();
        // Map recipe.CategoriesIds to Names
        Categories = await Requests.ListCategories();
        CategoriesNames = GetCategoriesNames(Recipes, Categories);
        Message = $"Update handler fired {id}";
    }

    public async Task OnPostDelete(Guid id)
    {
        Recipes = await Requests.ListRecipes();
        // Map recipe.CategoriesIds to Names
        Categories = await Requests.ListCategories();
        CategoriesNames = GetCategoriesNames(Recipes, Categories);
        Message = $"Delete handler fired {id}";
    }

    private static List<List<string>> GetCategoriesNames(List<Models.Recipe> Recipes, List<Models.Category> categories)
    {
        Dictionary<Guid, string> catDict = new();
        foreach (Models.Category cat in categories)
            catDict.Add(cat.Id, cat.Name);
        List<List<string>> catNames = new();
        foreach(Models.Recipe recipe in Recipes)
        {
            List<string> names = new();
            foreach (var id in recipe.CategoriesIds)
                names.Add(catDict[id]);
            catNames.Add(names);
        }
        return catNames;
    }
}
