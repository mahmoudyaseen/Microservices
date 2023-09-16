using FluentValidation;
using PlatformService.Dtos;

namespace PlatformService.Validators;

public class PlatformCreateValidator : AbstractValidator<PlatformCreateDto> 
{
    public PlatformCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Cost).NotEmpty();
        RuleFor(x => x.Publisher).NotEmpty();
    }
}