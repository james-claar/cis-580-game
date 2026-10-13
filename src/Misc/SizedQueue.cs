using System.Collections.Generic;

namespace cis_580_game;

// Copied from https://stackoverflow.com/a/49924960
public sealed class SizedQueue<T>(int fixedCapacity) : Queue<T>
{
    public int FixedCapacity { get; } = fixedCapacity;

    /// <summary>
    /// If the total number of item exceed the capacity, the oldest ones automatically dequeues.
    /// </summary>
    /// <returns>The dequeued value, if any.</returns>
    public new T Enqueue(T item)
    {
        base.Enqueue(item);
        if (Count > FixedCapacity)
        {
            return Dequeue();
        }
        return default;
    }
}
