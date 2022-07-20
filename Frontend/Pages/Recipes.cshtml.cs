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

    public async Task OnGet()
    {
        Recipes = await Requests.ListRecipes();
        Message = "List handler fired";
    }

    public void OnPostCreate()
    {
        Message = "Create handler fired";
    }

    public void OnPostDetail(int id)
    {
        Message = $"Get handler fired {id}";
    }

    public async Task OnPostUpdate(Guid id)
    {
        Recipes = await Requests.ListRecipes();
        Message = $"Update handler fired {id}";
    }

    public async Task OnPostDelete(Guid id)
    {
        Recipes = await Requests.ListRecipes();
        Message = $"Delete handler fired {id}";
    }
}
