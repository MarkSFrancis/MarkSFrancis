﻿using MarkSFrancis.Console;
using SyntaxTest.Demos.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyntaxTest.Demos
{
    public class ThreadsDemo : IDemo
    {
        public ThreadsDemo(ConsoleIo console, Mode mode)
        {
            _console = console;
            ThreadMode = mode;
        }

        private readonly ConsoleIo _console;
        private readonly object _syncContext = new object();
        public Mode ThreadMode { get; }
        private int _val = 0;
        private const int IncCount = 100000000;

        private delegate void Incrementer<T1, in T2>(ref T1 value, T2 timesToIncrement);

        public void Run()
        {
            var delegateMethod = GetMethodToRun();

            Task thd1 = new Task(() => delegateMethod(ref _val, IncCount / 2));
            Task thd2 = new Task(() => delegateMethod(ref _val, IncCount / 2));
            thd1.Start();
            thd2.Start();

            using (var progress = _console.ProgressBar(IncCount))
            {
                while (!thd1.IsCompleted && !thd2.IsCompleted)
                {
                    progress.Progress = _val;
                    Thread.Sleep(100);
                }

                progress.WriteCompleted();
            }

            Task.WaitAll(thd1, thd2);

            _console.WriteLine("Value of val: ");
            _console.WriteLine(_val);
            _console.WriteLine();

            _console.WriteLine("Value should be: ");
            _console.WriteLine(IncCount);
        }

        private Incrementer<int, int> GetMethodToRun()
        {
            switch (ThreadMode)
            {
                case Mode.Unsafe:
                    return IncrementLotsUnsafe;
                case Mode.Interlocked:
                    return IncrementLotsInterlocked;
                case Mode.Synced:
                    return IncrementLotsSynced;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void IncrementLotsUnsafe(ref int value, int timesToIncrement)
        {
            for (int incrementCount = 0; incrementCount < timesToIncrement; ++incrementCount)
            {
                ++value;
            }
        }

        private void IncrementLotsSynced(ref int value, int timesToIncrement)
        {
            for (int incrementCount = 0; incrementCount < timesToIncrement; ++incrementCount)
            {
                lock (_syncContext)
                {
                    ++value;
                }
            }
        }

        private void IncrementLotsInterlocked(ref int value, int timesToIncrement)
        {
            for (int incrementCount = 0; incrementCount < timesToIncrement; ++incrementCount)
            {
                Interlocked.Increment(ref value);
            }
        }

        public enum Mode
        {
            Unsafe,
            Interlocked,
            Synced
        }
    }
}
