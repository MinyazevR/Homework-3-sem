namespace MatrixMultiplicationTest;
using MatrixOPerations;

public class MatrixMultiplicationTest
{
    [SetUp]
    public void Setup()
    {
    }

    private static IEnumerable<TestCaseData> MatrixMultiplyCaseData() => new TestCaseData[]
        {
        new TestCaseData(new int[,] { { 1 }, { 2 } }, new int[,] { { 2, 1} }, new int[,] {{2, 1 }, {4,2 } }),
        new TestCaseData(new int[,] { { 1, 2}, {3, 4 } }, new int[,]{ {5, 6}, {7, 8 } }, new int[,] { {19 , 22}, {43, 50 } }),
        new TestCaseData(new int[,] { {1, 2, 3 }, { 4, 5, 6} }, new int[,]{{7, 8}, { 9, 10}, { 11, 12} }, new int[,] { {58, 64 }, {139, 154 } }),
        new TestCaseData(new int[,] { {0, 0, 0 }, { 0, 0, 0} }, new int[,]{{0, 0}, { 0, 0}, { 0, 0} }, new int[,] { { 0, 0 }, { 0, 0 } }),
        new TestCaseData(new int[,] { {0, 0}, { 0, 0} }, new int[,]{{0, 0}, { 0, 0} }, new int[,] {{ 0, 0}, { 0, 0 } }),
    };

    [Test]
    public void ShouldExpectedArgumentOutOfRangeExceptiont()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MatrixOperations.Generate(-1, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => MatrixOperations.Generate(2, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => MatrixOperations.Generate(0, 0));

    }

    [Test]
    public void ShouldExpectedArgumentException()
    {
        Assert.Throws<ArgumentException>(() => MatrixOperations.StandartMultiply(new int[,] { { 2, 1 }, { 1, 2 } }, new int[,] { { 1, 2 } }));
        Assert.Throws<ArgumentException>(() => MatrixOperations.ParallelMultiply(new int[,] { { 2, 1 }, { 1, 2 } }, new int[,] { { 1, 2 } }));
        Assert.Throws<ArgumentException>(() => MatrixOperations.StandartMultiply(new int[,] { { 1 } }, new int[,] { { 1, 2 }, { 5, 6} }));
        Assert.Throws<ArgumentException>(() => MatrixOperations.ParallelMultiply(new int[,] { { 1 } }, new int[,] { { 1, 2 }, { 5, 6} }));

    }

    [TestCaseSource(nameof(MatrixMultiplyCaseData))]
    public void ShouldExpectedTrueOfComparsionForActualAndExpectedMatrix(int[,] firstMatrix, int[,] secondMatrix, int[,] answer)
    {
        var standartResult  = MatrixOperations.StandartMultiply(firstMatrix, secondMatrix);
        var parallelResult  = MatrixOperations.ParallelMultiply(firstMatrix, secondMatrix);
        Assert.True(MatrixOperations.Equals(answer, standartResult));
        Assert.True(MatrixOperations.Equals(answer, parallelResult));
    }

    [Test]
    public void ShouldExpectedTrueOfComparsionForMatrixBeforeAndAfterPrintMatrix()
    {
        var firstMatrix = MatrixOperations.ReadMatrix("..//..//..//firstMatrix.txt");
        var secondMatrix = MatrixOperations.ReadMatrix("..//..//..//secondMatrix.txt");
        var result = MatrixOperations.ParallelMultiply(firstMatrix, secondMatrix);
        MatrixOperations.PrintMatrix("..//..//..//resultMatrix.txt", result);
        var matrixFromFile = MatrixOperations.ReadMatrix("..//..//..//resultMatrix.txt");
        Assert.True(MatrixOperations.Equals(result, matrixFromFile));
    }
}
