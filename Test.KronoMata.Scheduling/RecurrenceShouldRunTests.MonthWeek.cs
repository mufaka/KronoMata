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

        [Test]
        public void ShouldOnlyRunOnFridayTheThirteenth()
        {
            var currentDate = new DateTime(2023, 8, 4, _now.Hour, _now.Minute, _now.Second); // Friday the 4th
            _job.StartTime = _now;
            _job.Frequency = ScheduleFrequency.Month;
            _job.DayOfWeeks = "Friday";
            _job.Days = "13";
            _job.Interval = 1; 

            for (int x = 0; x < 104; x++)
            {
                currentDate = currentDate.AddDays(7);

                var shouldRun = _recurrence.ShouldRun(currentDate, _job);

                if (currentDate.DayOfWeek == DayOfWeek.Friday && currentDate.Day == 13)
                {
                    Console.WriteLine($"Found a Friday the 13th on {currentDate.ToShortDateString()}");
                    Assert.That(shouldRun, Is.True);
                }
                else
                {
                    Assert.That(shouldRun, Is.False);
                }
            }
        }
    }
}
