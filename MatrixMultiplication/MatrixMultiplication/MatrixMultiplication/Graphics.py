import matplotlib.pyplot as plt
import pandas as pd

sizes = [8, 16, 32, 64, 128, 526, 1024]
resultsForSequentialMultiplication = []
resultsForParallelMultiplication = []

lol = pd.read_csv("Calculations.csv", sep=' ')

plt.figure(figsize=(8, 5))

plt.plot(lol.Size, lol.StandartAverage, label="StandartAverage")
plt.plot(lol.Size, lol.ParallelAverage, label="ParallelAverage")

plt.xlabel("size")
plt.ylabel("time, ms")
plt.show()