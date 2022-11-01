namespace MatrixMultiplication;

public class ParallelStrategy : IStrategy
{
    public Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix)
    {
        return firstMatrix.ParallelMultiply(secondMatrix);
    }
}