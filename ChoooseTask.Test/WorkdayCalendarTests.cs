using FluentAssertions;

namespace ChoooseTask.Test
{
    public class WorkdayCalendarTests
    {

        [Fact]
        public void GetWorkdayIncrement_ShouldReturnCorrectIncrementedDate()
        {
            // Arrange
            var workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkdayStartAndStop(8, 0, 16, 0);
            workdayCalendar.SetRecurringHoliday(5, 17); // recurring holidays
            workdayCalendar.SetHoliday(new DateTime(2004, 5, 27)); //Specific holidays
            var start = new DateTime(2004, 5, 24, 18, 5, 0);
            decimal increment = -5.5m;


            // Act
            var incrementedDate = workdayCalendar.GetWorkdayIncrement(start, increment);


            // Assert
            incrementedDate.Should().Be(new DateTime(2004, 5, 14, 12, 0, 0)); //It should be January 2nd, at 8:00 AM
        }

        [Fact]
        public void GetWorkdayIncrement_ShouldReturnCorrectIncrementedDate1()
        {
            // Arrange
            var workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkdayStartAndStop(8, 0, 16, 0);
            workdayCalendar.SetRecurringHoliday(5, 17); //recurring holidays
            workdayCalendar.SetHoliday(new DateTime(2004, 5, 27)); //Specific holidays
            var start = new DateTime(2004, 5, 24, 19, 3, 0);
            var increment = (decimal)44.723656;

            // Act
            var incrementedDate = workdayCalendar.GetWorkdayIncrement(start, increment);


            // Assert
            incrementedDate.Should().Be(new DateTime(2004, 7, 27, 13, 47, 0)); //It should be 27th July , at 13:47 
        }

        [Fact]
        public void GetWorkdayIncrement_ShouldReturnCorrectIncrementedDate2()
        {
            // Arrange
            var workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkdayStartAndStop(8, 0, 16, 0);
            workdayCalendar.SetRecurringHoliday(5, 17); //curring holidays
            workdayCalendar.SetHoliday(new DateTime(2004, 5, 27)); //Specific holidays
            var start = new DateTime(2004, 5, 24, 18, 3, 0);
            var increment = (decimal)-6.7470217;

            // Act
            var incrementedDate = workdayCalendar.GetWorkdayIncrement(start, increment);


            // Assert
            incrementedDate.Should().Be(new DateTime(2004, 5, 13, 10, 2, 0)); //It should be 13th May , at 10:2 
        }


        [Fact]
        public void GetWorkdayIncrement_ShouldReturnCorrectIncrementedDate3()
        {
            // Arrange
            var workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkdayStartAndStop(8, 0, 16, 0);
            workdayCalendar.SetRecurringHoliday(5, 17); //curring holidays
            workdayCalendar.SetHoliday(new DateTime(2004, 5, 27)); //Specific holidays
            var start = new DateTime(2004, 5, 24, 7, 3, 0);
            var increment = (decimal)8.276628;

            // Act
            var incrementedDate = workdayCalendar.GetWorkdayIncrement(start, increment);


            // Assert
            incrementedDate.Should().Be(new DateTime(2004, 6, 4, 10, 12, 0)); //It should be 4th Jun , at 10:12 
        }

        [Fact]
        public void GetWorkdayIncrement_ShouldReturnCorrectIncrementedDate4()
        {
            // Arrange
            var workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkdayStartAndStop(8, 0, 16, 0);
            workdayCalendar.SetRecurringHoliday(5, 17); //curring holidays
            workdayCalendar.SetHoliday(new DateTime(2004, 5, 27)); //Specific holidays
            var start = new DateTime(2004, 5, 24, 8, 3, 0);
            var increment = (decimal)12.782709;

            // Act
            var incrementedDate = workdayCalendar.GetWorkdayIncrement(start, increment);


            // Assert
            incrementedDate.Should().Be(new DateTime(2004, 6, 10, 14, 18, 0)); //It should be 10th Jun , at 14:18 
        }

        [Fact]
        public void SetWorkdayStartAndStop_InvalidData_ShouldThrowException()
        {
            // Arrange
            var workdayCalendar = new WorkdayCalendar();


            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => workdayCalendar.SetWorkdayStartAndStop(8, 0, 8, 0));


            // Assert
            Assert.Equal("Workday start time must be earlier than end time.", exception.Message);
        }


        [Fact]
        public void SetRecurringHoliday_InvalidDayOfMonth_ShouldThrowException()
        {
            // Arrange
            var workdayCalendar = new WorkdayCalendar();
            workdayCalendar.SetWorkdayStartAndStop(8, 0, 16, 0);

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => workdayCalendar.SetRecurringHoliday(2, 31));


            // Assert
            Assert.Equal("Invalid month (2) or day (31) for a recurring holiday.", exception.ParamName);
        }
    }
}
