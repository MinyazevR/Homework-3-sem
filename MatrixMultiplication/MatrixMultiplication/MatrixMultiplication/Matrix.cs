﻿namespace MatrixMultiplication;

/// <summary>
/// Сlass representing a matrix
/// </summary>
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
    private static readonly Random random = new();

    /// <summary>
    /// Indexer that returns an element of the matrix according to the given coordinates
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
    /// Constructor for matrix initialization
    /// </summary>
    /// <param name="data">two-dimensional array for matrix initialization</param>
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
