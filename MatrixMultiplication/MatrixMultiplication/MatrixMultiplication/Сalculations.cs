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

foreach (var size in sizes)
{
    var (standardCalculations, parallelCalculations) = Calculate(size);
    var averageForStandardCalculations = Enumerable.Average(standardCalculations);
    var averageForParallelCalculations = Enumerable.Average(parallelCalculations);
    var varianceForStandardCalculations = Enumerable.Average(standardCalculations.Select(x => x * x)) - averageForStandardCalculations * averageForStandardCalculations;
    var varianceForParallelCalculations = Enumerable.Average(parallelCalculations.Select(x => x * x)) - averageForParallelCalculations * averageForParallelCalculations;

    Console.WriteLine($"Average operation time of standart multiplication for a matrix of size {size} * {size} : {Math.Round(averageForStandardCalculations, 3)}");
    Console.WriteLine($"Standard deviation operation time of standart multiplication for a matrix of size {size} * {size} : {Math.Round(Math.Sqrt(varianceForStandardCalculations), 3)}");
    Console.WriteLine();
    Console.WriteLine($"Average operation time of parallel multiplication for a matrix of size {size} * {size} : {Math.Round(averageForParallelCalculations, 3)}");
    Console.WriteLine($"Standard deviation operation time of parallel multiplication for a matrix of size {size} * {size} : {Math.Round(Math.Sqrt(varianceForParallelCalculations), 3)}");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();

}


// PREVIOUS RESULTS------------------------------------------------------------------------------------

/*
Average operation time of standart multiplication for a matrix of size 8 * 8 : 0
Standard deviation operation time of standart multiplication for a matrix of size 8 * 8 : 0

Average operation time of parallel multiplication for a matrix of size 8 * 8 : 40,8
Standard deviation operation time of parallel multiplication for a matrix of size 8 * 8 : 14,270949512909086



Average operation time of standart multiplication for a matrix of size 16 * 16 : 0
Standard deviation operation time of standart multiplication for a matrix of size 16 * 16 : 0

Average operation time of parallel multiplication for a matrix of size 16 * 16 : 53,42
Standard deviation operation time of parallel multiplication for a matrix of size 16 * 16 : 8,158651849417277



Average operation time of standart multiplication for a matrix of size 32 * 32 : 0
Standard deviation operation time of standart multiplication for a matrix of size 32 * 32 : 0

Average operation time of parallel multiplication for a matrix of size 32 * 32 : 52,23
Standard deviation operation time of parallel multiplication for a matrix of size 32 * 32 : 9,95575712841572



Average operation time of standart multiplication for a matrix of size 64 * 64 : 2,02
Standard deviation operation time of standart multiplication for a matrix of size 64 * 64 : 0,13999999999999863

Average operation time of parallel multiplication for a matrix of size 64 * 64 : 53,97
Standard deviation operation time of parallel multiplication for a matrix of size 64 * 64 : 10,309660518174224



Average operation time of standart multiplication for a matrix of size 128 * 128 : 17,49
Standard deviation operation time of standart multiplication for a matrix of size 128 * 128 : 1,4594176920950426

Average operation time of parallel multiplication for a matrix of size 128 * 128 : 73,79
Standard deviation operation time of parallel multiplication for a matrix of size 128 * 128 : 23,017947345495404



Average operation time of standart multiplication for a matrix of size 256 * 256 : 146,36
Standard deviation operation time of standart multiplication for a matrix of size 256 * 256 : 6,56432784068539

Average operation time of parallel multiplication for a matrix of size 256 * 256 : 138,93
Standard deviation operation time of parallel multiplication for a matrix of size 256 * 256 : 31,20488904002064



Average operation time of standart multiplication for a matrix of size 512 * 512 : 1735,58
Standard deviation operation time of standart multiplication for a matrix of size 512 * 512 : 112,28768231645189

Average operation time of parallel multiplication for a matrix of size 512 * 512 : 461,95
Standard deviation operation time of parallel multiplication for a matrix of size 512 * 512 : 62,070182052254516



Average operation time of standart multiplication for a matrix of size 1024 * 1024 : 14982,31
Standard deviation operation time of standart multiplication for a matrix of size 1024 * 1024 : 654,6658337656055

Average operation time of parallel multiplication for a matrix of size 1024 * 1024 : 2771,56
Standard deviation operation time of parallel multiplication for a matrix of size 1024 * 1024 : 241,3810812802043
*/
