using FluentValidation;

namespace KronoMata.Model.Validation
{
    public class ConfigurationValueValidator : AbstractValidator<ConfigurationValue>
    {
        public ConfigurationValueValidator()
        {
            // NotEmpty validates against default values, like int = 0, 
            // but we want to guard against negative values as well.
            RuleFor(v => v.ScheduledJobId).GreaterThan(0);
            RuleFor(v => v.PluginConfigurationId).GreaterThan(0);
            RuleFor(v => v.InsertDate).NotEmpty();
            RuleFor(v => v.UpdateDate).NotEmpty();

            // .Value can be empty.
        }
    }
}
