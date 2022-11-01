namespace MatrixMultiplication;

public class Matrix
{
    /// <summary>
    /// Number of rows in the matrix
    /// </summary>
    public int NumberOfRows { get; private set; }

    /// <summary>
    /// Number of cols in the matrix
    /// </summary>
    public int NumberOfCols { get; private set; }

    private readonly int[,] data;
    static readonly Random random = new();

    /// <summary>
    /// Indexer
    /// </summary>
    /// <param name="i">row index</param>
    /// <param name="j">column index</param>
    /// <returns></returns>
    public int this[int i, int j]
    {
        get => data[i, j];
        set => data[i, j] = value;
    }

    /// <summary>
    /// Two-dimensional array for matrix initialization
    /// </summary>
    /// <param name="data"></param>
    public Matrix(int[,] data)
    {
        this.data = data;
        this.NumberOfRows = data.GetLength(0);
        this.NumberOfCols = data.GetLength(1);
    }

    /// <summary>
    /// Function for matrix multiplication
    /// </summary>
    /// <param name="matrix">First matrix</param>
    /// <param name="strategy">Second matrix</param>
    /// <returns>Result of multiplication</returns>
    public Matrix Multiply(Matrix matrix, IStrategy strategy)
    {
        return strategy.Multiply(this, matrix);
    }

    /// <summary>
    /// Function for sequential matrix multiplication
    /// </summary>
    /// <param name="this">First matrix</param>
    /// <param name="secondMatrix">Second matrix</param>
    /// <returns>Result of multiplication</returns>
    /// <exception cref="ArgumentException">When number of columns of the first matrix is not equal to the number of rows of the second</exception>
    public Matrix SequentialMultiply(Matrix secondMatrix)
    {
        if (this.NumberOfCols != secondMatrix.NumberOfRows)
        {
            throw new ArgumentException("the number of columns of the first matrix is not equal to the number of rows of the second");
        }

        var result = new Matrix(new int[this.NumberOfRows, secondMatrix.NumberOfCols]);

        for (int i = 0; i < this.NumberOfRows; i++)
        {
            for (int j = 0; j < secondMatrix.NumberOfCols; j++)
            {
                for (int k = 0; k < secondMatrix.NumberOfRows; k++)
                {
                    result[i, j] += this[i, k] * secondMatrix[k, j];
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
    public Matrix ParallelMultiply(Matrix secondMatrix)
    {
        if (this.NumberOfCols != secondMatrix.NumberOfRows)
        {
            throw new ArgumentException("the number of columns of the first matrix is not equal to the number of rows of the second");
        }

        var result = new Matrix(new int[this.NumberOfRows, secondMatrix.NumberOfCols]);
        var threads = new Thread[Math.Min(Environment.ProcessorCount, this.NumberOfRows)];
        var chunkSize = this.NumberOfRows / threads.Length;

        for (int i = 0; i < threads.Length; i++)
        {
            var locali = i;

            threads[locali] = new Thread(() => {
                for (int j = locali * chunkSize; j < Math.Min(chunkSize * (locali + 1), this.NumberOfRows); j++)
                {
                    for (int l = 0; l < secondMatrix.NumberOfCols; l++)
                    {
                        for (int k = 0; k < secondMatrix.NumberOfRows; k++)
                        {
                            result[j, l] += this[j, k] * secondMatrix[k, l];
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
    public static Matrix Generate(int numberOfRows, int numberOfColumns)
    {
        if (numberOfRows <= 0)
        {
            throw new ArgumentOutOfRangeException("invalid value for the number of rows");
        }
        else if (numberOfColumns <= 0)
        {
            throw new ArgumentOutOfRangeException("invalid value for the number of columns");
        }

        var result = new Matrix(new int[numberOfRows, numberOfColumns]);

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
    public static Matrix ReadMatrix(string pathToFile)
    {
        var strings = System.IO.File.ReadAllLines(pathToFile);
        var matrix = new Matrix(new int[strings.Length, strings[0].Split().Length]);

        for (int i = 0; i < strings.Length; i++)
        {
            var results = strings[i].Split();
            for (int k = 0; k < results.Length; k++)
            {
                if (Int32.TryParse(results[k], out int result))
                {
                    matrix[i, k] = result;
                }
                else
                {
                    throw new InvalidDataException();
                }
            }
        }

        return matrix;
    }

    /// <summary>
    /// Function for comparing matrices
    /// </summary>
    /// <param name="this">First matrix</param>
    /// <param name="secondMatrix">Second matrix</param>
    /// <returns>True if the matrices are equal, otherwise false</returns>
    public bool Equals(Matrix secondMatrix)
    {
        if (this.NumberOfRows != secondMatrix.NumberOfRows || this.NumberOfCols != secondMatrix.NumberOfCols)
        {
            return false;
        }

        for (int i = 0; i < this.NumberOfRows; i++)
        {
            for (int j = 0; j < this.NumberOfCols; j++)
            {
                if (this[i, j] != secondMatrix[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Function for writing matrices to file
    /// </summary>
    /// <param name="pathToFile">Paath to file</param>
    /// <param name="this">The matrix to be written to the file</param>
    public void PrintMatrix(string pathToFile)
    {
        using StreamWriter writer = new(pathToFile);
        for (int i = 0; i < this.NumberOfRows; i++)
        {
            for (int j = 0; j < this.NumberOfCols - 1; j++)
            {
                writer.Write($"{this[i, j]} ");
            }

            writer.Write($"{this[i, this.NumberOfCols - 1]}");

            if (i != this.NumberOfRows - 1)
            {
                writer.WriteLine();
            }
        }
    }
}
