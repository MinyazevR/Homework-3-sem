namespace MatrixMultiplication;

public class SequentialStrategy : IStrategy
{
    public Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix)
    {
        return firstMatrix.SequentialMultiply(secondMatrix);
    }
}
