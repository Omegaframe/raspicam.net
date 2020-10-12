using System;
using System.Collections;
using System.Collections.Generic;

namespace MMALSharp.Processing
{
    public class CircularBuffer<T> : IEnumerable<T>
    {
        readonly T[] _buffer;

        int _start;
        int _end;

        public CircularBuffer(int capacity) : this(capacity, new T[] { }) { }

        public CircularBuffer(int capacity, T[] items)
        {
            if (capacity < 1)
                throw new ArgumentException("Circular buffer cannot have negative or zero capacity.", nameof(capacity));

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (items.Length > capacity)
                throw new ArgumentException("Too many items to fit circular buffer", nameof(items));

            _buffer = new T[capacity];

            Array.Copy(items, _buffer, items.Length);
            Size = items.Length;

            _start = 0;
            _end = Size == capacity ? 0 : Size;
        }

        public int Capacity => _buffer.Length;
        public bool IsFull => Size == Capacity;
        public bool IsEmpty => Size == 0;
        public int Size { get; private set; }

        public T Back()
        {
            ThrowIfEmpty();
            return _buffer[(_end != 0 ? _end : Capacity) - 1];
        }

        public T this[int index]
        {
            get
            {
                if (IsEmpty)
                    throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer is empty");

                if (index >= Size)
                    throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer size is {Size}");

                var actualIndex = InternalIndex(index);
                return _buffer[actualIndex];
            }
            set
            {
                if (IsEmpty)
                    throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer is empty");

                if (index >= Size)
                    throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer size is {Size}");

                var actualIndex = InternalIndex(index);
                _buffer[actualIndex] = value;
            }
        }

        public void PushBack(T item)
        {
            if (IsFull)
            {
                _buffer[_end] = item;
                Increment(ref _end);
                _start = _end;
            }
            else
            {
                _buffer[_end] = item;
                Increment(ref _end);
                ++Size;
            }
        }

        public T[] ToArray()
        {
            var newArray = new T[Size];
            var newArrayOffset = 0;
            var segments = new[] { ArrayOne(), ArrayTwo() };

            foreach (var segment in segments)
            {
                Array.Copy(segment.Array, segment.Offset, newArray, newArrayOffset, segment.Count);
                newArrayOffset += segment.Count;
            }

            return newArray;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var segments = new[] { ArrayOne(), ArrayTwo() };

            foreach (var segment in segments)
            {
                for (var i = 0; i < segment.Count; i++)
                    yield return segment.Array[segment.Offset + i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ThrowIfEmpty(string message = "Cannot access an empty buffer.")
        {
            if (IsEmpty)
                throw new InvalidOperationException(message);
        }

        void Increment(ref int index)
        {
            if (++index == Capacity)
                index = 0;
        }

        int InternalIndex(int index) => _start + (index < (Capacity - _start) ? index : index - Capacity);

        ArraySegment<T> ArrayOne()
        {
            if (IsEmpty)
                return new ArraySegment<T>(new T[0]);

            if (_start < _end)
                return new ArraySegment<T>(_buffer, _start, _end - _start);

            return new ArraySegment<T>(_buffer, _start, _buffer.Length - _start);
        }

        ArraySegment<T> ArrayTwo()
        {
            if (IsEmpty)
                return new ArraySegment<T>(new T[0]);

            if (_start < _end)
                return new ArraySegment<T>(_buffer, _end, 0);

            return new ArraySegment<T>(_buffer, 0, _end);
        }
    }
}
