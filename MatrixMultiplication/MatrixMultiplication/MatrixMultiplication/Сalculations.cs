using System.Diagnostics;

/*if (args.Length != 2)
{
    Console.WriteLine("at the input, the program should receive two files");
    return;
}

var firstMatrix = MatrixMultiplication.Matrix.ReadMatrix(args[0]);
var secondMatrix = MatrixMultiplication.Matrix.ReadMatrix(args[1]);
var result = firstMatrix.Multiply(secondMatrix, new MatrixMultiplication.ParallelStrategy());
result.PrintMatrix("result.txt");*/

static (IEnumerable<long>, IEnumerable<long>) Calculate(int size)
{
    var firstMatrix = MatrixMultiplication.Matrix.Generate(size, size);
    var secondMatrix = MatrixMultiplication.Matrix.Generate(size, size);
    var stopWatch = new Stopwatch();
    var numberOfIterations = 100;
    var standardCalculations = new long[numberOfIterations];
    var parallelCalculations = new long[numberOfIterations];
    var sequentialStrategy = new MatrixMultiplication.SequentialStrategy();
    var parallelStrategy = new MatrixMultiplication.ParallelStrategy();
    for (int i = 0; i < numberOfIterations; i++)
    {
        stopWatch.Reset();
        stopWatch.Start();
        firstMatrix.Multiply(secondMatrix, sequentialStrategy);
        stopWatch.Stop();
        standardCalculations[i] = stopWatch.ElapsedMilliseconds;

        stopWatch.Reset();
        stopWatch.Start();
        firstMatrix.Multiply(secondMatrix, parallelStrategy);
        stopWatch.Stop();
        parallelCalculations[i] = stopWatch.ElapsedMilliseconds;
    }

    return (standardCalculations, parallelCalculations);
}

// тут должны быть размеры матриц, но уже все посчитано
var sizes = new int[] {8, 16, 32, 64, 128, 256, 512, 1024};

using StreamWriter stream = new StreamWriter("Calculation.csv");
stream.Write("Size StandartAverage StandartDeviation ParallelAverage ParallelDeviation");

foreach (var size in sizes)
{
    var (standardCalculations, parallelCalculations) = Calculate(size);
    var averageForStandardCalculations = Enumerable.Average(standardCalculations);
    var averageForParallelCalculations = Enumerable.Average(parallelCalculations);
    var varianceForStandardCalculations = Enumerable.Average(standardCalculations.Select(x => x * x)) - averageForStandardCalculations * averageForStandardCalculations;
    var varianceForParallelCalculations = Enumerable.Average(parallelCalculations.Select(x => x * x)) - averageForParallelCalculations * averageForParallelCalculations;
    stream.WriteLine();
    stream.Write($"{size} {Math.Round(averageForStandardCalculations, 3)} {Math.Round(Math.Sqrt(varianceForStandardCalculations), 3)} {Math.Round(averageForParallelCalculations, 3)} {Math.Round(Math.Sqrt(varianceForParallelCalculations), 3)}");
    Console.WriteLine($"Average operation time of standart multiplication for a matrix of size {size} * {size} : {Math.Round(averageForStandardCalculations, 3)}");
    Console.WriteLine($"Standard deviation operation time of standart multiplication for a matrix of size {size} * {size} : {Math.Round(Math.Sqrt(varianceForStandardCalculations), 3)}");
    Console.WriteLine();
    Console.WriteLine($"Average operation time of parallel multiplication for a matrix of size {size} * {size} : {Math.Round(averageForParallelCalculations, 3)}");
    Console.WriteLine($"Standard deviation operation time of parallel multiplication for a matrix of size {size} * {size} : {Math.Round(Math.Sqrt(varianceForParallelCalculations), 3)}");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();

}

/*
 * Average operation time of standart multiplication for a matrix of size 8 * 8 : 0
Standard deviation operation time of standart multiplication for a matrix of size 8 * 8 : 0

Average operation time of parallel multiplication for a matrix of size 8 * 8 : 52,45
Standard deviation operation time of parallel multiplication for a matrix of size 8 * 8 : 56,882



Average operation time of standart multiplication for a matrix of size 16 * 16 : 0,03
Standard deviation operation time of standart multiplication for a matrix of size 16 * 16 : 0,298

Average operation time of parallel multiplication for a matrix of size 16 * 16 : 61,37
Standard deviation operation time of parallel multiplication for a matrix of size 16 * 16 : 17,762



Average operation time of standart multiplication for a matrix of size 32 * 32 : 0,37
Standard deviation operation time of standart multiplication for a matrix of size 32 * 32 : 0,658

Average operation time of parallel multiplication for a matrix of size 32 * 32 : 73,55
Standard deviation operation time of parallel multiplication for a matrix of size 32 * 32 : 22,835



Average operation time of standart multiplication for a matrix of size 64 * 64 : 6,47
Standard deviation operation time of standart multiplication for a matrix of size 64 * 64 : 3,116

Average operation time of parallel multiplication for a matrix of size 64 * 64 : 74,18
Standard deviation operation time of parallel multiplication for a matrix of size 64 * 64 : 18,777



Average operation time of standart multiplication for a matrix of size 128 * 128 : 54,66
Standard deviation operation time of standart multiplication for a matrix of size 128 * 128 : 16,602

Average operation time of parallel multiplication for a matrix of size 128 * 128 : 106,64
Standard deviation operation time of parallel multiplication for a matrix of size 128 * 128 : 33,589



Average operation time of standart multiplication for a matrix of size 256 * 256 : 416,19
Standard deviation operation time of standart multiplication for a matrix of size 256 * 256 : 50,566

Average operation time of parallel multiplication for a matrix of size 256 * 256 : 205,77
Standard deviation operation time of parallel multiplication for a matrix of size 256 * 256 : 40,362



Average operation time of standart multiplication for a matrix of size 512 * 512 : 4387,75
Standard deviation operation time of standart multiplication for a matrix of size 512 * 512 : 329,556

Average operation time of parallel multiplication for a matrix of size 512 * 512 : 785,29
Standard deviation operation time of parallel multiplication for a matrix of size 512 * 512 : 61,57



Average operation time of standart multiplication for a matrix of size 1024 * 1024 : 39185,98
Standard deviation operation time of standart multiplication for a matrix of size 1024 * 1024 : 10638,475

Average operation time of parallel multiplication for a matrix of size 1024 * 1024 : 5329,88
Standard deviation operation time of parallel multiplication for a matrix of size 1024 * 1024 : 1176,101
*/