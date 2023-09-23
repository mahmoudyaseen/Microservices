// using CommandsService.Dtos;
// using FluentValidation;

// namespace CommandsService.Validators;
// 
// public class CreatePlatformValidator : AbstractValidator<PlatformCreateDto>
// {
//     public CreatePlatformValidator()
//     {
//         RuleFor(c => c.HowTo).NotEmpty().NotNull();
//         RuleFor(c => c.CommandLine).NotEmpty().NotNull();
//     }
// }