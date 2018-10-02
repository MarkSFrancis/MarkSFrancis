﻿using Phnx.Console.Progress;
using NUnit.Framework;
using System;

namespace Phnx.Console.Tests.Progress
{
    public class ProgressBarRendererTests
    {
        [Test]
        public void CreateNew_WithNegativeMaxValue_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentLessThanZeroException>(() => new ProgressBarRenderer(-1));
        }

        [Test]
        public void CreateNew_With20MaxValue_SetsMaxValue()
        {
            var expected = 20;

            var prog = new ProgressBarRenderer(expected);

            Assert.AreEqual(expected, prog.MaxValue);
        }

        [Test]
        public void ToString_With0Value_RendersEmptyBar()
        {
            var expected = "[----------] 0%";

            var prog = new ProgressBarRenderer(10);
            var result = prog.ToString();

            Assert.AreEqual(expected, result);
        }
    }
}
