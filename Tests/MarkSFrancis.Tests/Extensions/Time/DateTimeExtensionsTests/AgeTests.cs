﻿using MarkSFrancis.Extensions.Time;
using NUnit.Framework;
using System;

namespace MarkSFrancis.Tests.Extensions.Time.DateTimeExtensionsTests
{
    public class AgeTests
    {
        [Test]
        public void GetAge_WhenDobIsSameDayOfYear_ReturnsAge()
        {
            DateTime dob = new DateTime(2000, Sample.DateTime.Month, Sample.DateTime.Day);
            var age = dob.Age(Sample.DateTime);

            Assert.AreEqual(12, age);
        }

        [Test]
        public void GetAge_WhenDobIsLaterInYear_ReturnsAge()
        {
            DateTime dob = new DateTime(2000, Sample.DateTime.Month, Sample.DateTime.Day).AddMonths(1);
            var age = dob.Age(Sample.DateTime);

            Assert.AreEqual(11, age);
        }

        [Test]
        public void GetAge_WhenDobIsEarlierInYear_ReturnsAge()
        {
            DateTime dob = new DateTime(2000, Sample.DateTime.Month, Sample.DateTime.Day).AddMonths(-1);
            var age = dob.Age(Sample.DateTime);

            Assert.AreEqual(12, age);
        }

        [Test]
        public void GetAge_WhenDobIsInFuture_Returns0()
        {
            DateTime dob = new DateTime(2014, 1, 1);
            var age = dob.Age(Sample.DateTime);

            Assert.AreEqual(0, age);
        }

        [Test]
        public void GettingFirstDayOfWeek_WhenFirstDayIsSundayAndDateIsSunday_ReturnsSameDate()
        {
            DateTime testDate = new DateTime(2000, 1, 2);
            var firstDayOfWeek = testDate.StartOfWeek(false);

            Assert.AreEqual(testDate, firstDayOfWeek);
        }
    }
}
