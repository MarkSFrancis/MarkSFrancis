﻿using System;

namespace MarkSFrancis.Console.Progress
{
    /// <summary>
    /// Provides a way of writing the progress of an operation to the console
    /// </summary>
    internal class ProgressBarRenderer
    {
        /// <summary>
        /// The maximum value of progress possible
        /// </summary>
        public decimal MaxValue { get; }

        private const int BlockCount = 10;

        private static readonly char[] Frames = {

            '|',
            '/',
            '-',
            '\\'
        };

        /// <summary>
        /// Create a new <see cref="ProgressBarRenderer"/>
        /// </summary>
        /// <param name="maxValue">The maximum value, representing the progress bar at its completed state</param>
        public ProgressBarRenderer(decimal maxValue)
        {
            MaxValue = maxValue;
            _currentAnimationFrame = 0;
        }

        private int _currentAnimationFrame;

        private decimal _progress;

        /// <summary>
        /// Get or set the current progress
        /// </summary>
        public decimal Progress
        {
            get => _progress;
            set
            {
                if (value > MaxValue)
                {
                    throw ErrorFactory.Default.IndexOutOfRange(nameof(value));
                }

                _progress = value;
            }
        }

        /// <summary>
        /// Get the completed state
        /// </summary>
        public bool IsComplete => Progress == MaxValue;

        private void UpdateAnimationChar()
        {
            if (++_currentAnimationFrame >= Frames.Length)
            {
                _currentAnimationFrame = 0;
            }
        }

        /// <summary>
        /// Return this <see cref="ProgressBarRenderer"/> formatted as a progress bar, with the spinner
        /// </summary>
        /// <returns>This <see cref="ProgressBarRenderer"/> formatted as a progress bar</returns>
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>
        /// Return this <see cref="ProgressBarRenderer"/> formatted as a progress bar
        /// </summary>
        /// <param name="renderSpinner">Whether to render the spinner</param>
        /// <returns>This <see cref="ProgressBarRenderer"/> formatted as a progress bar</returns>
        public string ToString(bool renderSpinner)
        {
            if (renderSpinner)
            {
                UpdateAnimationChar();
            }

            var percProgress = (Progress / MaxValue) * 100;
            var blocksToPrint = (int)Math.Truncate(percProgress / BlockCount);

            var progressDoneIndicator = new string('#', blocksToPrint);
            var progressToGoIndicator = new string('-', BlockCount - blocksToPrint);

            string renderedBar = $"[{progressDoneIndicator}{progressToGoIndicator}] {percProgress,3}%";

            if (renderSpinner)
            {
                var frame = Frames[_currentAnimationFrame];
                renderedBar += $" {frame}";
            }

            return renderedBar;
        }
    }
}
