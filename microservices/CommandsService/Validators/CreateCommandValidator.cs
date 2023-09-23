using CommandsService.Dtos;
using FluentValidation;

namespace CommandsService.Validators;

public class CreateCommandValidator : AbstractValidator<CommandCreateDto>
{
    public CreateCommandValidator()
    {
        RuleFor(c => c.HowTo).NotEmpty().NotNull();
        RuleFor(c => c.CommandLine).NotEmpty().NotNull();
    }
}