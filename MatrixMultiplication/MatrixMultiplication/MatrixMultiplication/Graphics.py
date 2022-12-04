import matplotlib.pyplot as plt
import pandas as pd


def func(sizes, sequential_result, parallel_result, filename, title, first_index, second_index):
    plt.figure(figsize=(8, 5))
    plt.plot(sizes, sequential_result, 'orange', label='Sequential Average', marker='.')
    plt.plot(sizes, parallel_result, 'purple', label='Parallel Average', marker='.')
    plt.xticks(sizes)
    plt.title(title)
    plt.yticks(parallel_result[first_index::] + sequential_result[second_index::])
    for i in range(len(sizes)):
        plt.plot([0, sizes[i]], [sequential_result[i], sequential_result[i]], 'orange', linestyle='--', marker='.')
        plt.plot([0, sizes[i]], [parallel_result[i], parallel_result[i]], 'purple', linestyle='--', marker='.')
    plt.xlabel("size")
    plt.ylabel("time, ms")
    plt.legend()
    plt.savefig(filename)


data = pd.read_csv("Calculation.csv", sep=' ')
parallel_average = list(map(float, map(lambda s: s.replace(',', '.'), data.parallel_average)))
sequential_average = list(map(float, map(lambda s: s.replace(',', '.'), data.standart_average)))
sequential_deviation = list(map(float, map(lambda s: s.replace(',', '.'), data.StandartDeviation)))
parallel_deviation = list(map(float, map(lambda s: s.replace(',', '.'), data.ParallelDeviation)))

size = list(map(int, data.Size))
func(size[0:5], sequential_average[0:5], parallel_average[0:5], "Average_time_for_small_sizes.pdf",
     "Average time for small sizes", 0, 2)
func(size[5::], sequential_average[5::], parallel_average[5::], "Average_time_for_large_sizes.pdf",
     "Average time for large sizes", 1, 1)

# func(size[0:5], sequential_deviation[0:5], parallel_deviation[0:5], "Deviation_for_small_sizes.pdf",
# "Deviation for small sizes")
# func(size[5::], sequential_deviation[5::], parallel_deviation [5::], "Deviation_for_large_sizes.pdf",
# "Deviation for large sizes")
