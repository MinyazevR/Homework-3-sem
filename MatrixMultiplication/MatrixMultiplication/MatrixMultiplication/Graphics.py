import matplotlib.pyplot as plt
import pandas as pd


def func(sizes, sequentialresult, parallelresult, filename):
    plt.figure(figsize=(8, 5))
    plt.plot(sizes, sequentialresult, 'orange', label='Sequential Average', marker='.')
    plt.plot(sizes, parallelresult, 'purple', label='Parallel Average', marker='.')
    plt.xticks(sizes[2::])
    plt.yticks(sequentialresult.tolist() + parallelresult.tolist())
    for i in range(8):
        plt.plot([0, sizes[i]], [sequentialresult[i], sequentialresult[i]], 'orange', marker='.')
        plt.plot([0, sizes[i]], [parallelresult[i], parallelresult[i]], 'purple', marker='.')
    plt.xlabel("size")
    plt.ylabel("time, ms")
    plt.legend()
    plt.savefig(filename)


lol = pd.read_csv("Calculation.csv", sep=' ')
func(lol.Size, lol.StandartAverage, lol.ParallelAverage, "Average_time.pdf")
func(lol.Size, lol.StandartDeviation, lol.ParallelDeviation, "Deviation.pdf")
