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
    
    [TestCaseSource(nameof(TOLL_FREE_DAYS))]
    public void should_return_a_toll_fee_of_zero_if_provided_date_is_a_toll_free_day(DateTime dateTime) {
        int tollFee = TollCalculator.GetTollFee(dateTime, new Car());
        Assert.That(tollFee, Is.Zero);
    }
    
    [Test]
    public void should_return_a_toll_fee_of_zero_if_provided_vehicle_type_is_marked_as_toll_free() {
        int tollFee = TollCalculator.GetTollFee(PAY_DAY, new Motorbike());
        Assert.That(tollFee, Is.Zero);
    }
    
    [Test]
    public void should_return_a_toll_fee_of_zero_if_provided_vehicle_type_is_not_marked_as_toll_free() {
        int tollFee = TollCalculator.GetTollFee(PAY_DAY, new Car());
        Assert.That(tollFee, Is.Not.Zero);
    }
    
    [TestCase("2024-04-03 00:00", 0)]
    [TestCase("2024-04-03 06:23", 8)]
    [TestCase("2024-04-03 06:45", 13)]
    [TestCase("2024-04-03 07:45", 18)]
    [TestCase("2024-04-03 08:25", 13)]
    [TestCase("2024-04-03 12:00", 8)]
    [TestCase("2024-04-03 15:25", 13)]
    [TestCase("2024-04-03 16:25", 18)]
    [TestCase("2024-04-03 17:25", 13)]
    [TestCase("2024-04-03 18:25", 8)]
    [TestCase("2024-04-03 18:45", 0)]
    public void should_return_the_expected_toll_fee_provided_specific_time(string dateTimeString, int expectedTollFee) {
        int tollFee = TollCalculator.GetTollFee(DateTime.Parse(dateTimeString), new Car());
        Assert.That(tollFee, Is.EqualTo(expectedTollFee));
    }
}