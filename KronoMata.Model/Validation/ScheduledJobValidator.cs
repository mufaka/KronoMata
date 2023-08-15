using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronoMata.Model.Validation
{
    public class ScheduledJobValidator : AbstractValidator<ScheduledJob>
    {
        public ScheduledJobValidator() 
        {
            RuleFor(v => v.PluginMetaDataId).GreaterThan(0);
            RuleFor(v => v.Name).NotEmpty();
            RuleFor(v => v.Description).NotEmpty();
            RuleFor(v => v.Frequency).NotEmpty();
            RuleFor(v => v.Interval).GreaterThan(0);
            AddDaysRule();
            AddDaysOfWeekRule();
            AddHoursRule();
            AddMinutesRule();
            RuleFor(v => v.InsertDate).NotEmpty();
            RuleFor(v => v.UpdateDate).NotEmpty();
        }

        private void AddDaysRule()
        {
            // Days can be 1 to 31 only but stored as comma separated list ...
            RuleFor(v => v.Days).Custom((days, context) =>
            {
                try
                {
                    // allow empty
                    if (String.IsNullOrWhiteSpace(days)) return;

                    var dayTokens = days.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    for (int x = 0; x < dayTokens.Length; x++)
                    {
                        if (int.TryParse(dayTokens[x], out int day))
                        {
                            if (day < 1 || day > 31)
                            {
                                context.AddFailure("Days must be between 1 and 31 (inclusive).");
                                break;
                            }
                        }
                        else
                        {
                            context.AddFailure("Days requires numeric only values.");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.AddFailure("Unable to parse Days.");
                }
            });
        }

        private void AddHoursRule()
        {
            // Hours can be 1 to 23 only but stored as comma separated list ...
            RuleFor(v => v.Hours).Custom((hours, context) =>
            {
                try
                {
                    // allow empty
                    if (String.IsNullOrWhiteSpace(hours)) return;

                    var hourTokens = hours.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    for (int x = 0; x < hourTokens.Length; x++)
                    {
                        if (int.TryParse(hourTokens[x], out int hour))
                        {
                            if (hour < 1 || hour > 23)
                            {
                                context.AddFailure("Hours must be between 1 and 23 (inclusive).");
                                break;
                            }
                        }
                        else
                        {
                            context.AddFailure("Hours requires numeric only values.");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.AddFailure("Unable to parse Hours.");
                }
            });
        }

        private void AddMinutesRule()
        {
            // Minutes can be 1 to 59 only but stored as comma separated list ...
            RuleFor(v => v.Minutes).Custom((minutes, context) =>
            {
                try
                {
                    // allow empty
                    if (String.IsNullOrWhiteSpace(minutes)) return;

                    var minuteTokens = minutes.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    for (int x = 0; x < minuteTokens.Length; x++)
                    {
                        if (int.TryParse(minuteTokens[x], out int minute))
                        {
                            if (minute < 1 || minute > 59)
                            {
                                context.AddFailure("Minutes must be between 1 and 59 (inclusive).");
                                break;
                            }
                        }
                        else
                        {
                            context.AddFailure("Minutes requires numeric only values.");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.AddFailure("Unable to parse Minutes.");
                }
            });
        }

        private void AddDaysOfWeekRule()
        {
            RuleFor(v => v.DaysOfWeek).Custom((daysOfWeek, context) =>
            {
                // allow empty
                if (String.IsNullOrWhiteSpace(daysOfWeek)) return;

                var dayOfWeekTokens = daysOfWeek.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < dayOfWeekTokens.Length; x++)
                {
                    if (!Enum.TryParse(dayOfWeekTokens[x], out DayOfWeek dayOfWeek))
                    {
                        context.AddFailure("Invalid DayOfWeek defined.");
                        break;
                    }
                }
            });
        }
    }
}
