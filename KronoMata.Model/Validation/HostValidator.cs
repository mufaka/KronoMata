using FluentValidation;

namespace KronoMata.Model.Validation
{
    public class HostValidator : AbstractValidator<Host>
    {
        public HostValidator() 
        { 
            RuleFor(v => v.MachineName).NotEmpty();
            RuleFor(v => v.InsertDate).NotEmpty();
            RuleFor(v => v.UpdateDate).NotEmpty();
        }
    }
}
