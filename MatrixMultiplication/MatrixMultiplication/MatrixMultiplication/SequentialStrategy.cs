namespace MatrixMultiplication;

/// <summary>
/// Strategy for sequential multiplication
/// </summary>
public class SequentialStrategy : IStrategy
{
    public Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix)
    {
        return firstMatrix.SequentialMultiply(secondMatrix);
    }
}
