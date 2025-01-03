namespace ChoooseTask
{

    public class WorkdayCalendar : IWorkdayCalendar
    {
        private readonly HashSet<DateTime> _specificHolidays = new();
        private readonly HashSet<(int Month, int Day)> _recurringHolidays = new();
        private TimeSpan _workdayStart = TimeSpan.Zero;
        private TimeSpan _workdayEnd = TimeSpan.Zero;

        public void SetHoliday(DateTime date)
        {
            _specificHolidays.Add(date.Date);
        }

        public void SetRecurringHoliday(int month, int day)
        {
            if (month is < 1 or > 12 || day is < 1 || day > DateTime.DaysInMonth(2024, month))
                throw new ArgumentOutOfRangeException($"Invalid month ({month}) or day ({day}) for a recurring holiday.");

            _recurringHolidays.Add((month, day));
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            _workdayStart = new TimeSpan(startHours, startMinutes, 0);
            _workdayEnd = new TimeSpan(stopHours, stopMinutes, 0);

            if (_workdayStart >= _workdayEnd)
                throw new ArgumentException("Workday start time must be earlier than end time.");
        }

        public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
        {
            if (_workdayStart == _workdayEnd)
                throw new InvalidOperationException("Workday start and end times must be set before calculating increments.");

            DateTime currentDate = NormalizeToWorkday(startDate);
            double remainingWorkHours = (double)incrementInWorkdays * GetWorkdayLengthInHours();

            while (Math.Abs(remainingWorkHours) > double.Epsilon)
            {
                double hoursInCurrentDay = remainingWorkHours > 0
                    ? (GetWorkdayEnd(currentDate) - currentDate).TotalHours
                    : (currentDate - GetWorkdayStart(currentDate)).TotalHours;

                if (Math.Abs(remainingWorkHours) <= Math.Abs(hoursInCurrentDay))
                {
                    currentDate = currentDate.AddHours(remainingWorkHours);
                    if (remainingWorkHours < 0 && currentDate.Second > 0)
                        currentDate = currentDate.AddMinutes(1);
                    break;
                }

                remainingWorkHours -= Math.Sign(remainingWorkHours) * hoursInCurrentDay;
                currentDate = remainingWorkHours > 0
                    ? MoveToNextWorkday(currentDate)
                    : MoveToPreviousWorkday(currentDate);
            }

            currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, 0);
            return currentDate;
        }

        private bool IsWorkday(DateTime date)
        {
            if (date.DayOfWeek is < DayOfWeek.Monday or > DayOfWeek.Friday)
                return false;

            if (_specificHolidays.Contains(date.Date))
                return false;

            if (_recurringHolidays.Contains((date.Month, date.Day)))
                return false;

            return true;
        }

        private DateTime NormalizeToWorkday(DateTime date)
        {
            if (IsWorkday(date))
            {
                if (date.TimeOfDay < _workdayStart)
                    return GetWorkdayStart(date);

                if (date.TimeOfDay > _workdayEnd)
                    return MoveToNextWorkday(date);

                return date;
            }

            return MoveToNextWorkday(date);
        }

        private DateTime MoveToNextWorkday(DateTime date)
        {
            do
            {
                date = date.AddDays(1);
            } while (!IsWorkday(date));

            return GetWorkdayStart(date);
        }

        private DateTime MoveToPreviousWorkday(DateTime date)
        {
            do
            {
                date = date.AddDays(-1);
            } while (!IsWorkday(date));

            return GetWorkdayEnd(date);
        }

        private DateTime GetWorkdayStart(DateTime date) => date.Date + _workdayStart;

        private DateTime GetWorkdayEnd(DateTime date) => date.Date + _workdayEnd;

        private double GetWorkdayLengthInHours() => (_workdayEnd - _workdayStart).TotalHours;
    }
}




