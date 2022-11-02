import matplotlib.pyplot as plt
import pandas as pd


def func(sizes, sequentialresult, parallelresult, filename, title, firstindex, secondindex):
    plt.figure(figsize=(8, 5))
    plt.plot(sizes, sequentialresult, 'orange', label='Sequential Average', marker='.')
    plt.plot(sizes, parallelresult, 'purple', label='Parallel Average', marker='.')
    plt.xticks(sizes)
    plt.title(title)
    plt.yticks(parallelresult[firstindex::] + sequentialresult[secondindex::])
    for i in range(len(sizes)):
        plt.plot([0, sizes[i]], [sequentialresult[i], sequentialresult[i]], 'orange', linestyle='--', marker='.')
        plt.plot([0, sizes[i]], [parallelresult[i], parallelresult[i]], 'purple', linestyle='--', marker='.')
    plt.xlabel("size")
    plt.ylabel("time, ms")
    plt.legend()
    plt.savefig(filename)


data = pd.read_csv("Calculation.csv", sep=' ')
parallelAverage = list(map(float, map(lambda s: s.replace(',', '.'), data.ParallelAverage)))
standartAverage = list(map(float, map(lambda s: s.replace(',', '.'), data.StandartAverage)))
standartDeviation = list(map(float, map(lambda s: s.replace(',', '.'), data.StandartDeviation)))
parallelDeviation = list(map(float, map(lambda s: s.replace(',', '.'), data.ParallelDeviation)))

size = list(map(int, data.Size))
func(size[0:5], standartAverage[0:5], parallelAverage[0:5], "Average_time_for_small_sizes.pdf",
     "Average time for small sizes", 0, 2)
func(size[5::], standartAverage[5::], parallelAverage[5::], "Average_time_for_large_sizes.pdf",
     "Average time for large sizes", 1, 1)

# func(size[0:5], standartDeviation[0:5], parallelDeviation[0:5], "Deviation_for_small_sizes.pdf",
# "Deviation for small sizes")
# func(size[5::], standartDeviation[5::], parallelDeviation[5::], "Deviation_for_large_sizes.pdf",
# "Deviation for large sizes")
