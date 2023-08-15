using FluentValidation;

namespace KronoMata.Model.Validation
{
    public class GlobalConfigurationValidator : AbstractValidator<GlobalConfiguration>
    {
        public GlobalConfigurationValidator()
        {
            RuleFor(v => v.Category).NotEmpty();
            RuleFor(v => v.Name).NotEmpty();
            RuleFor(v => v.InsertDate).NotEmpty();
            RuleFor(v => v.UpdateDate).NotEmpty();
        }
    }
}
