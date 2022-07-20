using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class CategoriesModel : PageModel
{
    public string Message { get; set; } = "Error";
    public List<Models.Category> Categories { get; set; } = new();

    public async Task OnGet()
    {
        Categories = await Requests.ListCategories();
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
        Categories = await Requests.ListCategories();
        Console.WriteLine($"{id}");
        Message = $"Update handler fired {id}";
    }

    public async Task OnPostDelete(Guid id)
    {
        Categories = await Requests.ListCategories();
        Console.WriteLine($"{id}");
        Message = $"Delete handler fired {id}";
    }
}
