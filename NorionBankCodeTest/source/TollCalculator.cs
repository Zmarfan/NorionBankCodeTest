using PublicHoliday;
using TollFeeCalculator;

public static class TollCalculator {
    public static readonly int MAX_TOLL_FEE = 60;
    private static readonly int COMBINE_TOLL_FEE_WINDOW_IN_MINUTES = 60;
    private const int JULY = 7;

    private static readonly HashSet<VehicleType> TOLL_FREE_VEHICLE_TYPES = new() {
        VehicleType.MOTORBIKE,
        VehicleType.TRACTOR,
        VehicleType.EMERGENCY,
        VehicleType.DIPLOMAT,
        VehicleType.FOREIGN,
        VehicleType.MILITARY
    };

    private static readonly List<PaymentTimeRange> PAYMENT_TIME_RANGES = new() {
        new PaymentTimeRange(8, new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 0)),
        new PaymentTimeRange(13, new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 0)),
        new PaymentTimeRange(18, new TimeSpan(7, 00, 0), new TimeSpan(7, 59, 0)),
        new PaymentTimeRange(13, new TimeSpan(8, 00, 0), new TimeSpan(8, 29, 0)),
        new PaymentTimeRange(8, new TimeSpan(8, 30, 0), new TimeSpan(14, 59, 0)),
        new PaymentTimeRange(13, new TimeSpan(15, 00, 0), new TimeSpan(15, 29, 0)),
        new PaymentTimeRange(18, new TimeSpan(15, 30, 0), new TimeSpan(16, 59, 0)),
        new PaymentTimeRange(13, new TimeSpan(17, 00, 0), new TimeSpan(17, 59, 0)),
        new PaymentTimeRange(8, new TimeSpan(18, 00, 0), new TimeSpan(18, 29, 0))
    };

    private static readonly SwedenPublicHoliday SWEDEN_PUBLIC_HOLIDAY = new();

    public static int GetTollFee(IVehicle vehicle, IEnumerable<DateTime> dateTimes) {
        if (TOLL_FREE_VEHICLE_TYPES.Contains(vehicle.Type)) {
            return 0;
        }
        
        return dateTimes
            .Order()
            .GroupBy(dateTime => dateTime.Date)
            .Where(group => !IsTollFreeDate(group.Key))
            .Sum(group => CalculateTollFeeOneDay(group.ToList()));
    }

    private static int GetTollFee(DateTime date) {
        return PAYMENT_TIME_RANGES.FirstOrDefault(paymentTimeRange => paymentTimeRange.IsWithinRange(date.TimeOfDay))?.paymentAmount ?? 0;
    }

    private static int CalculateTollFeeOneDay(List<DateTime> dateTimes) {
        DateTime referenceDateTime = dateTimes.First();
        int totalTollFee = 0;
        int highestHourTollFee = 0;
        foreach (DateTime dateTime in dateTimes) {
            int tollFee = GetTollFee(dateTime);
            TimeSpan timeSpan = dateTime - referenceDateTime;
            if (timeSpan.TotalMinutes < COMBINE_TOLL_FEE_WINDOW_IN_MINUTES) {
                highestHourTollFee = Math.Max(highestHourTollFee, tollFee);
            }
            else {
                referenceDateTime = dateTime;
                totalTollFee += highestHourTollFee;
                highestHourTollFee = tollFee;
            }

            if (totalTollFee >= MAX_TOLL_FEE) {
                return MAX_TOLL_FEE;
            }
        }

        return Math.Min(totalTollFee + highestHourTollFee, MAX_TOLL_FEE);
    }
    
    private static bool IsTollFreeDate(DateTime dateTime) {
        return dateTime.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
               || dateTime.Month == JULY
               || SWEDEN_PUBLIC_HOLIDAY.IsPublicHoliday(dateTime)
               || SWEDEN_PUBLIC_HOLIDAY.IsPublicHoliday(dateTime.AddDays(1));
    }
}