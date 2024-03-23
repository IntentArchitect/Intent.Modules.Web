using System;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.FluentValidation.Dtos.DTOValidator", Version = "2.0")]

namespace NetApplication.Application.Integration
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CustomDTOValidator : AbstractValidator<CustomDTO>
    {
        [IntentManaged(Mode.Merge)]
        public CustomDTOValidator()
        {
            ConfigureValidationRules();
        }

        [IntentManaged(Mode.Fully)]
        private void ConfigureValidationRules()
        {
            RuleFor(v => v.ReferenceNumber)
                .NotNull();
        }
    }
}