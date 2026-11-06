
using System;

public static class TimeSpanExtensions
{
    extension(TimeSpan source)
    {
        /// <summary>
        /// Modulo operation defined for the lengths of TimeSpans
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static TimeSpan operator %(TimeSpan a, TimeSpan b) => new(a.Ticks % b.Ticks);
    }
}
