using KronoMata.Model;
using KronoMata.Scheduling;

namespace Test.KronoMata.Scheduling
{
    [TestFixture()]
    public partial class RecurrenceShouldRunTests
    {
        /*
            Scheduling Parameters are the following:

            StartDate - Return false if run time would be before this date
            EndDate - Return false if run time would be after this date
            Frequency - Month, Week, Day, Hour, Minute
            Interval - How many of each frequency between runs
            Days - A comma separated list of days by name (Monday, Tuesday, etc) to run on. Not always considered
            Hours - A comma separated list of hours (24 hour format) to run on. Not always considered.
            Minutes - A comma separated list of minutes to run on. Not always considered.

            This is a close copy of the Azure scheduling options for Recurrence in the Logic Apps and allows
            for pretty flexible scheduling of tasks (eg: Run on every Monday, Tuesday, and Saturday of the month at 3:15 AM)

            It's important to get the scheduling right so using TDD for this piece.
        */

        private IShouldRun _recurrence;
        private DateTime _now;
        private ScheduledJob _job; 

        [SetUp]
        public void Setup()
        {
            _recurrence = new RecurrenceShouldRun();
            _now = DateTime.Now;
            _job = new ScheduledJob();
        }

        // NOTE: Test cases should be grouped into separate files based on parameters. 
    }
}