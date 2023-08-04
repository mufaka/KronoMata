using KronoMata.Model;

namespace Test.KronoMata.Scheduling
{
    public partial class RecurrenceShouldRunTests
    {
        [Test]
        public void ShouldRunNextMonth()
        {
            _job.StartTime = _now;
            _job.Frequency = ScheduleFrequency.Month;
            _job.Interval = 1; // run monthly on the start times day and time (to the minute)

            var currentDate = _now.AddMonths(1);
            
            var shouldRun = _recurrence.ShouldRun(currentDate, _job);

            Assert.That(shouldRun, Is.True);
        }

        [Test]
        public void ShouldNotRunNextMonth()
        {
            _job.StartTime = _now;
            _job.Frequency = ScheduleFrequency.Month;
            _job.Interval = 2; // run monthly on the start times day and time (to the minute)

            var currentDate = _now.AddMonths(1);

            var shouldRun = _recurrence.ShouldRun(currentDate, _job);

            Assert.That(shouldRun, Is.False);
        }

    }
}
