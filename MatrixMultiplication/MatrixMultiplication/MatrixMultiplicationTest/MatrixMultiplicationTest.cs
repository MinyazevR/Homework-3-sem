namespace MatrixMultiplicationTest;

using MatrixMultiplication;

public class MatrixMultiplicationTest
{
    private SequentialStrategy standartStrategy = new();
    private ParallelStrategy parallelStrategy = new();

    private static IEnumerable<TestCaseData> MatrixMultiplyCaseData() => new TestCaseData[]
    {
        new TestCaseData(
            new Matrix(new int[,] {{1}, {2}}),
            new Matrix(new int[,] {{2, 1}}),
            new Matrix(new int[,] {{2, 1}, {4,2}})),
        new TestCaseData(
            new Matrix(new int[,] {{1, 2}, {3, 4}}),
            new Matrix(new int[,] {{5, 6}, {7, 8}}),
            new Matrix(new int[,] {{19 , 22}, {43, 50}})),
        new TestCaseData(
            new Matrix(new int[,] {{1, 2, 3}, {4, 5, 6}}),
            new Matrix(new int[,] {{7, 8}, {9, 10}, {11, 12}}),
            new Matrix(new int[,] {{58, 64}, {139, 154}})),
        new TestCaseData(
            new Matrix(new int[,] {{0, 0, 0}, {0, 0, 0}}),
            new Matrix(new int[,] {{0, 0}, {0, 0}, {0, 0}}),
            new Matrix(new int[,] {{0, 0}, {0, 0}})),
        new TestCaseData(
            new Matrix(new int[,] {{0, 0}, {0, 0}}),
            new Matrix(new int[,] {{0, 0}, {0, 0}}),
            new Matrix(new int[,] {{0, 0}, {0, 0}}))
    };

    [TestCaseSource(nameof(MatrixMultiplyCaseData))]
    public void ShouldExpectedTrueOfComparsionForActualAndExpectedMatrix(Matrix firstMatrix, Matrix secondMatrix, Matrix answer)
    {
        var standartResult = firstMatrix.Multiply(secondMatrix, standartStrategy);
        var parallelResult = firstMatrix.Multiply(secondMatrix, parallelStrategy);

        Assert.Multiple(() =>
        {
            Assert.That(answer.Equals(standartResult), Is.True);
            Assert.That(answer.Equals(parallelResult), Is.True);
        });
    }

    [Test]
    public void ShouldExpectedArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Matrix.Generate(-1, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Matrix.Generate(2, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Matrix.Generate(1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Matrix.Generate(0, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Matrix.Generate(0, 0));
    }

    [Test]
    public void ShouldExpectedArgumentException()
    {
        var firstMatrix = new Matrix(new int[,] {{2, 1}, {1, 2}});
        var firstMatrixToMultiply = new Matrix(new int[,] {{1, 2}});

        var secondMatrix = new Matrix(new int[,] {{1}});
        var secondMatrixToMultiply = new Matrix(new int[,] {{1, 2}, {5, 6}});

        Assert.Throws<ArgumentException>(() => firstMatrix.Multiply(firstMatrixToMultiply, standartStrategy));
        Assert.Throws<ArgumentException>(() => firstMatrix.Multiply(firstMatrixToMultiply, parallelStrategy));
        Assert.Throws<ArgumentException>(() => secondMatrix.Multiply(secondMatrixToMultiply, standartStrategy));
        Assert.Throws<ArgumentException>(() => secondMatrix.Multiply(secondMatrixToMultiply, parallelStrategy));
    }

    [Test]
    public void ShouldExpectedTrueOfComparsionForMatrixBeforeAndAfterPrintMatrix()
    {
        var firstMatrix = Matrix.ReadMatrix("..//..//..//firstMatrix.txt");
        var secondMatrix = Matrix.ReadMatrix("..//..//..//secondMatrix.txt");
        var result = firstMatrix.Multiply(secondMatrix, parallelStrategy);
        result.PrintMatrix("resultMatrix.txt");
        var matrixFromFile = Matrix.ReadMatrix("resultMatrix.txt");
        Assert.That(result.Equals(matrixFromFile), Is.True);
    }

    [Test]
    public void ShouldExpectedTrueOfAreEqualForEqualMatrices()
    {
        var firstMatrix = new Matrix(new int[,] {{}});
        var martixIsEqualFirst = new Matrix(new int[,] {{}});

        var secondMatrix = new Matrix(new int[,] {{}, {}});
        var matrixIsEqualSecond = new Matrix(new int[,] {{}, {}});

        var thirdMatrix = new Matrix(new int[,] {{0}});
        var matrixIsEqualThird = new Matrix(new int[,] {{0}});

        var fourthMatrix = new Matrix(new int[,] {{1}, {1}});
        var matrixIsEqualFourth = new Matrix(new int[,] {{1}, {1}});

        var fifthMatrix = new Matrix(new int[,] {{1}, {1}, {1}});
        var matrixIsEqualFifth = new Matrix(new int[,] {{1}, {1}, {1}});

        Assert.Multiple(() =>
        {
            Assert.That(firstMatrix.Equals(martixIsEqualFirst), Is.True);
            Assert.That(secondMatrix.Equals(matrixIsEqualSecond), Is.True);
            Assert.That(thirdMatrix.Equals(matrixIsEqualThird), Is.True);
            Assert.That(fourthMatrix.Equals(matrixIsEqualFourth), Is.True);
            Assert.That(fifthMatrix.Equals(matrixIsEqualFifth), Is.True);
        });
    }

    [Test]
    public void ShouldExpectedFalseOfAreEqualForUnequalMatrices()
    {
        var firstMatrix = new Matrix(new int[,]{{}});
        var martixIsNoEqualFirst = new Matrix(new int[,] {{0}});

        var secondMatrix = new Matrix(new int[,] {{}});
        var matrixIsNoEqualSecond = new Matrix(new int[,] {{}, {}});

        var thirdMatrix = new Matrix(new int[,] {{}});
        var matrixIsNoEqualThird = new Matrix(new int[,] {});

        var fourthMatrix = new Matrix(new int[,] {{0}});
        var matrixIsNoEqualFourth = new Matrix(new int[,] {{0, 1}});

        var fifthMatrix = new Matrix(new int[,] {{1}, {1}});
        var matrixIsNoEqualFifth = new Matrix(new int[,] {{}, {}});

        Assert.Multiple(() =>
        {
            Assert.That(firstMatrix.Equals(martixIsNoEqualFirst), Is.False);
            Assert.That(secondMatrix.Equals(matrixIsNoEqualSecond), Is.False);
            Assert.That(thirdMatrix.Equals(matrixIsNoEqualThird), Is.False);
            Assert.That(fourthMatrix.Equals(matrixIsNoEqualFourth), Is.False);
            Assert.That(fifthMatrix.Equals(matrixIsNoEqualFifth), Is.False);
        });
    }
}
