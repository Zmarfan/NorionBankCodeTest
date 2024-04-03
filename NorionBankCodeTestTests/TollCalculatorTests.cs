using TollFeeCalculator;

namespace NorionBankCodeTestTests;

public class TollCalculatorTests {
    private static readonly DateTime PAY_DAY = DateTime.Parse("2024-04-03 12:45");
    
    private static readonly DateTime SATURDAY = DateTime.Parse("2024-04-06 12:00");
    private static readonly DateTime SUNDAY = DateTime.Parse("2024-04-07 12:00");
    private static readonly DateTime CHRISTMAS_DAY = DateTime.Parse("2024-12-25 12:00");
    private static readonly DateTime CHRISTMAS_EVE = DateTime.Parse("2024-12-24 12:00");
    private static readonly DateTime DAY_IN_JULY = DateTime.Parse("2024-06-24 12:00");
    
    private static readonly object[] TOLL_FREE_DAYS = { SATURDAY, SUNDAY, CHRISTMAS_DAY,CHRISTMAS_EVE,DAY_IN_JULY };
    
    private static readonly TollCalculator TOLL_CALCULATOR = new();

    [TestCaseSource(nameof(TOLL_FREE_DAYS))]
    public void should_return_a_toll_fee_of_zero_if_provided_date_is_a_toll_free_day(DateTime dateTime) {
        int tollFee = TOLL_CALCULATOR.GetTollFee(dateTime, new Car());
        Assert.That(tollFee, Is.Zero);
    }
    
    [Test]
    public void should_return_a_toll_fee_of_zero_if_provided_vehicle_type_is_marked_as_toll_free() {
        int tollFee = TOLL_CALCULATOR.GetTollFee(PAY_DAY, new Motorbike());
        Assert.That(tollFee, Is.Zero);
    }
    
    [Test]
    public void should_return_a_toll_fee_of_zero_if_provided_vehicle_type_is_not_marked_as_toll_free() {
        int tollFee = TOLL_CALCULATOR.GetTollFee(PAY_DAY, new Car());
        Assert.That(tollFee, Is.Not.Zero);
    }
}