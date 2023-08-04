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

            // check two years worth of dates
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

        [Test]
        public void ShouldOnlyRunOnSpecificDaysOfTheMonth()
        {
            // run at 1:15 PM on the 3rd, 9th, 11th, 19th, and 22nd of the month.
            _job.StartTime = new DateTime(2022, 1, 1);
            _job.Frequency = ScheduleFrequency.Month;
            _job.Interval = 1;
            _job.Days = "3,9,11,19,22";
            _job.Hours = "13";
            _job.Minutes = "15";

            var startDate = new DateTime(2023, 7, 1, 13, 15, 0);

            for (int x = 0; x < 365; x++)
            {
                startDate = startDate.AddDays(x); // we've already set the correct hour and minute

                var shouldRun = _recurrence.ShouldRun(startDate, _job);

                switch (startDate.Day)
                {
                    case 3:
                    case 9:
                    case 11:
                    case 19:
                    case 22:
                        Assert.That(shouldRun, Is.True);
                        break;
                    default:
                        Assert.That(shouldRun, Is.False);
                        break;
                }
            }
        }

        [Test]
        public void ShouldOnlyRunEveryMondayWednesdayFriday()
        {
            // run at 1:15 PM on the 3rd, 9th, 11th, 19th, and 22nd of the month.
            _job.StartTime = new DateTime(2022, 1, 1);
            _job.Frequency = ScheduleFrequency.Month;
            _job.Interval = 1;
            _job.Days = "";
            _job.DayOfWeeks = "Monday,Wednesday,Friday";
            _job.Hours = "13";
            _job.Minutes = "15";

            var startDate = new DateTime(2023, 7, 1, 13, 15, 0);

            for (int x = 0; x < 365; x++)
            {
                startDate = startDate.AddDays(x); // we've already set the correct hour and minute

                var shouldRun = _recurrence.ShouldRun(startDate, _job);

                switch (startDate.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Friday:
                        Assert.That(shouldRun, Is.True);
                        break;
                    default:
                        Assert.That(shouldRun, Is.False);
                        break;
                }
            }
        }

    }
}
