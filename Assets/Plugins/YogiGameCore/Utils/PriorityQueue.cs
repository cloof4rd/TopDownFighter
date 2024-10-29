using System;
using System.Collections.Generic;

namespace YogiGameCore.Utils
{
    /// <summary>
    /// 优先队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> _data;

        public int Count => _data.Count;
        public int Capacity => _data.Capacity;
        public bool IsEmpty => _data.Count <= 0;

        public PriorityQueue() : this(8)
        {
        }

        public PriorityQueue(int size)
        {
            _data = new List<T>(size);
        }

        public void Enqueue(T item)
        {
            _data.Add(item);
            PushHeap(_data);
        }

        public T Dequeue()
        {
            T dequeTarget = Peek();
            PopHeap(_data);
            _data.RemoveAt(_data.Count - 1);
            return dequeTarget;
        }

        public T Peek()
        {
            if (_data.Count <= 0) throw new IndexOutOfRangeException();
            return _data[0];
        }

        public void Clear()
        {
            _data.Clear();
        }

        public void TrimExcess()
        {
            _data.TrimExcess();
        }

        public bool Contains(T item)
        {
            return _data.Contains(item);
        }

        public static void PushHeap(IList<T> list)
        {
            var count = list.Count;
            if (count < 0 || count >= int.MaxValue) throw new IndexOutOfRangeException();
            if (count >= 2)
            {
                var uLast = count - 1;
                var val = list[uLast];
                PushHeapByIndex(list, uLast, 0, val);
            }
        }

        private static void PushHeapByIndex(IList<T> list, int hole, int top, T value)
        {
            for (var idx = (hole - 1) >> 1;
                 top < hole && value.CompareTo(list[idx]) < 0;
                 idx = (hole - 1) >> 1)
            {
                list[hole] = list[idx];
                hole = idx;
            }

            list[hole] = value;
        }

        public static void PopHeap(IList<T> list)
        {
            var count = list.Count;
            if (count < 0 || count >= int.MaxValue) throw new IndexOutOfRangeException();
            if (count >= 2)
            {
                var uLast = count - 1;
                var val = list[uLast];
                list[uLast] = list[0];
                var hole = 0;
                var bottom = uLast;
                var top = hole;
                var idx = hole;
                var maxSequenceNonLeaf = (bottom - 1) >> 1;
                while (idx < maxSequenceNonLeaf)
                {
                    idx = 2 * idx + 2;
                    if (list[idx - 1].CompareTo(list[idx]) < 0)
                    {
                        --idx;
                    }

                    list[hole] = list[idx];
                    hole = idx;
                }

                if (idx == maxSequenceNonLeaf && bottom % 2 == 0)
                {
                    list[hole] = list[bottom - 1];
                    hole = bottom - 1;
                }

                PushHeapByIndex(list, hole, top, val);
            }
        }
    }
}