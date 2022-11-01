namespace MatrixMultiplication;

/// <summary>
/// Strategy for parallel multiplication
/// </summary>
public class ParallelStrategy : IStrategy
{
    public Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix)
    {
        return firstMatrix.ParallelMultiply(secondMatrix);
    }
}