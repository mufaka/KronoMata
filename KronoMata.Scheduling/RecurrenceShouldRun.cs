using KronoMata.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronoMata.Scheduling
{
    public class RecurrenceShouldRun : IShouldRun
    {
        public bool ShouldRun(DateTime currentDate, ScheduledJob scheduledJob)
        {
            if (scheduledJob.StartTime > currentDate) return false;

            return true;
        }
    }
}
