namespace Test;

/// <summary>
/// Class representing the calculation of the check sum
/// </summary>
public class MD5
{
    public static async Task<byte[]> ComputeCheckSumForFile(string path)
    {
        var md5 = System.Security.Cryptography.MD5.Create();
        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }
        using var fileStream = new FileStream(path, FileMode.Open);
        var bytes = await md5.ComputeHashAsync(fileStream);
        return bytes;
    }

    /// <summary>
    /// Function for sequential calculation of the check sum of the directory
    /// </summary>
    /// <param name="pathToDirectory">Path to directory</param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public static async Task<byte[]> SequentiallyComputeCheckSumForDirectory(string pathToDirectory)
    {
        if(!Directory.Exists(pathToDirectory))
        {
            throw new DirectoryNotFoundException();
        }
        
        var files = Directory.GetFiles(pathToDirectory);
        var directories = Directory.GetDirectories(pathToDirectory);
        var list = new List<(byte[], string)>();

        foreach(var directory in directories)
        {
            list.Add((await SequentiallyComputeCheckSumForDirectory(directory), directory));
        }

        foreach (var file in files)
        {
            list.Add((await ComputeCheckSumForFile(file), file));
        }
        var directoryName = System.Text.Encoding.UTF8.GetBytes(Path.GetDirectoryName(pathToDirectory)!);
        foreach(var (lol, elem) in list)
        {
            var result = new byte[elem.Length + lol.Length];
            directoryName.CopyTo(result, 0);
            lol.CopyTo(result, elem.Length);
            directoryName = result;
        }
        var md5 = System.Security.Cryptography.MD5.Create();
        return md5.ComputeHash(directoryName);
    }

    /// <summary>
    /// Function for parallel calculation of the check sum of the directory
    /// </summary>
    /// <param name="pathToDirectory">Path to directory</param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public static byte[] ParallelComputeCheckSumForDirectory(string pathToDirectory)
    {
        // Если директории не существует 
        if (!Directory.Exists(pathToDirectory))
        {
            throw new DirectoryNotFoundException();
        }

        // Берем имена файлов
        var files = Directory.GetFiles(pathToDirectory);

        // Имена директорий
        var directories = Directory.GetDirectories(pathToDirectory);

        var list = new List<(byte[], string)>();

        // Изначально хотел сделать как в домашке с матрицами, а именно дать каждому изпотоков какое то количество файлов на обработку
        // после просто Task.Run(() => {...}) и объединять Task.Result
        // Но так вроде выглядит проще
        Parallel.ForEach(directories, directory => list.Add((ParallelComputeCheckSumForDirectory(directory), directory)));
        Parallel.ForEach(files, file => list.Add((ParallelComputeCheckSumForDirectory(file), file)));

        list.Sort();
        var directoryName = System.Text.Encoding.UTF8.GetBytes(Path.GetDirectoryName(pathToDirectory)!);


        foreach (var (lol, elem) in list)
        {
            var result = new byte[elem.Length + lol.Length];
            directoryName.CopyTo(result, 0);
            lol.CopyTo(result, elem.Length);
            directoryName = result;
        }
        var md5 = System.Security.Cryptography.MD5.Create();
        return md5.ComputeHash(directoryName);
    }
}
