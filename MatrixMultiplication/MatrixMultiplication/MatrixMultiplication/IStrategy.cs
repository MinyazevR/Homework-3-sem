namespace MatrixMultiplication;

public interface IStrategy
{
    public Matrix Multiply(Matrix firstMatrix, Matrix secondMatrix);
}
