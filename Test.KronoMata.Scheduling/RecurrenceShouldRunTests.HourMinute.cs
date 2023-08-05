using KronoMata.Model;

namespace Test.KronoMata.Scheduling
{
    public partial class RecurrenceShouldRunTests
    {
        [Test]
        public void ShouldRunEvery15Minutes()
        {
            _job.StartTime = new DateTime(2023, 1, 1, 10, 7, 0); // 1/1/2023 10:07:00
            _job.Frequency = ScheduleFrequency.Minute;
            _job.Interval = 15; // run monthly on the start times day and time (to the minute)

            // 15 minute intervals starting from minute 7
            // 22, 37, 52, 7

            var currentDate = new DateTime(2023, 2, 1, 1, 0, 0);

            for (int x = 0; x < 120; x++)
            {
                currentDate = currentDate.AddMinutes(1);

                var shouldRun = _recurrence.ShouldRun(currentDate, _job);

                switch (currentDate.Minute)
                {
                    case 7:
                    case 22:
                    case 37:
                    case 52:
                        Assert.That(shouldRun, Is.True);
                        break;
                    default:
                        Assert.That(shouldRun, Is.False);
                        break;
                }

            }
        }

        [Test]
        public void ShouldRunEveryHour()
        {
            _job.StartTime = new DateTime(2023, 1, 1, 10, 7, 0); // 1/1/2023 10:07:00
            _job.Frequency = ScheduleFrequency.Hour;
            _job.Interval = 1; 

            var currentDate = _now.AddMonths(1);

            for (int x = 0; x < 480; x++)
            {
                currentDate = currentDate.AddMinutes(1);

                var shouldRun = _recurrence.ShouldRun(currentDate, _job);

                // start time is at 7 minute mark, so should run every 7th minute of every hour.
                switch (currentDate.Minute)
                {
                    case 7:
                        Assert.That(shouldRun, Is.True);
                        break;
                    default:
                        Assert.That(shouldRun, Is.False);
                        break;
                }

            }
        }

        [Test]
        public void ShouldRunEveryHourAtSpeciedMinutes()
        {
            _job.StartTime = new DateTime(2023, 1, 1, 10, 7, 0); // 1/1/2023 10:07:00
            _job.Frequency = ScheduleFrequency.Hour;
            _job.Interval = 1;
            _job.Minutes = "3,23,39,57";

            // 15 minute intervals starting from minute 7
            // 22, 37, 52, 7

            var currentDate = _now.AddMonths(1);

            for (int x = 0; x < 480; x++)
            {
                currentDate = currentDate.AddMinutes(1);

                var shouldRun = _recurrence.ShouldRun(currentDate, _job);

                // start time is at 7 minute mark, so should run every 7th minute of every hour.
                switch (currentDate.Minute)
                {
                    case 3:
                    case 23:
                    case 39:
                    case 57:
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
