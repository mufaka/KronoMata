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
            Days - A comma separated list of days by number (1, 2, 5, 12, etc) to run on. Not always considered.
            DayOfWeeks - A comma separated list of days by name (Monday, Tuesday, etc) to run on. Not always considered
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
            _job.IsEnabled = true; // default to enabled
        }

        [Test]
        public void ShouldNotRunIfDisabled()
        {
            _job.IsEnabled = false;

            bool shouldRun = _recurrence.ShouldRun(_now, _job);

            Assert.That(shouldRun, Is.False);
        }

        [Test]
        public void ShouldNotRunBeforeStartDate()
        {
            _job.StartTime = _now.AddDays(1);

            var shouldRun = _recurrence.ShouldRun(_now, _job);

            Assert.That(shouldRun, Is.False);
        }

        [Test]
        public void ShouldNotRunAfterEndDate()
        {
            _job.StartTime = _now.AddDays(-7);
            _job.EndTime = _now.AddDays(-1);

            var shouldRun = _recurrence.ShouldRun(_now, _job);

            Assert.That(shouldRun, Is.False);
        }

        /// <summary>
        /// This test should always pass but used as an easy way to try
        /// and reproduce production bugs. TestCases should be defined 
        /// using the exact parameters of the bug report and use a comment
        /// specifying the issue # the test case is for so that regressions can
        /// be easily identified.
        /// </summary>
        /// <param name="check">The DateTime that the recurrence check should return true</param>
        /// <param name="start">The ScheduledJob start time</param>
        /// <param name="end">The nullable ScheduledJob end time</param>
        /// <param name="isEnabled">Whether or not the job is enabled. If false, recurrence ShouldRun should return false</param>
        /// <param name="frequency">The ScheduledJob ScheduledFrequency</param>
        /// <param name="interval">The ScheduledJob interval</param>
        /// <param name="days">The nullable comma separated list of days for the ScheduledJob</param>
        /// <param name="hours">The nullable comma separated list of hours for the ScheduledJob</param>
        /// <param name="minutes">The nullable comma separated list of minutes for the ScheduledJob</param>
        /// <param name="daysOfWeek">The nullable comma separated list containing one or more of Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday</param>
        [Test]
        [TestCase("08/25/2023 9:20 PM", "08/25/2023 6:19 PM", null, true, 2, 1, null, "3,6,9,12,15,18,21", "5,10,15,20", null)] // #8
        [TestCase("08/26/2023 3:20 AM", "08/25/2023 6:19 PM", null, true, 2, 1, null, "3,6,9,12,15,18,21", "5,10,15,20", null)] // #8
        public void ScheduledJobShouldRun(DateTime check, DateTime start, DateTime? end, bool isEnabled, ScheduleFrequency frequency,
            int interval, string days, string hours, string minutes, string daysOfWeek)
        {
            var scheduledJob = new ScheduledJob();

            scheduledJob.StartTime = start;
            scheduledJob.EndTime = end;
            scheduledJob.IsEnabled = isEnabled;
            scheduledJob.Frequency = frequency;
            scheduledJob.Interval = interval;
            scheduledJob.Days = days;
            scheduledJob.Hours = hours;
            scheduledJob.Minutes = minutes;
            scheduledJob.DaysOfWeek = daysOfWeek;

            var shouldRun = _recurrence.ShouldRun(check, scheduledJob);

            // If the ScheduledJob is not enabled, shouldRun should always be false. Otherwise
            // it is assumed that all provided parameters are for a case where the scheduled should run.
            Assert.That(shouldRun, Is.EqualTo(isEnabled));
        }

        // NOTE: Additional test cases should be grouped into separate files based on parameters. 
    }
}