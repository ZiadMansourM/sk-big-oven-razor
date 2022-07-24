using FluentValidation;
namespace Frontend.Models;

public class CategoryValidator : AbstractValidator<Models.Category>
{
    public CategoryValidator()
    {
        RuleFor(category => category.Name).NotEmpty();
    }
}