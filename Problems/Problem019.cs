namespace Project_Euler.Problems;

public class Problem019 : Problem {
    public override object Solve() {
        return NumberOfSundays();
    }

    private int NumberOfSundays() {
        int numberOfSundays = 0;
        int dayOfTheWeek = 2;

        for (int year = 1901; year <= 2000; year++)
        for (int month = 1; month <= 12; month++) {
            if (dayOfTheWeek == 0) numberOfSundays++;
            dayOfTheWeek = (dayOfTheWeek + GetNumberOfDays(month, year)) % 7;
        }

        return numberOfSundays;
    }

    private int GetNumberOfDays(int month, int year) {
        return month switch {
            4 or 6 or 9 or 11 => 30,
            2 => IsLeapYear(year) ? 29 : 28,
            _ => 31
        };
    }

    private bool IsLeapYear(int year) {
        return (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
    }
}