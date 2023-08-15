using FluentValidation;

namespace KronoMata.Model.Validation
{
    public class PluginMetaDataValidator : AbstractValidator<PluginMetaData>
    {
        public PluginMetaDataValidator() 
        {
            RuleFor(v => v.PackageId).GreaterThan(0);
            RuleFor(v => v.Name).NotEmpty();
            RuleFor(v => v.Description).NotEmpty();
            RuleFor(v => v.Version).NotEmpty();
            RuleFor(v => v.AssemblyName).NotEmpty();
            RuleFor(v => v.ClassName).NotEmpty();
            RuleFor(v => v.InsertDate).NotEmpty();
            RuleFor(v => v.UpdateDate).NotEmpty();
        }
    }
}
