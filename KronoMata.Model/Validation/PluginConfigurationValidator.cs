using FluentValidation;

namespace KronoMata.Model.Validation
{
    public class PluginConfigurationValidator : AbstractValidator<PluginConfiguration>
    {
        public PluginConfigurationValidator() 
        {
            RuleFor(v => v.PluginMetaDataId).GreaterThan(0);
            RuleFor(v => v.DataType).NotEmpty();
            RuleFor(v => v.Name).NotEmpty();
            RuleFor(v => v.Description).NotEmpty();
            RuleFor(v => v.InsertDate).NotEmpty();
            RuleFor(v => v.UpdateDate).NotEmpty();
        }
    }
}
