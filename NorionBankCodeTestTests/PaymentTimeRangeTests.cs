using TollFeeCalculator;

namespace NorionBankCodeTestTests; 

public class PaymentTimeRangeTests {
    private static readonly TimeSpan FROM = new(12, 0, 0);
    private static readonly TimeSpan TO = new(13, 30, 0);
    private static readonly PaymentTimeRange PAYMENT_TIME_RANGE = new(10, FROM, TO);

    private static readonly object[] WITHIN_RANGE_TIMES = { FROM, TO };
    private static readonly object[] OUTSIDE_RANGE_TIMES = { new TimeSpan(11, 59, 59), TO.Add(new TimeSpan(0, 0, 1)) };

    [TestCaseSource(nameof(WITHIN_RANGE_TIMES))]
    public void should_return_that_time_is_within_range(TimeSpan timeSpan) {
        bool isWithinRange = PAYMENT_TIME_RANGE.IsWithinRange(timeSpan);
        Assert.That(isWithinRange, Is.True);
    }

    
    [TestCaseSource(nameof(OUTSIDE_RANGE_TIMES))]
    public void should_return_that_time_is_not_within_range(TimeSpan timeSpan) {
        bool isWithinRange = PAYMENT_TIME_RANGE.IsWithinRange(timeSpan);
        Assert.That(isWithinRange, Is.False);
    }
}