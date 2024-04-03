namespace TollFeeCalculator; 

public class PaymentTimeRange {
    public readonly int paymentAmount;
    private readonly TimeSpan _fromInclusive;
    private readonly TimeSpan _toInclusive;

    public PaymentTimeRange(int paymentAmount, TimeSpan fromInclusive, TimeSpan toInclusive) {
        this.paymentAmount = paymentAmount;
        _fromInclusive = fromInclusive;
        _toInclusive = toInclusive;
    }

    public bool IsWithinRange(TimeSpan timeSpan) {
        return timeSpan >= _fromInclusive && timeSpan <= _toInclusive;
    }
}