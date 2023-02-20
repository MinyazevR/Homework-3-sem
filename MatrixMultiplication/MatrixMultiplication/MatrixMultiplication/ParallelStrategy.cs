namespace MatrixMultiplication;

/// <summary>
/// Strategy for parallel multiplication
/// </summary>
public class ParallelStrategy : IStrategy
{
    /// <summary>
    /// Function for parallel matrix multiplication
    /// </summary>
    /// <param name="firstMatrix">First matrix</param>
    /// <param name="secondMatrix">Second matrix</param>
    /// <returns>Result of multiplication</returns>
    /// <exception cref="ArgumentException">When number of columns of the first matrix is not equal to the number of rows of the second</exception>
    public Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (firstMatrix.NumberOfCols != secondMatrix.NumberOfRows)
        {
            throw new ArgumentException("the number of columns of the first matrix is not equal to the number of rows of the second");
        }

        var result = new Matrix(new int[firstMatrix.NumberOfRows, secondMatrix.NumberOfCols]);
        var threads = new Thread[Math.Min(Environment.ProcessorCount, firstMatrix.NumberOfRows)];
        var chunkSize = firstMatrix.NumberOfRows / threads.Length;

        for (int i = 0; i < threads.Length; i++)
        {
            var locali = i;

            threads[locali] = new Thread(() => {
                for (int j = locali * chunkSize; j < Math.Min(chunkSize * (locali + 1), firstMatrix.NumberOfRows); j++)
                {
                    for (int l = 0; l < secondMatrix.NumberOfCols; l++)
                    {
                        for (int k = 0; k < secondMatrix.NumberOfRows; k++)
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
}