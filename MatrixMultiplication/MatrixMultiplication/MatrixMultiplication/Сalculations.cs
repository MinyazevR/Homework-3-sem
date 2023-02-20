using System.Diagnostics;
using MatrixMultiplication;

if (args.Length != 2)
{
    Console.WriteLine("at the input, the program should receive two files");
    return;
}

var firstMatrix = Matrix.ReadMatrix(args[0]);
var secondMatrix = Matrix.ReadMatrix(args[1]);
var result = firstMatrix.Multiply(secondMatrix, new MatrixMultiplication.ParallelStrategy());
result.PrintMatrix("result.txt");

static (IEnumerable<long>, IEnumerable<long>) Calculate(int size)
{
    var firstMatrix = Matrix.Generate(size, size);
    var secondMatrix = Matrix.Generate(size, size);
    var stopWatch = new Stopwatch();
    var numberOfIterations = 100;
    var standardCalculations = new long[numberOfIterations];
    var parallelCalculations = new long[numberOfIterations];
    var sequentialStrategy = new SequentialStrategy();
    var parallelStrategy = new ParallelStrategy();
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
var sizes = new int[] {/*8, 16, 32, 64, 128, 256, 512, 1024*/};

//using StreamWriter stream = new StreamWriter("Calculation.csv");
//stream.Write("Size standart_average StandartDeviation parallel_average ParallelDeviation");

foreach (var size in sizes)
{
    var (standardCalculations, parallelCalculations) = Calculate(size);
    var averageForStandardCalculations = Enumerable.Average(standardCalculations);
    var averageForParallelCalculations = Enumerable.Average(parallelCalculations);
    var varianceForStandardCalculations = Enumerable.Average(standardCalculations.Select(x => x * x)) - averageForStandardCalculations * averageForStandardCalculations;
    var varianceForParallelCalculations = Enumerable.Average(parallelCalculations.Select(x => x * x)) - averageForParallelCalculations * averageForParallelCalculations;
    //stream.WriteLine();
    //stream.Write($"{size} {Math.Round(averageForStandardCalculations, 3)} {Math.Round(Math.Sqrt(varianceForStandardCalculations), 3)} {Math.Round(averageForParallelCalculations, 3)} {Math.Round(Math.Sqrt(varianceForParallelCalculations), 3)}");
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
Average operation time of standart multiplication for a matrix of size 8 * 8 : 0
Standard deviation operation time of standart multiplication for a matrix of size 8 * 8 : 0

Average operation time of parallel multiplication for a matrix of size 8 * 8 : 34,96
Standard deviation operation time of parallel multiplication for a matrix of size 8 * 8 : 14,788



Average operation time of standart multiplication for a matrix of size 16 * 16 : 0
Standard deviation operation time of standart multiplication for a matrix of size 16 * 16 : 0

Average operation time of parallel multiplication for a matrix of size 16 * 16 : 48,32
Standard deviation operation time of parallel multiplication for a matrix of size 16 * 16 : 7,571



Average operation time of standart multiplication for a matrix of size 32 * 32 : 0,03
Standard deviation operation time of standart multiplication for a matrix of size 32 * 32 : 0,171

Average operation time of parallel multiplication for a matrix of size 32 * 32 : 45,51
Standard deviation operation time of parallel multiplication for a matrix of size 32 * 32 : 9,444



Average operation time of standart multiplication for a matrix of size 64 * 64 : 5,15
Standard deviation operation time of standart multiplication for a matrix of size 64 * 64 : 0,517

Average operation time of parallel multiplication for a matrix of size 64 * 64 : 56,87
Standard deviation operation time of parallel multiplication for a matrix of size 64 * 64 : 12,45



Average operation time of standart multiplication for a matrix of size 128 * 128 : 42,41
Standard deviation operation time of standart multiplication for a matrix of size 128 * 128 : 0,971

Average operation time of parallel multiplication for a matrix of size 128 * 128 : 72,71
Standard deviation operation time of parallel multiplication for a matrix of size 128 * 128 : 21,6



Average operation time of standart multiplication for a matrix of size 256 * 256 : 343,86
Standard deviation operation time of standart multiplication for a matrix of size 256 * 256 : 4,98

Average operation time of parallel multiplication for a matrix of size 256 * 256 : 132,01
Standard deviation operation time of parallel multiplication for a matrix of size 256 * 256 : 22,592



Average operation time of standart multiplication for a matrix of size 512 * 512 : 3423,96
Standard deviation operation time of standart multiplication for a matrix of size 512 * 512 : 22,406

Average operation time of parallel multiplication for a matrix of size 512 * 512 : 584,37
Standard deviation operation time of parallel multiplication for a matrix of size 512 * 512 : 33,468



Average operation time of standart multiplication for a matrix of size 1024 * 1024 : 35753,9
Standard deviation operation time of standart multiplication for a matrix of size 1024 * 1024 : 26004,022

Average operation time of parallel multiplication for a matrix of size 1024 * 1024 : 4854,64
Standard deviation operation time of parallel multiplication for a matrix of size 1024 * 1024 : 381,964
*/