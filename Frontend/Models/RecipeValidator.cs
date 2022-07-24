﻿using FluentValidation;
namespace Frontend.Models;

public class RecipeValidator : AbstractValidator<Models.Recipe>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Name).NotEmpty();
        RuleFor(recipe => recipe.Ingredients).NotEmpty();
        RuleFor(recipe => recipe.Instructions).NotEmpty();
        RuleFor(recipe => recipe.CategoriesIds).NotEmpty();
    }
}

