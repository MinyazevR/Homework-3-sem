using System.Diagnostics;

if (args.Length < 1)
{
    Console.WriteLine("Количество аргументв должно быть не меньше 1");
    return;
}


static (IEnumerable<long>, IEnumerable<long>) Calculate(string path)
{
    var stopWatch = new Stopwatch();
    var numberOfIterations = 100;
    var standardCalculations = new long[numberOfIterations];
    var parallelCalculations = new long[numberOfIterations];
    for (int i = 0; i < numberOfIterations; i++)
    {
        stopWatch.Reset();
        stopWatch.Start();
        var second = Test.MD5.SequentiallyComputeCheckSumForDirectory(path);
        stopWatch.Stop();
        standardCalculations[i] = stopWatch.ElapsedMilliseconds;

        stopWatch.Reset();
        stopWatch.Start();
        var first = Test.MD5.ParallelComputeCheckSumForDirectory(path);
        stopWatch.Stop();
        parallelCalculations[i] = stopWatch.ElapsedMilliseconds;
    }

    return (standardCalculations, parallelCalculations);
}

foreach (var path in args)
{
    var (standardCalculations, parallelCalculations) = Calculate(path);
    var averageForStandardCalculations = Enumerable.Average(standardCalculations);
    var averageForParallelCalculations = Enumerable.Average(parallelCalculations);
    var varianceForStandardCalculations = Enumerable.Average(standardCalculations.Select(x => x * x)) - averageForStandardCalculations * averageForStandardCalculations;
    var varianceForParallelCalculations = Enumerable.Average(parallelCalculations.Select(x => x * x)) - averageForParallelCalculations * averageForParallelCalculations;
    //stream.WriteLine();
    //stream.Write($"{size} {Math.Round(averageForStandardCalculations, 3)} {Math.Round(Math.Sqrt(varianceForStandardCalculations), 3)} {Math.Round(averageForParallelCalculations, 3)} {Math.Round(Math.Sqrt(varianceForParallelCalculations), 3)}");
    Console.WriteLine($"Average operation time of standart : {Math.Round(averageForStandardCalculations, 3)}");
    Console.WriteLine($"Standard deviation operation time of standart: {Math.Round(Math.Sqrt(varianceForStandardCalculations), 3)}");
    Console.WriteLine();
    Console.WriteLine($"Average operation time of parallel : {Math.Round(averageForParallelCalculations, 3)}");
    Console.WriteLine($"Standard deviation operation time of parallel : {Math.Round(Math.Sqrt(varianceForParallelCalculations), 3)}");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();

}
