using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FluentValidation.Results;

namespace Frontend.Pages;

public class CategoriesModel : PageModel
{
    public List<Models.Category> Categories { get; set; } = new();
    public List<string> Messages = new();

    public async Task OnGet(List<string> msgs)
    {
        Categories = await Requests.ListCategories();
        Messages = msgs;
    }

    public async Task<IActionResult> OnPostCreate(string categoryName)
    {
        Models.CategoryValidator validator = new();
        ValidationResult results = validator.Validate(
            new Models.Category(categoryName)
        );
        if (results.IsValid)
            _ = await Requests.CreateCategory(categoryName);
        else
        {
            List<string> msgs = new();
            foreach (var failure in results.Errors)
                msgs.Add(
                    $"Property {failure.PropertyName}: {failure.ErrorMessage}"
                );
            Messages = msgs;
        }
        return RedirectToPage("./Categories", new { msgs = Messages });
    }

    public async Task<IActionResult> OnPostUpdate(string id, string categoryName)
    {
        Models.CategoryValidator validator = new();
        ValidationResult results = validator.Validate(
            new Models.Category(categoryName)
        );
        if (results.IsValid)
            _ = await Requests.UpdateCategory(new Guid(id), categoryName);
        else
        {
            List<string> msgs = new();
            foreach (var failure in results.Errors)
                msgs.Add(
                    $"Property {failure.PropertyName}: {failure.ErrorMessage}"
                );
            Messages = msgs;
        }
        return RedirectToPage("./Categories", new { msgs = Messages });
    }

    public async Task<IActionResult> OnPostDelete(Guid id)
    {
        await Requests.DeleteCategory(id);
        return RedirectToPage("./Categories", new { msgs = Messages });
    }
}
