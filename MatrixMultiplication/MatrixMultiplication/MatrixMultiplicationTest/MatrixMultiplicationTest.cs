namespace MatrixMultiplicationTest;
using MatrixOperations;

public class MatrixMultiplicationTest
{
    private static IEnumerable<TestCaseData> MatrixMultiplyCaseData() => new TestCaseData[]
        {
        new TestCaseData(new int[,] { { 1 }, { 2 } }, new int[,] { { 2, 1} }, new int[,] {{2, 1 }, {4,2 } }),
        new TestCaseData(new int[,] { { 1, 2}, {3, 4 } }, new int[,]{ {5, 6}, {7, 8 } }, new int[,] { {19 , 22}, {43, 50 } }),
        new TestCaseData(new int[,] { {1, 2, 3 }, { 4, 5, 6} }, new int[,]{{7, 8}, { 9, 10}, { 11, 12} }, new int[,] { {58, 64 }, {139, 154 } }),
        new TestCaseData(new int[,] { {0, 0, 0 }, { 0, 0, 0} }, new int[,]{{0, 0}, { 0, 0}, { 0, 0} }, new int[,] { { 0, 0 }, { 0, 0 } }),
        new TestCaseData(new int[,] { {0, 0}, { 0, 0} }, new int[,]{{0, 0}, { 0, 0} }, new int[,] {{ 0, 0}, { 0, 0 } }),
    };

    [Test]
    public void ShouldExpectedArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MatrixOperations.Generate(-1, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => MatrixOperations.Generate(2, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => MatrixOperations.Generate(1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => MatrixOperations.Generate(0, 1));
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
        Assert.Multiple(() =>
        {
            Assert.That(MatrixOperations.Equals(answer, standartResult), Is.True);
            Assert.That(MatrixOperations.Equals(answer, parallelResult), Is.True);
        });
    }

    [Test]
    public void ShouldExpectedTrueOfComparsionForMatrixBeforeAndAfterPrintMatrix()
    {
        var firstMatrix = MatrixOperations.ReadMatrix("..//..//..//firstMatrix.txt");
        var secondMatrix = MatrixOperations.ReadMatrix("..//..//..//secondMatrix.txt");
        var result = MatrixOperations.ParallelMultiply(firstMatrix, secondMatrix);
        MatrixOperations.PrintMatrix("..//..//..//resultMatrix.txt", result);
        var matrixFromFile = MatrixOperations.ReadMatrix("..//..//..//resultMatrix.txt");
        Assert.That(MatrixOperations.Equals(result, matrixFromFile), Is.True);
    }

    [Test]
    public void ShouldExpectedTrueOfAreEqualForEqualMatrices()
    {
        Assert.Multiple(() =>
        {
            Assert.That(MatrixOperations.Equals(new int[,] { { } }, new int[,] { { } }), Is.True);
            Assert.That(MatrixOperations.Equals(new int[,] { { }, { } }, new int[,] { { }, { } }), Is.True);
            Assert.That(MatrixOperations.Equals(new int[,] { { 0 } }, new int[,] { { 0 } }), Is.True);
            Assert.That(MatrixOperations.Equals(new int[,] { { 1 }, { 1 } }, new int[,] { { 1 }, { 1 } }), Is.True);
            Assert.That(MatrixOperations.Equals(new int[,] { { 1 }, { 1 }, { 1 } }, new int[,] { { 1 }, { 1 }, { 1 } }), Is.True);
        });
    }

    [Test]
    public void ShouldExpectedFalseOfAreEqualForUnequalMatrices()
    {
        Assert.Multiple(() =>
        {
            Assert.That(MatrixOperations.Equals(new int[,] { { } }, new int[,] { { 0 } }), Is.False);
            Assert.That(MatrixOperations.Equals(new int[,] { { } }, new int[,] { { }, { } }), Is.False);
            Assert.That(MatrixOperations.Equals(new int[,] { { } }, new int[,] { }), Is.False);
            Assert.That(MatrixOperations.Equals(new int[,] { { 0 } }, new int[,] { { 0, 1 } }), Is.False);
            Assert.That(MatrixOperations.Equals(new int[,] { { 1 }, { 1 } }, new int[,] { { }, { } }), Is.False);
        });
    }
}
