Testing different implementation for passing single item collection. Test includes both parts - creation and full iteration. It intentionally uses IEnumerable<T> contract. Struct contracts can be faster and have less allocations, but such optimizations will be broken by calling linq methods. 

Conclusion: Yield is good enough as for year 2017.

https://stackoverflow.com/questions/1577822/passing-a-single-item-as-ienumerablet

``` ini

BenchmarkDotNet=v0.10.6, OS=Windows 10 Redstone 1 (10.0.14393)
Processor=Intel Core i7-4770 CPU 3.40GHz (Haswell), ProcessorCount=8
Frequency=3318387 Hz, Resolution=301.3512 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1648.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1648.0


```
 |           Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
 |----------------- |---------:|----------:|----------:|-------:|----------:|
 |            Array | 24.49 ns | 0.1977 ns | 0.1753 ns | 0.0152 |      64 B |
 | EnumerableRepeat | 29.71 ns | 0.1216 ns | 0.0879 ns | 0.0152 |      64 B |
 |            Yield | 21.80 ns | 0.2598 ns | 0.2303 ns | 0.0076 |      32 B |
 | SingleEnumerator | 17.65 ns | 0.1694 ns | 0.1584 ns | 0.0076 |      32 B |
