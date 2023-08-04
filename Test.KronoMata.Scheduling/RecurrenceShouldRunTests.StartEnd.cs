namespace Test.KronoMata.Scheduling
{
    public partial class RecurrenceShouldRunTests
    {
        [Test]
        public void ShouldNotRunBeforeStartDate()
        {
            _job.StartTime = _now.AddDays(1);

            var shouldRun = _recurrence.ShouldRun(_now, _job);

            Assert.That(shouldRun, Is.False);
        }

        [Test]
        public void ShouldRunAfterStartDate()
        {
            _job.StartTime = _now.AddDays(-1);

            var shouldRun = _recurrence.ShouldRun(_now, _job);

            Assert.That(shouldRun, Is.True);
        }

        [Test]
        public void ShouldNotRunAfterEndDate()
        {
            _job.StartTime = _now.AddDays(-7);
            _job.EndTime = _now.AddDays(-1);

            var shouldRun = _recurrence.ShouldRun(_now, _job);

            Assert.That(shouldRun, Is.False);
        }

        [Test]
        public void ShouldRunBetweenDates()
        {
            _job.StartTime = _now.AddDays(-7);
            _job.EndTime = _now.AddDays(1);

            var shouldRun = _recurrence.ShouldRun(_now, _job);

            Assert.That(shouldRun, Is.True);
        }
    }
}
