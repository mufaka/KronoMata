using FluentValidation;

namespace KronoMata.Model.Validation
{
    public class PackageValidator : AbstractValidator<Package>
    {
        public PackageValidator() 
        { 
            RuleFor(v => v.Name).NotEmpty();
            RuleFor(v => v.FileName).NotEmpty();
            RuleFor(v => v.UploadDate).NotEmpty();
        }
    }
}
