using TollFeeCalculator;

namespace NorionBankCodeTestTests;

public class TollCalculatorTests {
    private static readonly DateTime PAY_DAY = DateTime.Parse("2024-04-03 12:45");
    
    private static readonly DateTime SATURDAY = DateTime.Parse("2024-04-06 12:00");
    private static readonly DateTime SUNDAY = DateTime.Parse("2024-04-07 12:00");
    private static readonly DateTime CHRISTMAS_DAY = DateTime.Parse("2024-12-25 12:00");
    private static readonly DateTime CHRISTMAS_EVE = DateTime.Parse("2024-12-24 12:00");
    private static readonly DateTime DAY_IN_JULY = DateTime.Parse("2024-07-24 12:00");
    
    private static readonly object[] TOLL_FREE_DAYS = { SATURDAY, SUNDAY, CHRISTMAS_DAY,CHRISTMAS_EVE,DAY_IN_JULY };
    private static readonly IVehicle TOLL_FREE_VEHICLE = new Motorbike();
    private static readonly IVehicle TOLL_VEHICLE = new Car();
    
    [Test]
    public void should_return_the_highest_toll_fee_if_all_times_are_within_one_hour() {
        int tollFee = TollCalculator.GetTollFee(TOLL_VEHICLE, new List<DateTime> {
            DateTime.Parse("2024-04-03 07:45"),
            DateTime.Parse("2024-04-03 07:55"),
            DateTime.Parse("2024-04-03 08:15"),
            DateTime.Parse("2024-04-03 08:44"),
        });
        Assert.That(tollFee, Is.EqualTo(18));
    }
    
    [Test]
    public void should_return_the_sum_of_toll_fees_that_are_separated_by_more_1_or_more_hour_but_is_grouped_within_1_hour_segments() {
        int tollFee = TollCalculator.GetTollFee(TOLL_VEHICLE, new List<DateTime> {
            DateTime.Parse("2024-04-03 03:45"),
            DateTime.Parse("2024-04-03 04:40"),
            DateTime.Parse("2024-04-03 07:45"),
            DateTime.Parse("2024-04-03 08:25"),
            DateTime.Parse("2024-04-03 08:44"),
            DateTime.Parse("2024-04-03 10:45"),
            DateTime.Parse("2024-04-03 10:55"),
            DateTime.Parse("2024-04-03 11:44"),
            DateTime.Parse("2024-04-03 17:15"),
            DateTime.Parse("2024-04-03 18:14"),
            DateTime.Parse("2024-04-03 22:00"),
            DateTime.Parse("2024-04-03 22:35"),
        });
        Assert.That(tollFee, Is.EqualTo(39));
    }
    
    [Test]
    public void should_return_the_sum_of_toll_fees_that_span_more_than_one_hour() {
        int tollFee = TollCalculator.GetTollFee(TOLL_VEHICLE, new List<DateTime> {
            DateTime.Parse("2024-04-03 07:45"),
            DateTime.Parse("2024-04-03 08:45"),
            DateTime.Parse("2024-04-03 09:45"),
            DateTime.Parse("2024-04-03 11:00"),
        });
        Assert.That(tollFee, Is.EqualTo(42));
    }
    
    [Test]
    public void should_return_the_correct_toll_fee_even_if_date_times_are_out_of_order() {
        int tollFee = TollCalculator.GetTollFee(TOLL_VEHICLE, new List<DateTime> {
            DateTime.Parse("2024-04-03 03:45"),
            DateTime.Parse("2024-04-03 08:25"),
            DateTime.Parse("2024-04-03 22:35"),
            DateTime.Parse("2024-04-03 08:44"),
            DateTime.Parse("2024-04-03 17:15"),
            DateTime.Parse("2024-04-03 07:45"),
            DateTime.Parse("2024-04-03 10:45"),
            DateTime.Parse("2024-04-03 10:55"),
            DateTime.Parse("2024-04-03 22:00"),
            DateTime.Parse("2024-04-03 11:44"),
            DateTime.Parse("2024-04-03 04:40"),
            DateTime.Parse("2024-04-03 18:14"),
        });
        Assert.That(tollFee, Is.EqualTo(39));
    }
    
    [Test]
    public void should_return_correct_toll_fee_even_if_times_overlap_but_are_different_days() {
        int tollFee = TollCalculator.GetTollFee(TOLL_VEHICLE, new List<DateTime> {
            DateTime.Parse("2024-04-03 07:45"),
            DateTime.Parse("2024-04-03 08:25"),
            DateTime.Parse("2024-04-03 08:44"),
            DateTime.Parse("2024-04-04 07:45"),
            DateTime.Parse("2024-04-04 08:25"),
            DateTime.Parse("2024-04-04 08:44"),
        });
        Assert.That(tollFee, Is.EqualTo(36));
    }
    
    [Test]
    public void should_return_max_toll_fee_if_combined_each_toll_exceeds_max_fee_one_day() {
        int tollFee = TollCalculator.GetTollFee(TOLL_VEHICLE, new List<DateTime> {
            DateTime.Parse("2024-04-03 07:45"),
            DateTime.Parse("2024-04-03 08:46"),
            DateTime.Parse("2024-04-03 09:47"),
            DateTime.Parse("2024-04-03 10:48"),
            DateTime.Parse("2024-04-03 11:49"),
            DateTime.Parse("2024-04-03 12:50"),
            DateTime.Parse("2024-04-03 13:51"),

        });
        Assert.That(tollFee, Is.EqualTo(TollCalculator.MAX_TOLL_FEE));
    }
    
    [Test]
    public void should_return_sum_of_max_toll_fee_if_combined_each_toll_exceeds_max_fee_over_multiple_days() {
        int tollFee = TollCalculator.GetTollFee(TOLL_VEHICLE, new List<DateTime> {
            DateTime.Parse("2024-04-03 07:45"),
            DateTime.Parse("2024-04-03 08:46"),
            DateTime.Parse("2024-04-03 09:47"),
            DateTime.Parse("2024-04-03 10:48"),
            DateTime.Parse("2024-04-03 11:49"),
            DateTime.Parse("2024-04-03 12:50"),
            DateTime.Parse("2024-04-03 13:51"),
            
            DateTime.Parse("2024-04-04 07:45"),
            DateTime.Parse("2024-04-04 08:46"),
            DateTime.Parse("2024-04-04 09:47"),
            DateTime.Parse("2024-04-04 10:48"),
            DateTime.Parse("2024-04-04 11:49"),
            DateTime.Parse("2024-04-04 12:50"),
            DateTime.Parse("2024-04-04 13:51"),

        });
        Assert.That(tollFee, Is.EqualTo(TollCalculator.MAX_TOLL_FEE * 2));
    }
    
    [TestCaseSource(nameof(TOLL_FREE_DAYS))]
    public void should_return_a_toll_fee_of_zero_if_provided_date_is_a_toll_free_day(DateTime dateTime) {
        int tollFee = TollCalculator.GetTollFee(TOLL_VEHICLE, dateTime);
        Assert.That(tollFee, Is.Zero);
    }
    
    [Test]
    public void should_return_a_toll_fee_of_zero_if_provided_vehicle_type_is_marked_as_toll_free() {
        int tollFee = TollCalculator.GetTollFee(TOLL_FREE_VEHICLE, PAY_DAY);
        Assert.That(tollFee, Is.Zero);
    }
    
    [Test]
    public void should_return_a_toll_fee_of_zero_if_provided_vehicle_type_is_not_marked_as_toll_free() {
        int tollFee = TollCalculator.GetTollFee(TOLL_VEHICLE, PAY_DAY);
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
        int tollFee = TollCalculator.GetTollFee(TOLL_VEHICLE, DateTime.Parse(dateTimeString));
        Assert.That(tollFee, Is.EqualTo(expectedTollFee));
    }
}