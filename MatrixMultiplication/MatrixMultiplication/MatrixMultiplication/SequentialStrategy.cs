namespace MatrixMultiplication;

/// <summary>
/// Strategy for sequential multiplication
/// </summary>
public class SequentialStrategy : IStrategy
{
    /// <summary>
    /// Function for sequential matrix multiplication
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

        for (int i = 0; i < firstMatrix.NumberOfRows; i++)
        {
            for (int j = 0; j < secondMatrix.NumberOfCols; j++)
            {
                for (int k = 0; k < secondMatrix.NumberOfRows; k++)
                {
                    result[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                }
            }
        }

        return result;
    }
}
