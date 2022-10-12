namespace MatrixOperations;

using System.Threading;

/// <summary>
/// A class that implements operations with matrices
/// </summary>
public static class MatrixOperations
{
    /// <summary>
    /// Function for sequential matrix multiplication
    /// </summary>
    /// <param name="firstMatrix">First matrix</param>
    /// <param name="secondMatrix">Second matrix</param>
    /// <returns>Result of multiplication</returns>
    /// <exception cref="ArgumentException">When number of columns of the first matrix is not equal to the number of rows of the second</exception>
    public static int[,] StandartMultiply(int[,] firstMatrix, int[,] secondMatrix)
    {
        if (firstMatrix.GetLength(1) != secondMatrix.GetLength(0))
        {
            throw new ArgumentException("the number of columns of the first matrix is not equal to the number of rows of the second");
        }

        int[,] result = new int[firstMatrix.GetLength(0), secondMatrix.GetLength(1)];

        for (int i = 0; i < firstMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < secondMatrix.GetLength(1); j++)
            {
                for (int k = 0; k < secondMatrix.GetLength(0); k++)
                {
                    result[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Function for parallel matrix multiplication
    /// </summary>
    /// <param name="firstMatrix">First matrix</param>
    /// <param name="secondMatrix">Second matrix</param>
    /// <returns>Result of multiplication</returns>
    /// <exception cref="ArgumentException">When number of columns of the first matrix is not equal to the number of rows of the second</exception>
    public static int[,] ParallelMultiply(int[,] firstMatrix, int[,] secondMatrix)
    {
        if (firstMatrix.GetLength(1) != secondMatrix.GetLength(0))
        {
            throw new ArgumentException("the number of columns of the first matrix is not equal to the number of rows of the second");
        }

        int[,] result = new int[firstMatrix.GetLength(0), secondMatrix.GetLength(1)];
        Thread[] threads = new Thread[Math.Min(Environment.ProcessorCount, firstMatrix.GetLength(0))];
        int chunkSize = firstMatrix.GetLength(0) / threads.Length;
        for (int i = 0; i < threads.Length; i++)
        {
            var locali = i;

            threads[locali] = new Thread(() => {
                for (int j = locali * chunkSize; j < Math.Min(chunkSize * (locali + 1), firstMatrix.GetLength(0)); j++)
                {
                    for (int l = 0; l < secondMatrix.GetLength(1); l++)
                    {
                        for (int k = 0; k < secondMatrix.GetLength(0); k++)
                        {
                            result[j, l] += firstMatrix[j, k] * secondMatrix[k, l];
                        }
                    }
                }
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        return result;
    }

    /// <summary>
    /// Function for generating matrices of a given size
    /// </summary>
    /// <param name="numberOfRows">Number of rows</param>
    /// <param name="numberOfColumns">Number of columns</param>
    /// <returns>Matrix of a given size</returns>
    /// <exception cref="ArgumentOutOfRangeException">invalid value for the number of rows or columns</exception> 
    public static int[,] Generate(int numberOfRows, int numberOfColumns)
    {
        if (numberOfRows <= 0) {
            throw new ArgumentOutOfRangeException("invalid value for the number of rows");
        }

        else if (numberOfColumns <= 0) {
            throw new ArgumentOutOfRangeException("invalid value for the number of columns");
        }

        Random random = new();
        int[,] result = new int[numberOfRows, numberOfColumns];
        for (int j = 0; j < numberOfRows; j++)
        {
            for (int i = 0; i < numberOfColumns; i++)
            {
                result[j, i] = random.Next();
            }
        }

        return result;
    }

    /// <summary>
    /// Function for reading a matrix from a file
    /// </summary>
    /// <param name="pathToFile">Path to file</param>
    /// <returns>The matrix read from the file</returns>
    public static int[,] ReadMatrix(string pathToFile)
    {
        var strings = System.IO.File.ReadAllLines(pathToFile);
        int[,] matrix = new int[strings.Length, strings[0].Split().Length];
        for (int i = 0; i < strings.Length; i++)
        {
            var results = strings[i].Split();
            for (int k = 0; k < results.Length; k++)
            {
                if (Int32.TryParse(results[k], out int result))
                {
                    matrix[i, k] = result;
                }
            }
        }

        return matrix;
    }

    /// <summary>
    /// Function for writing matrices to file
    /// </summary>
    /// <param name="pathToFile">Paath to file</param>
    /// <param name="matrix">The matrix to be written to the file</param>
    public static void PrintMatrix(string pathToFile, int[,] matrix)
    {
        using StreamWriter writer = new (pathToFile);
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1) - 1; j++)
            {
                writer.Write($"{matrix[i, j]} ");
            }
            writer.Write($"{matrix[i, matrix.GetLength(1) - 1]}");

            if (i != matrix.GetLength(0) - 1)
            {
                writer.WriteLine();
            }
        }
    }

    /// <summary>
    /// Function for comparing matrices
    /// </summary>
    /// <param name="firstMatrix">First matrix</param>
    /// <param name="secondMatrix">Second matrix</param>
    /// <returns>True if the matrices are equal, otherwise false</returns>
    public static bool Equals(int[,] firstMatrix, int[,] secondMatrix)
    {
        if (firstMatrix.GetLength(0) != secondMatrix.GetLength(0) || firstMatrix.GetLength(1) != secondMatrix.GetLength(1))
        {
            return false;
        }

        for (int i = 0; i < firstMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < firstMatrix.GetLength(1); j++)
            {
                if (firstMatrix[i, j] != secondMatrix[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }
}