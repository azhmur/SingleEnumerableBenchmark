using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace ConsoleApp10
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SingleEnumerable>();
        }
    }

    [MemoryDiagnoser]
    public class SingleEnumerable
    {
        private static readonly string value = "value";

        [Benchmark]
        public void Array()
        {
            foreach (var i in GetArray())
            {
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<string> GetArray()
        {
            return new[] { value };
        }

        [Benchmark]
        public void EnumerableRepeat()
        {
            foreach (var i in GetEnumerableRepeat())
            {
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<string> GetEnumerableRepeat()
        {
            return Enumerable.Repeat(value, 1);
        }

        [Benchmark]
        public void Yield()
        {
            foreach (var i in GetYield())
            {
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<string> GetYield()
        {
            yield return value;
        }

        [Benchmark]
        public void SingleEnumerator()
        {
            foreach (var i in GetSingleEnumerator())
            {
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<string> GetSingleEnumerator()
        {
            return new SingleEnumerable<string>(value);
        }
    }

    public sealed class SingleEnumerable<T> : IEnumerable<T>, IEnumerator<T>
    {
        private T _value;
        private int _state;

        public SingleEnumerable(T value)
        {
            _value = value;
            _state = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_state == 0)
            {
                _state = 1;
                return this;
            }

            return new SingleEnumerable<T>(_value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            switch (_state)
            {
                case 0:
                case 1:
                    _state = 2;
                    return true;

                case 2:
                    _state = 3;
                    return false;

                default:
                    return false;
            }
        }

        public void Reset()
        {
            _state = 1;
        }

        public T Current
        {
            get
            {
                if (_state == 2)
                    return _value;

                throw new InvalidOperationException();
            }
        }

        object IEnumerator.Current => Current;
    }
}
