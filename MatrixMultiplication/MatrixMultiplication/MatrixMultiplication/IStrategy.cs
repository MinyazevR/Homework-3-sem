namespace MatrixMultiplication;

/// <summary>
/// Interface for the pattern strategy
/// </summary>
public interface IStrategy
{
    /// <summary>
    /// Function for matrix multiplication
    /// </summary>
    /// <param name="firstMatrix">First matrix</param>
    /// <param name="secondMatrix">Second matrix</param>
    /// <returns>Result of multiplication</returns>
    public Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix);
}
