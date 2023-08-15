using FluentValidation;

namespace KronoMata.Model.Validation
{
    public class JobHistoryValidator : AbstractValidator<JobHistory>
    {
        public JobHistoryValidator() 
        {
            RuleFor(v => v.ScheduledJobId).GreaterThan(0);
            RuleFor(v => v.HostId).GreaterThan(0);
            RuleFor(v => v.Status).NotEmpty();
            RuleFor(v => v.Message).NotEmpty();
            RuleFor(v => v.RunTime).NotEmpty();
            RuleFor(v => v.CompletionTime).NotEmpty();
        }
    }
}
