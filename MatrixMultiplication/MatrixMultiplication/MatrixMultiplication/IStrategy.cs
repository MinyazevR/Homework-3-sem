namespace MatrixMultiplication;

/// <summary>
/// Interface for the pattern strategy
/// </summary>
public interface IStrategy
{
    public Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix);
}
